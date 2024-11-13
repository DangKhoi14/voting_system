

using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("vote")]
    public class Vote
    {
        public string VoteID { get; set; }
        public string Email { get; set; }
        public bool IsVerified {  get; set; }
        public int PollID { get; set; }
        public Poll Poll { get; set; }
    }
}
