using DailyLife.Application.Abstractions;
using DailyLife.Domain.Entities;
using DailyLife.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace DailyLife.Infrastructure.Authentication;

internal class JwtProvider : IJwtProvider
{
    private readonly IOptionsMonitor<JwtOptions> _options;

    public JwtProvider(IOptionsMonitor<JwtOptions> options)
    {
        _options = options;
    }

    public string Generate(AppUser user, string role)
    {
        var jwt = _options.CurrentValue;
        var authClaims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role , role),
            new(JwtRegisteredClaimNames.Sid, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                authClaims,
                null,
                DateTime.UtcNow.AddDays(jwt.LifeTme),
                signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
