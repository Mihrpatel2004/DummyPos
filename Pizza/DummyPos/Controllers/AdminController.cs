using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult MenuManagement()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Pizza", "Home");
        }
    }
}
