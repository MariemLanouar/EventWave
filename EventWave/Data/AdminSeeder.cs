using EventWave.Models;
using Microsoft.AspNetCore.Identity;

namespace EventWave.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@test.com";
            string adminPassword = "Admin123!";

           
     
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Admin creation error: {error.Code} - {error.Description}");

                }
            }
        }

    }
}
