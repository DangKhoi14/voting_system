using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    public enum Role
    {
        Banned = -1,
        Administrator = 0,
        User = 1,
        Guest = 2
    }

    [Table("account")]
    public class Account
    {
        //public string AccountId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public DateOnly CreateAt { get; set; }
        public bool IsEmailVerified { get; set; }

        // Additional attributes
        public sbyte Role { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
        public string ProfilePictureUrl { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
