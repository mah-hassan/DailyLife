using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Review.Commands.Update;

internal sealed class UpdateReviewCommandHandiler
    : ICommandHandiler<UpdateReviewCommand>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandiler(IHttpContextAccessor contextAccessor, IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _contextAccessor = contextAccessor;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetById(new Id(request.id));
        if (review is null)
            return Result.Failure(StatusCodes.Status404NotFound,
                new Error("ID", $"No review was foun with the Id: {request.id}"));
        if (review.OwnerId != _contextAccessor.GetUserId())
            return Result.Failure(StatusCodes.Status401Unauthorized,
                new Error("User", $"User is Unauthorized to access this resource"));
    
        var comment = Comment.Create(request.comment);
        if (comment.IsFailure)
            return comment;
        if (request.rate is < 0 or > 5)
            return Result.Failure(StatusCodes.Status400BadRequest,
                new Error(nameof(request.rate),
                 $"{request.rate} is in valid rate value, value shouid be in range 0 to 5"));

        review.Update(request.rate, comment);

        await _unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}
