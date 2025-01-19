using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("account")]
    public class Account
    {
        [Key]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        //public string PasswordHash { get; set; }
        //public string Salt { get; set; }

        public string Email { get; set; }

        public DateOnly CreateAt { get; set; }

        public bool IsEmailVerified { get; set; }

        // Additional attributes
        public sbyte Role { get; set; }

        public DateTime LastLogin { get; set; }

        //public bool IsActive { get; set; } = true;

        public string ProfilePictureUrl { get; set; }

        public string ResetPasswordToken { get; set; }
    }
}
