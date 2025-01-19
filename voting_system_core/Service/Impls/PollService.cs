using AutoMapper;
using Microsoft.EntityFrameworkCore;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Poll;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class PollService : IPollService
    {
        private readonly VotingDbContext _context;
        private readonly IMapper _mapper;

        public PollService(VotingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<GetPollRes>>> GetAll()
        {
            var polls = await _context.Polls.ToListAsync();

            var res = polls.Select(poll => new GetPollRes
            {
                PollId = poll.PollId,
                Username = poll.Username,
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

        public async Task<APIResponse<GetPollRes>> GetById(Ulid PollId)
        {
            var poll = await _context.Polls.FindAsync(PollId);

            if (poll == null)
            {
                return new APIResponse<GetPollRes>
                {
                    StatusCode = 404,
                    Message = "Poll not found",
                };
            }

            var item = _mapper.Map<GetPollRes>(poll);

            return new APIResponse<GetPollRes>
            {
                StatusCode = 200,
                Message = "OK",
                Data = item
            };
        }
    }
}
