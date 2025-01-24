

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        
        [ForeignKey("Username")]
        public string Username { get; set; }
        public Account Account { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
    }
}
