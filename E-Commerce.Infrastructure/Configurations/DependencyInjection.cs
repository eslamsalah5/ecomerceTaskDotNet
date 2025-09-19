using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Add HttpContextAccessor
        services.AddHttpContextAccessor();

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                // configure identity options as needed
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        // Add SignInManager separately
        services.AddScoped<SignInManager<ApplicationUser>>();

        // Register Application Services
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
