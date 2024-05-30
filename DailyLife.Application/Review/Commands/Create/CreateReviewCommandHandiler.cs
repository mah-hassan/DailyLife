using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Review.Commands.Create;

internal sealed class CreateReviewCommandHandiler
    : ICommandHandiler<CreateReviewCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBusinessRepository _businessRepository;
    public CreateReviewCommandHandiler(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, IHttpContextAccessor contextAccessor, IBusinessRepository businessRepository)
    {
        _unitOfWork = unitOfWork;
        _reviewRepository = reviewRepository;
        _contextAccessor = contextAccessor;
        _businessRepository = businessRepository;
    }

    public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetById(new Id(request.businessId));
        if (business is null)
            return Result.Failure<Guid>(StatusCodes.Status404NotFound,
            new Error(nameof(request.businessId),
             $"Business with Id {request.businessId} was not found"));

        var comment = Comment.Create(request.comment);

        var createReviewResult = ReviewEntity
                        .Create(request.rate,
                                comment,
                                _contextAccessor.GetUserId(),
                                business.Id);

        if (createReviewResult.IsFailure)
            return Result.Failure<Guid>(StatusCodes.Status400BadRequest,
                createReviewResult.Errors.ToArray());

        _reviewRepository.Add(createReviewResult.Value);
        await _unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success(createReviewResult.Value.Id.Value);
    }
}