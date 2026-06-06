using FootballPrediction.Services;
using System.Windows;
using System.Windows.Controls;

namespace FootballPrediction.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            LoadProfile();
        }

        private void LoadProfile()
        {
            if (AuthService.CurrentUser == null)
                return;

            UsernameText.Text =
                "👤 " +
                AuthService.CurrentUser.Username;

            TotalPredictionsText.Text =
                AuthService.CurrentUser.TotalPredictions
                .ToString();

            TodayPredictionsText.Text =
                AuthService.CurrentUser.TodayPredictions
                .ToString();
        }

        private void Logout_Click(
            object sender,
            RoutedEventArgs e)
        {
            AuthService.Logout();

            if (Application.Current.MainWindow
    is MainWindow mainWindow)
            {
                mainWindow.RefreshUI();
            }

            NavigationService?.Navigate(
                new LoginPage());
        }
    }
}
