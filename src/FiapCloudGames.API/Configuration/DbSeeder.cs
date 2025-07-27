using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.API.Configuration;

public class DbSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        if (userManager.Users.Any() == false)
        {
            var user = new ApplicationUser
            {
                Name = "Admin",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if ((await roleManager.RoleExistsAsync(UserRoleEnum.Admin.ToString())) == false)
                await roleManager.CreateAsync(new IdentityRole(UserRoleEnum.Admin.ToString()));

            if ((await roleManager.RoleExistsAsync(Roles.User)) == false)
                await roleManager.CreateAsync(new IdentityRole(Roles.User));

            var createUserResult = await userManager.CreateAsync(user: user, password: "Admin@123");

            var addUserToRoleResult = await userManager.AddToRoleAsync(user: user, role: UserRoleEnum.Admin.ToString());
        }
    }
}
