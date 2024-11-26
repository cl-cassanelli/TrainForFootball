using Microsoft.AspNetCore.Mvc;
using TrainForFootball.MVC.Models; // Il tuo namespace per il modello Team
using System.Linq;
using TrainForFootball.MVC.Data;

namespace TrainForFootball.MVC.Controllers
{
    public class TeamController : Controller
    {
        private readonly TrainForFootballDbContext _context;

        // Iniettiamo il contesto del database tramite il costruttore
        public TeamController(TrainForFootballDbContext context)
        {
            _context = context;
        }

        // Elenco di tutte le squadre
        public IActionResult Index()
        {
            var teams = _context.Teams.ToList();  // Ottieni tutte le squadre dal database
            return View(teams); // Passa la lista alla vista
        }

        // Mostra il form per aggiungere una nuova squadra
        public IActionResult Create()
        {
            return View();
        }

        // Aggiungi una nuova squadra
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Teams.Add(team); // Aggiungi la squadra al contesto
                _context.SaveChanges(); // Salva le modifiche nel database
                return RedirectToAction(nameof(Index)); // Torna alla lista delle squadre
            }
            return View(team); // Ritorna alla vista di creazione in caso di errore
        }

        // Modifica una squadra esistente
        public IActionResult Edit(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // Modifica la squadra nel database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(team);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // Cancella una squadra
        public IActionResult Delete(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var team = _context.Teams.Find(id);
            _context.Teams.Remove(team);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
