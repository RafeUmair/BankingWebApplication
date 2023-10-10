using System.ComponentModel.DataAnnotations;

namespace assignment2A_real.Models
{
    public class UserProfile
    {   
        
        public string? Email { get; set; }
        [Key]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public long Phone { get; set; }
        public string? Picture { get; set; }
        public string? Password { get; set; }
    }
}
