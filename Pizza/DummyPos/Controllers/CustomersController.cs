using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Customer()
        {
            return View();
        }
    }
}
