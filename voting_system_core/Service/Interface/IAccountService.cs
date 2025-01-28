using voting_system_core.DTOs.Requests.Account;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Account;
using voting_system_core.DTOs.Responses.Accounts;

namespace voting_system_core.Service.Interface
{
    public interface IAccountService
    {
        Task<APIResponse<List<GetAccountRes>>> GetAll();
        Task<APIResponse<GetAccountInfoRes>> GetAccountInfo(string UsernameOrEmail);
        Task<APIResponse<LoginRes>> Login(LoginReq loginReq);
        Task<APIResponse<string>> Create(CreateReq createReq);
    }
}