using DailyLife.Application.Abstractions;
using DailyLife.Application.Identity.Commands.SetProfilePicture;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DailyLife.Application.Identity.Commands.UpdateProfilePicture;

internal sealed class SetProfilePictureCommandHandiler(
    IHttpContextAccessor contextAccessor,
    IFileService fileService,
    IUserRepository userRepository)
        : ICommandHandiler<SetProfilePictureCommand, string>
{
    public async Task<Result<string>> Handle(SetProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor
            .HttpContext
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Result.Failure<string>(StatusCodes.Status400BadRequest,
                new Error("UserId","UserId is null"));
        }

        var user = await userRepository.GetAsunc(userId);
        if (user is null)
            return Result
                .Failure<string>(StatusCodes.Status404NotFound,
                new Error("UserNotFound", "User was not registerd"));

        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "user");
        Directory.CreateDirectory(physicalPath);
        fileService.Delete(user.ProfilePicture);
        var paths = await fileService.StoreAsync(request.Picture, physicalPath);
        user.ProfilePicture = paths.physicalPath;
        await userRepository.UpdateAsync(user);
        return Result.Success(paths.absolutePath);
    }
}