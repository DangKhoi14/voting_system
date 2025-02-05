using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using voting_system_core.Data;
using voting_system_core.DTOs.Requests.Poll;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Poll;
using voting_system_core.Models;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class PollService : IPollService
    {
        private readonly VotingDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PollService(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<List<GetPollRes>>> GetAll()
        {
            var polls = await _context.Polls.ToListAsync();

            var res = polls.Select(poll => new GetPollRes
            {
                PollId = poll.PollId,
                UserId = poll.UserId,
                Title = poll.Title,
                Description = poll.Description,
                StartTime = poll.StartTime,
                EndTime = poll.EndTime,

            }).ToList();

            return new APIResponse<List<GetPollRes>>
            {
                StatusCode = 200,
                Message = "OK",
                Data = res
            };
        }

        public async Task<APIResponse<List<GetPollRes>>> GetAllPollsOfCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return new APIResponse<List<GetPollRes>>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            var UserId = Guid.Parse(user.FindFirstValue("UserId"));

            var polls = await _context.Polls
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            var pollList = polls.Select(p => new GetPollRes
            {
                PollId = p.PollId,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                StartTime = p.StartTime,
                EndTime = p.EndTime
            }).ToList();

            return new APIResponse<List<GetPollRes>>
            {
                StatusCode = 200,
                Message = "OK",
                Data = pollList
            };
        }

        public async Task<APIResponse<List<GetPollRes>>> GetByTitle(string title)
        {
            var polls = await _context.Polls
                .Where(x => x.Title == title)
                .ToListAsync();

            if (!polls.Any())
            {
                return new APIResponse<List<GetPollRes>>
                {
                    StatusCode = 404,
                    Message = "No polls found with the given title",
                };
            }

            var pollList = polls.Select(p => new GetPollRes
            {
                PollId = p.PollId,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                StartTime = p.StartTime,
                EndTime = p.EndTime
            }).ToList();

            return new APIResponse<List<GetPollRes>>
            {
                StatusCode = 200,
                Message = "OK",
                Data = pollList
            };
        }

        public async Task<APIResponse<string>> CreatePoll(CreatePollReq req)
        {
            try
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

                var userIdStr = user.FindFirstValue("UserId");

                if (!Guid.TryParse(userIdStr, out Guid UserId))
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = "Invalid UserId"
                    };
                }

                // Check if an active poll with the same title exists for this user
                var existingPoll = await _context.Polls
                    .Where(p => p.UserId == UserId && p.Title == req.Title)
                    .FirstOrDefaultAsync();

                if (existingPoll != null && existingPoll.IsActive)
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = "You already have an active poll with this title."
                    };
                }

                var newPoll = new Poll
                {
                    PollId = Ulid.NewUlid(),
                    UserId = UserId,
                    Title = req.Title,
                    Description = req.Description,
                    StartTime = req.StartTime,
                    EndTime = req.EndTime
                };

                _context.Polls.Add(newPoll);
                await _context.SaveChangesAsync();

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = "Success"
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
    }
}
