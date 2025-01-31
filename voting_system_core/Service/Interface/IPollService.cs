using voting_system_core.DTOs.Requests.Poll;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Poll;

namespace voting_system_core.Service.Interface
{
    public interface IPollService
    {
        //Task<APIResponse<GetPollRes>> GetById(Ulid PollId);
        Task<APIResponse<List<GetPollRes>>> GetAll();
        Task<APIResponse<string>> CreatePoll(CreateReq req);
    }
}
