namespace voting_system_core.DTOs.Requests.Account
{
    public class CreateAccountReq
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
