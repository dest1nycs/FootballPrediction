using FootballPrediction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FootballPrediction.Pages
{
    public partial class LeaguesPage : Page
    {
        private readonly HttpClient client =
            new HttpClient();

        private List<LeagueStanding> premierLeague =
            new();

        private List<LeagueStanding> laLiga =
            new();

        private List<LeagueStanding> bundesliga =
            new();

        private List<LeagueStanding> championsLeague =
            new();

        private List<LeagueStanding> upl =
            new();

        public LeaguesPage()
        {
            InitializeComponent();

            client.DefaultRequestHeaders.Add(
                "X-Auth-Token",
                "0a42a70085f9447da8d4c30c5a65812e");

            LoadAllLeagues();
        }

        // LOAD ALL

        private async void LoadAllLeagues()
        {
            await LoadLeague(
                "PL",
                PremierLeagueGrid,
                premierLeague);

            await LoadLeague(
                "PD",
                LaLigaGrid,
                laLiga);

            await LoadLeague(
                "BL1",
                BundesligaGrid,
                bundesliga);

            await LoadLeague(
                "CL",
                ChampionsLeagueGrid,
                championsLeague);

            await LoadLeague(
                "PPL",
                UPLGrid,
                upl);
        }

        // LOAD LEAGUE

        private async Task LoadLeague(
            string leagueCode,
            DataGrid grid,
            List<LeagueStanding> storage)
        {
            try
            {
                string url =
                    $"https://api.football-data.org/v4/competitions/{leagueCode}/standings";

                var response =
                    await client.GetAsync(url);

                string json =
                    await response.Content.ReadAsStringAsync();

                using JsonDocument doc =
                    JsonDocument.Parse(json);

                var table =
                    doc.RootElement
                        .GetProperty("standings")[0]
                        .GetProperty("table");

                storage.Clear();

                foreach (var team in table.EnumerateArray())
                {
                    storage.Add(
                        new LeagueStanding
                        {
                            Position =
                                team.GetProperty("position")
                                    .GetInt32(),

                            Team =
                                team.GetProperty("team")
                                    .GetProperty("name")
                                    .GetString(),

                            Points =
                                team.GetProperty("points")
                                    .GetInt32()
                        });
                }

                grid.ItemsSource =
                    storage.Take(7).ToList();
            }

            catch (Exception ex)
            {
              
            }
        }

        // EXPAND

        private void PremierLeagueExpand_Click(
            object sender,
            RoutedEventArgs e)
        {
            PremierLeagueGrid.ItemsSource =
                premierLeague;
        }

        private void LaLigaExpand_Click(
            object sender,
            RoutedEventArgs e)
        {
            LaLigaGrid.ItemsSource =
                laLiga;
        }

        private void BundesligaExpand_Click(
            object sender,
            RoutedEventArgs e)
        {
            BundesligaGrid.ItemsSource =
                bundesliga;
        }

        private void ChampionsLeagueExpand_Click(
            object sender,
            RoutedEventArgs e)
        {
            ChampionsLeagueGrid.ItemsSource =
                championsLeague;
        }

        private void UPLExpand_Click(
            object sender,
            RoutedEventArgs e)
        {
            UPLGrid.ItemsSource =
                upl;
        }
    }
}
