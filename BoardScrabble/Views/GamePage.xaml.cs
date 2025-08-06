using BoardScrabble.Controller;
using BoardScrabble.Views;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BoardScrabble
{
    public partial class GamePage : Page
    {
        private TextBlock? _draggedTile = null;
        private Point _mouseOffset;
        private Border? _potentialDropTarget = null;
        private UIElement? _originalContainer = null;
        private bool _dragStartedFromRack = false;
        private int _draggedTileInitialRackIndex = -1;
        private string _draggedTileInitialRackText = string.Empty;
        private Brush? _draggedTileInitialRackBackground = null;
        private Dictionary<object, (int, int)> _tilePosition = new();
        public List<Tile> _tileRack = new();
        public List<TextBlock> _objectTileRack = new();
        private GameControl _game;
        private IPlayer _currentPlayer;
        
       

        public GamePage(GameControl game)
        {
            _game = game;
            InitializeComponent();
            StartGame();
            GenerateBoard();
            GenerateRack(_tileRack);



        }

        public void StartGame()
        {
            _currentPlayer = _game._players[_game._activePlayerIndex];
            playerName.Text = _currentPlayer.GetName();
            playerScore.Text = _currentPlayer.GetScore().ToString();
            foreach (var tile in _game._playerRacks[_currentPlayer])
            {
                _tileRack.Add(tile);
            }
            System.Diagnostics.Debug.WriteLine("Checkpoint 5");
        }


        public void GenerateRack(List<Tile> _tileRack)
        {
            int col = 7;
            TilePanel.Children.Clear();
            _objectTileRack.Clear();

            System.Diagnostics.Debug.WriteLine("Checkpoint 6");

            for (int i = 0; i < col; i++)
            {
                var tileInRack = new TextBlock
                {
                    Name = $"Tile_{i}",
                    Text = _tileRack[i].GetLetter().ToString(),
                    ToolTip = _tileRack[i].GetLetter().ToString(),
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Width = 30,
                    Height = 30,
                    Padding = new Thickness(3),
                    Background = (Brush)new BrushConverter().ConvertFromString("#ffda9e")!,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Hand,
                    Tag = i,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                _objectTileRack.Add(tileInRack);
                tileInRack.MouseLeftButtonDown += Tile_MouseLeftButtonDown;
                TilePanel.Children.Add(tileInRack);
            }
            System.Diagnostics.Debug.WriteLine("Checkpoint 7");
        }

        private void GenerateBoard()
        {
            int size = 15;

            for (int i = 0; i < size; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            }

            var tripleWord = new List<(int x, int y)>
            {
                (0, 0), (0, 7), (0, 14),
                (7, 0), (7, 14),
                (14, 0), (14, 7), (14, 14)
            };

            var doubleWord = new List<(int x, int y)>
            {
                (1,1), (2,2), (3,3), (4,4),
                (10,10), (11,11), (12,12), (13,13), (7,7),
                (10,4), (11,3), (12,2), (13,1), (4,10), 
                (3,11), (2,12),(1,13)

            };

            var doubleLetter = new List<(int x, int y)>
            {
                (0,3), (0,11), (2,6), (2,8), (3,0), (3,7),
                (3,14), (6,2), (6,6), (6,8), (6,12), (7,3),
                (7,11), (8,2), (8,6), (8,8), (8,12), (11,0),
                (11,7), (11,14), (12,6), (12,8), (14,3), (14,11)
            };


            var tripleLetter = new List<(int x, int y)>
            {
                (1,5), (1,9), (5,1), (5,5), (5,9), (5,13),
                (9,1), (9,5), (9,9), (9,13), (13,5), (13,9)
            };



            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var border = new Border
                    {
                        Background = (Brush)new BrushConverter().ConvertFromString("#c4c4d1")!,
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(4),
                        Margin = new Thickness(1)
                    };

                    if (tripleWord.Contains((row, col)))
                    {
                        border.Background = (Brush)new BrushConverter().ConvertFromString("#c04d4d")!;
                    }
                    if (doubleWord.Contains((row, col)))
                    {
                        border.Background = (Brush)new BrushConverter().ConvertFromString("#e8a4a4")!;
                    }
                    if (tripleLetter.Contains((row, col)))
                    {
                        border.Background = (Brush)new BrushConverter().ConvertFromString("#0c679c")!;
                    }
                    if (doubleLetter.Contains((row, col)))
                    {
                        border.Background = (Brush)new BrushConverter().ConvertFromString("#68a2c3")!;
                    }

                    if (row == 7 && col == 7)
                    {
                        border.Background = Brushes.Gold;
                    }

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    BoardGrid.Children.Add(border);
                }
            }
        }

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock currentTileInRack)
            {
                _draggedTileInitialRackIndex = int.Parse(currentTileInRack.Tag.ToString());
                _draggedTileInitialRackText = currentTileInRack.Text;
                _draggedTileInitialRackBackground = currentTileInRack.Background;
                if (currentTileInRack.Text == " ")
                {
                    // ✅ Buat page input huruf dan daftarkan event handler-nya
                    var blankTilePage = new BlankTileInput(_game, this, _draggedTileInitialRackIndex);
                    blankTilePage.OnLetterSelected += (selectedChar, index) =>
                    {
                        _game.FillTileBlank(index, selectedChar);
                        StartGame();
                        GenerateRack(_tileRack); 

                    };

                    NavigationService?.Navigate(blankTilePage);
                    return;
                }
                _originalContainer = VisualTreeHelper.GetParent(currentTileInRack) as UIElement;
                _dragStartedFromRack = true;
                _mouseOffset = e.GetPosition(currentTileInRack);
                _draggedTile = CreateCloneTile(currentTileInRack);
                PositionDraggedTile(e.GetPosition(FloatingCanvas));
                FloatingCanvas.Children.Add(_draggedTile);
                currentTileInRack.Text = "";
                currentTileInRack.Background = Brushes.LightGray;
                currentTileInRack.Visibility = Visibility.Visible;
                Mouse.Capture(this);
            }
        }


        private void Tile_OnBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock currentTileOnBoard)
            {
                _originalContainer = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(currentTileOnBoard)) as UIElement;
                _dragStartedFromRack = false;

                if (currentTileOnBoard.Tag != null && int.TryParse(currentTileOnBoard.Tag.ToString(), out int originalRackIndex))
                {
                    _draggedTileInitialRackIndex = originalRackIndex;
                }
                else
                {
                    _draggedTileInitialRackIndex = -1;
                }

                _mouseOffset = e.GetPosition(currentTileOnBoard);
                _draggedTile = CreateCloneTile(currentTileOnBoard);
                PositionDraggedTile(e.GetPosition(FloatingCanvas));
                FloatingCanvas.Children.Add(_draggedTile);

                if (_originalContainer is Border border)
                {
                    border.Child = null;
                }

                Mouse.Capture(this);
            }
        }

        private void PositionDraggedTile(Point position)
        {
            Canvas.SetLeft(_draggedTile, position.X - _mouseOffset.X);
            Canvas.SetTop(_draggedTile, position.Y - _mouseOffset.Y);
        }

        private TextBlock CreateCloneTile(TextBlock source)
        {
            return new TextBlock
            {
                Text = source.Text,
                FontSize = source.FontSize,
                Padding = source.Padding,
                Background = source.Background,
                Foreground = source.Foreground,
                FontWeight = source.FontWeight,
                Tag = source.Tag,
                Cursor = Cursors.Hand,
                Opacity = 0.9,
                TextAlignment = TextAlignment.Center,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 10,
                    ShadowDepth = 3
                }
            };
        }

        private void GamePage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedTile != null)
            {
                PositionDraggedTile(e.GetPosition(FloatingCanvas));

                var hit = VisualTreeHelper.HitTest(BoardGrid, e.GetPosition(BoardGrid));
                var border = FindParent<Border>(hit?.VisualHit);

                if (_potentialDropTarget != null && _potentialDropTarget != border)
                {
                    ResetBorderAppearance(_potentialDropTarget);
                }

                if (border != null && BoardGrid.Children.Contains(border) && border.Child == null)
                {
                    border.BorderBrush = Brushes.DodgerBlue;
                    border.BorderThickness = new Thickness(2);
                    _potentialDropTarget = border;
                }
            }
        }

        private void GamePage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggedTile == null) return;

            try
            {
                bool placedOnBoard = false;
                var pointOnBoard = e.GetPosition(BoardGrid);
                var hitBoard = VisualTreeHelper.HitTest(BoardGrid, pointOnBoard);
                var targetBorder = FindParent<Border>(hitBoard?.VisualHit);

                bool isValidBoardDropTarget = targetBorder != null && BoardGrid.Children.Contains(targetBorder) && targetBorder.Child == null;

                if (isValidBoardDropTarget)
                {
                    int row = Grid.GetRow(targetBorder);
                    int col = Grid.GetColumn(targetBorder);
                    var newTileForBoard = CreateCloneTile(_draggedTile);
                    newTileForBoard.Opacity = 1.0;
                    newTileForBoard.MouseLeftButtonDown += Tile_OnBoard_MouseLeftButtonDown;

                    targetBorder.Child = new Grid { Children = { newTileForBoard } };
                    placedOnBoard = true;

                    _tilePosition[newTileForBoard.Tag] = (row, col);
                    
                }

                if (!placedOnBoard && _draggedTileInitialRackIndex != -1)
                {
                    if (_draggedTile.Tag != null && _tilePosition.TryGetValue(_draggedTile.Tag, out var position))
                    {
                        int row = position.Item1;
                        int col = position.Item2;
                        _tilePosition.Remove(_draggedTile.Tag);
                    }
                    ReturnTileToSpecificRackSlot(_draggedTile, _draggedTileInitialRackIndex);
                }
            }
            finally
            {
                CleanUpDrag();
            }
        }

        private void ReturnTileToSpecificRackSlot(TextBlock tileToReturn, int targetIndex)
        {
            if (targetIndex >= 0 && targetIndex < TilePanel.Children.Count)
            {
                if (TilePanel.Children[targetIndex] is TextBlock targetSlot && string.IsNullOrEmpty(targetSlot.Text) &&
                    (targetSlot.Background as SolidColorBrush)?.Color == Colors.LightGray)
                {
                    targetSlot.Text = tileToReturn.Text;
                    targetSlot.Background = tileToReturn.Background;
                    targetSlot.Visibility = Visibility.Visible;
                }
            }
        }

        private void CleanUpDrag()
        {
            if (_potentialDropTarget != null)
            {
                ResetBorderAppearance(_potentialDropTarget);
                _potentialDropTarget = null;
            }

            if (_draggedTile != null && FloatingCanvas.Children.Contains(_draggedTile))
            {
                FloatingCanvas.Children.Remove(_draggedTile);
            }

            _draggedTile = null;
            _originalContainer = null;
            _dragStartedFromRack = false;
            _draggedTileInitialRackIndex = -1;
            _draggedTileInitialRackText = string.Empty;
            _draggedTileInitialRackBackground = null;
            Mouse.Capture(null);
        }

        private void ResetBorderAppearance(Border border)
        {
            border.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#c4c4d1");
            border.BorderThickness = new Thickness(1);
        }

        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null && !(child is T))
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as T;
        }

        public Tile? ConvertToTile(string tileString)
        {
            Tile? tileObject = null;
            foreach (var tileObj in _tileRack)
            {
                if (tileObj.GetLetter().ToString() == tileString)
                {
                    tileObject = tileObj;
                }
            }

            return tileObject;
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SwapTiles(_tileRack, _game, _currentPlayer, this));
            StartGame();
            GenerateRack(_tileRack);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EndGame(_game._players));
            
        }

        private void ReplaceBorderAt(int targetRow, int targetCol, string text)
        {
            // 1. Cari dan hapus elemen Border yang ada di posisi target
            UIElement elementToRemove = null;
            foreach (UIElement child in BoardGrid.Children)
            {
                if (Grid.GetRow(child) == targetRow && Grid.GetColumn(child) == targetCol)
                {
                    if (child is Border)
                    {
                        elementToRemove = child;
                        break;
                    }
                }
            }

            if (elementToRemove != null)
            {
                BoardGrid.Children.Remove(elementToRemove);
            }

            // 2. Tambahkan Border baru
            Border newBorder = new Border
            {
                Background = Brushes.DarkGray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Margin = new Thickness(1)
            };

            var label = new Label
            {
                Content = text,
                FontSize = 16,
                Foreground = Brushes.White, // ⬅️ Warna teks putih
                Background = Brushes.SlateGray,
                FontWeight = FontWeights.Bold,
                Width = 30,
                Height = 30,

                // Agar teks tepat di tengah kontainer Label
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,

                // Posisi Label di parent container (misalnya Grid)
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };



            newBorder.Child = label;
            Grid.SetRow(newBorder, targetRow);
            Grid.SetColumn(newBorder, targetCol);
            BoardGrid.Children.Add(newBorder);
        }


        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            List<TilePlacement> placements = new();

            foreach (var pos in _tilePosition)
            {
                foreach (var tileObject in _objectTileRack)
                {
                    if (pos.Key == tileObject.Tag)
                    {
                        (var x, var y) = pos.Value;
                        int index = (int)tileObject.Tag;
                        var tile = _tileRack[index]; // ✅ Ambil langsung dari posisi aslinya
                        placements.Add(new TilePlacement(tile, x, y));
                    }
                }
            }


            foreach (var tl in _tileRack)
            {
                tileBagElement.Text = tl.GetLetter().ToString();
            }

            MoveError error = _game.PerformTurn(_currentPlayer, placements);
            if (error != MoveError.None)
            {
                tileBagElement.Text = "error";
            }
            else
            {
                tileBagElement.Text = "success";
                foreach (var placement in placements)
                {
                    ReplaceBorderAt(placement.GetX(), placement.GetY(), placement.GetTile().GetLetter().ToString());
                }

                _tileRack.Clear();
                _tilePosition.Clear();
                StartGame();
                GenerateRack(_tileRack);
            }
        }

    }
}
