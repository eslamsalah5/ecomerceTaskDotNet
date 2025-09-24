using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Api.Configurations;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("ECommercePolicy", policy =>
            {
                policy.WithOrigins("http://localhost:4200", 
                                 "http://localhost:3000", 
                                 "http://localhost:5173",
                                 "https://localhost:4200",
                                 "https://localhost:3000", 
                                 "https://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });

            // Add a more permissive policy for development
            options.AddPolicy("DevPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}