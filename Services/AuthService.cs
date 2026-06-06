using FootballPrediction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FootballPrediction.Services
{
    public static class AuthService
    {
        private static readonly string UsersFile =
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "users.json");

        private static readonly string RememberFile =
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "remember.json");

        public static User? CurrentUser = null;

        // LOAD USERS

        public static List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(UsersFile))
                    return new List<User>();

                string json =
                    File.ReadAllText(UsersFile);

                return JsonSerializer.Deserialize<List<User>>(json)
                    ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        // SAVE USERS

        public static void SaveUsers(List<User> users)
        {
            string json =
                JsonSerializer.Serialize(
                    users,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(
                UsersFile,
                json);
        }

        // REGISTER

        public static bool Register(
            string username,
            string password)
        {
            var users = LoadUsers();

            bool exists =
                users.Any(u =>
                    u.Username.ToLower() ==
                    username.ToLower());

            if (exists)
                return false;

            User user = new User
            {
                Username = username,
                Password = password,
                TotalPredictions = 0,
                TodayPredictions = 0,
                LastPredictionDate =
                    DateTime.Now.ToString("yyyy-MM-dd")
            };

            users.Add(user);

            SaveUsers(users);

            return true;
        }

        // LOGIN

        public static bool Login(
            string username,
            string password,
            bool rememberMe)
        {
            var users = LoadUsers();

            var user =
                users.FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password);

            if (user == null)
                return false;

            CurrentUser = user;

            // DAILY RESET

            string today =
                DateTime.Now.ToString("yyyy-MM-dd");

            if (CurrentUser.LastPredictionDate != today)
            {
                CurrentUser.TodayPredictions = 0;
                CurrentUser.LastPredictionDate = today;

                SaveCurrentUser();
            }

            // REMEMBER ME

            if (rememberMe)
            {
                string rememberJson =
                    JsonSerializer.Serialize(
                        user.Username);

                File.WriteAllText(
                    RememberFile,
                    rememberJson);
            }

            return true;
        }

        // AUTO LOGIN

        public static bool AutoLogin()
        {
            try
            {
                if (!File.Exists(RememberFile))
                    return false;

                string json =
                    File.ReadAllText(RememberFile);

                string? username =
                    JsonSerializer.Deserialize<string>(json);

                if (string.IsNullOrEmpty(username))
                    return false;

                var users = LoadUsers();

                var user =
                    users.FirstOrDefault(u =>
                        u.Username == username);

                if (user == null)
                    return false;
                string today =
    DateTime.Now.ToString("yyyy-MM-dd");

                if (user.LastPredictionDate != today)
                {
                    user.TodayPredictions = 0;

                    user.LastPredictionDate = today;

                    SaveUsers(users);
                }
                CurrentUser = user;

                return true;
            }
            catch
            {
                return false;
            }
        }

        // LOGOUT

        public static void Logout()
        {
            CurrentUser = null;

            if (File.Exists(RememberFile))
            {
                File.Delete(RememberFile);
            }
        }

        // SAVE CURRENT USER

        public static void SaveCurrentUser()
        {
            if (CurrentUser == null)
                return;

            var users = LoadUsers();

            var existing =
                users.FirstOrDefault(u =>
                    u.Username == CurrentUser.Username);

            if (existing == null)
                return;

            existing.Password =
                CurrentUser.Password;

            existing.TotalPredictions =
                CurrentUser.TotalPredictions;

            existing.TodayPredictions =
                CurrentUser.TodayPredictions;

            existing.LastPredictionDate =
                CurrentUser.LastPredictionDate;

            SaveUsers(users);
        }
    }
}
