using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.Update;

internal sealed class UpdateBusinessCommandHandiler(IUnitOfWork unitOfWork, IBusinessRepository businessRepository, IHttpContextAccessor contextAccessor)
        : ICommandHandiler<UpdateBusinessCommand>
{
    public async Task<Result> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
    {

        var business = await businessRepository.GetById(new Id(request.id));

        if (business is null)
        {
            return Result.Failure(StatusCodes.Status404NotFound,
                DomainErrors.BusinessErrors.NotExsist(request.id));
        }

        var ownerId = contextAccessor.GetUserId();

        if (business.OwnerId != ownerId)
        {
            return Result.Failure(StatusCodes.Status403Forbidden,
            DomainErrors.BusinessErrors.Forbidden);
        }

        var result = business.Update(request.name,
            request.description,
            request.workTimes,
            request.latituade,
            request.longituade,
            request.city,
            request.state,
            request.street,
            request.addressDescription);

        if (result.IsFailure)
            return result;  

        businessRepository.Update(business);
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}