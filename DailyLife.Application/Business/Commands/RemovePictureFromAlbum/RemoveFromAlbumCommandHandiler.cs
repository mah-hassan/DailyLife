using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.RemovePictureFromAlbum;

internal sealed class RemoveFromAlbumCommandHandiler(
    IBusinessRepository businessRepository,
    IHttpContextAccessor contextAccessor)
    : ICommandHandiler<RemoveFromAlbumCommand>
{
    public async Task<Result> Handle(RemoveFromAlbumCommand request, CancellationToken cancellationToken)
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

        var filePath = Directory.GetFiles(business.Album)
            .FirstOrDefault(file => file.Contains(request.fileName,
            StringComparison.OrdinalIgnoreCase));

        if (filePath is null)
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                new Error("FileName", $"there is no file named '{request.fileName}' in business album"));
        }

        File.Delete(filePath);

        return Result.Success();
    }
}
