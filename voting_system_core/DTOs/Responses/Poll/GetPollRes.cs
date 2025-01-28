using voting_system_core.Models;

namespace voting_system_core.DTOs.Responses.Poll
{
    public class GetPollRes
    {
        public Ulid PollId { get; set; }
        public Ulid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsActive => StartTime <= DateTime.Now && DateTime.Now <= EndTime;
        
        //public List<Option> Options { get; set; }
        //public List<Vote> Votes { get; set; }
    }
}
