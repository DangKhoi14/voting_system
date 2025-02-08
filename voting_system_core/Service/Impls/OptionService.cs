using Microsoft.EntityFrameworkCore;
using voting_system_core.Data;
using voting_system_core.DTOs.Responses;
using voting_system_core.DTOs.Responses.Accounts;
using voting_system_core.DTOs.Responses.Option;
using voting_system_core.Service.Interface;

namespace voting_system_core.Service.Impls
{
    public class OptionService : IOptionService
    {
        private readonly VotingDbContext _context;

        public OptionService(VotingDbContext context)
        {
            _context = context;
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
