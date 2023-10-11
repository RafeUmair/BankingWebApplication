using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    public class AdminController : Controller
    {
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile != null && userProfile.Type == "admin")
            {
                if (password == userProfile.Password)
                {
                    TempData["Message"] = userProfile.Name; // Store in TempData
                    return RedirectToAction("LoggedIn");
                }
            }

            ViewData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("FailedLogin");
        }

        public IActionResult LoggedIn()
        {
            ViewBag.Message = TempData["Message"] as string;
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(ViewBag.Message);
            return View("LoggedInAdmin", userProfile);
        }

        public IActionResult FailedLogin()
        {
            return View("FailedLoginAdmin");
        }
    }
}
