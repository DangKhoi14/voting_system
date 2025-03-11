using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using voting_system_core.Data;
using voting_system_core.DTOs.Requests.Vote;
using voting_system_core.DTOs.Responses;
using voting_system_core.Models;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class VoteService : IVoteService
    {
        private readonly VotingDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoteService(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<string>> AuthenticatedVote(VoteReq voteReq)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            // check authentication
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            var userId = user.FindFirstValue("UserId");
            var email = user.FindFirstValue(ClaimTypes.Email);

            if (!Ulid.TryParse(voteReq.PollId, out var pollId) || !Ulid.TryParse(voteReq.OptionId, out var optionId))
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Invalid PollId or OptionId"
                };
            }

            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.PollId == pollId);

            // check if poll exists
            if (poll == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 404,
                    Message = "Poll not found"
                };
            }

            // check if poll has started
            if (poll.StartTime > DateTime.UtcNow)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Poll has not started"
                };
            }

            // check if poll has ended
            if (poll.EndTime < DateTime.UtcNow)
            {
                return new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Poll has ended"
                };
            }

            var option = await _context.Options.FirstOrDefaultAsync(o => o.OptionId == optionId && o.PollId == poll.PollId);

            // check if option exists
            if (option == null)
            {
                return new APIResponse<string>
                {
                    StatusCode = 404,
                    Message = "Option not found or does not belong to this poll"
                };
            }

            if (poll.UserId.ToString() == userId)
            {
                return new APIResponse<string>
                {
                    StatusCode = 403,
                    Message = "Poll owner cannot vote"
                };
            }

            var voted = await _context.Votes.AnyAsync(v => v.PollId == poll.PollId && v.Email == email);

            // check if user has already voted
            if (voted)
            {
                return new APIResponse<string>
                {
                    StatusCode = 409,
                    Message = "Already voted"
                };
            }

            // Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var vote = new Vote
                {
                    VoteId = Ulid.NewUlid(),
                    Email = email,
                    IsVerified = false,
                    PollId = poll.PollId,
                    OptionId = option.OptionId
                };

                _context.Votes.Add(vote);
                await _context.SaveChangesAsync();

                // Commit transaction after saving changes
                await transaction.CommitAsync();

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "OK",
                    Data = "Success"
                };
            }
            catch (Exception ex)
            {
                // Rollback if exception occurs
                await transaction.RollbackAsync();
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    Data = ex.Message
                };
            }
        }

        //public async Task<APIResponse<string>> AnonymousVote(VoteReq voteReq)
        //{

        //}
    }
}
