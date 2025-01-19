using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _roleService.GetAll();

            if (res.StatusCode != 200)
            {
                return StatusCode(res.StatusCode, new { res.Message });
            }

            return Ok(res.Data);
        }
    }
}
