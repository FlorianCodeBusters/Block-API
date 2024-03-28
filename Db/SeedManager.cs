using Blocks_api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace Blocks_api.Db
{
    public class SeedManager
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            await SeedRoles(serviceProvider);
            await SeedAdminUser(serviceProvider);
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        private static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IdentificationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminUser = await context.Users.FirstOrDefaultAsync(user => user.UserName == "AuthenticationAdmin");

            if(adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "AuthenticationAdmin",
                    Email = "placeholder@mail.com"
                };
                var creationResult = await  userManager.CreateAsync(adminUser, "AdminPassword01!");
                if (creationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
                else
                {
                    Console.Error.WriteLine($"Unable to create admin user: {string.Join(", ", creationResult.Errors)}");
                }
            }
        }
    }
}
