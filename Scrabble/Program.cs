
// using System;
// using System.Collections.Generic;

// class Program
// {
//     static void Main(string[] args)
//     {
//         // Initialize game components
//         string path = "WordBank.txt";
//         IDictionary dictionary = new Dictionary(path);
//         ITileBag tileBag = new TileBag();
//         IBoard board = new Board();

//         // Create game controller
//         GameControl game = new GameControl(dictionary, tileBag, board);

//         // Set up event handlers
//         game.OnDisplayMessage += DisplayMessage;
//         game.OnRequestBlankTileChar += RequestBlankTileChar;
//         game.OnConfirmAction += ConfirmAction;
//         game.OnGameEvent += HandleGameEvent;

//         // Add players
//         Console.WriteLine("Scrabble Game Setup");
//         Console.WriteLine("-------------------");

//         int playerCount = GetIntInput("Enter number of players (2-4): ", 2, 4);

//         for (int i = 1; i <= playerCount; i++)
//         {
//             string name = GetStringInput($"Enter name for Player {i}: ");
//             game.AddPlayer(new Player(name));
//         }

//         // Start the game
//         game.StartGame();

//         // Main game loop
//         while (game._currentState == GameState.InProgress)
//         {
//             IPlayer? currentPlayer = game.GetCurrentPlayer();
//             Console.Clear();

//             // Display scores
//             Console.WriteLine("Player Scores:");
//             foreach (var player in game.GetAllPlayers())
//             {
//                 string currentIndicator = (player == currentPlayer) ? " (Current)" : "";
//                 Console.WriteLine($"{player.GetName()}: {player.GetScore()}{currentIndicator}");
//             }
//             Console.WriteLine();

//             // Display current player's rack
//             Console.WriteLine($"{currentPlayer!.GetName()}'s Tiles:");
//             var rack = game._playerRacks[currentPlayer];
//             for (int i = 0; i < rack.Count; i++)
//             {
//                 Console.Write($"{i}:{rack[i].GetLetter()}({rack[i].GetPoints()}) ");
//             }
//             Console.WriteLine("\n");

//             // Display board
//             ((Board)board).Display();

//             // Get player action
//             Console.WriteLine("\nChoose an action:");
//             Console.WriteLine("1. Place tiles");
//             Console.WriteLine("2. Swap tiles");
//             Console.WriteLine("3. Skip turn");
//             Console.WriteLine("4. Quit game");

//             int choice = GetIntInput("Enter your choice (1-4): ", 1, 4);

//             try
//             {
//                 switch (choice)
//                 {

//                     case 1: // Place tiles
//                         List<TilePlacement> placements = new List<TilePlacement>();
//                         bool placingTiles = true;

//                         while (placingTiles)
//                         {
//                             Console.Clear();

//                             // Display current state
//                             Console.WriteLine("Player Scores:");
//                             foreach (var player in game.GetAllPlayers())
//                             {
//                                 Console.WriteLine($"{player.GetName()}: {player.GetScore()}");
//                             }
//                             Console.WriteLine($"\n{currentPlayer.GetName()}'s Turn - Placing Tiles");

//                             // Display board with temporary placements
//                             Console.WriteLine("\nCurrent Board:");
//                             DisplayBoardWithPlacements(board, placements);

//                             // Display rack with available tiles
//                             Console.WriteLine("\nYour Tiles:");
//                             for (int i = 0; i < rack.Count; i++)
//                             {
//                                 var tile = rack[i];
//                                 // Cek apakah tile ini sudah dipindahkan ke board
//                                 bool isPlaced = placements.Exists(p => p.GetTile() == tile);

//                                 if (isPlaced)
//                                 {
//                                     Console.Write($"    "); // Kosongkan posisi tile yang sudah dipindahkan
//                                 }
//                                 else
//                                 {
//                                     Console.Write($"{i}:{tile.GetLetter()}({tile.GetPoints()}) ");
//                                 }
//                             }
//                             Console.WriteLine("\n");

//                             // Display placed tiles this turn
//                             if (placements.Count > 0)
//                             {
//                                 Console.WriteLine("Currently Placed Tiles This Turn:");
//                                 for (int i = 0; i < placements.Count; i++)
//                                 {
//                                     Console.WriteLine($"{i}: {placements[i].GetTile().GetLetter()} at ({placements[i].GetX()},{placements[i].GetY()})");
//                                 }
//                                 Console.WriteLine();
//                             }

//                             // Get placement input
//                             Console.WriteLine("Commands:");
//                             Console.WriteLine("- 'add tileIndex,row,col' to place a tile");
//                             Console.WriteLine("- 'move placementIndex,newRow,newCol' to move a placed tile");
//                             Console.WriteLine("- 'remove placementIndex' to remove a placed tile");
//                             Console.WriteLine("- 'done' to confirm placement");
//                             Console.WriteLine("- 'cancel' to cancel this turn");
//                             Console.Write("\nEnter command: ");

//                             string input = Console.ReadLine()?.Trim() ?? "";
//                             string[] parts = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

//                             try
//                             {
//                                 if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
//                                 {
//                                     if (placements.Count > 0)
//                                     {
//                                         MoveError error = game.PerformTurn(currentPlayer, placements);
//                                         if (error != MoveError.None)
//                                         {
//                                             Console.WriteLine($"Move error: {error}");
//                                             Console.WriteLine("Press any key to continue...");
//                                             Console.ReadKey();
//                                             continue;
//                                         }
//                                         placingTiles = false;
//                                     }
//                                     else
//                                     {
//                                         Console.WriteLine("No tiles placed. Please place at least one tile.");
//                                         Console.WriteLine("Press any key to continue...");
//                                         Console.ReadKey();
//                                     }
//                                 }
//                                 else if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
//                                 {
//                                     placements.Clear();
//                                     placingTiles = false;
//                                 }
//                                 else if (parts.Length > 0)
//                                 {
//                                     if (parts[0].Equals("add", StringComparison.OrdinalIgnoreCase) && parts.Length == 4)
//                                     {
//                                         // Add new tile placement
//                                         int tileIndex = int.Parse(parts[1]);
//                                         int row = int.Parse(parts[2]);
//                                         int col = int.Parse(parts[3]);

//                                         if (tileIndex < 0 || tileIndex >= rack.Count)
//                                             throw new ArgumentException("Invalid tile index");

//                                         if (row < 0 || row >= 15 || col < 0 || col >= 15)
//                                             throw new ArgumentException("Invalid coordinates (must be 0-14)");

//                                         // Check if tile is already placed
//                                         if (placements.Exists(p => p.GetTile() == rack[tileIndex]))
//                                             throw new ArgumentException("Tile already placed this turn");

//                                         // Check if position is already occupied
//                                         if (placements.Exists(p => p.GetX() == row && p.GetY() == col) ||
//                                         !game.IsSquareEmptyOnBoard(row, col))
//                                             throw new ArgumentException("Position already occupied");

//                                         placements.Add(new TilePlacement(rack[tileIndex], row, col));
//                                     }
//                                     else if (parts[0].Equals("move", StringComparison.OrdinalIgnoreCase) && parts.Length == 4)
//                                     {
//                                         // Move existing placement
//                                         int placementIndex = int.Parse(parts[1]);
//                                         int newRow = int.Parse(parts[2]);
//                                         int newCol = int.Parse(parts[3]);

//                                         if (placementIndex < 0 || placementIndex >= placements.Count)
//                                             throw new ArgumentException("Invalid placement index");

//                                         if (newRow < 0 || newRow >= 15 || newCol < 0 || newCol >= 15)
//                                             throw new ArgumentException("Invalid coordinates (must be 0-14)");

//                                         // Check if new position is already occupied
//                                         if (placements.Where((p, idx) => idx != placementIndex)
//                                                     .Any(p => p.GetX() == newRow && p.GetY() == newCol) ||
//                                         !game.IsSquareEmptyOnBoard(newRow, newCol))
//                                             throw new ArgumentException("Position already occupied");

//                                         // Update the placement
//                                         placements[placementIndex] = new TilePlacement(
//                                             placements[placementIndex].GetTile(),
//                                             newRow,
//                                             newCol);
//                                     }
//                                     else if (parts[0].Equals("remove", StringComparison.OrdinalIgnoreCase) && parts.Length == 2)
//                                     {
//                                         // Remove placement
//                                         int placementIndex = int.Parse(parts[1]);
//                                         if (placementIndex < 0 || placementIndex >= placements.Count)
//                                             throw new ArgumentException("Invalid placement index");

//                                         placements.RemoveAt(placementIndex);
//                                     }
//                                     else
//                                     {
//                                         throw new ArgumentException("Invalid command format");
//                                     }
//                                 }
//                             }
//                             catch (Exception ex)
//                             {
//                                 Console.WriteLine($"Error: {ex.Message}");
//                                 Console.WriteLine("Press any key to continue...");
//                                 Console.ReadKey();
//                             }
//                         }
//                         break;
//                     case 2: // Swap tiles
//                         Console.WriteLine("\nYour Tiles:");
//                         for (int i = 0; i < rack.Count; i++)
//                         {
//                             Console.Write($"{i}:{rack[i].GetLetter()}({rack[i].GetPoints()}) ");
//                         }
//                         Console.WriteLine("\n");

//                         Console.WriteLine("Enter tile indices to swap (separated by spaces):");
//                         string swapInput = Console.ReadLine() ?? "";
//                         string[] swapIndices = swapInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                         List<Tile> tilesToSwap = new List<Tile>();
//                         foreach (string indexStr in swapIndices)
//                         {
//                             if (int.TryParse(indexStr, out int index) && index >= 0 && index < rack.Count)
//                             {
//                                 tilesToSwap.Add(rack[index]);
//                             }
//                         }

//                         if (tilesToSwap.Count > 0)
//                         {
//                             MoveError error = game.SwapTiles(currentPlayer, tilesToSwap);
//                             if (error != MoveError.None)
//                             {
//                                 Console.WriteLine($"Swap error: {error}");
//                                 Console.WriteLine("Press any key to continue...");
//                                 Console.ReadKey();
//                             }
//                         }
//                         break;

//                     case 3: // Skip turn
//                         game.SkipTurn(currentPlayer);
//                         break;

//                     case 4: // Quit game
//                         Console.WriteLine("Are you sure you want to quit? (y/n)");
//                         if (Console.ReadLine()?.Trim().ToLower() == "y")
//                         {
//                             game.EndGame();
//                             return;
//                         }
//                         break;
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error: {ex.Message}");
//                 Console.WriteLine("Press any key to continue...");
//                 Console.ReadKey();
//             }
//         }

//         // Game over
//         Console.Clear();
//         IPlayer? winner = game.GetWinner();
//         Console.WriteLine($"Game Over! Winner: {winner?.GetName()} with {winner?.GetScore()} points");
//         Console.WriteLine("\nFinal Scores:");
//         foreach (var player in game.GetAllPlayers())
//         {
//             Console.WriteLine($"{player.GetName()}: {player.GetScore()}");
//         }
//     }

//     // Helper method to display board with temporary placements
//     static void DisplayBoardWithPlacements(IBoard board, List<TilePlacement> placements)
//     {
//         // Create a temporary copy of the board to show placements
//         Square[,] tempGrid = (Square[,])board.GetGrid().Clone();

//         // Apply temporary placements
//         foreach (var placement in placements)
//         {
//             int x = placement.GetX();
//             int y = placement.GetY();
//             tempGrid[x, y] = new Square(x, y, tempGrid[x, y].GetBonusType());
//             tempGrid[x, y].SetTile(placement.GetTile());
//         }

//         // Display the temporary board
//         Console.Write("   ");
//         for (int x = 0; x < 15; x++)
//         {
//             Console.Write($"{x:D2} ");
//         }
//         Console.WriteLine();

//         for (int y = 0; y < 15; y++)
//         {
//             Console.Write($"{y:D2} ");
//             for (int x = 0; x < 15; x++)
//             {
//                 var square = tempGrid[x, y];
//                 var tile = square.GetTile();

//                 if (tile != null)
//                 {
//                     Console.Write($" {tile.GetLetter()} ");
//                 }
//                 else
//                 {
//                     // Special handling for center square
//                     if (x == 7 && y == 7 && square.GetBonusType() == BonusSquareType.DoubleWord)
//                     {
//                         Console.Write(" ★ "); // Star symbol for center
//                     }
//                     else
//                     {
//                         Console.Write($" {GetBonusSymbol(square.GetBonusType())} ");
//                     }
//                 }
//             }
//             Console.WriteLine();
//         }
//     }

//     // Helper to get bonus symbol
//     static char GetBonusSymbol(BonusSquareType type)
//     {
//         return type switch
//         {
//             BonusSquareType.TripleWord => '3',
//             BonusSquareType.DoubleWord => '2',
//             BonusSquareType.TripleLetter => '#',
//             BonusSquareType.DoubleLetter => '+',
//             BonusSquareType.Normal => '.',
//             _ => '?'
//         };
//     }

//     // Event handler implementations
//     static void DisplayMessage(string message)
//     {
//         Console.WriteLine(message);
//     }

//     static char RequestBlankTileChar(Tile tile)
//     {
//         Console.Write("Enter letter for blank tile: ");
//         char ch = Console.ReadKey().KeyChar;
//         Console.WriteLine();
//         return char.ToUpper(ch);
//     }

//     static bool ConfirmAction(string prompt)
//     {
//         Console.Write($"{prompt} (y/n): ");
//         return Console.ReadLine()?.Trim().ToLower() == "y";
//     }

//     static void HandleGameEvent(string eventName, object? data)
//     {
//         Console.WriteLine($"Game event: {eventName}");
//     }

//     // Helper methods
//     static int GetIntInput(string prompt, int min, int max)
//     {
//         while (true)
//         {
//             Console.Write(prompt);
//             if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
//             {
//                 return result;
//             }
//             Console.WriteLine($"Please enter a number between {min} and {max}");
//         }
//     }

//     static string GetStringInput(string prompt)
//     {
//         Console.Write(prompt);
//         return Console.ReadLine()?.Trim() ?? "";
//     }
// }



string path = "WordBank.txt";
var dictionary = new Dictionary(path);
var  board = new Board();
var tileBag = new TileBag();
var game = new GameControl(dictionary, tileBag, board);
Console.WriteLine(game._gameBoard);


