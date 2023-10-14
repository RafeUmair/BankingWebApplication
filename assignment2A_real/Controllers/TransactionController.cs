using Microsoft.AspNetCore.Mvc;
using assignment2A_real.Data;
using assignment2A_real.Models;
using System;

namespace assignment2A_real.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
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

            if(TransactionManager.TransactionExists(transaction.TransactionId))
            {
                return BadRequest("TransactionId already exists.");
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
            account.TransactionId = depositTransaction.TransactionId;
            AccountManager.UpdateAccount(account.AcctNo, account);
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

            if(TransactionManager.TransactionExists(transaction.TransactionId))
            {
                return BadRequest("TransactionId already exists.");
            }

            decimal newBalance = account.Bal - transaction.Amount;
            account.Bal = newBalance;

            Transaction withdrawalTransaction = new Transaction
            {
                TransactionId = transaction.TransactionId,
                Amount = -transaction.Amount,
                AcctNo = transaction.AcctNo,
                Type = "Withdraw"
            };

            TransactionManager.InsertTransaction(withdrawalTransaction);
            account.TransactionId = withdrawalTransaction.TransactionId;
            AccountManager.UpdateAccount(account.AcctNo, account);
            return Ok($"Withdrawn {transaction.Amount:C} from account {transaction.AcctNo}. New balance: {account.Bal:C}");
        }

        [HttpGet("{AcctNo}")]
        public IActionResult GetAllTransactionsByAcctNo(int acctNo)
        {          
            List<Transaction> transactions = TransactionManager.GetTransactionsByAcctNo(acctNo);

            if (transactions.Count == 0)
            {
                return NotFound("No Transactions found for the specified account number.");
            }

            return Ok(transactions);
        }
    }
}
