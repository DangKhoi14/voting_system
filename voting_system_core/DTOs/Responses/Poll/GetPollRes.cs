namespace voting_system_core.DTOs.Responses.Poll
{
    public class GetPollRes
    {
        public Ulid PollId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsActive => StartTime <= DateTime.Now && DateTime.Now <= EndTime;
        public int ParticipantCount { get; set; }
    }
}
