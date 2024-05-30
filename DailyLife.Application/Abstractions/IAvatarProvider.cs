using DailyLife.Domain.ValueObjects;

namespace DailyLife.Application.Abstractions;

public interface IAvatarProvider
{
    bool HasAvatar(string pictureName, FullName fullName);
    Task<string?> TryGenerateAvatarAsync(FullName fullName, string? picturName = null);
}
