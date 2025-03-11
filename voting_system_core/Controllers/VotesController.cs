using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;
using voting_system_core.DTOs.Requests.Vote;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VotesController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AuthenticatedVote(VoteReq voteReq)
        {
            var res = await _voteService.AuthenticatedVote(voteReq);
            return Ok(res);
        }
    }
}
