﻿using Microsoft.AspNetCore.Mvc;
using voting_system_core.Service.Interface;
using voting_system_core.DTOs.Requests.Account;

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

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginReq loginReq)
        {
            var res = await _accountService.Login(loginReq);
            return Ok(res);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Create(CreateAccountReq req)
        {
            var res = await _accountService.Create(req);
            return Ok(res);
        }
    }
}
