
using E_Commerce.Api.Configurations;
using E_Commerce.Api.Middleware;
using E_Commerce.Infrastructure.Configurations;
using E_Commerce.Infrastructure.Data;

namespace E_Commerce.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Infrastructure (DbContext + Identity)
        builder.Services.AddInfrastructure(builder.Configuration);

        // CORS Configuration
        builder.Services.AddCorsConfiguration(builder.Configuration);

        // Authentication & Swagger
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddSwaggerDocumentation();

        var app = builder.Build();

        // Seed data
        using (var scope = app.Services.CreateScope())
        {
            await DataSeeder.SeedAsync(scope.ServiceProvider);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerDocumentation();
            app.UseCors("DevPolicy"); // Use permissive policy in development
            
            // Configure HTTPS redirection for development
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseCors("ECommercePolicy"); // Use restrictive policy in production
            app.UseHttpsRedirection();
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Enable static file serving for uploaded images
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
