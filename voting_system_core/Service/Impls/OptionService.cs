using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using voting_system_core.Data;
using voting_system_core.Models;
using voting_system_core.DTOs.Requests.Option;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Option;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class OptionService : IOptionService
    {
        private readonly VotingDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public OptionService(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<APIResponse<string>> CreateOption(CreateOptionReq req)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            if (!Ulid.TryParse(req.PollIdStr, out var pollId))
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Invalid poll Id"
                };

            var poll = _context.Polls.FirstOrDefault(p => p.PollId == pollId);
            if (poll == null)
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Poll not found"
                };

            var UserId = Guid.Parse(user.FindFirstValue("UserId"));
            if (poll.UserId != UserId)
                return new APIResponse<string>
                {
                    StatusCode = 403,
                    Message = "Forbidden"
                };

            var option = new Option
            {
                OptionId = Ulid.NewUlid(),
                PollId = pollId,
                OptionText = req.OptionText
            };

            await _context.Options.AddAsync(option);
            await _context.SaveChangesAsync();

            return new APIResponse<string>
            {
                StatusCode = 200,
                Message = "OK",
            };
        }


        public async Task DeleteOptionAndAllVotesOfIt(Option opt)
        {
            var votes = _context.Votes.Where(v => v.OptionId == opt.OptionId);
            _context.Votes.RemoveRange(votes);
            _context.Options.Remove(opt);

            await _context.SaveChangesAsync();
        }


        public async Task<APIResponse<string>> DeleteOption(string OptionId)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            var option = await _context.Options.FirstOrDefaultAsync(o => o.OptionId == Ulid.Parse(OptionId));
            if (option == null)
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Option not found"
                };

            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.PollId == option.PollId);
            var UserId = Guid.Parse(user.FindFirstValue("UserId"));
            if (poll.UserId != UserId)
            {
                return new APIResponse<string>
                {
                    StatusCode = 403,
                    Message = "Forbidden"
                };
            }

            DeleteOptionAndAllVotesOfIt(option);

            return new APIResponse<string>
            {
                StatusCode = 200,
                Message = "OK",
            };
        }


        public async Task<APIResponse<List<GetOptionsRes>>> GetOptionsByPollId(string pollIdStr)
        {
            try
            {
                if (!Ulid.TryParse(pollIdStr, out var pollId))
                    return new APIResponse<List<GetOptionsRes>>
                    {
                        StatusCode = 400,
                        Message = "Invalid poll Id"
                    };

                var options = await _context.Options.Where(o => o.PollId == pollId).ToListAsync();
                List<GetOptionsRes> res = new List<GetOptionsRes>();

                foreach (var option in options)
                {
                    GetOptionsRes item = new GetOptionsRes();
                    item.OptionId = option.OptionId;
                    item.OptionText = option.OptionText;

                    res.Add(item);
                }

                return new APIResponse<List<GetOptionsRes>>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<GetOptionsRes>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }
    }
}
