using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Review.Commands.Delete;

internal sealed class DeleteReviewCommandHandiler
    : ICommandHandiler<DeleteReviewCommand>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteReviewCommandHandiler(IHttpContextAccessor contextAccessor, IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _contextAccessor = contextAccessor;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetById(new Id(request.reviewId));
        if (review is null)
            return Result.Failure(StatusCodes.Status404NotFound,
                new Error("ID", $"No review was foun with the Id: {request.reviewId}"));
        if (review.OwnerId != _contextAccessor.GetUserId())
            return Result.Failure(StatusCodes.Status401Unauthorized,
                new Error("User", $"User is Unauthorized to access this resource"));

        _reviewRepository.Remove(review);
        await _unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}
