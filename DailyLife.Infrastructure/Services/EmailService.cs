using DailyLife.Application.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailSettings = DailyLife.Infrastructure.Settings.MailSettings;

namespace DailyLife.Infrastructure.Services;

internal class EmailService : IEmailService
{
    private readonly MailSettings _settings;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptionsMonitor<MailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.CurrentValue;
        _logger = logger;
    }

    public async Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        var mailMessage = new MimeMessage();
        mailMessage.Subject = message.subject;
        mailMessage.From.Add(new MailboxAddress(_settings.DisplayedName, _settings.Email));
        mailMessage.To.Add(new MailboxAddress(message.recipientAddress, message.recipientAddress));

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.body
        };

        mailMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.SmtpServer, _settings.Port,
                    SecureSocketOptions.StartTls, cancellationToken);
                await client.AuthenticateAsync(_settings.Email, _settings.Password, cancellationToken);
                await client.SendAsync(mailMessage, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }

            _logger.LogInformation("Email successfully sent to {email} for user {name}", message.recipientAddress, message.recipientName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send email to {email} for user {name}", message.recipientAddress, message.recipientName);
        }
    }
}
