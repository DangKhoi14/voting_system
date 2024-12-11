using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("vote")]
    public class Vote
    {
        public int VoteId { get; set; } // Use int for IDs
        public string Email { get; set; }
        public bool IsVerified { get; set; }

        // Foreign Key
        public int PollId { get; set; }
        public Poll Poll { get; set; }

        // Option chosen by the voter
        public int OptionId { get; set; }
        public Option Option { get; set; }
    }
}
