using E_Commerce.Domain.Exceptions;
using E_Commerce.Domain.Shared;
using System.Text.Json;

namespace E_Commerce.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                NotFoundException => ApiResponse.ErrorResult(exception.Message),
                ValidationException validationEx => ApiResponse.ErrorResult("Validation failed", validationEx.Errors),
                UnauthorizedException => ApiResponse.ErrorResult(exception.Message),
                BadRequestException => ApiResponse.ErrorResult(exception.Message),
                _ => ApiResponse.ErrorResult("An error occurred")
            };

            context.Response.StatusCode = exception switch
            {
                NotFoundException => 404,
                ValidationException => 400,
                UnauthorizedException => 401,
                BadRequestException => 400,
                _ => 500
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}