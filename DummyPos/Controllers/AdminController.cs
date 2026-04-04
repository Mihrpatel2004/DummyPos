using DummyPos.Filters;
using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace DummyPos.Controllers
{
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(LogActivityFilter))]
    public class AdminController : Controller
    {
        private readonly IStaffRepo _staffRepo;
        private readonly IAdminRepo _adminRepo;
        public AdminController(IStaffRepo staffRepo, IAdminRepo adminRepo)
        {
            _staffRepo = staffRepo;
            _adminRepo = adminRepo;
        }

        //-------------------
        // profile
        //---------------------

        [HttpGet]
        public IActionResult Profile()
        {
            // 1. Get the logged-in Admin's Staff_Id from the secure cookie claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            int adminId = Convert.ToInt32(userIdString);

            // 2. Fetch the admin's details using your existing Staff Repository
            var adminProfile = _staffRepo.GetStaffById(adminId);

            if (adminProfile == null)
            {
                return NotFound("Admin profile not found.");
            }
            ViewBag.Branches = _adminRepo.GetAllBranches();
            // 3. Send the data to the Profile view
            return View(adminProfile);
        }

        //---------------------------------------------------
        // DASHBOARD
        //---------------------------------------------------
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public IActionResult Dashboard(int? branchId)
        {
            ViewBag.Branches = _adminRepo.GetAllBranches();
            ViewBag.SelectedBranch = branchId;

            var model = _adminRepo.GetDashboardMetrics(branchId);
            return View(model);
        }

        //---------------------------------------------------
        // STAFF LIST
        //---------------------------------------------------
        public IActionResult StaffList()
        {
            var staff = _staffRepo.GetAllStaff();
            return View(staff);
        }

        //---------------------------------------------------
        // STAFF DETAILS
        //---------------------------------------------------
        [HttpGet]
        public IActionResult StaffDetails(int id)
        {
            if (id <= 0) return BadRequest();

            var staff = _staffRepo.GetStaffById(id);
            if (staff == null) return NotFound();

            return View(staff);
        }
        /*   // GET: Admin/StaffDetails/5
           [HttpGet]
           public IActionResult StaffDetails(int id)
           {
               // Safety check for ID
               if (id <= 0)
               {
                   return BadRequest();
               }

               // Fetch data from Repository
               var staff = _staffRepo.GetStaffById(id);

               // If no record found in DB
               if (staff == null)
               {
                   return NotFound();
               }

               // Return the view with the staff object
               return View(staff);
           }*/

        //---------------------------------------------------
        // CREATE STAFF - GET
        //---------------------------------------------------
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _staffRepo.GetRoles();
            ViewBag.Branches = _staffRepo.GetBranch();
            return View();
        }

        //---------------------------------------------------
        // CREATE STAFF - POST
        //---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Staffs staff, string plainPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Passes the plainPassword to the Repo, where it gets hashed
                    _staffRepo.AddStaff(staff, plainPassword);
                    TempData["SuccessMessage"] = "Staff member added successfully!";
                    return RedirectToAction("StaffList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving data: " + ex.Message);
                }
            }

            // Reload dropdowns if validation fails
            ViewBag.Roles = _staffRepo.GetRoles();
            ViewBag.Branches = _staffRepo.GetBranch();
            return View(staff);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Staffs staff, string plainPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _staffRepo.AddStaff(staff, plainPassword);
                    TempData["SuccessMessage"] = "Staff member added successfully!";
                    return RedirectToAction("StaffList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving data: " + ex.Message);
                }
            }

            // If we got here, something failed; reload dropdowns
            ViewBag.Roles = _staffRepo.GetRoles();
            ViewBag.Branches = _staffRepo.GetBranch();
            return View(staff);
        }*/
        [HttpGet]
        public IActionResult EditStaff(int id)
        {
            var staff = _staffRepo.GetStaffById(id);
            if (staff == null) return NotFound();

            ViewBag.Roles = _staffRepo.GetRoles();
            ViewBag.Branches = _staffRepo.GetBranch();

            return View(staff);
        }

        //---------------------------------------------------
        // EDIT STAFF - POST
        //---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStaff(Staffs staff)
        {
            // We do not update passwords here. That requires a separate "Reset Password" feature.
            if (ModelState.IsValid)
            {
                _staffRepo.UpdateStaff(staff);
                TempData["SuccessMessage"] = "Staff updated successfully";
                return RedirectToAction("StaffList");
            }

            ViewBag.Roles = _staffRepo.GetRoles();
            ViewBag.Branches = _staffRepo.GetBranch();
            return View(staff);
        }

        //---------------------------------------------------
        // DEACTIVATE STAFF (Toggle Status)
        //---------------------------------------------------
        public IActionResult DeactivateStaff(int id)
        {
            if (id <= 0) return BadRequest();

            _staffRepo.DeactivateStaff(id);
            return RedirectToAction("StaffList");
        }

        // 1. Show all Branches (Cards)
        public IActionResult BranchDirectory()
        {
            var branches = _adminRepo.GetAllBranches();
            return View(branches);
        }

        // 2. Show Staff List for a specific branch (Table)
        public IActionResult BranchStaffList(int branchId, string branchName)
        {
            ViewBag.BranchName = branchName ?? "Branch";
            var staffList = _adminRepo.GetStaffByBranchId(branchId);
            return View(staffList);
        }

        // 3. Show specific Staff Details
        public IActionResult ViewStaffDetails(int staffId)
        {
            var staff = _adminRepo.GetStaffDetailsById(staffId);
            return View(staff);
        }

        //---------------------------------------------------
        // ALL ORDERS REPORT
        //---------------------------------------------------
        [HttpGet]
        public IActionResult AllOrders(int branchId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            // Save selections so the dropdowns stay selected after clicking 'Filter'
            ViewBag.Branches = _adminRepo.GetAllBranches();
            ViewBag.SelectedBranch = branchId;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            var orders = _adminRepo.GetFilteredOrders(branchId, startDate, endDate);
            return View(orders);
        }

        [HttpGet]
        public IActionResult GetOrderItems(int orderId)
        {
            var items = _adminRepo.GetOrderItemsAdmin(orderId);
            return Json(items);
        }

    }


    
}