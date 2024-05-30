using DailyLife.Application.Abstractions;
using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DailyLife.Application.Identity.Commands.Login;

internal sealed class LoginCommandHandiler
    (UserManager<AppUser> userManager,
    IJwtProvider jwtProvider,
    IFileService fileService)
        : ICommandHandiler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var user = await userManager.FindByEmailAsync(request.email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.password))
        {
            return Result.Failure<LoginResponse>(StatusCodes.Status400BadRequest, new Error("Invalid Credintials:",
                "Invalid Email Or Password"));
        }
        if (!user.EmailConfirmed)
            return Result.Failure<LoginResponse>(StatusCodes.Status400BadRequest,
                new Error("Email:",
                "Email Not Confirmed"));

        var roles = await userManager.GetRolesAsync(user);
        roles = roles.ToList();
        var token = jwtProvider.Generate(user, roles.First());
        var response = new LoginResponse()
        {
            Name = user.FullName.Value,
            Token = token,
            ProfilePicture = fileService.GetAbsolutePath(user.ProfilePicture),
        };
        return Result.Success<LoginResponse>(response);
    }
}