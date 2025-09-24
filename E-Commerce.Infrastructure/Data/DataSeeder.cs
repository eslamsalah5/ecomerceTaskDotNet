using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Persistence;
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
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DataSeeder");

            try
            {
                // Create roles
                await CreateRolesAsync(roleManager, logger);
                
                // Create admin user
                await CreateAdminUserAsync(userManager, logger);
                
                // Create sample customer
                await CreateCustomerUserAsync(userManager, logger);
                
                // Seed products
                await SeedProductsAsync(context, logger);
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

        private static async Task CreateCustomerUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            const string customerEmail = "customer@test.com";
            const string customerPassword = "Customer123";

            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    FirstName = "Test",
                    LastName = "Customer",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(customerUser, customerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                    logger.LogInformation("Customer user created successfully with email: {Email}", customerEmail);
                }
                else
                {
                    logger.LogError("Failed to create customer user. Errors: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedProductsAsync(AppDbContext context, ILogger logger)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Category = "Electronics",
                        ProductCode = "P01",
                        Name = "Gaming Laptop",
                        ImagePath = "/images/laptop1.jpg",
                        Price = 1500.00m,
                        MinimumQuantity = 5,
                        DiscountRate = 10,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Electronics",
                        ProductCode = "P02",
                        Name = "Smartphone",
                        ImagePath = "/images/phone1.jpg",
                        Price = 800.00m,
                        MinimumQuantity = 10,
                        DiscountRate = 5,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Clothing",
                        ProductCode = "P03",
                        Name = "Cotton T-Shirt",
                        ImagePath = "/images/shirt1.jpg",
                        Price = 25.99m,
                        MinimumQuantity = 20,
                        DiscountRate = null,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Books",
                        ProductCode = "P04",
                        Name = "Programming Guide",
                        ImagePath = "/images/book1.jpg",
                        Price = 49.99m,
                        MinimumQuantity = 15,
                        DiscountRate = 15,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Home",
                        ProductCode = "P05",
                        Name = "Office Chair",
                        ImagePath = "/images/chair1.jpg",
                        Price = 199.99m,
                        MinimumQuantity = 3,
                        DiscountRate = 20,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Electronics",
                        ProductCode = "P06",
                        Name = "Wireless Mouse",
                        ImagePath = "/images/mouse1.jpg",
                        Price = 45.00m,
                        MinimumQuantity = 50,
                        DiscountRate = 5,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Electronics",
                        ProductCode = "P07",
                        Name = "Mechanical Keyboard",
                        ImagePath = "/images/keyboard1.jpg",
                        Price = 120.00m,
                        MinimumQuantity = 25,
                        DiscountRate = 10,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Clothing",
                        ProductCode = "P08",
                        Name = "Jeans",
                        ImagePath = "/images/jeans1.jpg",
                        Price = 65.99m,
                        MinimumQuantity = 30,
                        DiscountRate = null,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Home",
                        ProductCode = "P09",
                        Name = "Table Lamp",
                        ImagePath = "/images/lamp1.jpg",
                        Price = 35.50m,
                        MinimumQuantity = 15,
                        DiscountRate = 8,
                        IsDeleted = false,
                        DeletedAt = null
                    },
                    new Product
                    {
                        Category = "Books",
                        ProductCode = "P10",
                        Name = "Design Patterns Book",
                        ImagePath = "/images/book2.jpg",
                        Price = 55.00m,
                        MinimumQuantity = 10,
                        DiscountRate = 12,
                        IsDeleted = false,
                        DeletedAt = null
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
                logger.LogInformation("Sample products seeded successfully");
            }
        }
    }
}