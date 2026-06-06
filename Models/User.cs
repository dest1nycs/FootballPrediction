namespace FootballPrediction.Models
{
    public class User
    {
        public string Username { get; set; } = "";

        public List<string> PredictedMatches { get; set; }
    = new List<string>();
        public string Password { get; set; } = "";

        public int TotalPredictions { get; set; }

        public int TodayPredictions { get; set; }

        public string LastPredictionDate { get; set; } = "";
    }
}
