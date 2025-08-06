using BoardScrabble.Controller;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BoardScrabble.Views
{
    public partial class BlankTileInput : Page
    {
        public char SelectedLetter { get; private set; } = '_';
        public bool IsConfirmed { get; private set; } = false;

        private int _index;
        private GameControl _game;
        private GamePage _gamePage;

        // ✅ Tambahan: Event untuk callback ke GamePage
        public event Action<char, int> OnLetterSelected;

        public BlankTileInput(GameControl game, GamePage gamePage, int index)
        {
            InitializeComponent();
            LetterInputBox.Focus();
            _index = index;
            _game = game;
            _gamePage = gamePage;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string input = LetterInputBox.Text.ToUpper();
            if (!string.IsNullOrEmpty(input) && char.IsLetter(input[0]))
            {
                SelectedLetter = input[0];
                IsConfirmed = true;

                // ✅ Panggil event
                OnLetterSelected?.Invoke(SelectedLetter, _index);

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Masukkan satu huruf A-Z.", "Input Tidak Valid", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
