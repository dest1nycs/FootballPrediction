using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FootballPrediction.ViewModels;
using FootballPrediction.Models;

namespace FootballPrediction
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private async void LeagueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (LeagueComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selected == "Premier League")
                await LoadMatchesFromApi("PL");

            else if (selected == "La Liga")
                await LoadMatchesFromApi("PD");

            else if (selected == "Bundesliga")
                await LoadMatchesFromApi("BL1");
        }

        private async Task LoadMatchesFromApi(string leagueCode)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Auth-Token", "0a42a70085f9447da8d4c30c5a65812e");

                string url = $"https://api.football-data.org/v4/competitions/{leagueCode}/matches?status=SCHEDULED";

                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var matches = doc.RootElement.GetProperty("matches");

                    viewModel.Matches.Clear();

                    foreach (var match in matches.EnumerateArray())
                    {
                        var home = match.GetProperty("homeTeam").GetProperty("name").GetString();
                        var away = match.GetProperty("awayTeam").GetProperty("name").GetString();

                        viewModel.Matches.Add(new Match
                        {
                            HomeTeam = home ?? "",
                            AwayTeam = away ?? "",
                            HomeGoals = 0,
                            AwayGoals = 0
                        });
                    }
                }
            }
        }
    }
}