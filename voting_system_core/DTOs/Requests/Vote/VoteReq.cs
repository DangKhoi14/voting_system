namespace voting_system_core.DTOs.Requests.Vote
{
    public class VoteReq
    {
        public Ulid PollId { get; set; }
        public Ulid OptionId { get; set; }
    }
}
