using FootballPrediction.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FootballPrediction.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(
            object sender,
            RoutedEventArgs e)
        {
            ErrorText.Foreground =
                Brushes.Red;

            ErrorText.Text = "";

            string username =
                UsernameTextBox.Text.Trim();

            string password =
                PasswordBox.Password.Trim();

            // EMPTY

            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                ErrorText.Text =
                    "Заповніть усі поля";

                return;
            }

            bool success =
                AuthService.Login(
                    username,
                    password,
                    RememberCheckBox.IsChecked == true);

            if (!success)
            {
                ErrorText.Text =
                    "Невірний логін або пароль";

                return;
            }

            ErrorText.Foreground =
                Brushes.LimeGreen;

            ErrorText.Text =
                "Вхід успішний";

            if (Application.Current.MainWindow
                is MainWindow mainWindow)
            {
                mainWindow.RefreshUI();
            }

            NavigationService?.Navigate(
                new DashboardPage());
        }

        private void GoToRegister_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService?.Navigate(
                new RegisterPage());
        }
    }
}