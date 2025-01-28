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

        public DebugService(VotingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<APIResponse<RefreshToken>> Test(string Username)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Username == Username);

            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetValue<string>("AppSettings:SecretKey");
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                var options = new DbContextOptionsBuilder<VotingDbContext>()
                    .UseNpgsql(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"))
                    .Options;
                VotingDbContext context = new VotingDbContext(options);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    }),

                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                var accessToken = jwtTokenHandler.WriteToken(token);
                var refreshToken = GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    TokenId = Guid.NewGuid(),
                    JwtId = token.Id,
                    Token = refreshToken,
                    UserId = user.UserId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddHours(1),
                    IsRevoked = false,
                    IsUsed = false
                };

                return new APIResponse<RefreshToken>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = refreshTokenEntity,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<RefreshToken>
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
