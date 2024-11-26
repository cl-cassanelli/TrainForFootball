namespace TrainForFootball.MVC.Models
{
    public class MatchResult
    {
        public int MatchResultId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        // Chiave esterna per Match
        public int MatchId { get; set; }
        public Match? Match { get; set; } // Relazione uno-a-uno con Match
    }
}
