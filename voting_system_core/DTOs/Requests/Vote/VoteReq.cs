namespace voting_system_core.DTOs.Requests.Vote
{
    public class VoteReq
    {
        public string PollId { get; set; }
        public string OptionId { get; set; }
    }
}
