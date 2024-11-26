using System.Globalization;

namespace TrainForFootball.MVC.Models
{
    public class League
    {
        public int LeagueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Relazione con le squadre
        public ICollection<Team>? Teams { get; set; }

        // Relazione con i calendari
        //public ICollection<Calendar>? Calendars { get; set; }
    }
}
