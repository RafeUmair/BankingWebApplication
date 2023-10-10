using assignment2A_real.Data;
using assignment2A_real.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("createAccount")]
        public ActionResult<Account> CreateAccount(Account account)
        {
            if (AccountManager.AccountExists(account.AcctNo))
            {
                return Conflict($"An account with AcctNo {account.AcctNo} already exists.");
            }

            AccountManager.Insert(account);

            return CreatedAtAction(nameof(GetAccountByAcctNo), new { acctNo = account.AcctNo }, account);
        }

        [HttpGet("/api/Accounts")]
        public IActionResult GetAllAccounts()
        {
            // Retrieve all accounts from your data source
            List<Account> accounts = AccountManager.GetAllAccounts();

            // Check if any accounts were found
            if (accounts.Count == 0)
            {
                return NotFound("No accounts found.");
            }

            // Return the list of accounts as JSON
            return Ok(accounts);
        }

        [HttpGet("{acctNo}")]
        public ActionResult<Account> GetAccountByAcctNo(int acctNo)
        {
            Account account = AccountManager.GetAccountByAcctNo(acctNo);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(account);
        }

        [HttpPut("{acctNo}")]
        public IActionResult UpdateAccount(int acctNo, Account updatedAccount)
        {
            // Check if the account exists
            if (!AccountManager.AccountExists(acctNo))
            {
                return NotFound("Account not found.");
            }

            // Update the account
            updatedAccount.AcctNo = acctNo; // Ensure AcctNo is not changed
            AccountManager.UpdateAccount(updatedAccount);

            return NoContent();
        }

        [HttpDelete("{acctNo}")]
        public IActionResult DeleteAccount(int acctNo)
        {
            // Check if the account exists
            if (!AccountManager.AccountExists(acctNo))
            {
                return NotFound("Account not found.");
            }

            // Delete the account
            AccountManager.DeleteAccount(acctNo);

            return NoContent();
        }
    }
}
    

