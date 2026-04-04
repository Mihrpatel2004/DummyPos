using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using DummyPos.Helpers; 

namespace DummyPos.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthRepo _authRepo;

        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }
        [HttpGet]
        public IActionResult Login()
        {
            // If they are already logged in, send them to their proper working pages!
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (User.IsInRole("Staff"))
                {
                    return RedirectToAction("Order", "Staffs"); // Matches your POST redirect!
                }

                // Default fallback
                return RedirectToAction("Index", "Home");
            }

            // If they are NOT logged in, show them the login form
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authRepo.ValidateLogin(model.Email, model.Password);

                if (user != null)
                {
                    // 1. Setup the secure cookie with their Role and Name
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Staff_Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Staff_Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.RoleName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    // 2. Role-Based Redirection
                    if (user.RoleName == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.RoleName == "Staff")
                    {
                        return RedirectToAction("Order", "Staffs");
                    }

                    return RedirectToAction("Pizza", "Home");
                }

                ModelState.AddModelError("", "Invalid Email or Password");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Pizza", "Home");
        }
        // Add this right above your Logout method!
        public IActionResult AccessDenied()
        {
            return View();
        }
        ////
        ///

        [HttpGet]
        public IActionResult ChangePassword(string email)
        {
            // Grabs the email from the login page and pre-fills it
            var model = new ChangePasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            // 1. Check if the new passwords match
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "New passwords do not match!";
                return View(model);
            }

            // 2. 🚨 USE YOUR HELPER HERE: Hash both passwords before sending to the database
            string hashedOldPassword = PasswordHelper.HashPassword(model.OldPassword);
            string hashedNewPassword = PasswordHelper.HashPassword(model.NewPassword);

            // 3. Send the hashed versions to your Repository
            bool success = _authRepo.ChangePassword(model.Email, hashedOldPassword, hashedNewPassword);

            if (success)
            {
                // Success! Send them back to the login screen with a nice message.
                TempData["SuccessMessage"] = "Password changed successfully! Please log in.";
                return RedirectToAction("Login");
            }

            // 4. If success is false, they typed the wrong email or wrong old password
            ViewBag.Error = "Invalid Email or Incorrect Old Password.";
            return View(model);
        }
    }
}