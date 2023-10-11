﻿using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile != null && userProfile.Type == "user")
            {
                if (password == userProfile.Password)
                {
                    TempData["Message"] = userProfile.Name; 
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
            userProfile.isLoggedin = true;
            return View("LoggedIn", userProfile);
        }

        public IActionResult FailedLogin()
        {
            return View("FailedLogin");
        }
    }
}
