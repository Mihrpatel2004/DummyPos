using System.Diagnostics;
using DummyPos.Models;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Burger()
        {
            return View();
        }

        public IActionResult Pizza()
        {
            return View();
        }
        public IActionResult Drink()
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
