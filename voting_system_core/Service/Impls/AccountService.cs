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
                var res = await _context.Accounts
                    .Select(account => new GetAccountRes
                    {
                        Username = account.Username,
                        Email = account.Email
                    })
                    .ToListAsync();

                return new APIResponse<List<GetAccountRes>>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<GetAccountRes>>
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse<GetAccountInfoRes>> GetAccountInfo(string UsernameOrEmail)
        {
            try
            {
                var account = await _context.Accounts
                    .Where(x => 
                        x.Username == UsernameOrEmail || 
                        x.Email == UsernameOrEmail)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    return new APIResponse<GetAccountInfoRes>()
                    {
                        StatusCode = 404,
                        Message = "Account does not exist",
                    };
                }

                var res = new GetAccountInfoRes();
                res.Username = account.Username;
                res.Email = account.Email;
                res.IsEmailVerified = account.IsEmailVerified;
                res.Role = account.Role;
                res.CreateAt = account.CreateAt;
                res.LastLogin = account.LastLogin;
                res.ProfilePictureUrl = account.ProfilePictureUrl;

                return new APIResponse<GetAccountInfoRes>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<GetAccountInfoRes>()
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponse<string>> Create(CreateReq req)
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

        public async Task<APIResponse<string>> ChangeUsername(ChangeUsernameReq req, string Username)
        {
            try
            {
                //var acc = _context.Accounts
                //    .Where(x => x.Username == req.Username)
                //    .FirstOrDefaultAsync();

                var acc = _context.Accounts.FirstOrDefault(x => x.Username == Username);

                if (acc == null)
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = $"User {Username} not found"
                    };
                }

                acc.Username = req.NewUserName;
                _context.Accounts.Update(acc);
                await _context.SaveChangesAsync();

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = acc.Username
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

        //public async Task<APIResponse<string>> ChangeEmail(ChangeEmailReq req, string Username)
        //{

        //}

        //public async Task<APIResponse<string>> ChangePassword(ChangePasswordReq req)
        //{
            
        //} 
    }
}
