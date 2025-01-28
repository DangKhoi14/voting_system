namespace voting_system_core.DTOs.Responses.Account
{
    public class GetAccountInfoRes
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public sbyte Role { get; set; }

        public DateOnly CreateAt { get; set; }

        public DateTime LastLogin { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? ResetPasswordToken { get; set; }
    }
}
