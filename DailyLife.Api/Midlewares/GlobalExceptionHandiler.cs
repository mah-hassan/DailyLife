using DailyLife.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DailyLife.Api.Midlewares;

public class GlobalExceptionHandiler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandiler> _logger;

    public GlobalExceptionHandiler(ILogger<GlobalExceptionHandiler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int statusCode, string message) = MapException(exception);
        httpContext.Response.StatusCode = statusCode;
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        _logger.LogError(exception,
            "An Error occured {traceId}", traceId);
        var response = new ProblemDetails
        {
            Status = statusCode,
            Detail = message,
            Extensions =
            {
                { nameof(traceId), traceId }
            },
            Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{statusCode}"
        };
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
    private (int statusCode, string message) MapException(Exception exception)
        => exception switch
        {
            InValidExtentionException => (StatusCodes.Status415UnsupportedMediaType, exception.Message),
            _ => (500, "An error occured while processing the request")
        };
}
