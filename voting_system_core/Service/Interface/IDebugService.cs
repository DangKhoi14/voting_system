using voting_system_core.DTOs.Responses;
using voting_system_core.Models;

namespace voting_system_core.Service.Interface
{
    public interface IDebugService
    {
        Task<APIResponse<RefreshToken>> Test(string Username);
    }
}
