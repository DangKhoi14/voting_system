namespace voting_system_core.DTOs.Requests.Account
{
    public class ChangePasswordReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
