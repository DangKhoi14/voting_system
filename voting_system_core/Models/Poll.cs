

using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("poll")]
    public class Poll
    {
        public string PollID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive => DateTime.Now >= StartTime && DateTime.Now <= EndTime;
        public List<Vote> Votes { get; set; }
    }
}
