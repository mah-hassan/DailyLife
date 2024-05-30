using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DailyLife.Application.Identity.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandiler
    : ICommandHandiler<ResetPasswordCommand, IdentityResult>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordCommandHandiler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IdentityResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.email);
        if (user is null) 
        {
            return IdentityResult.Failed(_userManager
                .ErrorDescriber
                .InvalidEmail(request.email));
        }
        return await _userManager
            .ResetPasswordAsync(user,
            request.code,
            request.newPassword);
    }
}
