using Microsoft.EntityFrameworkCore;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses;
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
    }
}
