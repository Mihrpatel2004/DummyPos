using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;

namespace DummyPos.Controllers
{
    public class ItemCategoryController : Controller
    {
        private readonly IItemCategoryRepo _repo;

        public ItemCategoryController(IItemCategoryRepo repo)
        {
            _repo = repo;
        }

        // INDEX: Show List
        public IActionResult Index() 
        {
            var list = _repo.GetAllItemCategories();
            return View(list);
         }

        // CREATE: Get and Post
        // NOTE: Returning explicitly 'new ItemCategory()' prevents the InvalidOperationException
      
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ItemCategory { Is_Active = true});
        }
        [HttpPost]
        public IActionResult Create(ItemCategory model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddItemCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // EDIT: Get and Post
        public IActionResult Edit(int id)
        {
            var category = _repo.GetItemCategoryById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(ItemCategory model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateItemCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var category = _repo.GetItemCategoryById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            _repo.DeleteItemCategory(id);
            return RedirectToAction("Index");
        }

        // ACTIVATE / DEACTIVATE
        public IActionResult ActivateCategory(int id)
        {
            _repo.ActivateItemCategory(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateCategory(int id)
        {
            _repo.DeactivateItemCategory(id);
            return RedirectToAction("Index");
        }
    }
}