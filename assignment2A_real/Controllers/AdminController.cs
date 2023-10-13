﻿using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
                    return RedirectToAction("LoggedIn", new { username = userProfile.Name });
                }
            }

            ViewData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("FailedLogin");
        }

        [HttpGet]
        public IActionResult LoggedIn(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile != null)
            {
                return View("LoggedInAdmin", userProfile);
            }

            return NotFound();
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

        [HttpGet]
        public IActionResult EditProfile(string name)
        {
           // string username = TempData["Message2"] as string; // Retrieve the username from TempData or another source
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(name);

            if (userProfile != null)
            {
                return View("EditProfile", userProfile);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateProfile(UserProfile userProfile)
        {
            if (userProfile != null)
            {
                UserProfileManager.UpdateUserProfile(userProfile.Name, userProfile);

                return View("LoggedInAdmin", userProfile);
            }

            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error");
        }

        [HttpPost]
        public IActionResult DeactivateUser(string Name)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(Name);

            if ((userProfile != null))
            {
                UserProfileManager.DeleteUserProfile(userProfile.Name); 

                return View("UserDeactivated", userProfile.Name);
            }
             return View("Error");
        }
    }
}