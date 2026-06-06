using FootballPrediction.Pages;
using FootballPrediction.Services;
using System.Windows;

namespace FootballPrediction
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // AUTO LOGIN

            bool autoLogin =
                AuthService.AutoLogin();

            UpdateSidebar();

            MainFrame.Navigate(
                new DashboardPage());
        }

        // SIDEBAR UPDATE

        private void UpdateSidebar()
        {
            bool loggedIn =
                AuthService.CurrentUser != null;

            // LOGGED IN

            if (loggedIn)
            {
                ProfileButton.Visibility =
                    Visibility.Visible;

                SettingsButton.Visibility =
                    Visibility.Visible;

                LoginButton.Visibility =
                    Visibility.Collapsed;

                RegisterButton.Visibility =
                    Visibility.Collapsed;
            }

            // NOT LOGGED

            else
            {
                ProfileButton.Visibility =
                    Visibility.Collapsed;

                SettingsButton.Visibility =
                    Visibility.Collapsed;

                LoginButton.Visibility =
                    Visibility.Visible;

                RegisterButton.Visibility =
                    Visibility.Visible;
            }
        }

        private void DashboardButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new DashboardPage());
        }

        private void PredictionsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new PredictionsPage());
        }

        private void LeaguesButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new LeaguesPage());
        }

        private void ProfileButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (AuthService.CurrentUser == null)
            {
                MessageBox.Show(
                    "Спочатку увійдіть в акаунт");

                return;
            }

            MainFrame.Navigate(
                new ProfilePage());
        }

        private void SettingsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new SettingsPage());
        }

        // LOGIN

        private void LoginButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new LoginPage());
        }

        // REGISTER

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new RegisterPage());
        }

        // REFRESH SIDEBAR

        public void RefreshUI()
        {
            UpdateSidebar();
        }
    }
}