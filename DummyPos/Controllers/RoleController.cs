using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace DummyPos.Controllers
{
    public class RoleController : Controller
    {
        // CHANGE THIS: Use the Interface, not the Class
        private readonly IRoleRepo _roleRepo;

        // CHANGE THIS: Inject the Interface
        public RoleController(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public IActionResult Index()
        {
            List<Role> roles = _roleRepo.GetAllRole();
            return View(roles);
        }
        /* [HttpGet]
         public IActionResult Details(int id)
         {
             // Validate ID
             if (id <= 0) return BadRequest();

             // Call Repository through the Interface
             var role = _roleRepo.GetRoleById(id);

             if (role == null)
             {
                 return NotFound();
             }

             return View(role);
         }*/
        // GET: Role/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var role = _roleRepo.GetRoleById(id);
            if (role == null) return NotFound();

            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                _roleRepo.UpdateRole(role);
                TempData["SuccessMessage"] = "Role updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Return an empty model so the checkbox is handled correctly
            return View(new Role { Is_Active = true });
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _roleRepo.AddRole(role);
                TempData["SuccessMessage"] = "New role created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // POST: Role/Delete/5
        /*    [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Delete(int id)
            {
                try
                {
                    _roleRepo.DeleteRole(id);
                    TempData["SuccessMessage"] = "Role deleted successfully!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error deleting role: It might be linked to existing users.";
                }

                return RedirectToAction(nameof(Index));
            }*/

        //---------------------------------------------------
        // ACTIVATE ROLE
        //---------------------------------------------------
        public IActionResult ActivateRole(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            _roleRepo.ActivateRole(id);

            TempData["SuccessMessage"] = "Role has been activated successfully!";

            return RedirectToAction("Index");
        }
        //---------------------------------------------------
        // DEACTIVATE ROLE
        //---------------------------------------------------
        public IActionResult DeactivateRole(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            _roleRepo.DeactivateRole(id);

            // Using TempData for a success message (optional but good for UX)
            TempData["SuccessMessage"] = "Role has been deactivated.";

            return RedirectToAction("Index");
        }


    }
}
