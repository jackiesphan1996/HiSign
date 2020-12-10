using HiSign.Application.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;

namespace HiSign.Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "nguyenbatruong",
                Email = "nguyenbatruong@gmail.com",
                FirstName = "Truong",
                LastName = "Nguyen",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.CompanyAdmin.ToString());
                }

            }
        }
    }

    public static class PermissionSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var listPermission = new List<Permission>
            {
                new Permission
                {
                    Name = "UpdateCompany",
                    Description = "Update Company"
                },
                new Permission
                {
                    Name = "GetAllCompanyList",
                    Description = "Get All Company List"
                },
                new Permission
                {
                    Name = "CreateSignature",
                    Description = "Create Signature"
                },
                new Permission
                {
                    Name = "UpdateSignature",
                    Description = "Update Signature"
                },
                new Permission
                {
                    Name = "ActiveDeactiveSignature",
                    Description = "Active/Deactive Signature"
                },
                new Permission
                {
                    Name = "CreateAccount",
                    Description = ""
                },
                new Permission
                {
                    Name = "UpdateAccountPermission",
                    Description = ""
                },
                new Permission
                {
                    Name = "GetCompanyAccountListByCompanyId",
                    Description = ""
                },
                new Permission
                {
                    Name = "ActiveDeactiveAccount",
                    Description = ""
                },
                new Permission
                {
                    Name = "GetCompanyAdminList(ByRole)",
                    Description = ""
                },
                new Permission
                {
                    Name = "GetAllCompanyAccount(ByCompanyId)",
                    Description = ""
                },
                new Permission
                {
                    Name = "CreateTemplate",
                    Description = ""
                },
                new Permission
                {
                    Name = "UpdateTemplate",
                    Description = ""
                },
                new Permission
                {
                    Name = "ActiveDeactiveTemplate",
                    Description = ""
                },
                new Permission
                {
                    Name = "GetAllCompanyTemplate(ByCompanyId)",
                    Description = ""
                },
                new Permission
                {
                    Name = "CreateContract",
                    Description = ""
                },
                new Permission
                {
                    Name = "UpdateContract",
                    Description = ""
                },
                new Permission
                {
                    Name = "ActiveDeactiveContract",
                    Description = ""
                },
                new Permission
                {
                    Name = "GetContractList(ByCompanyId)",
                    Description = ""
                },
                new Permission
                {
                    Name = "Sign",
                    Description = ""
                },
                new Permission
                {
                    Name = "AddCustomer",
                    Description = ""
                },
                new Permission
                {
                    Name = "UpdateCustomer",
                    Description = ""
                },
                new Permission
                {
                    Name = "ActiveDeactiveCustomer",
                    Description = "GetAllCompanyCustomer(ByCompanyId)"
                }
            };

            var entities = context.Set<Permission>().ToList();
            var names = entities.Select(x => x.Name).ToList();

            var addData = listPermission.Where(x => !names.Contains(x.Name)).ToList();

            if (addData.Any())
            {
                context.AddRange(addData);

                await context.SaveChangesAsync();

                var users = context.Users.Where(x => x.CompanyId.HasValue).ToList();

                foreach (var permission in addData)
                {
                    foreach (var user in users)
                    {
                        var userPermission = new UserPermission
                        {
                            UserId = user.Id,
                            PermissionId = permission.Id,
                            Enabled = true
                        };

                        context.Set<UserPermission>().Add(userPermission);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
