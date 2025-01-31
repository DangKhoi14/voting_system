using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Key]
        public Guid TokenId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        [NotMapped]
        public Account Account { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
    }
}
