using System.ComponentModel.DataAnnotations;

namespace voting_system_core.DTOs.Requests.Account
{
    public class ResetPasswordReq
    {
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
