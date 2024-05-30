using DailyLife.Api.Contracts;
using DailyLife.Api.Extentions;
using DailyLife.Application;
using DailyLife.Application.Review.Commands.Create;
using DailyLife.Application.Review.Commands.Delete;
using DailyLife.Application.Review.Commands.Update;
using DailyLife.Application.Review.Queries.GetBusinessReviewsSummary;
using DailyLife.Application.Review.Queries.Pagenate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DailyLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ApiBaseController
{

    public ReviewsController(ISender sender)
        : base(sender)
    {
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(ReviewRequest request)
    {
        var command = new CreateReviewCommand(request.rate,
                            request.comment,
                            request.businessId);
        return await PrepareResponse(command);

    }


    [HttpGet]
    public async Task<IActionResult> GetReviewsAsync([FromQuery]Guid businessId,
     [FromQuery]int page,
     [FromQuery]int size)
    {
        var query = new GetBusinessReviewsQuery(businessId, page, size);
        return await PrepareResponse(query);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetReviewsSummaryAsync(Guid businessId)
    {
        var query = new GetBusinessReviewsSummaryQuery(businessId);
        return await PrepareResponse(query);

    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, ReviewRequest request)
    {
        var command = new UpdateReviewCommand(id, request.rate,
                            request.comment);
        return await PrepareResponse(command);
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteReviewCommand(id);
        return await PrepareResponse(command);
    }
}
