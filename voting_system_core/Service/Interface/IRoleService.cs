using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Role;

namespace voting_system_core.Service.Interface
{
    public interface IRoleService
    {
        Task<APIResponse<List<GetRoleRes>>> GetAll();
    }
}
