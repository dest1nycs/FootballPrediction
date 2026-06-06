using System;
using System.Windows;

namespace FootballPrediction.Services
{
    public static class LanguageManager
    {
        public static void ChangeLanguage(string languageCode)
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (languageCode)
            {
                case "en":
                    dict.Source = new Uri(
                        "Languages/Strings.en.xaml",
                        UriKind.Relative);
                    break;

                case "es":
                    dict.Source = new Uri(
                        "Languages/Strings.es.xaml",
                        UriKind.Relative);
                    break;

                default:
                    dict.Source = new Uri(
                        "Languages/Strings.uk.xaml",
                        UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Clear();

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
