using Microsoft.AspNetCore.Mvc;
using voting_system_core.DTOs.Requests.Option;
using voting_system_core.Service.Interface;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionService _optionService;

        public OptionsController(IOptionService optionService)
        {
            _optionService = optionService;
        }


        //[Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOption(CreateOptionReq req)
        {
            var res = await _optionService.CreateOption(req);
            return Ok(res);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetOptionsByPollId(string PollId)
        {
            var res = await _optionService.GetOptionsByPollId(PollId);
            return Ok(res);
        }
    }
}
