using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("option")]
    public class Option
    {
        public int OptionID { get; set; } // Use int for IDs
        public string OptionText { get; set; }

        // Foreign Key
        public int PollID { get; set; }
        public Poll Poll { get; set; }

        // Navigation Property
        public List<Vote> Votes { get; set; }
    }
}
