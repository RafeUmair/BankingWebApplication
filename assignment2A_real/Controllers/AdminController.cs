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
                    TempData["Message"] = userProfile.Name;
                    TempData["Message2"] = userProfile.Name;

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

        public IActionResult UserManagement()
        {
            var userProfiles = UserProfileManager.GetAllUserProfiles();
            return View("UserManagement", userProfiles);
        }

        public IActionResult EditProfile(string username)
        {
            ViewBag.Message2 = TempData["Message2"] as string;

            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(ViewBag.Message2);

            if (userProfile != null)
            {
                return View("EditProfile", userProfile);
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateProfile(string name, string email, string address, long phone, string password)
        {
            // Retrieve the old user profile by username
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(name);

            if (userProfile != null)
            {
                // Update the properties of the retrieved profile with the new values
                userProfile.Email = email;
                userProfile.Address = address;
                userProfile.Phone = phone; // Ensure phone is a long
                userProfile.Password = password;

                // Call the UpdateUserProfile method to save the updated profile
                UserProfileManager.UpdateUserProfile(name, userProfile);

                TempData["Message"] = "Profile updated successfully.";
                return RedirectToAction("LoggedIn");
            }

            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error");
        }
    }
}
