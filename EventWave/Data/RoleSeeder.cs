using EventWave.Models;
using Microsoft.AspNetCore.Identity;

namespace EventWave.Data
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Client))
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));

            if (!await roleManager.RoleExistsAsync(Roles.Organizer))
                await roleManager.CreateAsync(new IdentityRole(Roles.Organizer));

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
    }
}
