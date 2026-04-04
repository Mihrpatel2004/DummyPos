using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

namespace DummyPos.Controllers
{
    [Authorize(Roles = "Admin")] // Locks this down to Admins only
    public class OrderStatusController : Controller
    {
        private readonly IOrderStatusRepo _repo;

        public OrderStatusController(IOrderStatusRepo repo) => _repo = repo;

        public IActionResult Index() => View(_repo.GetAllOrderStatuses());

        public IActionResult Create() => View(new OrderStatus());

        [HttpPost]
        public IActionResult Create(OrderStatus model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddOrderStatus(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var status = _repo.GetOrderStatusById(id);
            if (status == null) return NotFound();
            return View(status);
        }

        [HttpPost]
        public IActionResult Edit(OrderStatus model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateOrderStatus(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var status = _repo.GetOrderStatusById(id);
            if (status == null) return NotFound();
            return View(status);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteOrderStatus(id);
            return RedirectToAction("Index");
        }

        public IActionResult ActivateStatus(int id)
        {
            _repo.ActivateOrderStatus(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateStatus(int id)
        {
            _repo.DeactivateOrderStatus(id);
            return RedirectToAction("Index");
        }
    }
}