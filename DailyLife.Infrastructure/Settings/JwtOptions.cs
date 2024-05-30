namespace DailyLife.Infrastructure.Settings;

public class JwtOptions
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int LifeTme { get; set; }
}
