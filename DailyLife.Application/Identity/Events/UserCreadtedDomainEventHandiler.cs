using DailyLife.Application.Abstractions;
using DailyLife.Domain.DomainEvents;
using DailyLife.Domain.Entities;
using DailyLife.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
namespace DailyLife.Application.Identity.Events;

internal class UserCreadtedDomainEventHandiler : INotificationHandler<UserCreadtedDomainEvent>
{
    private readonly IAvatarProvider _avatarProvider;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _usuManager;
    private readonly ILogger<UserCreadtedDomainEventHandiler> _logger;
    private const string _bodyContentType = "html";
    public UserCreadtedDomainEventHandiler(IEmailService emailService,
        IUserRepository userRepository,
        ILogger<UserCreadtedDomainEventHandiler> logger,
        IAvatarProvider avatarProvider,
        UserManager<AppUser> usuManager)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _logger = logger;
        _avatarProvider = avatarProvider;
        _usuManager = usuManager;
    }

    public async Task Handle(UserCreadtedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _usuManager.FindByIdAsync(notification.userId);
        var emailMessage = await PrepareConfirmationMessage(notification);
        await GenerateDefaultAvatar(user!);
        await _emailService.SendAsync(emailMessage, cancellationToken);
    }
    private string GenerateConfirmationUrl(string serverUrl ,string token, string userId)
    {
        var url = serverUrl + "/auth/confirmEmail";
        var routeParams = new Dictionary<string, string>();
        routeParams.Add(nameof(token), token);
        routeParams.Add(nameof(userId), userId);
        var queryString = QueryString.Create(routeParams);
        return url + queryString;
    }
    private async Task GenerateDefaultAvatar(AppUser user)
    {
        var avatarUrl = await _avatarProvider.TryGenerateAvatarAsync(user.FullName);
        user.ProfilePicture = avatarUrl;
        await _usuManager.UpdateAsync(user);
    }

    private async Task<EmailMessage> PrepareConfirmationMessage(UserCreadtedDomainEvent notification)
    {
        var confirmationInfo = await _userRepository.GetEmailConfirmationInfo(notification.userId);
        var user = confirmationInfo.user;
        if (user is null)
        {
            _logger.LogError("User Is Null");
            throw new NullReferenceException(nameof(user));
        }

        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationInfo.token!));
        var _userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
        var appDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "App");

        var messageTemplatePath = Path.Combine(appDirectory, "Html", "Welcome.html");
        var message = new StringBuilder(await File.ReadAllTextAsync(messageTemplatePath));

        var confirmationUrl = GenerateConfirmationUrl(notification.serverUrl, token, _userId);

        message.Replace("serverUrl", notification.serverUrl);
        message.Replace("confirmationUrl", confirmationUrl);
        message.Replace("FullName", user.FullName.Value);

        var finalMessage = message.ToString();
        var emailMessage = new EmailMessage(
            recipientName: user.FullName.Value,
            recipientAddress: user.Email!,
            subject: "Email Confirmation",
            body: finalMessage,
            contentType: _bodyContentType
        );

        return emailMessage;
    }

}
