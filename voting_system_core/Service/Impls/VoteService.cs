using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using voting_system_core.Data;
using voting_system_core.DTOs.Requests.Vote;
using voting_system_core.DTOs.Responses;
using voting_system_core.Models;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class VoteService
    {
        private readonly VotingDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoteService(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<string>> Vote(VoteReq voteReq)
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

            if (voteReq.PollId == null || voteReq.OptionId == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Bad Request"
                };
            }

            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.PollId == voteReq.PollId);

            if (poll == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 404,
                    Message = "Poll not found"
                };
            }

            if (poll.EndTime < DateTime.Now)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Poll has ended"
                };
            }

            var option = await _context.Options.FirstOrDefaultAsync(o => o.OptionId == voteReq.OptionId);

            if (option == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 404,
                    Message = "Option not found"
                };
            }

            var vote = new Vote
            {
                VoteId = Ulid.NewUlid(),
                Email = user.FindFirst(ClaimTypes.Email).Value,
                IsVerified = false,
                PollId = poll.PollId,
                OptionId = option.OptionId
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return new APIResponse<string>
            {
                StatusCode = 200,
                Message = "OK",
                Data = "Success"
            };
        }
    }
}
