using DailyLife.Application.Abstractions;
using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DailyLife.Application.Identity.Commands.ForgetPassword;

internal sealed class ForgetPasswordCommandHandiler : ICommandHandiler<ForgetPasswordCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    public ForgetPasswordCommandHandiler(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.email);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            var error = _userManager.ErrorDescriber.InvalidEmail(request.email);
            return Result.Failure(StatusCodes.Status400BadRequest,new Error(
               error.Code, error.Description));
        }
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        user.ForgetPassword(code);
        await _unitOfWork.SaveIdentityChangesAsync(cancellationToken);
        return Result.Success();    
    }
}
