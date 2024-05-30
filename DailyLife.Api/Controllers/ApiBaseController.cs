using DailyLife.Api.Extentions;
using DailyLife.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyLife.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        private ISender _sender;

        protected ApiBaseController(ISender sender)
        {
            _sender = sender;
        }
        protected async Task<IActionResult> PrepareResponse(IRequest<Result> request)
        {
            var result = await _sender.Send(request);
            return result.IsSuccess ? Ok(result)
                : StatusCode(result.StatusCode, result.ToProblemDetails());
        }  
        public async Task<IActionResult> PrepareResponse<TResponse>(IRequest<Result<TResponse>> request)
        {
            var result = await _sender.Send(request);
            return result.IsSuccess ? Ok(result.Value)
                : StatusCode(result.StatusCode, result.ToProblemDetails());
        }
    }
}