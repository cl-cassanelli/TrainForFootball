using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainForFootball.MVC.Data;
using TrainForFootball.MVC.Models;

namespace TrainForFootball.MVC.Controllers
{
    public class MatchController : Controller
    {
        private readonly TrainForFootballDbContext _context;

        public MatchController(TrainForFootballDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? selectedLeague, DateTime? selectedDate)
        {
            // Carica tutte le leghe
            var leagues = await _context.Leagues.ToListAsync();
            ViewBag.Leagues = leagues;

            // Costruisci la query per caricare i match
            var matchesQuery = _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.MatchResult)
                .AsQueryable();

            // Filtra per lega se selezionata
            if (selectedLeague.HasValue && selectedLeague.Value > 0)
            {
                matchesQuery = matchesQuery.Where(m =>
                    m.HomeTeam.LeagueId == selectedLeague.Value && m.AwayTeam.LeagueId == selectedLeague.Value);
            }

            // Trova le date uniche disponibili
            var availableDates = await matchesQuery
                .Select(m => m.MatchDate.Date) // Prendi solo la parte della data (senza orario)
                .Distinct()
                .OrderBy(d => d) // Ordina cronologicamente
                .ToListAsync();

            ViewBag.AvailableDates = availableDates;

            // Seleziona la prima data disponibile se nessuna è stata scelta
            if (selectedDate == null && availableDates.Any())
            {
                selectedDate = availableDates.First();
            }

            // Filtra per data se selezionata
            if (selectedDate.HasValue)
            {
                matchesQuery = matchesQuery.Where(m => m.MatchDate.Date == selectedDate.Value.Date);
            }

            // Carica i match filtrati
            var matches = await matchesQuery.ToListAsync();

            // Passa i valori alla vista
            ViewBag.SelectedLeague = selectedLeague;
            ViewBag.SelectedDate = selectedDate;

            return View(matches);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllMatches(int leagueId)
        {
            // Recupera le squadre della lega specificata
            var teams = await _context.Teams
                .Where(t => t.LeagueId == leagueId)
                .Include(t => t.League)
                .ToListAsync();

            if (!teams.Any())
            {
                return BadRequest("Nessuna squadra trovata per la lega specificata.");
            }

            // Recupera la lega
            var league = teams.First().League;

            // Verifica se esistono già partite per la lega
            bool matchesExist = await _context.Matches
                .AnyAsync(m => m.HomeTeam.LeagueId == leagueId || m.AwayTeam.LeagueId == leagueId);

            if (matchesExist)
            {
                return BadRequest("Il calendario per questa lega è già stato generato.");
            }

            // Aggiunge una squadra fittizia per gestire un numero dispari di squadre
            if (teams.Count % 2 != 0)
            {
                teams.Add(new Team { SquadName = "Riposo" });
            }

            int numTeams = teams.Count;
            int numMatchDays = numTeams - 1;
            var matches = new List<Match>();

            // Genera il calendario (andata)
            for (int round = 0; round < numMatchDays; round++)
            {
                for (int i = 0; i < numTeams / 2; i++)
                {
                    int homeIndex = (round + i) % (numTeams - 1);
                    int awayIndex = (numTeams - 1 - i + round) % (numTeams - 1);

                    if (i == 0) awayIndex = numTeams - 1; // Fissa l'ultima squadra per il bilanciamento

                    matches.Add(new Match
                    {
                        HomeTeamId = teams[homeIndex].TeamId,
                        AwayTeamId = teams[awayIndex].TeamId,
                        MatchDayNum = round + 1,
                        MatchDate = DateTime.Now.AddDays(round * 7) // Una settimana di distanza tra le giornate
                    });
                }
            }

            // Genera il calendario (ritorno)
            int returnStartDay = numMatchDays + 1;
            foreach (var match in matches.ToList())
            {
                matches.Add(new Match
                {
                    HomeTeamId = match.AwayTeamId,
                    AwayTeamId = match.HomeTeamId,
                    MatchDayNum = returnStartDay + (match.MatchDayNum - 1),
                    MatchDate = DateTime.Now.AddDays((returnStartDay + (match.MatchDayNum - 1) - 1) * 7)
                });
            }

            // Salva le partite nel database
            _context.Matches.AddRange(matches);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect alla lista dei match generati
        }
    }
}
