using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace DailyLife.Application.Identity.Commands.ConfirmEmail;

internal class ConfirmEmailCommandHandiler : ICommandHandiler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandiler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));
        var userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.userId));
        if (token is null || userId is null)
            return Result.Failure(StatusCodes.Status500InternalServerError, Error.NullValue);

        var result = await _userRepository.ConfirmeEmailAsync(token, userId);
        return Result.Create(result.Succeeded,
            result.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError,
            result.Errors
            .Select(e => 
            new Error(e.Code, e.Description)).ToArray());
    }
}