using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.DTOs.Account;
using HiSign.Application.Interfaces;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
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
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public AccountController(IAccountService accountService, IAuthenticatedUserService authenticatedUserService, ApplicationDbContext context)
        {
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
            _context = context;
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

        [HttpGet]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var permissions = _context.Set<Permission>().ToList();

            var userPermissions =
                _context.Set<UserPermission>().Where(x => x.UserId == _authenticatedUserService.UserId).ToList();

            var data = permissions.Select(x =>
            {
                var userPermssion = userPermissions.FirstOrDefault(y => y.PermissionId == x.Id);

                if (userPermssion is null)
                {
                    return new UserPermissionViewModel
                    {
                        UserId = _authenticatedUserService.UserId,
                        PermissionId = x.Id,
                        Enabled = false,
                        PermissionName = x.Name
                    };
                }

                return new UserPermissionViewModel
                {
                    UserId = userPermssion.UserId,
                    Enabled = userPermssion.Enabled,
                    PermissionId = x.Id,
                    PermissionName = x.Name
                };
            });

            return Ok(data);
        }
    }

    public class UserPermissionViewModel
    {
        public string UserId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool Enabled { get; set; }
    }
}