using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login(string username, string password)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile != null && userProfile.Type == "admin")
            {
                if (password == userProfile.Password)
                {
                    return RedirectToAction("LoggedIn");
                }
            }

            ViewData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("FailedLogin");
        }

        public IActionResult LoggedIn()
        {
            return View("LoggedIn");
        }

        public IActionResult FailedLogin()
        {
            return View("FailedLogin");
        }
    }
}
