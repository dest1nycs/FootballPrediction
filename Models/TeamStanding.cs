namespace FootballPrediction.Models
{
    public class TeamStanding
    {
        public string TeamName { get; set; } = "";

        public int Position { get; set; }

        public int Won { get; set; }

        public int Draw { get; set; }

        public int Lost { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        public int Points { get; set; }
    }
}
