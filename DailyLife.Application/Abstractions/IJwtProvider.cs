using DailyLife.Domain.Entities;

namespace DailyLife.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(AppUser user, string role);
}
