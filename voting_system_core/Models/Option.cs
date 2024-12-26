using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("option")]
    public class Option
    {
        public Ulid OptionID { get; set; }
        public string OptionText { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }

        // Navigation Property
        public List<Vote> Votes { get; set; }
    }
}
