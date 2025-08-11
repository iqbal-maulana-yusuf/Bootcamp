using BoardScrabble.Controller;
using BoardScrabble.GameControllers.Interfaces;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BoardScrabble.Views
{
    public partial class SwapTiles : Page
    {
        public List<Tile> SelectedTiles { get; private set; } = new();
        private List<Tile> PlayerTiles;
        private GameControl _game;
        private IPlayer _currentPlayer;
        private GamePage _gamePage;

        public SwapTiles(List<Tile> playerTiles, GameControl game, IPlayer currentPlayer, GamePage gamePage)
        {
            InitializeComponent();
            _game = game;
            _gamePage = gamePage;
            _currentPlayer = currentPlayer;
            PlayerTiles = playerTiles;
            LoadTiles();
        }

        private void LoadTiles()
        {
            foreach (var tile in PlayerTiles)
            {
                var btn = new Button
                {
                    Content = $"{tile.GetLetter()}",
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Width = 30,
                    Height = 30,
                    Padding = new Thickness(3),
                    Background = (Brush)new BrushConverter().ConvertFromString("#ffda9e")!,
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Tag = tile
                };

                btn.Click += Tile_Click;
                TilePanel.Children.Add(btn);
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tile = button!.Tag as Tile;
            
            if (SelectedTiles.Contains(tile!))
            {
                SelectedTiles.Remove(tile!);
                button.Background = Brushes.LightGoldenrodYellow;
            }
            else
            {
                SelectedTiles.Add(tile!);
                button.Background = Brushes.LightBlue;
            }

            SelectedCountText.Text = $"{SelectedTiles.Count} Selected";
            SwapButton.Background = SelectedTiles.Count > 0 ? Brushes.MediumTurquoise : Brushes.LightGray;
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTiles.Count > 0)
            {
                _game.SwapTiles(_currentPlayer, SelectedTiles);

                MessageBox.Show($"Swapped {SelectedTiles.Count} tile(s).");

                _gamePage.StartGame();                    // Refresh data
                _gamePage.GenerateRack(_gamePage._tileRack); // Regenerate tampilan rack

                NavigationService?.Navigate(_gamePage);   // Kembali ke halaman game
            }
            if (!_game.PlayerHistory.ContainsKey(_game.GetCurrentPlayer()))
            {
                _game.PlayerHistory[_game.GetCurrentPlayer()] = new List<string>();
            }
            _game.PlayerHistory[_game.GetCurrentPlayer()].Add("SWAP_TILE");
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigasi kembali ke GamePage
            NavigationService.GoBack();
        }
    }
}
