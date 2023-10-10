using Microsoft.AspNetCore.Mvc;
using assignment2A_real.Data;
using assignment2A_real.Models;
using System;

namespace assignment2A_real.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {


        [HttpGet("/api/Transactions")]
        public IActionResult GetAllTransactions()
        {
            // Retrieve all transactions from your data source
            List<Transaction> transactions = TransactionManager.GetAllTransactions();

              if (transactions.Count == 0)
                {
              return NotFound("No Transactions found.");
              }

            return Ok(transactions); 
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] Transaction transaction)
        {
            if (transaction == null || transaction.Amount <= 0)
            {
                return BadRequest("Invalid deposit request.");
            }

            if (!AccountManager.AccountExists(transaction.AcctNo))
            {
                return NotFound("Account not found.");
            }

            Account account = AccountManager.GetAccountByAcctNo(transaction.AcctNo);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            decimal newBalance = account.Bal + transaction.Amount;

            account.Bal = newBalance;

            Transaction depositTransaction = new Transaction
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                AcctNo = transaction.AcctNo,
                Type = "Deposit"
            };

            TransactionManager.InsertTransaction(depositTransaction);

            account.Transactions.Add(depositTransaction);

            AccountManager.UpdateAccount(account);

            return Ok($"Deposited {transaction.Amount:C} into account {transaction.AcctNo}. New balance: {account.Bal:C}");
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] Transaction transaction)
        {
            if (transaction == null || transaction.Amount <= 0)
            {
                return BadRequest("Invalid withdrawal request.");
            }

            if (!AccountManager.AccountExists(transaction.AcctNo))
            {
                return NotFound("Account not found.");
            }

            Account account = AccountManager.GetAccountByAcctNo(transaction.AcctNo);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            if (account.Bal < transaction.Amount)
            {
                return BadRequest("Insufficient balance for withdrawal.");
            }

            Transaction withdrawalTransaction = new Transaction
            {
                TransactionId = transaction.TransactionId,
                Amount = -transaction.Amount,
                AcctNo = transaction.AcctNo,
                Type = "withdraw"
            };

            account.Transactions.Add(withdrawalTransaction);

            // Update account balance
            account.Bal -= transaction.Amount;

            account.Transactions.Add(withdrawalTransaction);

            // Update the account and save changes to the database
            AccountManager.UpdateAccount(account);

            TransactionManager.InsertTransaction(withdrawalTransaction);



            return Ok($"Withdrawn {transaction.Amount:C} from account {transaction.AcctNo}. New balance: {account.Bal:C}");
        }
    }
}
