using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        string[] roleNames = { "Admin", "Employee" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed default user
        var defaultAdmin = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        if (userManager.Users.All(u => u.UserName != defaultAdmin.UserName))
        {
            var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultAdmin, "Password123!");
                await userManager.AddToRoleAsync(defaultAdmin, "Admin");
            }
        }
        // Seed default user
        var defaultUser = new ApplicationUser
        {
            UserName = "employee@example.com",
            Email = "employee@example.com",
            EmailConfirmed = true
        };

        if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Password123!");
                await userManager.AddToRoleAsync(defaultUser, "Employee");
            }
        }
    }
}
