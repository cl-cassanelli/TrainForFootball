using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainForFootball.MVC.Data;

public class StandingsController : Controller
{
    private readonly TrainForFootballDbContext _context;

    public StandingsController(TrainForFootballDbContext context)
    {
        _context = context;
    }



    public async Task<IActionResult> Index(int? selectedLeague)
    {
        var leagues = await _context.Leagues.ToListAsync();
        ViewBag.Leagues = leagues;

        // Imposta la lega selezionata
        if(selectedLeague != null)
        {
            ViewBag.SelectedLeague = selectedLeague;

        } else
        {
            ViewBag.SelectedLeague = 1;
        }

        var teamsStatsQuery = _context.Teams
            .Include(t => t.TeamStats) // Include team stats
            .OrderByDescending(t => t.TeamStats.SeasonPoint) // Order by season points
            .ThenByDescending(t => t.TeamStats.MatchesWin) // If points are equal, order by wins
            .AsQueryable();

        // Filtra per lega se selezionata
        if (selectedLeague.HasValue && selectedLeague.Value > 0)
        {
            teamsStatsQuery = teamsStatsQuery.Where(t =>
                t.LeagueId == selectedLeague.Value);
        }
        else
        {
            teamsStatsQuery = teamsStatsQuery.Where(t =>
                t.LeagueId == 1);
        }
        var teams = await teamsStatsQuery.ToListAsync();

        return View(teams); // Pass the list to the view
    }

}
