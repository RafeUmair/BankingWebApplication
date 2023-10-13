using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
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

        public IActionResult Transactions()
        {
            List<Transaction> transactions = TransactionManager.GetAllTransactions(); 

            return View("Transactions", transactions);
        }


        [HttpGet]
        public IActionResult EditSelectProfile(string name)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(name);

            if (userProfile != null)
            {
                return View("EditSelectProfile", userProfile);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateSelectProfile(UserProfile userProfile)
        {
            if (userProfile != null)
            {
                UserProfileManager.UpdateUserProfile(userProfile.Name, userProfile);
                List<UserProfile> userProfilelist = UserProfileManager.GetAllUserProfiles().ToList(); 
                return View("UserManagement", userProfilelist);
            }

            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error");
        }

        [HttpPost]
        public IActionResult CreateUser(UserProfile userProfile)
        {
            if (userProfile != null)
            {
                if (AccountManager.AccountExists(userProfile.AcctNo))
                {
                    UserProfileManager.InsertUserProfile(userProfile);
                    List<UserProfile> userProfilelist = UserProfileManager.GetAllUserProfiles().ToList();
                    return View("UserManagement", userProfilelist);
                }
                ViewData["ErrorMessage"] = "AccountNo does not exist";
                return View("Error", new ErrorViewModel { RequestId = "Error, account number does not exist" }); 
            }
            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error", new ErrorViewModel { RequestId = "Failed to update the profile." });
            }

        [HttpGet]
        public IActionResult SearchUsers(String search)
        {
            var searchResults = new List<object>(); 

            if (string.IsNullOrEmpty(search))
            {
                return View("Error", new ErrorViewModel { RequestId = "No search term entered" });
            }


            if (int.TryParse(search, out int accountNumber))
            {
                Account account = AccountManager.GetAccountByAcctNo(accountNumber);
                if (account != null)
                {
                    searchResults.Add(account); 
                }
            } 
            else
            {
                UserProfile profile = UserProfileManager.GetUserProfileByUsername(search);
                if (profile != null)
                {
                    searchResults.Add(profile); 
                }
            }
            return View("SearchResultsView", searchResults);

        }
    }
}