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
            List<Account> accounts = AccountManager.GetAllAccounts();

            if (accounts.Count == 0)
            {
                return NotFound("No accounts found.");
            }

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
            if (!AccountManager.AccountExists(acctNo))
            {
                return NotFound("Account not found.");
            }

            //updatedAccount.AcctNo = acctNo;
            AccountManager.UpdateAccount(acctNo, updatedAccount);
            return Content("updated account with newly entered values");
        }

        [HttpDelete("{acctNo}")]
        public IActionResult DeleteAccount(int acctNo)
        {
            if (!AccountManager.AccountExists(acctNo))
            {
                return NotFound("Account not found.");
            }

            AccountManager.DeleteAccount(acctNo);

            return Content("deleted account: " + acctNo);
        }
    }
}
    

