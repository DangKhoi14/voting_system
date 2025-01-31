using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IDebugService _testService;

        public DebugController(IDebugService testService)
        {
            _testService = testService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DebugFunction(string name)
        {
            var res = await _testService.Test(name);
            return Ok(res);
        }
    }
}
