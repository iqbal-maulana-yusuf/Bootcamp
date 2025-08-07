public class GameControl
{
    private List<IPlayer> _players;
    public Dictionary<IPlayer, List<Tile>> _playerRacks;
    private int _activePlayerIndex;
    public IBoard _gameBoard;
    private ITileBag _tileBag;
    private IDictionary _dictionary;
    public GameState _currentState;
    private int _consecutiveSkips;
    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 4;
    private HashSet<Tuple<int, int>> _usedBonusSquaresThisTurn;
    private Random _randomGenerator;

    // Events
    public event Func<Tile, char>? OnRequestBlankTileChar;
    public event Action<string>? OnDisplayMessage;
    public event Func<string, bool>? OnConfirmAction;
    public event Action<string, object?>? OnGameEvent;
    public event Func<string, string>? OnGetUserInput;

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
        OnDisplayMessage?.Invoke($"Player '{player.GetName()}' added");
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

    public IPlayer? GetCurrentPlayer()
    {
        if (_players.Any() && _currentState == GameState.InProgress)
        {
            return _players[_activePlayerIndex];
        }
        return null;
    }

    public List<IPlayer> GetAllPlayers()
    {
        return new List<IPlayer>(_players);
    }

    public IPlayer? GetWinner()
    {
        if (_currentState != GameState.Ended || !_players.Any())
        {
            return null;
        }
        return _players.OrderByDescending(p => p.GetScore()).FirstOrDefault();
    }

    public MoveError PerformTurn(IPlayer player, List<TilePlacement> placements)
    {
        if (_currentState != GameState.InProgress)
        {
            return MoveError.GameAlreadyStarted; // Re-use for "game not in progress"
        }
        if (player != GetCurrentPlayer())
        {
            OnDisplayMessage?.Invoke("It's not your turn.");
            return MoveError.InvalidPlacement; // Generic error, could be more specific
        }

        // Reset used bonus squares for this turn
        _usedBonusSquaresThisTurn.Clear();

        // 1. Validasi penempatan
        MoveError placementError = IsPlacementValid(placements);
        if (placementError != MoveError.None)
        {
            return placementError;
        }

        // 2. Cek apakah tile ada di rak pemain
        List<Tile> tilesUsed = placements.Select(p => p.GetTile()).ToList();
        foreach (var tile in tilesUsed)
        {
            if (!CheckTileInPlayerRack(player, tile))
            {
                OnDisplayMessage?.Invoke($"Tile '{tile.GetLetter()}' not in your rack.");
                return MoveError.TileNotInRack;
            }
        }

        // Jika ada blank tile, minta input huruf yang diinginkan
        foreach (var placement in placements)
        {
            if (IsTileBlank(placement.GetTile()))
            {
                char chosenChar = OnRequestBlankTileChar?.Invoke(placement.GetTile()) ?? '_';
                placement.GetTile().SetLetter(chosenChar);
            }
        }

        // 3. Temporarily place tiles on board to form words
        // This is a simplified approach; a more robust solution might use a temporary board state
        // or validate words without actual placement first.
        foreach (var placement in placements)
        {
            _gameBoard.GetSquare(placement.GetX(), placement.GetY()).SetTile(placement.GetTile());
        }

        // 4. Dapatkan semua kata yang terbentuk dan validasi kamus
        List<string> formedWords = GetAllWordsFromPlacement(placements);
        foreach (var word in formedWords)
        {
            if (!IsWordInDictionary(word))
            {
                // Remove temporarily placed tiles if words are invalid
                foreach (var placement in placements)
                {
                    _gameBoard.GetSquare(placement.GetX(), placement.GetY()).SetTile(null!);
                }
                OnDisplayMessage?.Invoke($"Word '{word}' is not in the dictionary.");
                return MoveError.WordNotInDictionary;
            }
        }

        // 5. Hitung skor
        int score = CalculateWordScore(formedWords, placements);
        player.AddScore(score);
        OnDisplayMessage?.Invoke($"Player '{player.GetName()}' scored {score} points.");

        // 6. Hapus tile yang digunakan dari rak pemain
        foreach (var tile in tilesUsed)
        {
            _playerRacks[player].Remove(tile);
        }

        // 7. Isi ulang rak pemain
        RefillPlayerRack(player);

        _consecutiveSkips = 0; // Reset skip count on successful move
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
        if (tilesToSwap == null || !tilesToSwap.Any())
        {
            return MoveError.InvalidTilesToSwap;
        }

        // yang ini sepertinya tidak terlalu perlu
        if (tilesToSwap.Count > _playerRacks[player].Count)
        {
            return MoveError.TooManyTilesToSwap;
        }

        if (_tileBag.GetTilesList().Count < tilesToSwap.Count)
        {
            OnDisplayMessage?.Invoke("Not enough tiles in the bag to swap.");
            return MoveError.TooManyTilesToSwap; // Re-use error for now
        }

        // Cek apakah semua tile yang akan ditukar ada di rak pemain
        List<Tile> playerTiles = _playerRacks[player];
        foreach (var tile in tilesToSwap)
        {
            if (!playerTiles.Contains(tile))
            {
                return MoveError.TileNotInRack;
            }
        }

        // Hapus tile dari rak pemain
        foreach (var tile in tilesToSwap)
        {
            playerTiles.Remove(tile);
        }

        // Kembalikan tile ke dalam tas
        ReturnTilesToBag(tilesToSwap);

        // Ambil tile baru dari tas
        List<Tile> newTiles = DrawTilesFromBag(tilesToSwap.Count);
        playerTiles.AddRange(newTiles);

        _consecutiveSkips = 0; // Swapping is considered a turn, so reset skips
        OnDisplayMessage?.Invoke($"Player '{player.GetName()}' swapped {tilesToSwap.Count} tiles.");
        OnGameEvent?.Invoke("TilesSwapped", player);
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

        // Calculate score for each word formed
        foreach (string word in words)
        {
            int currentWordScore = 0;
            int wordMultiplier = 1;

            // Determine the direction of the word (assuming a single direction for now)
            Direction wordDirection;
            if (placements.Count == 1) // Single tile placement
            {
                // A single tile forms a word in both directions (if connected)
                // We need to calculate score based on how it connects horizontally and vertically
                // This is complex and might need specific logic for single tile placement.
                // For simplicity here, we assume it's part of a larger word or forms only a single-letter word.
                wordDirection = Direction.Horizontal; // Default or determined by surrounding tiles
            }
            else
            {
                wordDirection = placements.First().GetX() == placements.Last().GetX() ? Direction.Horizontal : Direction.Vertical;
            }

            // Get all squares involved in the word, including existing tiles
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
            else // Vertical
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
                        // Mark bonus square as used for this turn
                        MarkBonusSquareUsed(square.GetX(), square.GetY());
                    }
                    currentWordScore += tilePoints;
                }
            }
            totalScore += (currentWordScore * wordMultiplier);
        }

        // Scrabble bonus for using all 7 tiles (bingo)
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
            return MoveError.InvalidPlacement;
        }

        // Check if all coordinates are valid and squares are empty
        foreach (var placement in placements)
        {
            if (!IsCoordinateValid(placement.GetX(), placement.GetY()))
            {
                return MoveError.InvalidCoordinates;
            }
            if (!IsSquareEmptyOnBoard(placement.GetX(), placement.GetY()))
            {
                return MoveError.InvalidPlacement; // Square already occupied
            }
        }

        // Sort placements for easier processing
        placements = placements.OrderBy(p => p.GetX()).ThenBy(p => p.GetY()).ToList();

        // Check if placements are consecutive and in a single line (horizontal or vertical)
        bool isHorizontal = placements.All(p => p.GetX() == placements.First().GetX());
        bool isVertical = placements.All(p => p.GetY() == placements.First().GetY());

        if (!isHorizontal && !isVertical)
        {
            return MoveError.InvalidPlacement; // Not in a single line
        }

        if (placements.Count > 1)
        {
            if (!AreConsecutivePlacements(placements))
            {
                return MoveError.InvalidPlacement; // Tiles are not consecutive (gaps)
            }
        }

        // First move special rule: Must pass through center square (7,7)
        if (_currentState == GameState.NotStarted || !_gameBoard.GetSquare(7, 7).GetTile()?.GetLetter().ToString()
                                                 .Any(char.IsLetterOrDigit) == true) // Check if center is truly empty (no existing tile from previous game state)
        {
            if (!IsValidFirstMove(placements))
            {
                return MoveError.InvalidFirstMove;
            }
        }
        else // Subsequent moves must connect to existing tiles
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
        // For the first move, the placement must include the center square (7,7)
        return placements.Any(p => p.GetX() == 7 && p.GetY() == 7);
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
                // Check if the current square is part of the new placement OR has an existing tile
                bool isCoveredByPlacement = placements.Any(p => p.GetX() == row && p.GetY() == y);
                bool hasExistingTile = _gameBoard.GetSquare(row, y).GetTile() != null;

                if (!isCoveredByPlacement && !hasExistingTile)
                {
                    return false; // Gap in the middle
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
                    return false; // Gap in the middle
                }
            }
        }
        else
        {
            return false; // Not a single line
        }

        return true;
    }

    public bool ConnectsToExistingTiles(List<TilePlacement> placements)
    {
        // For subsequent moves, at least one new tile must be adjacent (horizontally or vertically)
        // to an existing tile on the board, OR fill a gap between existing tiles.

        // First, check if any new tile touches an existing tile
        foreach (var newPlacement in placements)
        {
            int x = newPlacement.GetX();
            int y = newPlacement.GetY();

            // Check neighbors
            if ((x > 0 && _gameBoard.GetSquare(x - 1, y).GetTile() != null) || // Top
                (x < 14 && _gameBoard.GetSquare(x + 1, y).GetTile() != null) || // Bottom
                (y > 0 && _gameBoard.GetSquare(x, y - 1).GetTile() != null) || // Left
                (y < 14 && _gameBoard.GetSquare(x, y + 1).GetTile() != null))   // Right
            {
                // Ensure the neighbor isn't also one of the tiles being placed in this turn
                // This is important to avoid false positives when placing multiple tiles next to each other
                bool isNeighborAlsoNewPlacement = placements.Any(p =>
                    (p.GetX() == x - 1 && p.GetY() == y) ||
                    (p.GetX() == x + 1 && p.GetY() == y) ||
                    (p.GetX() == x && p.GetY() == y - 1) ||
                    (p.GetX() == x && p.GetY() == y + 1));

                if (!isNeighborAlsoNewPlacement)
                {
                    return true; // Found a connection to an existing tile
                }
            }
        }

        // Second, check if the new placement spans between two existing tiles.
        // This is already partially handled by AreConsecutivePlacements if it includes existing tiles.
        // If AreConsecutivePlacements returns true, and the placements are connected to *something* (either existing or other new tiles),
        // and at least one existing tile is part of the formed line, then it's valid.

        // A simpler check: if after placing the new tiles, the entire line formed (new + existing) is continuous,
        // and at least one *original* tile from the board is part of that line.

        // Create a set of all coordinates involved in the potential new word
        HashSet<Tuple<int, int>> involvedCoords = new HashSet<Tuple<int, int>>();
        foreach (var p in placements)
        {
            involvedCoords.Add(Tuple.Create(p.GetX(), p.GetY()));
        }

        // Determine if horizontal or vertical
        bool isHorizontal = placements.All(p => p.GetX() == placements.First().GetX());

        if (placements.Count == 1) // Special case for single tile placement
        {
            int x = placements[0].GetX();
            int y = placements[0].GetY();
            // Check all 4 direct neighbors
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

            // Expand to find the full word horizontally
            int wordStartCol = FindWordStartOnBoard(row, minCol, Direction.Horizontal);
            int wordEndCol = FindWordEndOnBoard(row, maxCol, Direction.Horizontal);

            for (int col = wordStartCol; col <= wordEndCol; col++)
            {
                if (_gameBoard.GetSquare(row, col).GetTile() != null &&
                    !placements.Any(p => p.GetX() == row && p.GetY() == col))
                {
                    return true; // Found an existing tile in the formed horizontal word
                }
            }
        }
        else // Vertical
        {
            int col = placements.First().GetY();
            int minRow = placements.Min(p => p.GetX());
            int maxRow = placements.Max(p => p.GetX());

            // Expand to find the full word vertically
            int wordStartRow = FindWordStartOnBoard(minRow, col, Direction.Vertical);
            int wordEndRow = FindWordEndOnBoard(maxRow, col, Direction.Vertical);

            for (int row = wordStartRow; row <= wordEndRow; row++)
            {
                if (_gameBoard.GetSquare(row, col).GetTile() != null &&
                    !placements.Any(p => p.GetX() == row && p.GetY() == col))
                {
                    return true; // Found an existing tile in the formed vertical word
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
        else // Vertical
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

        // Determine if horizontal or vertical placement
        bool isHorizontalPlacement = placements.All(p => p.GetX() == placements.First().GetX());
        bool isVerticalPlacement = placements.All(p => p.GetY() == placements.First().GetY());

        if (isHorizontalPlacement)
        {
            // Main horizontal word
            int row = placements.First().GetX();
            int startCol = FindWordStartOnBoard(row, placements.Min(p => p.GetY()), Direction.Horizontal);
            string mainWord = GetWordFromBoard(row, startCol, Direction.Horizontal);
            if (mainWord.Length > 1) // A single tile forming a word of length 1 is not a "new" word unless it creates a side word.
            {
                formedWords.Add(mainWord);
            }

            // Check for vertical words formed by each newly placed tile
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
            // Main vertical word
            int col = placements.First().GetY();
            int startRow = FindWordStartOnBoard(placements.Min(p => p.GetX()), col, Direction.Vertical);
            string mainWord = GetWordFromBoard(startRow, col, Direction.Vertical);
            if (mainWord.Length > 1)
            {
                formedWords.Add(mainWord);
            }

            // Check for horizontal words formed by each newly placed tile
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
        else // This case should ideally be caught by IsPlacementValid, but as a fallback for single tile
        {
            // If only one tile is placed, it can form both horizontal and vertical words.
            if (placements.Count == 1)
            {
                var placement = placements.First();
                int x = placement.GetX();
                int y = placement.GetY();

                // Check horizontal word
                int startCol = FindWordStartOnBoard(x, y, Direction.Horizontal);
                string horizWord = GetWordFromBoard(x, startCol, Direction.Horizontal);
                if (horizWord.Length > 1)
                {
                    formedWords.Add(horizWord);
                }

                // Check vertical word
                int startRow = FindWordStartOnBoard(x, y, Direction.Vertical);
                string vertWord = GetWordFromBoard(startRow, y, Direction.Vertical);
                if (vertWord.Length > 1)
                {
                    formedWords.Add(vertWord);
                }
            }
            // For scattered tiles (not a single line), they should have been caught by IsPlacementValid
            // If we reach here, and it's multiple tiles not in a line, it's an invalid scenario.
        }

        // Filter out duplicate words (e.g., if a word is formed both horizontally and vertically through a single intersection, though uncommon)
        // And also words that are just a single letter, unless it's the only word formed and it's part of a valid play.
        return formedWords.Distinct().ToList();
    }

    public bool CheckTileInPlayerRack(IPlayer player, Tile tile)
    {
        return _playerRacks[player].Contains(tile);
    }

    public bool IsWordInDictionary(string word)
    {
        return _dictionary.GetAllWords().Contains(word.ToUpper());
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
        List<Tile> availableTiles = _tileBag.GetTilesList(); // Get reference to the actual list

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
        else // Vertical
        {
            while (row > 0 && _gameBoard.GetSquare(row - 1, col).GetTile() != null)
            {
                row--;
            }
            return row;
        }
    }

    // Helper to find word end (useful for full word extraction)
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
        else // Vertical
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
        return x >= 0 && x < 15 && y >= 0 && y < 15;
    }

    public bool IsSquareEmptyOnBoard(int x, int inty)
    {
        return _gameBoard.GetSquare(x, inty).GetTile() == null;
    }

    public bool IsTileBlank(Tile tile)
    {
        return tile.GetLetter() == ' ' || tile.GetPoints() == 0;
    }

    public void MarkBonusSquareUsed(int x, int y)
    {
        _usedBonusSquaresThisTurn.Add(Tuple.Create(x, y));
    }

    public bool IsBonusSquarePreviouslyUsed(int x, int y)
    {
        // This method checks if the bonus square was used in the current turn for scoring.
        // The actual bonus effect applies only on the first use of the square with a tile.
        // For a persistent game, you might need to store used bonus squares on the board itself.
        // For now, this is primarily for `CalculateWordScore`.
        return _gameBoard.GetSquare(x, y).GetTile() != null; // A square is "used" once a tile is placed on it
    }

    public void CheckEndGameConditions()
    {
        // Condition 1: One player has emptied their rack and no tiles left in the bag
        bool playerEmptiedRack = _playerRacks.Values.Any(rack => !rack.Any());
        bool noTilesInBag = !_tileBag.GetTilesList().Any();

        if (playerEmptiedRack && noTilesInBag)
        {
            OnDisplayMessage?.Invoke("Game Over: A player emptied their rack and the tile bag is empty.");
            EndGame();
            return;
        }

        // Condition 2: All players consecutively skip two turns (or three players in 3-4 player game, etc.)
        // Or more generally, if a certain number of consecutive skips occur (e.g., 2 consecutive skips per player, or 6 total skips for 3 players)
        // The current implementation is simpler: if a player skips, _consecutiveSkips increases.
        // Let's assume a threshold like 3-4 consecutive skips from *any* player for simplicity
        if (_consecutiveSkips >= _players.Count * 2) // Example: 2 skips per player in a round
        {
            OnDisplayMessage?.Invoke($"Game Over: {_consecutiveSkips} consecutive skips.");
            EndGame();
            return;
        }

        // Additional conditions:
        // - No valid moves left for any player (hard to check proactively)
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
        return _players.Count >= MIN_PLAYERS && _players.Count <= MAX_PLAYERS;
    }

    public bool IsPlayerNameUnique(string name)
    {
        return !_players.Any(p => p.GetName().Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public TilePlacement? ParsePlayerInputToTilePlacement(string rawInput, IPlayer player)
    {
        // Contoh format input: "A8C" (Tile A, baris 8, kolom C)
        // Ini adalah placeholder dan perlu implementasi lebih lanjut berdasarkan format input yang Anda inginkan
        // Contoh sederhana: "A,7,7" untuk Tile A di (7,7)
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
            int col = int.Parse(parts[2].Trim()); // Assuming 'col' is numeric, e.g., 0-14. If A-O, conversion needed.

            // Cari tile di rak pemain
            Tile? tileToPlace = _playerRacks[player].FirstOrDefault(t => t.GetLetter() == letter);
            if (tileToPlace == null && letter != ' ') // Allow blank tile if explicitly requested
            {
                tileToPlace = _playerRacks[player].FirstOrDefault(t => IsTileBlank(t));
                if (tileToPlace != null && OnRequestBlankTileChar != null)
                {
                    // If it's a blank tile, the user must specify what letter it represents for this move
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
