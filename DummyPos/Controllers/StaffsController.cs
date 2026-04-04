using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace DummyPos.Controllers
{
    [Authorize(Roles = "Staff,Admin")]
    public class StaffsController : Controller
    {
        private readonly IStaffsRepos _staffsRepos;
        private readonly IItemRepo _itemRepo;
        private readonly IItemCategoryRepo _itemCatRepo;
        private readonly IServiceTypeRepo _servicereRepo;
        private readonly IToppingRepo _toppingRepo;
        private readonly IOfferRepo _offerRepo;
        private readonly IItemGstRateRepo _gstRepo;
        private readonly IStaffRepo _staffRepoCore;
        public StaffsController(IStaffsRepos staffRepo, IStaffRepo staffRepoCore, IItemRepo itemRepo, IItemCategoryRepo itemCatRepo, IServiceTypeRepo servicereRepo, IToppingRepo toppingRepo, IOfferRepo offerRepo, IItemGstRateRepo gstRepo)
        {
            _staffRepoCore = staffRepoCore;
            _staffsRepos = staffRepo;
            _itemRepo = itemRepo;
            _itemCatRepo = itemCatRepo;
            _servicereRepo = servicereRepo;
            _toppingRepo = toppingRepo;
            _offerRepo = offerRepo;
            _gstRepo = gstRepo;
        }
        //---------------------------------------------------
        // STAFF PROFILE
        //---------------------------------------------------
        [HttpGet]
        public IActionResult Profile()
        {
            // 1. Get the logged-in Staff ID securely from the cookie
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            int staffId = Convert.ToInt32(userIdString);

            // 2. Fetch the profile using the method we already fixed!
            var staffProfile = _staffRepoCore.GetStaffById(staffId);

            if (staffProfile == null)
            {
                return NotFound("Staff profile not found.");
            }

            return View(staffProfile);
        }
        [HttpGet]
        public IActionResult Order(int? orderId)
        {
            int currentStaffId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int branchId = _staffsRepos.GetBranchIdByStaffId(currentStaffId); // 🚨 Dynamic

            ViewBag.BranchName = _staffsRepos.GetBranchNameByStaffId(currentStaffId);
            ViewBag.StaffName = User.Identity.Name ?? "Staff Member";

            ViewBag.Categories = _itemCatRepo.GetAllItemCategories();
            ViewBag.MenuItems = _itemRepo.GetAllItems().Where(i => i.Is_Active).ToList();
            ViewBag.ServiceTypes = _servicereRepo.GetAllServiceTypes();
            ViewBag.Toppings = _toppingRepo.GetAllToppings();
            ViewBag.Offers = _offerRepo.GetAllOffers();
            ViewBag.GstRates = _gstRepo.GetAllItemGstRates();
            ViewBag.ActiveDiscount = _staffsRepos.GetTodayActiveDiscount();

            ViewBag.Tables = _staffsRepos.GetTablesForPos(branchId); // 🚨 Load tables for THIS branch only!
            ViewBag.OrderSources = _staffsRepos.GetActiveOrderSources();
            ViewBag.PaymentTypes = _staffsRepos.GetActivePaymentTypes();

            if (orderId.HasValue && orderId.Value > 0)
            {
                ViewBag.ExistingOrderId = orderId.Value;
                ViewBag.ExistingOrderData = _staffsRepos.GetOrderDetailsById(orderId.Value);
            }
            else
            {
                ViewBag.ExistingOrderId = 0;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SubmitOrder([FromBody] CheckoutViewModel payload, [FromQuery] int existingOrderId = 0)
        {
            try
            {
                // 1. Get the logged-in Staff ID
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim)) return Json(new { success = false, message = "Session expired. Please log in." });

                int staffId = Convert.ToInt32(userIdClaim);
                int branchId = _staffsRepos.GetBranchIdByStaffId(staffId);

                // 2. 🚨 SAFETY FALLBACKS: Stop SQL from crashing if JS sends empty data
                if (payload.Payment_Type_Id <= 0) payload.Payment_Type_Id = 1;
                if (payload.Source_Id <= 0) payload.Source_Id = 1;

                if (existingOrderId > 0)
                {
                    bool success = _staffsRepos.UpdateOrder(existingOrderId, payload);
                    if (success)
                    {
                        return Json(new { success = true, orderId = existingOrderId });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to update order in database." });
                    }
                }
                else
                {
                    // 3. 🚨 FIXED: We capture the newOrderId (int) here, NOT a string!
                    int newOrderId = _staffsRepos.PlaceOrder(payload, staffId, branchId);

                    return Json(new { success = true, orderId = newOrderId });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Receipt(int orderId)
        {
            var receipt = _staffsRepos.GetReceipt(orderId);
            return View(receipt);
        }

        [HttpGet]
        public IActionResult GetCustomerByPhone(string phone)
        {
            string customerName = _staffsRepos.GetCustomerNameByPhone(phone);
            if (!string.IsNullOrEmpty(customerName))
            {
                return Json(new { success = true, name = customerName });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult RunningTables()
        {
            // 🚨 FIXED: No more hardcoded 1! Looks up the staff's exact branch.
            int currentStaffId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int branchId = _staffsRepos.GetBranchIdByStaffId(currentStaffId);

            var openTables = _staffsRepos.GetOpenOrders(branchId);
            return View(openTables);
        }

        [HttpGet]
        public IActionResult KitchenDisplay()
        {
            // 🚨 FIXED: No more hardcoded 1! Looks up the staff's exact branch.
            int currentStaffId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int branchId = _staffsRepos.GetBranchIdByStaffId(currentStaffId);

            var kitchenOrders = _staffsRepos.GetKitchenOrders(branchId);
            return View(kitchenOrders);
        }

        [HttpGet]
        public IActionResult AddOrderFeedback(int orderId)
        {
            ViewBag.FeedbackTypes = _staffsRepos.GetFeedbackTypesForPos();

            var model = new ManualFeedbackViewModel
            {
                Order_Id = orderId,
                Rating = 5
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrderFeedback(ManualFeedbackViewModel model)
        {
            if (model.Order_Id > 0)
            {
                bool success = _staffsRepos.SaveOrderFeedback(model.Order_Id, model.Feedback_Type_Id, model.Rating, model.Comments);

                if (success)
                {
                    TempData["SuccessMsg"] = "Feedback Saved Successfully!";
                    return RedirectToAction("Receipt", new { orderId = model.Order_Id });
                }
            }

            ViewBag.FeedbackTypes = _staffsRepos.GetFeedbackTypesForPos();
            TempData["ErrorMsg"] = "Failed to save feedback.";
            return View(model);
        }
    }
}