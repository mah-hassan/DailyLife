using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.AddPictureToAlbum;

internal sealed class AddToAlbumCommandHandiler(
    IFileService fileService,
    IBusinessRepository businessRepository,
    IHttpContextAccessor contextAccessor)
    : ICommandHandiler<AddToAlbumCommand, string>
{
    public async Task<Result<string>> Handle(AddToAlbumCommand request, CancellationToken cancellationToken)
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

        var result = new List<string>();

        int albumCount = Directory.GetFiles(business.Album).Length;
        if (albumCount >= 5 || 5 < (albumCount + request.pictures.Count))
            return Result.Failure<string>(StatusCodes.Status400BadRequest,
            new Error("Business.Album", $"{business.Name} album is full, maximum number of pictures is 5"));
        foreach (var picture in request.pictures)
        {
            result.Add(await SaveToAlbum(business.Album, picture));
        }
        return Result.Success(string.Join('\n', result));
    }

    private async Task<string> SaveToAlbum(string albumPath, IFormFile picture)
    {
        (string absPath, string _) = await fileService.StoreAsync(picture, albumPath);
        return absPath;
    }
}
