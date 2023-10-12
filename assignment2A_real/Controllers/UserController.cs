using assignment2A_real.Data;
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
            return View("LoggedIn", userProfile);
        }

        public IActionResult FailedLogin()
        {
            return View("FailedLogin");
        }

        public IActionResult AccountSummary(int acctNo)
        {
            Account account = AccountManager.GetAccountByAcctNo(acctNo);
            return View("AccountSummary", account);
        }

        public IActionResult EditUserProfile()
        {
            ViewBag.Message = TempData["Message"] as string;
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(ViewBag.Message);
            return View("EditUserProfile", userProfile);
        }

        public IActionResult UpdateUserProfile(string Email, long Phone, string Password)
        {
            ViewBag.Message = TempData["Message"] as string;
            try
            {
                UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(ViewBag.Message);

                if (userProfile != null)
                {
                    string oldname = userProfile.Name; 
                    userProfile.Email = Email;
                    userProfile.Phone = Phone;
                    userProfile.Password = Password;

                    UserProfileManager.UpdateUserProfile(oldname, userProfile);
                    return RedirectToAction("DetailChanged");
                }
                else
                {
                    ViewData["ErrorMessage"] = "User profile not found.";
                    return RedirectToAction("LoggedIn");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occurred while updating the user profile: " + ex.Message;
                return RedirectToAction("LoggedIn");
            }
        }
    }
}
