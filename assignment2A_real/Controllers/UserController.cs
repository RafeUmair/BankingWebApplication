using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using System.Xml.Linq;

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
                return View("LoggedInUser", userProfile);
            }

            return NotFound();
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
                return View("LoggedInUser", userProfile);
            }

            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error");
        }

        public IActionResult TransactionHistory(int acctNo, string sort)
        {
            List<Transaction> transactions = TransactionManager.GetTransactionsByAcctNo(acctNo);

            if (sort == "oldest")
            {
                transactions = transactions.OrderBy(t => t.Date).ToList();
            }
            else if (sort == "newest")
            {
                transactions = transactions.OrderByDescending(t => t.Date).ToList();
            }

            return View("TransactionHistory", transactions);
        }

        public IActionResult Transfer(int acctNo)
        {
            Account userAccount = AccountManager.GetAccountByAcctNo(acctNo);

            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }

            List<Account> accounts = AccountManager.GetAllAccounts();

            if (accounts != null && accounts.Count > 0)
            {
                ViewBag.SourceAccount = userAccount;
                ViewBag.Accounts = new SelectList(accounts, "AcctNo", "AcctNo");

                return View("Transfer", userAccount); 
            }

            return View("Transfer");
        }

        [HttpPost]
        public IActionResult Send(int sourceAcctNo, int destinationAcctNo, decimal amount, string description)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid transfer amount.");
            }

            if (!AccountManager.AccountExists(sourceAcctNo) || !AccountManager.AccountExists(destinationAcctNo))
            {
                return NotFound("One or more accounts not found.");
            }

            TransactionManager.SendFunds(sourceAcctNo, destinationAcctNo, amount, description); 
            UserProfile userProfile = UserProfileManager.GetUserProfileByAcctNo(sourceAcctNo);
            return View("LoggedInUser", userProfile);
        }       
    }
}
