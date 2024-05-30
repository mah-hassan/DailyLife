using Microsoft.AspNetCore.Http;
using DailyLife.Application.Helpers;
using DailyLife.Application.Abstractions;
using DailyLife.Domain.ValueObjects;
namespace DailyLife.Application.Business.Commands.Create;

internal sealed class CreateBusinessCommandHandiler
    : ICommandHandiler<CreateBusinessCommand>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBusinessRepository _businessRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateBusinessCommandHandiler(IHttpContextAccessor contextAccessor, IBusinessRepository businessRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _contextAccessor = contextAccessor;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var ownerId = _contextAccessor.GetUserId();

        if (await _categoryRepository.GetById(new Id(request.categoryId)) is null)
        {
            return Result.Failure(StatusCodes.Status404NotFound,
                DomainErrors.CategoryErrors.NotExsist(request.categoryId));
        }
        if (await _businessRepository.OwnerExist(ownerId))
        {
            return Result.Failure(StatusCodes.Status409Conflict,
                DomainErrors.BusinessErrors.OwnerExsists);
        }
        if (!await _businessRepository.IsNameUniqe(request.name))
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                DomainErrors.BusinessErrors.NameIsTaken(request.name));
        }

        var location = Location.Create(request.latituade,
            request.longituade);

        var address = Address.Create(request.city,
            request.state,
            request.street,
            request.addressDescription);

        var businessResult = BusinessAggregate.Create(
            location,
            address,
            request.name,
            request.description,
            ownerId,
            new Id(request.categoryId));

        if (businessResult.IsFailure)
            return businessResult;
        var business = businessResult.Value;

        if (!request.workTimes.Any())
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
            new Error("WorkTime", "Worktime is required, can not be empty"));
        }

        if (request.workTimes.Count > 7)
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                new Error("WorkTime", "can not add more than 7 worktimes"));
        }

        foreach (var wt in request.workTimes)
        {
            var result = business.AddWorkTime(wt);
            if (result.IsFailure)
                return result;
        }

        _businessRepository.Add(business);

        await _unitOfWork.SaveBusinessChangesAsync(cancellationToken);

        return Result.Success(business.Id.ToString());
    }
}