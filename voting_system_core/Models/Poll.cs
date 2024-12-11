using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("poll")]
    public class Poll
    {
        public int PollId { get; set; } // Use int for IDs
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive => DateTime.Now >= StartTime && DateTime.Now <= EndTime;

        // Navigation Properties
        public List<Option> Options { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
