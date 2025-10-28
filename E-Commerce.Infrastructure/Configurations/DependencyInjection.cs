using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Persistence;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

        // Register AutoMapper
        var applicationAssembly = Assembly.Load("E-Commerce.Application");
        services.AddAutoMapper(applicationAssembly);

        // Register FluentValidation
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Application Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}
