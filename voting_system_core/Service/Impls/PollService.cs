using Microsoft.EntityFrameworkCore;
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

        public PollService(VotingDbContext context)
        {
            _context = context;
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

        //public async Task<APIResponse<GetPollRes>> GetByTitle(Ulid PollId)
        //{
        //    var poll = await _context.Polls.FindAsync(PollId);

        //    if (poll == null)
        //    {
        //        return new APIResponse<GetPollRes>
        //        {
        //            StatusCode = 404,
        //            Message = "Poll not found",
        //        };
        //    }

        //    var item = _mapper.Map<GetPollRes>(poll);

        //    return new APIResponse<GetPollRes>
        //    {
        //        StatusCode = 200,
        //        Message = "OK",
        //        Data = item
        //    };
        //}

        public async Task<APIResponse<string>> CreatePoll(CreatePollReq req)
        {
            try
            {
                var newPoll = new Poll();
                newPoll.PollId = Ulid.NewUlid();
                var user = _context.Accounts.FirstOrDefault(x => x.Username == req.Username);
                if (user == null)
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = "User not found"
                    };
                }
                newPoll.UserId = user.UserId;
                newPoll.Title = req.Title;
                newPoll.Description = req.Description;
                
                // Need change this later bruh ==================
                //newPoll.StartTime = req.StartTime;
                newPoll.StartTime = DateTime.UtcNow;
                //newPoll.EndTime = req.EndTime;
                newPoll.EndTime = DateTime.MaxValue;

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
