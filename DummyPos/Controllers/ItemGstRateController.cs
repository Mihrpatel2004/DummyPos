using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;

namespace DummyPos.Controllers
{
    public class ItemGstRateController : Controller
    {
        private readonly IItemGstRateRepo _repo;

        public ItemGstRateController(IItemGstRateRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAllItemGstRates();
            return View(list);
        }
        /* => View(_repo.GetAllItemGstRates());*/

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ItemList = _repo.GetItemsDropdown();
            ViewBag.ServiceTypeList = _repo.GetServiceTypesDropdown();
            return View(new ItemGstRate());
        }

        [HttpPost]
        public IActionResult Create(ItemGstRate model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddItemGstRate(model);
                return RedirectToAction("Index");
            }
            ViewBag.ItemList = _repo.GetItemsDropdown();
            ViewBag.ServiceTypeList = _repo.GetServiceTypesDropdown();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var rate = _repo.GetItemGstRateById(id);
            if (rate == null) return NotFound();

            ViewBag.ItemList = _repo.GetItemsDropdown();
            ViewBag.ServiceTypeList = _repo.GetServiceTypesDropdown();
            return View(rate);
        }

        [HttpPost]
        public IActionResult Edit(ItemGstRate model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateItemGstRate(model);
                return RedirectToAction("Index");
            }
            ViewBag.ItemList = _repo.GetItemsDropdown();
            ViewBag.ServiceTypeList = _repo.GetServiceTypesDropdown();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var rate = _repo.GetItemGstRateById(id);
            if (rate == null)
            {
                return NotFound();
            }
            return View(rate);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteItemGstRate(id);
            return RedirectToAction("Index");
        }
    }
}