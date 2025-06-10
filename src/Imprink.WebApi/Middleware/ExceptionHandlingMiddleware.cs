using System.Net;
using System.Text.Json;
using Imprink.Application.Exceptions;

namespace Imprink.WebApi.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (_, _, shouldLog) = GetStatusCodeAndMessage(ex);
            
             if (shouldLog)
             {
                 logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
             }
             else
             {
                 logger.LogInformation("Handled: {Message}", ex.Message);
             }
                    
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, _) = GetStatusCodeAndMessage(exception);
        
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                message,
                timestamp = DateTime.UtcNow,
            }
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static (HttpStatusCode statusCode, string message, bool shouldLog) GetStatusCodeAndMessage(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message, false),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred", true)
        };
    }
}

public static class GlobalExceptionHandlingMiddlewareExtensions
{
    public static void UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
