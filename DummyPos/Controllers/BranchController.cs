using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace DummyPos.Controllers
{

    public class BranchController : Controller
    {
        private readonly IBranchRepo _branchRepo;

        // Constructor injection
        public BranchController(IBranchRepo branchRepo)
        {
            _branchRepo = branchRepo;
        }
        // GET: Branch/Create

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Branch { Is_Active = true }); // Return empty model to the view
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _branchRepo.AddBranch(branch);
                TempData["Success"] = "Branch added successfully!";
                return RedirectToAction("BranchList");
            }
            return View(branch); // If validation fails, return to form with errors
        }
        // GET: Branch/Index
        public IActionResult BranchList()
        {
            var branchList = _branchRepo.GetAllBranchList();
            return View(branchList);
        }
        // GET: Branch/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var branch = _branchRepo.GetBranchById(id);
            if (branch == null) return NotFound();
            return View(branch);
        }

        // POST: Branch/Edit
        [HttpPost]
        public IActionResult Edit(Branch branch)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    _branchRepo.UpdateBranch(branch);
                    TempData["SuccessMessage"] = "Branch updated successfully!";
                    return RedirectToAction(nameof(BranchList));
                }

            }
            return View(branch);
        }
        // GET: Branch/Details/5
        public IActionResult Details(int id)
        {
            var branch = _branchRepo.GetBranchDetails(id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }
        // POST: Branch/ToggleStatus/5
        // Action for Deactivate button
        public IActionResult DeactivateRole(int id)
        {
            _branchRepo.DeactivateBranch(id);
            return RedirectToAction("BranchList");
        }

        // Action for Activate button
        public IActionResult ActivateRole(int id)
        {
            _branchRepo.ActivateBranch(id);
            return RedirectToAction("BranchList");
        }
    }
}
