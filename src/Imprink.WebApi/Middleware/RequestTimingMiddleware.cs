using System.Diagnostics;

namespace Imprink.WebApi.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
            
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
                
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var method = context.Request.Method;
            var path = context.Request.Path;
            var statusCode = context.Response.StatusCode;
                
            logger.LogInformation(
                "Request {Method} {Path} completed in {ElapsedMs}ms with status code {StatusCode}",
                method, path, elapsedMs, statusCode);
        }
    }
}

public static class RequestTimingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimingMiddleware>();
    }
}