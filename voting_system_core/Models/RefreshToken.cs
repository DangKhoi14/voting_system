

namespace voting_system_core.Models
{
    public class RefreshToken
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked { get; set; } = false;

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
