using DailyLife.Domain.Entities;
using DailyLife.Domain.Errors;
using DailyLife.Domain.Repositories;
using DailyLife.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    
    public UserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }





    public async Task<(string? token, AppUser? user)> GetEmailConfirmationInfo(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) {
            return (string.Empty, user);
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return (token, user);
    }

    public async Task<bool> IsEmailUniqe(string email, CancellationToken cancellationToken)
        => !await _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task<IdentityResult> AddTnRoleAsync(AppUser user, string roleName)
        => await _userManager.AddToRoleAsync(user, roleName);

    public async Task<IdentityResult> ConfirmeEmailAsync(string token, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return IdentityResult.Failed( 
                new IdentityError()
                {
                    Code = "User.NotFound",
                    Description = $"user with id {userId} was not found"
                });
        return await _userManager.ConfirmEmailAsync(user, token);

    }



    public async Task<IdentityResult> CreateAsync(AppUser user, string password) 
        => await _userManager.CreateAsync(user, password);

    public async Task<AppUser?> GetAsunc(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task UpdateAsync(AppUser user)
    {
        await _userManager.UpdateAsync(user);
    }
}
