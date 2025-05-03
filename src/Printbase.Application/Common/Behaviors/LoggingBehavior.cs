using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Printbase.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        logger.LogInformation($"Handling {requestName}");
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();
            
            logger.LogInformation($"Handling {requestName} in {stopwatch.ElapsedMilliseconds}ms");
            
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, $"Error handling {requestName} after {stopwatch.ElapsedMilliseconds}ms");
            throw;
        }
    }
}