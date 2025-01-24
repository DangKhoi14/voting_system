namespace voting_system_core.DTOs.Requests.Account
{
    public class LoginReq
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
