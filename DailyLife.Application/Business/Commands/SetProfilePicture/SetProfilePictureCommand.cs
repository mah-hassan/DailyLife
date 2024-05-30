using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Commands.SetProfilePicture;

public sealed record SetProfilePictureCommand(Guid businessId, IFormFile Picture)
    : ICommand<string>;
