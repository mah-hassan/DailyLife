using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.AddPictureToAlbum;

public sealed record AddToAlbumCommand(Guid businessId, List<IFormFile> pictures)
    : ICommand<string>;
