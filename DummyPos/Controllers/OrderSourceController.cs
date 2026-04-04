using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

namespace DummyPos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderSourceController : Controller
    {
        private readonly IOrderSourceRepo _repo;

        public OrderSourceController(IOrderSourceRepo repo) => _repo = repo;

        public IActionResult Index() => View(_repo.GetAllOrderSources());

        public IActionResult Create() => View(new OrderSource());

        [HttpPost]
        public IActionResult Create(OrderSource model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddOrderSource(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var source = _repo.GetOrderSourceById(id);
            if (source == null) return NotFound();
            return View(source);
        }

        [HttpPost]
        public IActionResult Edit(OrderSource model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateOrderSource(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var source = _repo.GetOrderSourceById(id);
            if (source == null) return NotFound();
            return View(source);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteOrderSource(id);
            return RedirectToAction("Index");
        }

        public IActionResult ActivateSource(int id)
        {
            _repo.ActivateOrderSource(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateSource(int id)
        {
            _repo.DeactivateOrderSource(id);
            return RedirectToAction("Index");
        }
    }
}