using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;
using voting_system_core.DTOs.Requests.Poll;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Create(CreatePollReq req)
        {
            return Ok(await _pollService.CreatePoll(req));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _pollService.GetAll();
            return Ok(res);
        }
    }
}
