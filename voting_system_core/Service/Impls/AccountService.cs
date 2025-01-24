using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using voting_system_core.Models;
using voting_system_core.Data;
using voting_system_core.DTOs.Requests.Account;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Account;
using voting_system_core.DTOs.Responses.Accounts;
using voting_system_core.Helper;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class AccountService : IAccountService
    {
        private readonly VotingDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(VotingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                    item.Email = account.Email;

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

        public async Task<APIResponse<string>> Create(CreateAccountReq req)
        {
            try
            {
                var users = GetAll();
                foreach (var user in users.Result.Data)
                {
                    if (user.Username == req.Username)
                    {
                        return new APIResponse<string>
                        {
                            StatusCode = 200,
                            Message = "Username already exists"
                        };
                    }
                }

                var newAcc = new Account();

                newAcc.Username = req.Username;
                newAcc.Email = req.Email;
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
                newAcc.Password = passwordHash;
                newAcc.CreateAt = DateOnly.FromDateTime(DateTime.Now);
                newAcc.LastLogin = DateTime.UtcNow;
                newAcc.Role = 2;

                _context.Accounts.Add(newAcc);

                await _context.SaveChangesAsync();
                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>()
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse<LoginRes>> Login(LoginReq loginReq) 
        {
            try
            {
                if (string.IsNullOrEmpty(loginReq.UserNameOrEmail))
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Username or Email is required"
                    };
                }

                var user = _context.Accounts
                    .FirstOrDefault(x => 
                        x.Username == loginReq.UserNameOrEmail || 
                        x.Email == loginReq.UserNameOrEmail);
                if (user == null)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 200,
                        Message = $"User does not exist",
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

                var token = await TokenManager.GenerateToken(user, _configuration);
                if (token == null)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 500,
                        Message = "Failed to generate token"
                    };
                }

                return new APIResponse<LoginRes>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<LoginRes>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }
    }
}
