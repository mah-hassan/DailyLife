using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.RemoveProfilePicture;

internal sealed class RemoveProfilePictureCommandHandiler(
    IBusinessRepository businessRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor contextAccessor)
    : ICommandHandiler<RemoveProfilePictureCommand>
{
    public async Task<Result> Handle(RemoveProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetById(new Id(request.businessId));

        if (business is null)
        {
            return Result.Failure<string>(StatusCodes.Status404NotFound,
                DomainErrors.BusinessErrors.NotExsist(request.businessId));
        }

        var ownerId = contextAccessor.GetUserId();

        if (business.OwnerId != ownerId)
        {
            return Result.Failure<string>(StatusCodes.Status403Forbidden,
            DomainErrors.BusinessErrors.Forbidden);
        }

        if (business.ProfilePicture is { })
        {
            File.Delete(business.ProfilePicture);
        }
        business.DeleteProfilePicture();
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}
