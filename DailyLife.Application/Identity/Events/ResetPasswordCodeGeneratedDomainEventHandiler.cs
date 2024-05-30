using DailyLife.Application.Abstractions;
using DailyLife.Domain.DomainEvents;
using MediatR;

namespace DailyLife.Application.Identity.Events
{
    
    internal sealed class ResetPasswordCodeGeneratedDomainEventHandiler
        : INotificationHandler<ResetPasswordCodeGeneratedDomainEvent>
    {
        private const string contentType = "html";
        private readonly IEmailService _emailService;

        public ResetPasswordCodeGeneratedDomainEventHandiler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(ResetPasswordCodeGeneratedDomainEvent notification, CancellationToken cancellationToken)
        {
            var message = new EmailMessage(notification.email,
                notification.fullName.Value,
                "Reset Password Code",
                $"Hellow!<h1> {notification.fullName.Value}" +
                $"<br />" +
                $"<p>{notification.code} This is your reset password code  do not share it </p>",
                contentType);
            await _emailService.SendAsync(message, cancellationToken);
        }
    }
}
