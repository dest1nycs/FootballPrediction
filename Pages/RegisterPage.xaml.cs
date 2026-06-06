using FootballPrediction.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FootballPrediction.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(
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

            // USERNAME LENGTH

            if (username.Length < 3 ||
                username.Length > 20)
            {
                ErrorText.Text =
                    "Username: 3-20 символів";

                return;
            }

            // PASSWORD LENGTH

            if (password.Length < 6)
            {
                ErrorText.Text =
                    "Пароль мінімум 6 символів";

                return;
            }

            // SPACES

            if (username.Contains(" ") ||
                password.Contains(" "))
            {
                ErrorText.Text =
                    "Пробіли заборонені";

                return;
            }

            // USERNAME VALIDATION

            if (!Regex.IsMatch(
                username,
                @"^[a-zA-Z0-9_]+$"))
            {
                ErrorText.Text =
                    "Username: тільки англ букви, цифри та _";

                return;
            }

            // PASSWORD VALIDATION

            if (!Regex.IsMatch(
                password,
                @"^[a-zA-Z0-9]+$"))
            {
                ErrorText.Text =
                    "Пароль: тільки англ букви та цифри";

                return;
            }

            bool success =
                AuthService.Register(
                    username,
                    password);

            if (!success)
            {
                ErrorText.Text =
                    "Користувач уже існує";

                return;
            }

            ErrorText.Foreground =
                Brushes.LimeGreen;

            ErrorText.Text =
                "Акаунт успішно створено";

            NavigationService?.Navigate(
                new LoginPage());
        }

        private void GoToLogin_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService?.Navigate(
                new LoginPage());
        }
    }
}