using Microsoft.AspNetCore.Mvc;
using TrainForFootball.MVC.Models;
using System.Linq;
using TrainForFootball.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace TrainForFootball.MVC.Controllers
{
    public class MatchResultController : Controller
    {
        private readonly TrainForFootballDbContext _context;

        public MatchResultController(TrainForFootballDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? selectedLeague, DateTime? selectedDate)
        {

            var leagues = await _context.Leagues.ToListAsync();
            ViewBag.Leagues = leagues;



            var matchesQuery = _context.Matches
                .OrderBy(m => m.MatchDate)       // Ordina per data
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.MatchResult)
                .Where(m => m.MatchResult != null)
                .AsQueryable();

            var availableDates = await matchesQuery
                .Select(m => m.MatchDate.Date) // Prendi solo la parte della data (senza orario)
                .Distinct()
                .OrderBy(d => d) // Ordina cronologicamente
                .ToListAsync();

            // Filtra per lega se selezionata
            if (selectedLeague.HasValue && selectedLeague.Value > 0)
            {
                matchesQuery = matchesQuery.Where(m =>
                    m.HomeTeam.LeagueId == selectedLeague.Value || m.AwayTeam.LeagueId == selectedLeague.Value);
            }

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
        public async Task<IActionResult> GenerateResults(int? leagueId)
        {

            // Recupera le squadre della lega specificata
            var teams = await _context.Teams
                .Where(t => t.LeagueId == leagueId)
                .Include(t => t.League)
                .ToListAsync();

            var league = teams.First().League;


            if (!teams.Any())
            {
                return BadRequest("Nessuna squadra trovata per la lega specificata.");
            }

            var matches = _context.Matches
                .Where(m => m.MatchResult == null && (m.HomeTeam.League == league || m.AwayTeam.League == league)) // Solo partite senza risultato
                .OrderBy(m => m.MatchDate)
                .ToList();

            var today = matches.FirstOrDefault()?.MatchDate.Date;

            if (today != null)
            {
                matches = matches.Where(m => m.MatchDate.Date == today).ToList();

                foreach (var match in matches)
                {
                    // Calcola i goal basati sulla Strength
                    var homeStrength = match.HomeTeam?.TeamStats?.Strength ?? 1;
                    var awayStrength = match.AwayTeam?.TeamStats?.Strength ?? 1;

                    var homeGoals = GenerateGoals(homeStrength);
                    var awayGoals = GenerateGoals(awayStrength);

                    // Aggiorna le statistiche delle squadre
                    UpdateTeamStats(match.HomeTeamId, match.AwayTeamId, homeGoals, awayGoals);

                    // Crea il risultato della partita
                    var result = new MatchResult
                    {
                        HomeTeamScore = homeGoals,
                        AwayTeamScore = awayGoals,
                        MatchId = match.MatchId
                    };
                    match.MatchResultId = result.MatchResultId;
                    _context.MatchResults.Add(result);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        private int GenerateGoals(int strength)
        {
            // Genera goal basati sulla Strength (da 1 a 5)
            Random random = new Random();
            return random.Next(0, strength + 1);
        }

        public void UpdateTeamStats(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals)
        {
            // Recupera i team da database con le relative statistiche
            var homeTeam = _context.Teams
                .Include(t => t.TeamStats) // Includi TeamStats
                .FirstOrDefault(t => t.TeamId == homeTeamId);

            var awayTeam = _context.Teams
                .Include(t => t.TeamStats) // Includi TeamStats
                .FirstOrDefault(t => t.TeamId == awayTeamId);

            // Verifica che entrambi i team esistano e abbiano le statistiche
            if (homeTeam != null && homeTeam.TeamStats != null && awayTeam != null && awayTeam.TeamStats != null)
            {
                // Aggiorna le statistiche della squadra di casa
                homeTeam.TeamStats.GoalsScored += homeGoals;
                homeTeam.TeamStats.GoalsConceded += awayGoals;

                if (homeGoals > awayGoals)
                {
                    homeTeam.TeamStats.MatchesWin++;
                    homeTeam.TeamStats.SeasonPoint += 3; // 3 punti per la vittoria
                }
                else if (homeGoals == awayGoals)
                {
                    homeTeam.TeamStats.MatchesDrawn++;
                    homeTeam.TeamStats.SeasonPoint += 1; // 1 punto per il pareggio
                }
                else
                {
                    homeTeam.TeamStats.MatchesLost++;
                }

                // Aggiorna le statistiche della squadra in trasferta
                awayTeam.TeamStats.GoalsScored += awayGoals;
                awayTeam.TeamStats.GoalsConceded += homeGoals;

                if (awayGoals > homeGoals)
                {
                    awayTeam.TeamStats.MatchesWin++;
                    awayTeam.TeamStats.SeasonPoint += 3; // 3 punti per la vittoria
                }
                else if (awayGoals == homeGoals)
                {
                    awayTeam.TeamStats.MatchesDrawn++;
                    awayTeam.TeamStats.SeasonPoint += 1; // 1 punto per il pareggio
                }
                else
                {
                    awayTeam.TeamStats.MatchesLost++;
                }

                // Salva le modifiche nel database
                _context.SaveChanges();
            }
            else
            {
                // Se uno dei team o le statistiche non esistono, logga o gestisci l'errore
                Console.WriteLine("Team or TeamStats not found.");
            }
        }

    }
}
