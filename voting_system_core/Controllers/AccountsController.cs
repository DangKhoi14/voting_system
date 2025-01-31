using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;
using voting_system_core.DTOs.Requests.Account;
using Microsoft.Identity.Client;

namespace voting_system_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _accountService.GetAll();
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetInfo(string UsernameOrEmail)
        {
            var res = await _accountService.GetAccountInfo(UsernameOrEmail);
            return Ok(res);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginReq loginReq)
        {
            var res = await _accountService.Login(loginReq);
            return Ok(res);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Create(CreateReq req)
        {
            var res = await _accountService.Create(req);
            return Ok(res);
        }

        //[HttpPut("[action]")]
        //public async Task<ActionResult> ChangeUsername(ChangeUsernameReq req, [FromQuery] string Username)
        //{
        //    var res = await _accountService.ChangeUsername(req, Username);
        //    return Ok(res);
        //}
    }
}
