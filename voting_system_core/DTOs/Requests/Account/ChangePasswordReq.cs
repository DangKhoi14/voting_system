namespace voting_system_core.DTOs.Requests.Account
{
    public class ChangePasswordReq
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
