using DailyLife.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DailyLife.Api.Extentions
{
    public static class ResultToProblemDetails
    {
        public static ProblemDetails ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException($"can`nt convert success result to ProblemDetails");
            }
            return new()
            {
                Status = result.StatusCode,
                Extensions = result.Errors.ToDictionary(e => e.Code,
                e => (object?)e.Message),
            };
        }        
    }
}
