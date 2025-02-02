using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        [Authorize]
        public async Task<IActionResult> DebugFunction()
        {
            var res = await _testService.Test();
            return Ok(res);
        }

        [HttpGet]
        [Route("users/current")]
        public async Task<IActionResult> GetLoggedInUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var currentUser = HttpContext.User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(currentUser))
            {
                return BadRequest("UserId claim not found.");
            }

            return Ok(currentUser);
        }

    }
}
