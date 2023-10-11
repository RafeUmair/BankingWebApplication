using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace assignment2A_real.Models
{
    public class Account
    {

        public decimal Bal { get; set; } 
        [Key]
            public int AcctNo { get; set; }
            public int Pin { get; set; }
            public string? Fname { get; set; }
            public string? Lname { get; set; }

        // [ForeignKey("UserProfile")]
        //  public string? Name { get; set; }

         [ForeignKey("Transaction")]
        public int TransactionId { get; set; }
    }
}

