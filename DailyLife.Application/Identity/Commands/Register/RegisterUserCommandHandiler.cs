using DailyLife.Domain.Entities;
using DailyLife.Application.Abstractions;
using DailyLife.Domain.Enums;
using Microsoft.AspNetCore.Http;
using DailyLife.Application.Helpers;
namespace DailyLife.Application.Identity.Commands.Register;

internal sealed class RegisterUserCommandHandiler
    (IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor contextAccessor)
    : ICommandHandiler<RegisterUserCommand>
{
    private IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsEmailUniqe(request.email, cancellationToken))
        {
            return Result
                .Failure(StatusCodes.Status409Conflict,
                new Error("Email.AlreadyExists",
                               $"{request.email} is in use"));
        }

        var userCreationResult = AppUser.Create
            (request.email, request.fullname, request.dateOfBirth);

        if (userCreationResult.IsFailure)
        {
            return userCreationResult;
        }
        var user = userCreationResult.Value;
        var result = await _userRepository.CreateAsync(user,
            request.password);
        user.RaiseDomainEvent(new UserCreadtedDomainEvent(user.Id, contextAccessor.GetServerUrl()));
        var role = request.role switch
        {
            AppRoles.Admin => nameof(AppRoles.Admin),
            AppRoles.BusinessOwner => nameof(AppRoles.BusinessOwner),
            _ => nameof(AppRoles.Default),
        };
        var rsgisterInRoleResult = await _userRepository
            .AddTnRoleAsync(userCreationResult.Value, role);

        if (!rsgisterInRoleResult.Succeeded)
        {
            return Result.Failure(500,
                rsgisterInRoleResult.Errors
                .Select(e => new Error(e.Code, e.Description)).ToArray());
        }


        await _unitOfWork.SaveIdentityChangesAsync(cancellationToken);
        return Result.Create(
            result.Errors
            .Select(e => 
                    new Error(e.Code,
                    e.Description)).ToArray(),
            result.Succeeded ? StatusCodes.Status200OK 
            : StatusCodes.Status400BadRequest);
    }
}