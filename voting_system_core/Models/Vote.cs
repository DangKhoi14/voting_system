using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("vote")]
    public class Vote
    {
        public Ulid VoteId { get; set; } 
        public string Email { get; set; }
        public bool IsVerified { get; set; }

        // Foreign Key
        public Ulid PollId { get; set; }
        public Poll Poll { get; set; }

        // Option chosen by the voter
        public Ulid OptionId { get; set; }
        public Option Option { get; set; }
    }
}
