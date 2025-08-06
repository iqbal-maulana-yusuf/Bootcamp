using BoardScrabble.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BoardScrabble.Views
{
    public partial class EndGame : Page
    {
        public EndGame(List<IPlayer> players)
        {
            InitializeComponent();
            ShowResults(players);
        }

        private void ShowResults(List<IPlayer> players)
        {
            var sorted = players.OrderByDescending(p => p.GetScore()).ToList();
            var winner = sorted.First();

            WinnerText.Text = $"🏆 {winner.GetName()} MENANG!";
            ScoreList.Items.Clear();

            foreach (var player in sorted)
            {
                ScoreList.Items.Add($"{player.GetName()} - {player.GetScore()} poin");
            }
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FormGame()); // Ganti ini dengan halaman utama kamu
        }
    }
}
