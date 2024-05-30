using DailyLife.Domain.Entities;
using DailyLife.Infrastructure.Authentication;
using DailyLife.Infrastructure.Data.Identity;
using DailyLife.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DailyLife.Api.Extentions;

internal static class IdentityExtentions
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
       services.AddDataProtection();
       services.AddIdentityCore<AppUser>(options =>
        {
            options.User.AllowedUserNameCharacters += " ";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultEmailProvider)
            .AddTokenProvider<TotpTokenProvider<AppUser>>(TokenOptions.DefaultProvider);
       services
            .Configure<DataProtectionTokenProviderOptions>(
            options => options.TokenLifespan = TimeSpan.FromMinutes(5));
       services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer((options) =>
        {
            var sp = services.BuildServiceProvider();
            var _jwtOptions = sp.GetRequiredService<IOptionsMonitor<JwtOptions>>().CurrentValue;
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero,
            };
        });
        return services;
    }
}
