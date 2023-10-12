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

        public IActionResult UpdateUserProfile(string name, string Email, long Phone, string Password)
        {
            try
            {
                UserProfile userProfile = UserProfileManager.GetUserProfileByEmail(name);

                if (userProfile != null)
                {
                    userProfile.Email = Email;
                    userProfile.Phone = Phone;
                    userProfile.Password = Password;

                    UserProfileManager.UpdateUserProfile(userProfile.Name, userProfile);
                    return RedirectToAction("DetailChange");
                }
                else
                {
                    ViewData["ErrorMessage"] = "User profile not found.";
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occurred while updating the user profile: " + ex.Message;
            }

            return View("FailedLogin");
        }
    }
}
