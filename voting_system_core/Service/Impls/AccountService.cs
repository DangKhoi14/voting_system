using Microsoft.EntityFrameworkCore;
using voting_system_core.Data;
using voting_system_core.DTOs.Requests.Account;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Account;
using voting_system_core.DTOs.Responses.Accounts;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class AccountService : IAccountService
    {
        private readonly VotingDbContext _context;

        public AccountService(VotingDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse<List<GetAccountRes>>> GetAll()
        {
            try
            {
                var accounts = _context.Accounts.ToList();
                List<GetAccountRes> res = new List<GetAccountRes>();

                foreach (var account in accounts)
                {
                    GetAccountRes item = new GetAccountRes();
                    item.Username = account.Username;

                    res.Add(item);
                }

                return new APIResponse<List<GetAccountRes>>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<GetAccountRes>>()
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponse<LoginRes>> Login(LoginReq loginReq) 
        {
            try
            {
                var user = _context.Accounts.FirstOrDefault(x => x.Username == loginReq.UserName);
                if (user == null)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 200,
                        Message = $"User {loginReq.UserName} does not exist",
                    };
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginReq.Password, user.Password);
                
                if (!isValidPassword)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 401,
                        Message = "Password is wrong",
                    };
                }

                //var token = await TokenMana
            }
        }
    }
}
