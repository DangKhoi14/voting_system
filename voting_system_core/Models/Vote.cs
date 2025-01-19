using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("vote")]
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid VoteId { get; set; } 
        public string Email { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("PollId")]
        public Ulid PollId { get; set; }
        public Poll Poll { get; set; }

        [ForeignKey("OprionId")]
        public Ulid OptionId { get; set; }
        public Option Option { get; set; }
    }
}
