using voting_system_core.DTOs.Requests.Option;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Option;

namespace voting_system_core.Service.Interface
{
    public interface IOptionService
    {
        Task<APIResponse<string>> CreateOption(CreateOptionReq req);
        Task<APIResponse<List<GetOptionsRes>>> GetOptionsByPollId(string PollId);
    }
}
