﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses.Account;
using voting_system_core.Models;

namespace voting_system_core.Helper
{
    public class TokenManager
    {
        public static async Task<LoginRes> GenerateToken(Account user, IConfiguration configuration)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = configuration.GetValue<string>("AppSettings:SecretKey");
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var options = new DbContextOptionsBuilder<VotingDbContext>()
                .UseNpgsql(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"))
                .Options;

            VotingDbContext context = new VotingDbContext(options);

            // All things in token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

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

            context.RefreshTokens.Add(refreshTokenEntity);

            await context.SaveChangesAsync();

            return new LoginRes
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
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
