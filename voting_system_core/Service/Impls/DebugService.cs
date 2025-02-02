using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses;
using voting_system_core.Models;
using voting_system_core.DTOs.Responses.Account;

using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class DebugService : IDebugService
    {
        private readonly VotingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DebugService(VotingDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<string>> Test()
        {
            try
            {
                var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Id = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId").ToString();

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Id
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
            
        }

        private static string GenerateRefreshToken()
        {
            var random = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
