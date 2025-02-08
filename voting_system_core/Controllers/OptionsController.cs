using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOptionsByPollId(string PollId)
        {
            var res = await _optionService.GetOptionsByPollId(PollId);
            return Ok(res);
        }
    }
}
