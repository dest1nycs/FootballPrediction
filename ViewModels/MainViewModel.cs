using System.Collections.ObjectModel;
using System.Linq;
using FootballPrediction.Models;
using FootballPrediction.Data;

namespace FootballPrediction.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Match> Matches { get; set; }

        public MainViewModel()
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                if (!db.Matches.Any())
                {
                    db.Matches.Add(new Match { HomeTeam = "Arsenal", AwayTeam = "Chelsea", HomeGoals = 2, AwayGoals = 1 });
                    db.Matches.Add(new Match { HomeTeam = "Barcelona", AwayTeam = "Real Madrid", HomeGoals = 3, AwayGoals = 2 });
                    db.Matches.Add(new Match { HomeTeam = "Bayern", AwayTeam = "Dortmund", HomeGoals = 1, AwayGoals = 1 });

                    db.SaveChanges();
                }

                Matches = new ObservableCollection<Match>(db.Matches.ToList());
            }
        }
    }
}