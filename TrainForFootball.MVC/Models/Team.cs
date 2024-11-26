namespace TrainForFootball.MVC.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string SquadName { get; set; } = string.Empty;
        public int FondationYear { get; set; }
        public string City { get; set; } = string.Empty;
        public string ColorOfClub { get; set; } = string.Empty;
        public string StadiumName { get; set; } = string.Empty;
        public int LeagueId { get; set; } // Collegamento alla lega
        public League? League { get; set; } // Relazione con la League
        public TeamStats? TeamStats { get; set; }
    }
}
