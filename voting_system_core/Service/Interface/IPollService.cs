using voting_system_core.DTOs.Requests.Poll;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Poll;

namespace voting_system_core.Service.Interface
{
    public interface IPollService
    {
        Task<APIResponse<List<GetPollRes>>> GetByTitle(string title);
        Task<APIResponse<List<GetPollRes>>> GetAllPollsOfCurrentUser();
        Task<APIResponse<List<GetPollRes>>> GetAll();
        Task<APIResponse<string>> CreatePoll(CreatePollReq req);
        Task<APIResponse<string>> DeletePoll(DeletePollReq req);
    }
}
