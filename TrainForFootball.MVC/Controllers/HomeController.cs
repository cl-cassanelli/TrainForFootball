using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainForFootball.MVC.Data;

public class HomeController : Controller
{
    private readonly TrainForFootballDbContext _context;

    public HomeController(TrainForFootballDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Carica tutte le leghe
        var leagues = await _context.Leagues.ToListAsync();
        ViewBag.Leagues = leagues;

        // Carica le squadre top per ciascuna lega
        var topTeamsByLeague = new List<KeyValuePair<TrainForFootball.MVC.Models.League, List<TrainForFootball.MVC.Models.Team>>>();

        foreach (var league in leagues)
        {
            var topTeams = await _context.Teams
                .Include(t => t.TeamStats)
                .Where(t => t.LeagueId == league.LeagueId)  // Filtro per lega
                .OrderByDescending(t => t.TeamStats.SeasonPoint)  // Ordina per punti stagionali
                .Take(3)  // Prendi le prime 3
                .ToListAsync();

            topTeamsByLeague.Add(new KeyValuePair<TrainForFootball.MVC.Models.League, List<TrainForFootball.MVC.Models.Team>>(league, topTeams));
        }

        // Passa i risultati alla View
        ViewBag.TopTeamsByLeague = topTeamsByLeague;

        // Troviamo le partite per la Serie A e Serie B che sono nella giornata più vicina
        var upcomingMatches = await _context.Matches
            .Include(m => m.HomeTeam)
            .ThenInclude(ht => ht.League) // Include la lega della squadra di casa
            .Include(m => m.AwayTeam)
            .ThenInclude(at => at.League) // Include la lega della squadra ospite
            .Where(m => (m.HomeTeam.League.Name == "Serie A" || m.HomeTeam.League.Name == "Serie B")
                        && m.MatchDate.Date >= DateTime.Today)  // Filtro per partite future
            .OrderBy(m => m.MatchDate)  // Ordina per data
            .ToListAsync();

        // Gruppo le partite per la giornata più vicina
        var closestMatchDate = upcomingMatches
            .Select(m => m.MatchDate.Date)
            .Distinct()
            .OrderBy(d => d)
            .FirstOrDefault();

        // Filtro le partite che sono nella giornata più vicina per Serie A e Serie B
        var closestMatchesSerieA = upcomingMatches
            .Where(m => m.MatchDate.Date == closestMatchDate && m.HomeTeam.League.Name == "Serie A")
            .ToList();

        var closestMatchesSerieB = upcomingMatches
            .Where(m => m.MatchDate.Date == closestMatchDate && m.HomeTeam.League.Name == "Serie B")
            .ToList();

        // Passo le partite separate alla View
        ViewBag.ClosestMatchesSerieA = closestMatchesSerieA;
        ViewBag.ClosestMatchesSerieB = closestMatchesSerieB;

        return View();
    }





}




//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using TrainForFootball.MVC.Models;

//namespace TrainForFootball.MVC.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
