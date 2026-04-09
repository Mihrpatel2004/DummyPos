using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DummyPos.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepo _repo;

        public ItemController(IItemRepo repo)
        {
            _repo = repo;
        }

        // INDEX: Show List of Items
        /*public IActionResult Index()
        {
            var list = _repo.GetAllItems();
            return View(list);
        }*/
        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            AdminManageItemsViewModel model = new AdminManageItemsViewModel
            {
                SearchTerm = searchTerm,
                ItemList = _repo.SearchAdminItems(searchTerm)
            };

            return View(model);
        }
        // CREATE: Get and Post
        [HttpGet]
        public IActionResult Create()
        {
            // Populate the dropdown list for the View
            ViewBag.CategoryList = _repo.GetCategoryDropdown();
            return View(new Item());
        }

        [HttpPost]
        public IActionResult Create(Item model)
        {
            if (ModelState.IsValid)
            {
                // 1. Check if the user uploaded an image
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    // 2. Convert the uploaded file to a byte array for the database
                    using (var ms = new MemoryStream())
                    {
                        model.ImageUpload.CopyTo(ms);
                        model.Item_Image = ms.ToArray();
                    }
                }

                _repo.AddItem(model);
                return RedirectToAction("Index");
            }

            // If validation fails, we must reload the dropdown before returning to the view!
            ViewBag.CategoryList = _repo.GetCategoryDropdown();
            return View(model);
        }

        // EDIT: Get and Post
        public IActionResult Edit(int id)
        {
            var item = _repo.GetItemById(id);
            if (item == null) return NotFound();

            // Populate the dropdown list
            ViewBag.CategoryList = _repo.GetCategoryDropdown();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Item model)
        {
            if (ModelState.IsValid)
            {
                // Handle optional image upload during edit
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.ImageUpload.CopyTo(ms);
                        model.Item_Image = ms.ToArray();
                    }
                }
                // If model.ImageUpload is null, the Stored Procedure uses COALESCE to keep the old image

                _repo.UpdateItem(model);
                return RedirectToAction("Index");
            }

            // Reload dropdown if validation fails
            ViewBag.CategoryList = _repo.GetCategoryDropdown();
            return View(model);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var item = _repo.GetItemById(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            _repo.DeleteItem(id);
            return RedirectToAction("Index");
        }

        // STATUS TOGGLES
        public IActionResult ActivateItem(int id)
        {
            _repo.ActivateItem(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateItem(int id)
        {
            _repo.DeactivateItem(id);
            return RedirectToAction("Index");
        }
    }
}