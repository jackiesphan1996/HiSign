using HiSign.Application.Enums;
using HiSign.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HiSign.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.CompanyAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.CEO.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Secretary.ToString()));
        }
    }
}
