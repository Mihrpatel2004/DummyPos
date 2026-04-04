using System.Diagnostics;
using DummyPos.Models;
using DummyPos.Repositories;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepo _itemRepo;

        private readonly IMenuRepo _menuRepo;
        public HomeController(IItemRepo itemRepo,IMenuRepo menuRepo)
        {
            _itemRepo = itemRepo;
            _menuRepo = menuRepo;
        }
        public IActionResult Burger()
        {
            // Assuming 3 is the Category ID for Burgers in your database. Update if needed!
            ViewBag.AllBurgerItem = _itemRepo.GetAllItems()
                                             .Where(i => i.Is_Active && i.Item_Category_Id == 3)
                                             .ToList();
            return View();
        }

        public IActionResult Pizza()
        {
            // Make sure to filter by Category ID so Drinks and Burgers don't show up here!
            // Change "1" to whatever your Pizza Category ID is in the database.
            ViewBag.AllPizzaItem = _itemRepo.GetAllItems()
                                            .Where(i => i.Is_Active && i.Item_Category_Id == 1)
                                            .ToList();
            return View();
        }
        public IActionResult Drink()
        {
            // Do the exact same thing for drinks! (Assume category 2 is Drinks)
            ViewBag.AllDrinkItem = _itemRepo.GetAllItems()
                                            .Where(i => i.Is_Active && i.Item_Category_Id == 2)
                                            .ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //

        [HttpGet]
        public IActionResult Search(string searchQuery)
        {
            // 1. If the user clicks the search button without typing anything, just reload the main page
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return RedirectToAction("Pizza");
            }

            // 2. Ask the database to find items matching the search query
            var searchResults = _menuRepo.SearchItems(searchQuery);

            // 3. Save the exact word they searched for so we can display it nicely on the screen
            ViewBag.SearchTerm = searchQuery;

            // 4. Send the list of found items to the Search View
            return View(searchResults);
        }
    }
}
