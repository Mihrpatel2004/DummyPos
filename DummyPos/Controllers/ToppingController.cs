using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DummyPos.Controllers
{
    public class ToppingController : Controller
    {
        private readonly IToppingRepo _repo;

        public ToppingController(IToppingRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAllToppings();
            return View(list);
        }

        // CREATE
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Toppings { Is_Active = true }); // Return empty model to the view
        }
        [HttpPost]
        public IActionResult Create(Toppings model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddTopping(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var topping = _repo.GetToppingById(id);
            if (topping == null) return NotFound();
            return View(topping);
        }

        [HttpPost]
        public IActionResult Edit(Toppings model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateTopping(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var topping = _repo.GetToppingById(id);
            if (topping == null) return NotFound();
            return View(topping);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            _repo.DeleteTopping(id);
            return RedirectToAction("Index");
        }

        // STATUS TOGGLES
        public IActionResult ActivateTopping(int id)
        {
            _repo.ActivateTopping(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateTopping(int id)
        {
            _repo.DeactivateTopping(id);
            return RedirectToAction("Index");
        }
    }
}