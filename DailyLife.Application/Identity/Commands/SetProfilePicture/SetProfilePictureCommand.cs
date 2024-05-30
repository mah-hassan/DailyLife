using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Identity.Commands.SetProfilePicture;

public sealed record SetProfilePictureCommand(IFormFile Picture)
    : ICommand<string>;
