﻿using Eshop.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Eshop.WebApi.Infrastructure
{
    public static class UserSeeder
    {
        public static async Task SeedData(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var email = "test@test.com";
                var defaultUser = await userManager.FindByEmailAsync(email);

                // Vytvorenie role, ak ešte neexistuje
                if (await roleManager.FindByNameAsync("Administrator") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Administrator"));
                }

                // Vytvorenie používateľa, ak neexistuje
                if (defaultUser == null)
                {
                    defaultUser = new ApplicationUser(email);

                    var result = await userManager.CreateAsync(defaultUser, "Test!123");

                    if (result.Succeeded)
                    {
                        // Priradenie role admin
                        await userManager.AddToRoleAsync(defaultUser, "Administrator");
                    }
                }
            }
        }
    }

}
