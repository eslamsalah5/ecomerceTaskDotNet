using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DataSeeder");

            try
            {
                // Create roles
                await CreateRolesAsync(roleManager, logger);
                
                // Create admin user
                await CreateAdminUserAsync(userManager, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding data");
            }
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            var roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role {Role} created successfully", role);
                }
            }
        }

        private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "Admin123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Admin user created successfully with email: {Email}", adminEmail);
                }
                else
                {
                    logger.LogError("Failed to create admin user. Errors: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}