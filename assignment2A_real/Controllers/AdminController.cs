using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Xml.Linq;

namespace assignment2A_real.Controllers
{

    public class AdminController : Controller
    {
        private static List<AdminLog> adminlogs = new List<AdminLog>();
        private static String loggedInAdminName;

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
                LogAdminAction(userProfile.Name, "Admin Logged In");
                loggedInAdminName = userProfile.Name;
                return View("LoggedInAdmin", userProfile);
            }
           // LogAdminAction(userProfile.Name, "Admin Failed Login");

            return NotFound();
        }

        public IActionResult FailedLogin()
        {
            return View("FailedLoginAdmin");
        }

        public IActionResult UserManagement()
        {
            var userProfiles = UserProfileManager.GetAllUserProfiles();
            LogAdminAction(loggedInAdminName, "Viewed User Management");
            return View("UserManagement", userProfiles);
        }

        [HttpGet]
        public IActionResult EditProfile(string name)
        {
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(name);

            if (userProfile != null)
            {
                LogAdminAction(loggedInAdminName, "Viewed Edit Profile for self");

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
                LogAdminAction(loggedInAdminName, "Updated Self Profile " );

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
                LogAdminAction(loggedInAdminName, "Deactivated User: " + userProfile.Name);
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
                LogAdminAction(loggedInAdminName, "Viewed Edit Select Profile for USER : " + userProfile.Name);
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
                LogAdminAction(loggedInAdminName, "Updated Select Profile: " + userProfile.Name);
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
                    LogAdminAction(loggedInAdminName, "Created User: " + userProfile.Name);

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

        private void LogAdminAction(string adminName, string action)
        {
            var logEntry = new AdminLog
            {
                Time = DateTime.Now,
                AdminName = adminName,
                Action = action
            };

            adminlogs.Add(logEntry);
        }

        public IActionResult AdminLogs()
        {
            return View(adminlogs);
        }
    }
}