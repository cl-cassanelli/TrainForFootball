namespace TrainForFootball.MVC.Models
{
    public class TeamStats
    {
        public int TeamStatsId { get; set; }
        public int Strength { get; set; }
        public int MatchesWin { get; set; }
        public int MatchesLost { get; set; }
        public int MatchesDrawn { get; set; }
        public int SeasonPoint { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
