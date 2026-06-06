using FootballPrediction.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace FootballPrediction.Pages
{
    public partial class PredictionsPage : Page
    {
        private List<TeamStanding> CurrentStandings =
            new List<TeamStanding>();

        private Border? SelectedCard = null;

        private string SelectedHomeTeam = "";
        private string SelectedAwayTeam = "";

        

        public PredictionsPage()
        {
            InitializeComponent();

            LeagueComboBox.SelectedIndex = 0;
        }

        private async void LeagueComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            LeagueComboBox.IsEnabled = false;

            if (LeagueComboBox.SelectedItem == null)
            {
                LeagueComboBox.IsEnabled = true;
                return;
            }

            string league =
                ((ComboBoxItem)LeagueComboBox.SelectedItem)
                .Content.ToString() ?? "";

            string leagueCode = "PL";

            switch (league)
            {
                case "Premier League":
                    leagueCode = "PL";
                    break;

                case "La Liga":
                    leagueCode = "PD";
                    break;

                case "Bundesliga":
                    leagueCode = "BL1";
                    break;

                case "Champions League":
                    leagueCode = "CL";
                    break;

                case "UPL":
                    leagueCode = "UPL";
                    break;
            }

            await LoadStandings(leagueCode);

            await LoadMatches(leagueCode);

            LeagueComboBox.IsEnabled = true;
        }

        private async Task LoadStandings(string leagueCode)
        {
            CurrentStandings.Clear();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(
                        "X-Auth-Token",
                        "0a42a70085f9447da8d4c30c5a65812e");

                    string url =
                        $"https://api.football-data.org/v4/competitions/{leagueCode}/standings";

                    var response =
                        await client.GetAsync(url);

                    var json =
                        await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc =
                        JsonDocument.Parse(json))
                    {
                        if (!doc.RootElement.TryGetProperty("standings", out var standings))
                            return;

                        if (standings.GetArrayLength() == 0)
                            return;

                        var firstStanding = standings[0];

                        if (!firstStanding.TryGetProperty("table", out var table))
                            return;

                        foreach (var team in table.EnumerateArray())
                        {
                            CurrentStandings.Add(new TeamStanding
                            {
                                TeamName =
                                    team.GetProperty("team")
                                        .GetProperty("name")
                                        .GetString() ?? "",

                                Position =
                                    team.GetProperty("position")
                                        .GetInt32(),

                                Points =
                                    team.GetProperty("points")
                                        .GetInt32(),

                                GoalsFor =
                                    team.GetProperty("goalsFor")
                                        .GetInt32(),

                                GoalsAgainst =
                                    team.GetProperty("goalsAgainst")
                                        .GetInt32()
                            });
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private async Task LoadMatches(string leagueCode)
        {
            MatchesPanel.Children.Clear();

            SelectedCard = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(
                        "X-Auth-Token",
                        "0a42a70085f9447da8d4c30c5a65812e");

                    string url =
                        $"https://api.football-data.org/v4/competitions/{leagueCode}/matches?status=SCHEDULED";

                    var response =
                        await client.GetAsync(url);

                    var json =
                        await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc =
                        JsonDocument.Parse(json))
                    {
                        if (!doc.RootElement.TryGetProperty("matches", out var matches))
                        {
                            ShowNoMatches();
                            return;
                        }

                        int count = 0;

                        foreach (var match in matches.EnumerateArray())
                        {
                            if (count >= 5)
                                break;

                            string fullHome =
                                match.GetProperty("homeTeam")
                                     .GetProperty("name")
                                     .GetString() ?? "";

                            string fullAway =
                                match.GetProperty("awayTeam")
                                     .GetProperty("name")
                                     .GetString() ?? "";

                            string home =
                                match.GetProperty("homeTeam")
                                     .GetProperty("shortName")
                                     .GetString() ?? "";

                            string away =
                                match.GetProperty("awayTeam")
                                     .GetProperty("shortName")
                                     .GetString() ?? "";

                            string utcDate =
                                match.GetProperty("utcDate")
                                     .GetString() ?? "";

                            DateTime matchDate =
                                DateTime.Parse(utcDate)
                                        .ToLocalTime();

                            string formattedDate =
                                matchDate.ToString("dd.MM • HH:mm");

                            Border card = new Border
                            {
                                Background =
                                    new SolidColorBrush(
                                        Color.FromRgb(31, 41, 55)),

                                CornerRadius =
                                    new CornerRadius(15),

                                Padding =
                                    new Thickness(25),

                                Margin =
                                    new Thickness(0, 0, 0, 15),

                                Cursor = Cursors.Hand
                            };

                            Grid panel = new Grid();

                            panel.ColumnDefinitions.Add(
                                new ColumnDefinition
                                {
                                    Width = GridLength.Auto
                                });

                            panel.ColumnDefinitions.Add(
                                new ColumnDefinition
                                {
                                    Width = GridLength.Auto
                                });

                            panel.ColumnDefinitions.Add(
                                new ColumnDefinition
                                {
                                    Width = GridLength.Auto
                                });

                            panel.ColumnDefinitions.Add(
                                new ColumnDefinition
                                {
                                    Width = GridLength.Auto
                                });

                            TextBlock homeText = new TextBlock
                            {
                                Text = home,

                                Foreground =
                                    Brushes.White,

                                FontSize = 26,

                                FontWeight =
                                    FontWeights.Bold,

                                Margin =
                                    new Thickness(20, 0, 0, 0)
                            };

                            TextBlock vsText = new TextBlock
                            {
                                Text = "VS",

                                Foreground =
                                    new SolidColorBrush(
                                        Color.FromRgb(0, 255, 136)),

                                FontSize = 28,

                                FontWeight =
                                    FontWeights.Bold,

                                Margin =
                                    new Thickness(35, 0, 35, 0)
                            };

                            TextBlock awayText = new TextBlock
                            {
                                Text = away,

                                Foreground =
                                    Brushes.White,

                                FontSize = 26,

                                FontWeight =
                                    FontWeights.Bold
                            };

                            TextBlock dateText = new TextBlock
                            {
                                Text = formattedDate,

                                Foreground =
                                    new SolidColorBrush(
                                        Color.FromRgb(156, 163, 175)),

                                FontSize = 18,

                                Margin =
                                    new Thickness(50, 6, 0, 0),

                                VerticalAlignment =
                                    VerticalAlignment.Center
                            };

                            Grid.SetColumn(homeText, 0);
                            Grid.SetColumn(vsText, 1);
                            Grid.SetColumn(awayText, 2);
                            Grid.SetColumn(dateText, 3);

                            panel.Children.Add(homeText);
                            panel.Children.Add(vsText);
                            panel.Children.Add(awayText);
                            panel.Children.Add(dateText);

                            card.Child = panel;

                            // HOVER
                            card.MouseEnter += (s, e) =>
                            {
                                if (SelectedCard != card)
                                {
                                    card.Background =
                                        new SolidColorBrush(
                                            Color.FromRgb(55, 65, 81));

                                    card.BorderBrush =
                                        new SolidColorBrush(
                                            Color.FromRgb(0, 255, 136));

                                    card.BorderThickness =
                                        new Thickness(2);
                                }
                            };

                            card.MouseLeave += (s, e) =>
                            {
                                if (SelectedCard != card)
                                {
                                    card.Background =
                                        new SolidColorBrush(
                                            Color.FromRgb(31, 41, 55));

                                    card.BorderThickness =
                                        new Thickness(0);
                                }
                            };

                            // SELECT
                            card.MouseLeftButtonDown += (s, e) =>
                            {
                                if (SelectedCard == card)
                                {
                                    card.Background =
                                        new SolidColorBrush(
                                            Color.FromRgb(31, 41, 55));

                                    card.BorderThickness =
                                        new Thickness(0);

                                    SelectedCard = null;

                                    SelectedHomeTeam = "";
                                    SelectedAwayTeam = "";

                                    return;
                                }

                                if (SelectedCard != null)
                                {
                                    SelectedCard.Background =
                                        new SolidColorBrush(
                                            Color.FromRgb(31, 41, 55));

                                    SelectedCard.BorderThickness =
                                        new Thickness(0);
                                }

                                SelectedCard = card;

                                SelectedHomeTeam = fullHome;
                                SelectedAwayTeam = fullAway;

                                card.Background =
                                    new SolidColorBrush(
                                        Color.FromRgb(55, 65, 81));

                                card.BorderBrush =
                                    new SolidColorBrush(
                                        Color.FromRgb(0, 255, 136));

                                card.BorderThickness =
                                    new Thickness(2);
                            };

                            MatchesPanel.Children.Add(card);

                            count++;
                        }

                        if (count == 0)
                        {
                            ShowNoMatches();
                        }
                    }
                }
            }
            catch
            {
                ShowNoMatches();
            }
        }

        private void ShowNoMatches()
        {
            Border emptyCard = new Border
            {
                Background =
                    new SolidColorBrush(
                        Color.FromRgb(22, 31, 53)),

                CornerRadius =
                    new CornerRadius(15),

                Padding =
                    new Thickness(30),

                Margin =
                    new Thickness(0, 0, 0, 15)
            };

            TextBlock text = new TextBlock
            {
                Text =
                    "Наразі матчів у цій лізі немає ⚽",

                Foreground =
                    Brushes.White,

                FontSize = 24,

                FontWeight =
                    FontWeights.Bold
            };

            emptyCard.Child = text;

            MatchesPanel.Children.Add(emptyCard);
        }

        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedHomeTeam) ||
                string.IsNullOrEmpty(SelectedAwayTeam))
            {
                WinnerText.Text = "Оберіть матч";
                return;
            }

            var home =
                CurrentStandings.Find(t =>
                    SelectedHomeTeam.Contains(t.TeamName));

            var away =
                CurrentStandings.Find(t =>
                    SelectedAwayTeam.Contains(t.TeamName));

            if (home == null || away == null)
            {
                WinnerText.Text = "Немає даних";
                return;
            }

            string predictionKey =
                $"{home.TeamName}-{away.TeamName}";

            if (Services.AuthService.CurrentUser != null &&
    !Services.AuthService.CurrentUser
        .PredictedMatches
        .Contains(predictionKey))
            {
                Services.AuthService.CurrentUser
                    .PredictedMatches
                    .Add(predictionKey);

                App.TotalPredictions++;

                Services.AuthService.CurrentUser
                    .TotalPredictions++;

                Services.AuthService.CurrentUser
                    .TodayPredictions++;

                Services.AuthService.CurrentUser
                    .LastPredictionDate =
                        DateTime.Now.ToString("yyyy-MM-dd");

                Services.AuthService.SaveCurrentUser();
            }

            // IMPROVED AI LOGIC

            double homeStrength =
                (home.Points * 1.5) +
                (home.GoalsFor * 0.8) -
                (home.GoalsAgainst * 0.5) +
                8; // HOME ADVANTAGE

            double awayStrength =
                (away.Points * 1.5) +
                (away.GoalsFor * 0.8) -
                (away.GoalsAgainst * 0.5);

            string winner =
                homeStrength >= awayStrength
                ? home.TeamName
                : away.TeamName;

            int probability =
                50 + (int)Math.Abs(
                    homeStrength - awayStrength);

            if (probability > 78)
                probability = 78;

            WinnerText.Text =
                $"{winner} — {probability}%";

            // GOALS ANALYSIS

            double avgGoals =
                (home.GoalsFor + away.GoalsFor) / 2.0;

            if (avgGoals >= 60)
            {
                GoalsText.Text =
                    "Over 2.5 — 76%";
            }
            else if (avgGoals >= 40)
            {
                GoalsText.Text =
                    "Over 2.5 — 61%";
            }
            else
            {
                GoalsText.Text =
                    "Under 2.5 — 64%";
            }

            // CORNERS

            int cornersChance =
                60 + (home.Position + away.Position) / 4;

            if (cornersChance > 82)
                cornersChance = 82;

            CornersText.Text =
                $"Over 8.5 — {cornersChance}%";

            // CARDS

            int cardsChance =
                55 + Math.Abs(
                    home.Position - away.Position);

            if (cardsChance > 79)
                cardsChance = 79;

            CardsText.Text =
                $"Over 3.5 — {cardsChance}%";

            // BOTH TEAMS TO SCORE

            bool bothScore =
                home.GoalsFor > 30 &&
                away.GoalsFor > 30;

            int bttsChance =
                bothScore ? 68 : 54;

            BttsText.Text =
                bothScore
                ? $"YES — {bttsChance}%"
                : $"NO — {bttsChance}%";
        }
    }
}