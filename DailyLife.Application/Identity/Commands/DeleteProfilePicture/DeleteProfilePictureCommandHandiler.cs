using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DailyLife.Application.Identity.Commands.DeleteProfilePicture;

internal sealed class DeleteProfilePictureCommandHandiler
    : ICommandHandiler<DeleteProfilePictureCommand, string>
{
    private readonly IAvatarProvider _userAvatarProvider;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IFileService _fileService;
    private readonly UserManager<AppUser> _userManager;

    public DeleteProfilePictureCommandHandiler(IAvatarProvider userAvatarProvider, IHttpContextAccessor contextAccessor,
        IFileService fileService, UserManager<AppUser> userManager)
    {
        _userAvatarProvider = userAvatarProvider;
        _contextAccessor = contextAccessor;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor
                      .GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result
                .Failure<string>(StatusCodes.Status404NotFound,
                new Error("UserNotFound", "User was not registerd"));
        if (user.ProfilePicture is { } &&
            _userAvatarProvider.HasAvatar(Path.GetFileName(user.ProfilePicture),
            user.FullName))
        {
            return Result.Failure<string>(StatusCodes.Status400BadRequest,
                new Error("ProfilePictrue", "No ProfilePictrue To delete"));
        }
        _fileService.Delete(user.ProfilePicture);
        var pyhscialPath = await _userAvatarProvider
            .TryGenerateAvatarAsync(user.FullName);
        if (pyhscialPath is { })
        {
            user.ProfilePicture = pyhscialPath;
            await _userManager.UpdateAsync(user);
        }
        var absPath = _fileService.GetAbsolutePath(pyhscialPath);
        return absPath is { } ? Result.Success(absPath)
            : Result.Failure<string>(StatusCodes.Status500InternalServerError,Error.NullValue);
    }
}
