using Microsoft.EntityFrameworkCore;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Accounts;
using voting_system_core.DTOs.Responses.Role;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class RoleService : IRoleService
    {
        private readonly VotingDbContext _context;

        public RoleService(VotingDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse<List<GetRoleRes>>> GetAll()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                List<GetRoleRes> res = new List<GetRoleRes>();

                foreach (var role in roles)
                {
                    GetRoleRes item = new GetRoleRes();
                    item.Key = role.Key;
                    item.Value = role.Value;
                    item.Authority = role.Authority;

                    res.Add(item);
                }

                return new APIResponse<List<GetRoleRes>>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<GetRoleRes>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }
    }
}
