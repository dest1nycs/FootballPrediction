using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FootballPrediction.Pages
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();

            Loaded += DashboardPage_Loaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDashboardData();
        }

        private async Task LoadDashboardData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(
                        "X-Auth-Token",
                        "0a42a70085f9447da8d4c30c5a65812e");

                    // LEAGUES
                    string[] competitions =
                    {
                        "PL",   // Premier League
                        "PD",   // La Liga
                        "BL1",  // Bundesliga
                        "CL",   // Champions League
                        "SA"    // Serie A
                    };

                    int totalMatches = 0;
                    int liveMatches = 0;

                    string today =
                        DateTime.Now.ToString("yyyy-MM-dd");

                    foreach (string code in competitions)
                    {
                        string url =
                            $"https://api.football-data.org/v4/competitions/{code}/matches?dateFrom={today}&dateTo={today}";

                        var response =
                            await client.GetAsync(url);

                        var json =
                            await response.Content.ReadAsStringAsync();

                        using (JsonDocument doc =
                            JsonDocument.Parse(json))
                        {
                            if (!doc.RootElement.TryGetProperty("matches", out var matches))
                                continue;

                            foreach (var match in matches.EnumerateArray())
                            {
                                totalMatches++;

                                string status =
                                    match.GetProperty("status")
                                         .GetString() ?? "";

                                if (status == "IN_PLAY" ||
                                    status == "PAUSED")
                                {
                                    liveMatches++;
                                }
                            }
                        }
                    }

                    string dateText =
                        DateTime.Now.ToString(
                            "dddd, dd MMMM yyyy");

                    // NO MATCHES
                    if (totalMatches == 0)
                    {
                        DashboardInfoText.Text =
                            $"{dateText} · Сьогодні матчів немає";
                    }
                    else
                    {
                        DashboardInfoText.Text =
                            $"{dateText} · {totalMatches} матчів сьогодні";
                    }

                    PredictionsTodayText.Text =
    App.TotalPredictions.ToString();

                    LiveMatchesText.Text =
                        liveMatches.ToString();
                }
            }
            catch
            {
                DashboardInfoText.Text =
                    "Помилка завантаження даних";

                PredictionsTodayText.Text = "-";

                LiveMatchesText.Text = "-";
            }
        }

        private void NewPrediction_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(
                new PredictionsPage());
        }
    }
}