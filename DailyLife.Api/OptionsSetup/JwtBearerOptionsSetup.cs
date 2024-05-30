using DailyLife.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DailyLife.Api.OptionsSetup;

public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
{
    private readonly IOptionsMonitor<JwtOptions> _options;
    private readonly ILogger<JwtBearerOptionsSetup> _logger;
    public JwtBearerOptionsSetup(IOptionsMonitor<JwtOptions> options,
        ILogger<JwtBearerOptionsSetup> logger)
    {
        _options = options;
        _logger = logger;
    }

    public void Configure(JwtBearerOptions options)
    {
        _logger.LogInformation("successfully hit configure method");
        var _jwtOptions = _options.CurrentValue;
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero,
        };
       
    }
}