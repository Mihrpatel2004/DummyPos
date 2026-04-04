using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class TableController : Controller
    {
        private readonly ITableRepo _repo;

        public TableController(ITableRepo repo)
        {
            _repo = repo;
        }

        // 🚨 Requires BranchId so it knows which tables to show
        public IActionResult Index(int branchId)
        {
            ViewBag.BranchId = branchId;
            var list = _repo.GetTablesByBranch(branchId);
            return View(list);
        }

        [HttpGet]
        public IActionResult Create(int branchId)
        {
            return View(new RestaurantTable { Branch_Id = branchId, Status = "Available" });
        }

        [HttpPost]
        public IActionResult Create(RestaurantTable model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddTable(model);
                return RedirectToAction("Index", new { branchId = model.Branch_Id });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var table = _repo.GetTableById(id);
            if (table == null) return NotFound();
            return View(table);
        }

        [HttpPost]
        public IActionResult Edit(RestaurantTable model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateTable(model);
                return RedirectToAction("Index", new { branchId = model.Branch_Id });
            }
            return View(model);
        }

        public IActionResult Delete(int id, int branchId)
        {
            _repo.DeleteTable(id);
            return RedirectToAction("Index", new { branchId = branchId });
        }
    }
}