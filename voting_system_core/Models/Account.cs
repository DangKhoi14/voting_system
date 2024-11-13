

using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("account")]
    public class Account
    {
        public string AccountID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsEmailVerified { get; set; }

        // Additional attributes
        public string Role { get; set; } = "User";
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
        public string ProfilePictureUrl { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
