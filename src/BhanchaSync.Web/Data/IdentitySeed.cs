using Microsoft.AspNetCore.Identity;

namespace BhanchaSync.Web.Data;

// Ensures the three app roles exist, and creates one default Admin
// account so there's always a way to log in and manage the system.
// In a real production deployment you'd remove/rotate this default
// password immediately — it's here purely to unblock local development.
public static class IdentitySeed
{
    public static readonly string[] Roles = { "Admin", "Waiter", "Kitchen" };

    private const string DefaultAdminEmail = "admin@bhanchasync.local";
    private const string DefaultAdminPassword = "Admin@12345";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminUser = await userManager.FindByEmailAsync(DefaultAdminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = DefaultAdminEmail,
                Email = DefaultAdminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, DefaultAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
