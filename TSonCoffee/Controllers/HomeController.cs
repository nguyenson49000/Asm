using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSonCoffee.Models;

namespace TSonCoffee.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly TsonCoffeeContext db;

        public HomeController(ILogger<HomeController> logger,TsonCoffeeContext db)
        {
            this.db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var sp = db.Products.Include(x => x.Img).ToList();
            return View(sp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
