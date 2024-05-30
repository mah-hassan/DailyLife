using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.Delete;

internal sealed class DeleteBusinessCommandHandiler
    : ICommandHandiler<DeleteBusinessCommand>
{
    private readonly IBusinessRepository _businessRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public DeleteBusinessCommandHandiler(IBusinessRepository businessRepository, 
        IUnitOfWork unitOfWork,
        IHttpContextAccessor contextAccessor)
    {
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<Result> Handle(DeleteBusinessCommand request, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetById(new Id(request.id));
        if (business is null)
        {
            return Result.Failure(StatusCodes.Status404NotFound,
                DomainErrors.BusinessErrors.NotExsist(request.id));
        }
        var ownerId = _contextAccessor.GetUserId();
        if (business.OwnerId != ownerId)
        {
            return Result.Failure(StatusCodes.Status403Forbidden,
                DomainErrors.BusinessErrors.Forbidden);
        }

        RemoveAssets(business);

        _businessRepository.Delete(business);

        business.RaiseDomainEvent(new BusinessDeletedDomainEvent(business.Id));
        
        await _unitOfWork.SaveBusinessChangesAsync(cancellationToken);

        return Result.Success();    
    }
    private void RemoveAssets(BusinessAggregate business)
    {
        var assetsDirectory = Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot", "Businesses", business.Id.ToString());
        Directory.Delete(assetsDirectory, true);
    }
}