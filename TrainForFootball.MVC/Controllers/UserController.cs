using Microsoft.AspNetCore.Mvc;
using TrainForFootball.MVC.Data;
using TrainForFootball.MVC.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace TrainForFootball.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly TrainForFootballDbContext _context;

        public UserController(TrainForFootballDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] User model)
        {
            Console.WriteLine($"Email: {model.Email}, Password: {model.Password}");

            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                return Json(new { success = true, userEmail = model.Email });
            }

            return Json(new { success = false });
        }
    }
}
