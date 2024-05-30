
namespace DailyLife.Application.Abstractions;

public interface IEmailService
{

    Task SendAsync(EmailMessage message, CancellationToken cancellationToken);
}
/// <summary>
/// Create Email Message Instance
/// </summary>
/// <param name="subject"></param>
/// <param name="body"></param>
/// <param name="contentType"></param>
public record EmailMessage(string recipientAddress, string recipientName, string subject,
    string body,
    string contentType)
{
 
}