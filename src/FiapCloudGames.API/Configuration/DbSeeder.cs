﻿using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace FiapCloudGames.Infra.Data;

public class DbSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

        try
        {
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
                {
                    logger.LogInformation("Admin role is creating");
                    var roleResult = await roleManager
                        .CreateAsync(new IdentityRole(UserRoleEnum.Admin.ToString()));

                    if (roleResult.Succeeded == false)
                    {
                        var roleErros = roleResult.Errors.Select(e => e.Description);
                        logger.LogError($"Failed to create admin role. Errors : {string.Join(",", roleErros)}");

                        return;
                    }
                    logger.LogInformation("Admin role is created");
                }

                var createUserResult = await userManager
                        .CreateAsync(user: user, password: "Admin@123");

                if (createUserResult.Succeeded == false)
                {
                    var errors = createUserResult.Errors.Select(e => e.Description);
                    logger.LogError(
                        $"Failed to create admin user. Errors: {string.Join(", ", errors)}"
                    );
                    return;
                }

                var addUserToRoleResult = await userManager
                                .AddToRoleAsync(user: user, role: UserRoleEnum.Admin.ToString());

                if (addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    logger.LogError($"Failed to add admin role to user. Errors : {string.Join(",", errors)}");
                }
                logger.LogInformation("Admin user is created");
            }
        }

        catch (Exception ex)
        {
            logger.LogCritical(ex.Message);
        }
    }
}
