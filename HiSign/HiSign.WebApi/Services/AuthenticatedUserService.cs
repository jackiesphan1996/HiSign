using HiSign.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using HiSign.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HiSign.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            _userManager = userManager;
        }

        public string UserId { get; }

        public async Task<ApplicationUser> GetCurentApplicationUser()
        {
            return await _userManager.FindByIdAsync(UserId);
        }

    }
}
