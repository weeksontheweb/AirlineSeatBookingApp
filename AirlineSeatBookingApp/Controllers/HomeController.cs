using AirlineSeatBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirlineSeatBookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookSeats()
        {
            Classes.Database db = new Classes.Database();
            return View(db.ReadPassengerList());
        }

        [HttpPost]
        public IActionResult Add(PassengerModel passenger)
        {
            var db = new Classes.Database();
            db.AddPassenger(passenger.Name);

            return RedirectToAction("BookSeats");
        }

        
        public IActionResult Delete(int id, string name)
        {
            var db = new Classes.Database();
            db.DeletePassenger(name);

            return RedirectToAction("BookSeats");
        }

        [HttpPost]
        public IActionResult Delete(PassengerModel passenger)
        {
            var db = new Classes.Database();
            db.DeletePassenger(passenger.Name);

            return RedirectToAction("BookSeats");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}