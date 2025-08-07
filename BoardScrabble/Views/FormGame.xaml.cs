using BoardScrabble.Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;

namespace BoardScrabble
{
    public partial class FormGame : Page
    {
        public FormGame()
        {
            InitializeComponent();
            PlayerCountComboBox.SelectedIndex = 0;
        }

        private void PlayerCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedCount = int.Parse(((ComboBoxItem)PlayerCountComboBox.SelectedItem).Content.ToString()!);

            Player3Panel.Visibility = selectedCount >= 3 ? Visibility.Visible : Visibility.Collapsed;
            Player4Panel.Visibility = selectedCount == 4 ? Visibility.Visible : Visibility.Collapsed;
        }


        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            string name1 = Player1TextBox.Text.Trim();
            string name2 = Player2TextBox.Text.Trim();
            string? name3 = Player3Panel.Visibility == Visibility.Visible ? Player3TextBox.Text.Trim() : null;
            string? name4 = Player4Panel.Visibility == Visibility.Visible ? Player4TextBox.Text.Trim() : null;

           

            if (string.IsNullOrEmpty(name1) || string.IsNullOrEmpty(name2))
            {
                MessageBox.Show("Minimal 2 nama pemain harus diisi.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> listPlayerName = [name1, name2, name3, name4];
            string path = "WordBankEnglish.txt";


            if (!File.Exists(path))
            {
                MessageBox.Show("WordBank.txt tidak ditemukan di path: " + path, "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var board = new Board();
            var dictionary = new Dictionary(path);
            var tileBag = new TileBag();
            var game = new GameControl(dictionary, tileBag, board);

            game.OnDisplayMessage += message =>
            {
                // Pastikan dipanggil di UI thread
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            };

           

            foreach (var name in listPlayerName)
            {
                if (name != null)
                {
                    game.AddPlayer(new Player(name));
                }
            }

            game.StartGame();

            MessageBox.Show($"Game dimulai oleh:\n- {name1}\n- {name2}" +
                (name3 != null ? $"\n- {name3}" : "") +
                (name4 != null ? $"\n- {name4}" : ""), "Start Game");

            
             NavigationService?.Navigate(new GamePage(game));
        }
    }
}
