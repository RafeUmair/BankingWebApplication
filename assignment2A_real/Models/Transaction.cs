using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace assignment2A_real.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } 
        public decimal Amount { get; set; }

        [ForeignKey("Account")]
        public int AcctNo { get; set; }
        public string? Type { get; set; }
    }
}
