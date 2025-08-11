using BoardScrabble.GameControllers.Enums;
using BoardScrabble.GameControllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
    public class GameControl
    {
        public List<IPlayer> _players;
        public Dictionary<IPlayer, List<Tile>> _playerRacks;
        public int _activePlayerIndex;
        public IBoard _gameBoard;
        public ITileBag _tileBag;
        public IDictionary _dictionary;
        public GameState _currentState;
        public int _consecutiveSkips;
        private const int MIN_PLAYERS = 2;
        private const int MAX_PLAYERS = 4;
        public HashSet<Tuple<int, int>> _usedBonusSquaresThisTurn;
        private Random _randomGenerator;
        public Dictionary<IPlayer, List<string>> PlayerHistory = new();




        // Events
        public event Func<Tile, char>? OnRequestBlankTileChar;
        public event Action<string>? OnDisplayMessage;
        public event Action<string, object?>? OnGameEvent;


        public GameControl(IDictionary dictionary, ITileBag tileBag, IBoard board)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _tileBag = tileBag ?? throw new ArgumentNullException(nameof(tileBag));
            _gameBoard = board ?? throw new ArgumentNullException(nameof(board));

            _players = new List<IPlayer>();
            _playerRacks = new Dictionary<IPlayer, List<Tile>>();
            _activePlayerIndex = 0;
            _currentState = GameState.NotStarted;
            _consecutiveSkips = 0;
            _usedBonusSquaresThisTurn = new HashSet<Tuple<int, int>>();
            _randomGenerator = new Random();

            _gameBoard.InitializeBonusSquares();
            _tileBag.InitializeStandardTiles();
        }

        public bool AddPlayer(IPlayer player)
        {
            if (_currentState != GameState.NotStarted)
            {
                OnDisplayMessage?.Invoke("Cannot add players after the game has started.");
                return false;
            }
            if (_players.Count >= MAX_PLAYERS)
            {
                OnDisplayMessage?.Invoke($"Maximum {_players.Count} players reached.");
                return false;
            }
            if (!IsPlayerNameUnique(player.GetName()))
            {
                OnDisplayMessage?.Invoke($"Player with name '{player.GetName()}' already exists.");
                return false;
            }

            _players.Add(player);
            _playerRacks.Add(player, new List<Tile>());
            //OnDisplayMessage?.Invoke($"Player '{player.GetName()}' added");
            return true;
        }

 



        public bool RemovePlayer(string playerName)
        {
            if (_currentState != GameState.NotStarted)
            {
                OnDisplayMessage?.Invoke("Cannot remove players after the game has started.");
                return false;
            }
            IPlayer? playerToRemove = _players.FirstOrDefault(p => p.GetName().Equals(playerName, StringComparison.OrdinalIgnoreCase));
            if (playerToRemove == null)
            {
                OnDisplayMessage?.Invoke($"Player '{playerName}' not found.");
                return false;
            }

            _players.Remove(playerToRemove);
            _playerRacks.Remove(playerToRemove);
            OnDisplayMessage?.Invoke($"Player '{playerName}' removed.");
            return true;
        }

        public bool StartGame()
        {
            if (_currentState != GameState.NotStarted)
            {
                OnDisplayMessage?.Invoke("Game has already started or ended.");
                return false;
            }
            if (!ValidatePlayerCount())
            {
                OnDisplayMessage?.Invoke($"Need between {MIN_PLAYERS} and {MAX_PLAYERS} players to start.");
                return false;
            }

            // Refill all player racks
            foreach (var player in _players)
            {
                RefillPlayerRack(player);
            }

            _currentState = GameState.InProgress;
            OnDisplayMessage?.Invoke("Game started!");
            OnGameEvent?.Invoke("GameStarted", null);

            return true;
        }

        public IPlayer GetCurrentPlayer()
        {
            var  currentPlayer = _players[_activePlayerIndex];
            return currentPlayer;
        }

        public List<IPlayer> GetAllPlayers()
        {
            var listPlayer = new List<IPlayer>(_players);
            return listPlayer;
        }

        public IPlayer? GetWinner()
        {
            if (_currentState != GameState.Ended || !_players.Any())
            {
                return null;
            }
            var winner = _players.OrderByDescending(p => p.GetScore()).FirstOrDefault();
            return winner;
        }

        public void FillTileBlank(int idx, char tileChar)
        {
            _playerRacks[GetCurrentPlayer()][idx].SetLetter(tileChar);
        }

        public MoveError PerformTurn(IPlayer player, List<TilePlacement> placements)
        {
            

            if (_currentState != GameState.InProgress)
            {
                return MoveError.GameAlreadyStarted; 
            }
            if (player != GetCurrentPlayer())
            {
                OnDisplayMessage?.Invoke("It's not your turn.");
                return MoveError.InvalidPlacement;
            }

            _usedBonusSquaresThisTurn.Clear();


            MoveError placementError = IsPlacementValid(placements);
            if (placementError != MoveError.None)
            {
                return placementError;
            }

            List<Tile> tilesUsed = placements.Select(p => p.GetTile()).ToList();
            foreach (var tile in tilesUsed)
            {
                if (!CheckTileInPlayerRack(player, tile))
                {
                    OnDisplayMessage?.Invoke($"Tile '{tile.GetLetter()}' not in your rack.");
                    return MoveError.TileNotInRack;
                }
            }

            foreach (var placement in placements)
            {
                _gameBoard.GetSquare(placement.GetX(), placement.GetY()).SetTile(placement.GetTile());
            }

            List<string> formedWords = GetAllWordsFromPlacement(placements);
            foreach (var word in formedWords)
            {
                if (!IsWordInDictionary(word))
                {
 
                    foreach (var placement in placements)
                    {
                        _gameBoard.GetSquare(placement.GetX(), placement.GetY()).SetTile(null!);
                    }
                    OnDisplayMessage?.Invoke($"Word '{word}' is not in the dictionary.");
                    return MoveError.WordNotInDictionary;
                }

                if (!PlayerHistory.ContainsKey(GetCurrentPlayer()))
                {
                    PlayerHistory[GetCurrentPlayer()] = new List<string>();
                }
                PlayerHistory[GetCurrentPlayer()].Add(word);
            }

            int score = CalculateWordScore(formedWords, placements);
            player.AddScore(score);
            OnDisplayMessage?.Invoke($"Player '{player.GetName()}' scored {score} points.");


            foreach (var tile in tilesUsed)
            {
                _playerRacks[player].Remove(tile);
            }
 
            RefillPlayerRack(player);


            _consecutiveSkips = 0; 
            OnGameEvent?.Invoke("TurnCompleted", player);


            NextTurn();
            CheckEndGameConditions();

            return MoveError.None;
        }

        public MoveError SwapTiles(IPlayer player, List<Tile> tilesToSwap)
        {

            if (_currentState != GameState.InProgress)
            {
                return MoveError.GameAlreadyStarted;
            }
            if (player != GetCurrentPlayer())
            {
                OnDisplayMessage?.Invoke("It's not your turn.");
                return MoveError.InvalidPlacement;
            }
            if (tilesToSwap == null || !tilesToSwap.Any())
            {
                return MoveError.InvalidTilesToSwap;
            }

            if (tilesToSwap.Count > _playerRacks[player].Count)
            {
                return MoveError.TooManyTilesToSwap;
            }

            if (_tileBag.GetTilesList().Count < tilesToSwap.Count)
            {
                OnDisplayMessage?.Invoke("Not enough tiles in the bag to swap.");
                return MoveError.TooManyTilesToSwap; 
            }


            List<Tile> playerTiles = _playerRacks[player];
            List<int> indexTile = [];
            foreach (var tile in tilesToSwap)
            {
                indexTile.Add(playerTiles.IndexOf(tile));
                if (!playerTiles.Contains(tile))
                {
                    return MoveError.TileNotInRack;
                }
            }

            foreach (var tile in tilesToSwap)
            {
                playerTiles.Remove(tile);
            }

            ReturnTilesToBag(tilesToSwap);

            List<Tile> newTiles = DrawTilesFromBag(tilesToSwap.Count);
            foreach (var (idx, tile) in indexTile.Zip(newTiles, (i, t) => (i, t)))
            {
                playerTiles.Insert(idx, tile);
            }
            _consecutiveSkips = 0; 
            OnDisplayMessage?.Invoke($"Player '{player.GetName()}' swapped {tilesToSwap.Count} tiles.");
            NextTurn();
            return MoveError.None;
        }

        public MoveError SkipTurn(IPlayer player)
        {
            if (_currentState != GameState.InProgress)
            {
                return MoveError.GameAlreadyStarted;
            }
            if (player != GetCurrentPlayer())
            {
                OnDisplayMessage?.Invoke("It's not your turn.");
                return MoveError.InvalidPlacement;
            }

            _consecutiveSkips++;
            OnDisplayMessage?.Invoke($"Player '{player.GetName()}' skipped their turn. Consecutive skips: {_consecutiveSkips}");
            OnGameEvent?.Invoke("TurnSkipped", player);
            NextTurn();
            CheckEndGameConditions();
            return MoveError.None;
        }

        public void NextTurn()
        {
            _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
            OnDisplayMessage?.Invoke($"It's now '{GetCurrentPlayer()!.GetName()}'s turn.");
            OnGameEvent?.Invoke("PlayerTurnStarted", GetCurrentPlayer());
        }

        public void RefillPlayerRack(IPlayer player)
        {
            int tilesNeeded = 7 - _playerRacks[player].Count;
            if (tilesNeeded > 0)
            {
                List<Tile> newTiles = DrawTilesFromBag(tilesNeeded);
                _playerRacks[player].AddRange(newTiles);
            }
        }

        public int CalculateWordScore(List<string> words, List<TilePlacement> placements)
        {
            int totalScore = 0;
            foreach (string word in words)
            {
                int currentWordScore = 0;
                int wordMultiplier = 1;

                Direction wordDirection;
                if (placements.Count == 1) 
                {
                    wordDirection = Direction.Horizontal; 
                }
                else
                {
                    wordDirection = placements.First().GetX() == placements.Last().GetX() ? Direction.Horizontal : Direction.Vertical;
                }

                List<Square> wordSquares = new List<Square>();
                if (wordDirection == Direction.Horizontal)
                {
                    int startCol = placements.Min(p => p.GetY());
                    int endCol = placements.Max(p => p.GetY());
                    int row = placements.First().GetX();
                    for (int y = startCol; y <= endCol; y++)
                    {
                        wordSquares.Add(_gameBoard.GetSquare(row, y));
                    }
                }
                else 
                {
                    int startRow = placements.Min(p => p.GetX());
                    int endRow = placements.Max(p => p.GetX());
                    int col = placements.First().GetY();
                    for (int x = startRow; x <= endRow; x++)
                    {
                        wordSquares.Add(_gameBoard.GetSquare(x, col));
                    }
                }

                foreach (var square in wordSquares)
                {
                    if (square.GetTile() != null)
                    {
                        int tilePoints = square.GetTile()!.GetPoints();
                        BonusSquareType bonus = square.GetBonusType();

                        if (!_usedBonusSquaresThisTurn.Contains(Tuple.Create(square.GetX(), square.GetY())))
                        {
                            switch (bonus)
                            {
                                case BonusSquareType.DoubleLetter:
                                    tilePoints *= 2;
                                    break;
                                case BonusSquareType.TripleLetter:
                                    tilePoints *= 3;
                                    break;
                                case BonusSquareType.DoubleWord:
                                    wordMultiplier *= 2;
                                    break;
                                case BonusSquareType.TripleWord:
                                    wordMultiplier *= 3;
                                    break;
                            }
                            MarkBonusSquareUsed(square.GetX(), square.GetY());
                        }
                        currentWordScore += tilePoints;
                    }
                }
                totalScore += (currentWordScore * wordMultiplier);
            }

            if (placements.Count == 7)
            {
                totalScore += 50;
                OnDisplayMessage?.Invoke("Bingo! +50 points for using all 7 tiles!");
            }

            return totalScore;
        }

        public MoveError IsPlacementValid(List<TilePlacement> placements)
        {
            if (placements == null || !placements.Any())
            {
                var invalidPlacementError = MoveError.InvalidPlacement;
                return invalidPlacementError;
            }

            foreach (var placement in placements)
            {
                if (!IsCoordinateValid(placement.GetX(), placement.GetY()))
                {
                    var invalidCoordinatesError = MoveError.InvalidCoordinates;

                    return invalidCoordinatesError;
                }
                if (!IsSquareEmptyOnBoard(placement.GetX(), placement.GetY()))
                {



                    return MoveError.InvalidPlacement;
                }
            }

            placements = placements.OrderBy(p => p.GetX()).ThenBy(p => p.GetY()).ToList();

            bool isHorizontal = placements.All(p => p.GetX() == placements.First().GetX());
            bool isVertical = placements.All(p => p.GetY() == placements.First().GetY());

            if (!isHorizontal && !isVertical)
            {

                OnDisplayMessage?.Invoke("Placement must horizontal or vertical!");
                return MoveError.InvalidPlacement; 
            }

            if (placements.Count > 1)
            {


                if (!AreConsecutivePlacements(placements))
                {

                    
                    return MoveError.InvalidPlacement; 
                }
            }

            if (_gameBoard.GetSquare(7, 7).GetTile() == null && _currentState == GameState.InProgress) 
            {


                if (!IsValidFirstMove(placements))
                {

                    OnDisplayMessage?.Invoke("Invalid First Move");
                    return MoveError.InvalidFirstMove;
                }
            }
            else 
            {
                if (!ConnectsToExistingTiles(placements))
                {


                    return MoveError.NotConnected;
                }
            }

            return MoveError.None;
        }

        public bool IsValidFirstMove(List<TilePlacement> placements)
        {
            var validFirstMove =  placements.Any(p => p.GetX() == 7 && p.GetY() == 7);
            return validFirstMove;
        }

        public bool AreConsecutivePlacements(List<TilePlacement> placements)
        {
            if (placements.Count <= 1) return true;

            placements = placements.OrderBy(p => p.GetX()).ThenBy(p => p.GetY()).ToList();

            bool isHorizontal = placements.First().GetX() == placements.Last().GetX();
            bool isVertical = placements.First().GetY() == placements.Last().GetY();

            if (isHorizontal)
            {
                int row = placements.First().GetX();
                int startCol = placements.First().GetY();
                int endCol = placements.Last().GetY();

                for (int y = startCol; y <= endCol; y++)
                {
                    bool isCoveredByPlacement = placements.Any(p => p.GetX() == row && p.GetY() == y);
                    bool hasExistingTile = _gameBoard.GetSquare(row, y).GetTile() != null;

                    if (!isCoveredByPlacement && !hasExistingTile)
                    {
                        return false; 
                    }
                }
            }
            else if (isVertical)
            {
                int col = placements.First().GetY();
                int startRow = placements.First().GetX();
                int endRow = placements.Last().GetX();

                for (int x = startRow; x <= endRow; x++)
                {
                    bool isCoveredByPlacement = placements.Any(p => p.GetX() == x && p.GetY() == col);
                    bool hasExistingTile = _gameBoard.GetSquare(x, col).GetTile() != null;

                    if (!isCoveredByPlacement && !hasExistingTile)
                    {
                        return false; 
                    }
                }
            }
            else
            {
                return false; 
            }

            return true;
        }

        public bool ConnectsToExistingTiles(List<TilePlacement> placements)
        {
            foreach (var newPlacement in placements)
            {
                int x = newPlacement.GetX();
                int y = newPlacement.GetY();


                if ((x > 0 && _gameBoard.GetSquare(x - 1, y).GetTile() != null) || 
                    (x < 14 && _gameBoard.GetSquare(x + 1, y).GetTile() != null) || 
                    (y > 0 && _gameBoard.GetSquare(x, y - 1).GetTile() != null) || 
                    (y < 14 && _gameBoard.GetSquare(x, y + 1).GetTile() != null))   
                {

                    bool isNeighborAlsoNewPlacement = placements.Any(p =>
                        (p.GetX() == x - 1 && p.GetY() == y) ||
                        (p.GetX() == x + 1 && p.GetY() == y) ||
                        (p.GetX() == x && p.GetY() == y - 1) ||
                        (p.GetX() == x && p.GetY() == y + 1));

                    if (!isNeighborAlsoNewPlacement)
                    {
                        return true;
                    }
                }
            }

            HashSet<Tuple<int, int>> involvedCoords = new HashSet<Tuple<int, int>>();
            foreach (var p in placements)
            {
                involvedCoords.Add(Tuple.Create(p.GetX(), p.GetY()));
            }


            bool isHorizontal = placements.All(p => p.GetX() == placements.First().GetX());

            if (placements.Count == 1) 
            {
                int x = placements[0].GetX();
                int y = placements[0].GetY();
                if ((x > 0 && _gameBoard.GetSquare(x - 1, y).GetTile() != null) ||
                    (x < 14 && _gameBoard.GetSquare(x + 1, y).GetTile() != null) ||
                    (y > 0 && _gameBoard.GetSquare(x, y - 1).GetTile() != null) ||
                    (y < 14 && _gameBoard.GetSquare(x, y + 1).GetTile() != null))
                {
                    return true;
                }
                return false;
            }

            if (isHorizontal)
            {
                int row = placements.First().GetX();
                int minCol = placements.Min(p => p.GetY());
                int maxCol = placements.Max(p => p.GetY());

                int wordStartCol = FindWordStartOnBoard(row, minCol, Direction.Horizontal);
                int wordEndCol = FindWordEndOnBoard(row, maxCol, Direction.Horizontal);

                for (int col = wordStartCol; col <= wordEndCol; col++)
                {
                    if (_gameBoard.GetSquare(row, col).GetTile() != null &&
                        !placements.Any(p => p.GetX() == row && p.GetY() == col))
                    {
                        return true; 
                    }
                }
            }
            else 
            {
                int col = placements.First().GetY();
                int minRow = placements.Min(p => p.GetX());
                int maxRow = placements.Max(p => p.GetX());

                int wordStartRow = FindWordStartOnBoard(minRow, col, Direction.Vertical);
                int wordEndRow = FindWordEndOnBoard(maxRow, col, Direction.Vertical);

                for (int row = wordStartRow; row <= wordEndRow; row++)
                {
                    if (_gameBoard.GetSquare(row, col).GetTile() != null &&
                        !placements.Any(p => p.GetX() == row && p.GetY() == col))
                    {
                        return true; 
                    }
                }
            }

            return false; // No connection found
        }


        public string GetWordFromBoard(int row, int col, Direction direction)
        {
            string word = "";
            if (direction == Direction.Horizontal)
            {
                int startCol = FindWordStartOnBoard(row, col, Direction.Horizontal);
                for (int y = startCol; y < 15; y++)
                {
                    Tile? tile = _gameBoard.GetSquare(row, y).GetTile();
                    if (tile != null)
                    {
                        word += tile.GetLetter();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else 
            {
                int startRow = FindWordStartOnBoard(row, col, Direction.Vertical);
                for (int x = startRow; x < 15; x++)
                {
                    Tile? tile = _gameBoard.GetSquare(x, col).GetTile();
                    if (tile != null)
                    {
                        word += tile.GetLetter();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return word;
        }

        public List<string> GetAllWordsFromPlacement(List<TilePlacement> placements)
        {
            List<string> formedWords = new List<string>();
            HashSet<Tuple<int, int>> placedCoords = new HashSet<Tuple<int, int>>();
            foreach (var p in placements)
            {
                placedCoords.Add(Tuple.Create(p.GetX(), p.GetY()));
            }


            bool isHorizontalPlacement = placements.All(p => p.GetX() == placements.First().GetX());
            bool isVerticalPlacement = placements.All(p => p.GetY() == placements.First().GetY());

            if (isHorizontalPlacement)
            {

                int row = placements.First().GetX();
                int startCol = FindWordStartOnBoard(row, placements.Min(p => p.GetY()), Direction.Horizontal);
                string mainWord = GetWordFromBoard(row, startCol, Direction.Horizontal);
                if (mainWord.Length > 1) 
                {
                    formedWords.Add(mainWord);
                }

          
                foreach (var placement in placements)
                {
                    int x = placement.GetX();
                    int y = placement.GetY();
                    int startRow = FindWordStartOnBoard(x, y, Direction.Vertical);
                    string sideWord = GetWordFromBoard(startRow, y, Direction.Vertical);
                    if (sideWord.Length > 1)
                    {
                        formedWords.Add(sideWord);
                    }
                }
            }
            else if (isVerticalPlacement)
            {

                int col = placements.First().GetY();
                int startRow = FindWordStartOnBoard(placements.Min(p => p.GetX()), col, Direction.Vertical);
                string mainWord = GetWordFromBoard(startRow, col, Direction.Vertical);
                if (mainWord.Length > 1)
                {
                    formedWords.Add(mainWord);
                }


                foreach (var placement in placements)
                {
                    int x = placement.GetX();
                    int y = placement.GetY();
                    int startCol = FindWordStartOnBoard(x, y, Direction.Horizontal);
                    string sideWord = GetWordFromBoard(x, startCol, Direction.Horizontal);
                    if (sideWord.Length > 1)
                    {
                        formedWords.Add(sideWord);
                    }
                }
            }
            else 
            {
                if (placements.Count == 1)
                {
                    var placement = placements.First();
                    int x = placement.GetX();
                    int y = placement.GetY();

      
                    int startCol = FindWordStartOnBoard(x, y, Direction.Horizontal);
                    string horizWord = GetWordFromBoard(x, startCol, Direction.Horizontal);
                    if (horizWord.Length > 1)
                    {
                        formedWords.Add(horizWord);
                    }

                    int startRow = FindWordStartOnBoard(x, y, Direction.Vertical);
                    string vertWord = GetWordFromBoard(startRow, y, Direction.Vertical);
                    if (vertWord.Length > 1)
                    {
                        formedWords.Add(vertWord);
                    }
                }
            }

           
            return formedWords.Distinct().ToList();
        }

        public bool CheckTileInPlayerRack(IPlayer player, Tile tile)
        {
            var checkTileInRack = _playerRacks[player].Contains(tile);
            return checkTileInRack;
        }

        public bool IsWordInDictionary(string word)
        {
            var checkWordInDict = _dictionary.GetAllWords().Contains(word.ToUpper());
            return checkWordInDict;
        }

        public bool PlaceTileOnBoard(Tile tile, int x, int y)
        {
            if (IsCoordinateValid(x, y) && IsSquareEmptyOnBoard(x, y))
            {
                _gameBoard.GetSquare(x, y).SetTile(tile);
                return true;
            }
            return false;
        }

        public List<Tile> DrawTilesFromBag(int count)
        {
            List<Tile> drawnTiles = new List<Tile>();
            List<Tile> availableTiles = _tileBag.GetTilesList(); 

            for (int i = 0; i < count; i++)
            {
                if (availableTiles.Count > 0)
                {
                    int index = _randomGenerator.Next(availableTiles.Count);
                    Tile tile = availableTiles[index];
                    drawnTiles.Add(tile);
                    availableTiles.RemoveAt(index);
                }
                else
                {
                    break; // No more tiles in the bag
                }
            }
            return drawnTiles;
        }

        public void ReturnTilesToBag(List<Tile> tiles)
        {
            _tileBag.GetTilesList().AddRange(tiles);
        }

        public int FindWordStartOnBoard(int row, int col, Direction direction)
        {
            if (direction == Direction.Horizontal)
            {
                while (col > 0 && _gameBoard.GetSquare(row, col - 1).GetTile() != null)
                {
                    col--;
                }
                return col;
            }
            else 
            {
                while (row > 0 && _gameBoard.GetSquare(row - 1, col).GetTile() != null)
                {
                    row--;
                }
                return row;
            }
        }

        private int FindWordEndOnBoard(int row, int col, Direction direction)
        {
            if (direction == Direction.Horizontal)
            {
                while (col < 14 && _gameBoard.GetSquare(row, col + 1).GetTile() != null)
                {
                    col++;
                }
                return col;
            }
            else 
            {
                while (row < 14 && _gameBoard.GetSquare(row + 1, col).GetTile() != null)
                {
                    row++;
                }
                return row;
            }
        }

        public bool IsCoordinateValid(int x, int y)
        { 
            var isCoorValid = x >= 0 && x < 15 && y >= 0 && y < 15;
            return isCoorValid;
        }

        public bool IsSquareEmptyOnBoard(int x, int inty)
        {
            var isSquareEmptyOnBoard = _gameBoard.GetSquare(x, inty).GetTile() == null;
            return isSquareEmptyOnBoard;
        }

        public bool IsTileBlank(Tile tile)
        {
            var isTileBlank = tile.GetLetter() == ' ' || tile.GetPoints() == 0;
            return isTileBlank;
        }

        public void MarkBonusSquareUsed(int x, int y)
        {
             _usedBonusSquaresThisTurn.Add(Tuple.Create(x, y));
        }

        public bool IsBonusSquarePreviouslyUsed(int x, int y)
        {
            var isBonusSquarePreviouslyUsed = _gameBoard.GetSquare(x, y).GetTile() != null;
            return isBonusSquarePreviouslyUsed;
        }

        public void CheckEndGameConditions()
        {

            bool playerEmptiedRack = _playerRacks.Values.Any(rack => !rack.Any());
            bool noTilesInBag = !_tileBag.GetTilesList().Any();

            if (playerEmptiedRack && noTilesInBag)
            {
                OnDisplayMessage?.Invoke("Game Over: A player emptied their rack and the tile bag is empty.");
                EndGame();
                return;
            }


            if (_consecutiveSkips >= _players.Count * 2) 
            {
                OnDisplayMessage?.Invoke($"Game Over: {_consecutiveSkips} consecutive skips.");
                EndGame();
                return;
            }

        }

        public void EndGame()
        {
            _currentState = GameState.Ended;
            OnDisplayMessage?.Invoke("The game has ended!");
            IPlayer? winner = GetWinner();
            if (winner != null)
            {
                OnDisplayMessage?.Invoke($"The winner is {winner.GetName()} with a score of {winner.GetScore()}!");
            }
            OnGameEvent?.Invoke("GameEnded", winner);
        }

        public bool ValidatePlayerCount()
        {
            var validatePlayerCount = _players.Count >= MIN_PLAYERS && _players.Count <= MAX_PLAYERS;
            return validatePlayerCount;
        }

        public bool IsPlayerNameUnique(string name)
        {
            var isPlayerNameUnique = !_players.Any(p => p.GetName().Equals(name, StringComparison.OrdinalIgnoreCase));
            return isPlayerNameUnique;
        }

        public TilePlacement? ParsePlayerInputToTilePlacement(string rawInput, IPlayer player)
        {
           
            try
            {
                string[] parts = rawInput.Split(',');
                if (parts.Length != 3)
                {
                    OnDisplayMessage?.Invoke("Invalid input format. Expected 'LETTER,ROW,COL'.");
                    return null;
                }

                char letter = parts[0].Trim().ToUpper()[0];
                int row = int.Parse(parts[1].Trim());
                int col = int.Parse(parts[2].Trim()); 

                Tile? tileToPlace = _playerRacks[player].FirstOrDefault(t => t.GetLetter() == letter);
                if (tileToPlace == null && letter != ' ') 
                {
                    tileToPlace = _playerRacks[player].FirstOrDefault(t => IsTileBlank(t));
                    if (tileToPlace != null && OnRequestBlankTileChar != null)
                    {

                        char specifiedBlankChar = OnRequestBlankTileChar.Invoke(tileToPlace);
                        tileToPlace.SetLetter(specifiedBlankChar);
                    }
                    else if (tileToPlace == null)
                    {
                        OnDisplayMessage?.Invoke($"Tile '{letter}' not found in your rack.");
                        return null;
                    }
                }

                if (tileToPlace == null)
                {
                    return null;
                }

                return new TilePlacement(tileToPlace, row, col);
            }
            catch (Exception ex)
            {
                OnDisplayMessage?.Invoke($"Error parsing input: {ex.Message}");
                return null;
            }
        }

    }

}
