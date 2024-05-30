using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DailyLife.Application.Helpers;

internal static class HttpContextAccessorHelper
{
    internal static Guid GetUserId(this IHttpContextAccessor contextAccessor)
    {
        if (contextAccessor is null)
        {
            throw new ArgumentNullException(nameof(contextAccessor));
        }

        var userId = contextAccessor
                    .HttpContext
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            throw new InvalidOperationException($"user Id does not exsist");
        return Guid.Parse(userId);
    }
    internal static string GetServerUrl(this IHttpContextAccessor contextAccessor)
    {
        return $"{contextAccessor.HttpContext.Request.Scheme}://" +
                        $"{contextAccessor.HttpContext.Request.Host}";
    }
}
