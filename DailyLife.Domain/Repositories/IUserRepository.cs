using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DailyLife.Domain.Repositories;

public interface IUserRepository
{
    Task UpdateAsync(AppUser user);
    Task<AppUser?> GetAsunc(string id);
    Task<bool> IsEmailUniqe(string email, CancellationToken cancellationToken);  
    Task<IdentityResult> CreateAsync(AppUser user, string password);
    Task<(string? token, AppUser? user)> GetEmailConfirmationInfo(string userId);
    Task<IdentityResult> ConfirmeEmailAsync(string token, string userId);
    Task<IdentityResult> AddTnRoleAsync(AppUser user, string roleName); 
}