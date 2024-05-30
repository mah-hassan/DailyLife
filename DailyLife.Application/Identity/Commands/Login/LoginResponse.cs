namespace DailyLife.Application.Identity.Commands.Login;

public record LoginResponse
{
    public required string Name { get; init; }
    public string? ProfilePicture { get; init; }
    public required string Token { get; init; }
}
