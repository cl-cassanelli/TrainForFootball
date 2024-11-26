namespace TrainForFootball.MVC.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }

        public DateTime MatchDate { get; set; }
        public int MatchDayNum { get; set; }

        // Relazione uno-a-uno con MatchResult (opzionale)
        public int? MatchResultId { get; set; }
        public MatchResult? MatchResult { get; set; }
    }
}
