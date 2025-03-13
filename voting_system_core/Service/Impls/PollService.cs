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
            var polls = await _context.Polls
                .Select(poll => new GetPollRes
                {
                    PollId = poll.PollId,
                    UserName = _context.Accounts
                        .Where(u => u.UserId == poll.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault(),
                    Title = poll.Title,
                    Description = poll.Description,
                    StartTime = poll.StartTime,
                    EndTime = poll.EndTime,
                    ParticipantCount = _context.Votes.Count(v => v.PollId == poll.PollId)
                })
                .ToListAsync();

            return new APIResponse<List<GetPollRes>>
            {
                StatusCode = 200,
                Message = "OK",
                Data = polls
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
                UserName = _context.Accounts
                        .Where(u => u.UserId == p.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault(),
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
                UserName = _context.Accounts
                        .Where(u => u.UserId == p.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault(),
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


        public async Task DeleteOptionsAndAllVotesOfIt(List<Option> options)
        {
            var optionIds = options.Select(o => o.OptionId).ToList();
            var votes = _context.Votes.Where(v => optionIds.Contains(v.OptionId));

            _context.Votes.RemoveRange(votes);
            _context.Options.RemoveRange(options);

            await _context.SaveChangesAsync();
        }


        public async Task<APIResponse<string>> DeletePoll(DeletePollReq req)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !(user.Identity?.IsAuthenticated ?? false))
            {
                return new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            var poll = await _context.Polls
                .Include(p => p.Options) 
                .FirstOrDefaultAsync(p => p.PollId == Ulid.Parse(req.PollId));
            
            if (poll == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Poll not found"
                };
            }

            var UserId = Guid.Parse(user.FindFirstValue("UserId"));

            if (poll.UserId != UserId)
            {
                return new APIResponse<string>
                {
                    StatusCode = 403,
                    Message = "Forbidden"
                };
            }

            // Check if the poll is active
            if (poll.StartTime <= DateTime.Now && DateTime.Now <= poll.EndTime)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Cannot delete an active poll"
                };
            }

            // Delete all options (and their votes) before deleting the poll
            if (poll.Options != null && poll.Options.Any())
            {
                await DeleteOptionsAndAllVotesOfIt(poll.Options.ToList());
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return new APIResponse<string>
            {
                StatusCode = 200,
                Message = "OK",
            };
        }
    }
}
