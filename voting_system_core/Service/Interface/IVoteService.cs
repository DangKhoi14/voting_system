using voting_system_core.DTOs.Requests.Vote;
using voting_system_core.DTOs.Responses;

namespace voting_system_core.Service.Interface
{
    public interface IVoteService
    {
        Task<APIResponse<string>> AuthenticatedVote(VoteReq voteReq);
    }
}
