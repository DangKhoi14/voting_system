namespace voting_system_core.DTOs.Requests.Poll
{
    public class CreatePollReq
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
