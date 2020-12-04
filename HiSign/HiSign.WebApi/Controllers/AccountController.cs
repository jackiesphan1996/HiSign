using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.DTOs.Account;
using HiSign.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiSign.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public AccountController(IAccountService accountService, IAuthenticatedUserService authenticatedUserService)
        {
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        [HttpPost("register")]
        
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("register-employee")]
        [Authorize(Roles="CompanyAdmin,CEO")]
        public async Task<IActionResult> CreateAccountAsync(RegisterEmployeeRequest request)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            return Ok(await _accountService.RegisterAsync(currentUser.CompanyId.Value, request));
        }

        [HttpGet("employee")]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        public async Task<IActionResult> GetAllAccountAsync()
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            return Ok(await _accountService.GetAllEmployeeAsync(currentUser.CompanyId.Value));
        }

        [HttpPut("update-employee")]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        public async Task<IActionResult> UpdateEmployeeAccountAsync(UpdateEmployeeRequest request)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            return Ok(await _accountService.UpdateEmployeeAsync(request));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}