using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment2A_real.Models
{
    public class UserProfile
    {
        public string? Email { get; set; }
        [Key]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public long? Phone { get; set; }
        public byte[]? Picture { get; set; }
        public string? Password { get; set; }
        [ForeignKey("Account")]
        public int AcctNo { get; set; }
        public string? Type { get; set; }
    }
}

