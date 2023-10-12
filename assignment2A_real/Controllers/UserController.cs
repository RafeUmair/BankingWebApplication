using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

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
                    TempData["Message2"] = userProfile.Name;
                    TempData["Message3"] = userProfile.AcctNo;
                    TempData["Message4"] = userProfile.AcctNo;
                    TempData["Message5"] = userProfile.AcctNo;

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

        public IActionResult AccountSummary()
        {
            if (TempData.ContainsKey("Message3"))
            {
                int acctNo = (int)TempData["Message3"];
                Account account = AccountManager.GetAccountByAcctNo(acctNo);

                if (account != null)
                {
                    return View("AccountSummary", account);
                }
            }

            return NotFound();
        }

        public IActionResult EditProfile()
        {
            string username = TempData["Message2"] as string;
            UserProfile userProfile = UserProfileManager.GetUserProfileByUsername(username);

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
                return View("DetailChange");
            }

            ViewData["ErrorMessage"] = "Failed to update the profile.";
            return View("Error");
        }

        public IActionResult TransactionHistory()
        {
            if (TempData.ContainsKey("Message4"))
            {
                int acctNo = (int)TempData["Message4"];
                List<Transaction> transactions = TransactionManager.GetTransactionsByAcctNo(acctNo);

                if (transactions != null && transactions.Count > 0)
                {
                    return View("TransactionHistory", transactions);
                }
            }

            return View("TransactionHistory");
        }

        public IActionResult Transfer()
        {
            TempData.ContainsKey("Message5");
            int acctNo = (int)TempData["Message5"];
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
        public IActionResult Send(int sourceAcctNo, int destinationAcctNo, decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid transfer amount.");
            }

            if (!AccountManager.AccountExists(sourceAcctNo) || !AccountManager.AccountExists(destinationAcctNo))
            {
                return NotFound("One or more accounts not found.");
            }

            TransactionManager.SendFunds(sourceAcctNo, destinationAcctNo, amount);

            return View("FundsTransferred");
        }
    }
}
