namespace DailyLife.Infrastructure.Settings;

public class MailSettings
{
    public required string Password { get; set; }
    public required string SmtpServer { get; set; }
    public required string DisplayedName { get; set; }
    public required string Email { get; set; }
    public int Port { get; set; }

}
