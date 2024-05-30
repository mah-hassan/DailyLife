using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Utilities.Encoders;
using OtpNet;
using System.Text;

namespace DailyLife.Infrastructure.Authentication
{
    public sealed class TotpTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
    {
        private const int _step = 5 * 60;
        public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            var email = await manager.GetEmailAsync(user).ConfigureAwait(false);

            return !string.IsNullOrWhiteSpace(email) && await manager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var key = await manager.GetUserIdAsync(user);
            var totp = new Totp(Encoding.UTF8.GetBytes(key), _step);
            return totp.ComputeTotp();
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var key = Encoding.UTF8.GetBytes(await manager.GetUserIdAsync(user));
            var totp = new Totp(key, _step);
            return totp.VerifyTotp(token, out _);
        }
    }
}