using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyLife.Application.Abstractions.Behaviors;

internal class LoggingPipeLineBehavior<TRequest, TResponse>
    (ILogger<LoggingPipeLineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        logger.LogInformation("Start Processing {requestName}", requestName);

        TResponse result =  await next();

        if (result.IsSuccess)
        {
            logger.LogInformation("{requestName} Completed Successfully", requestName);
        }
        else
        {
                
             logger
                .LogWarning("{requestName} Completed with errors {@errors}", requestName, result.Errors);
        }

        return result;
    }
}
