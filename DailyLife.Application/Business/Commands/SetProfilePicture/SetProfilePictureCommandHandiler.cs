using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.SetProfilePicture;

internal sealed class SetProfilePictureCommandHandiler(IFileService fileService,
    IBusinessRepository businessRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor contextAccessor)
    : ICommandHandiler<SetProfilePictureCommand, string>
{

    public async Task<Result<string>> Handle(SetProfilePictureCommand request, CancellationToken cancellationToken)
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


        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(),
         "wwwroot", "Businesses", $"{business.Id}");
        Directory.CreateDirectory(uploadDirectory);

        (string absPath, string physicalPath) = await fileService.StoreAsync(request.Picture, uploadDirectory);
        business.SetProfilePictureUrl(physicalPath);
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success(absPath);
    }
}
