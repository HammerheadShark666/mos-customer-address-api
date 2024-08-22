using Microservice.Customer.Address.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microservice.Customer.Address.Api.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = EnvironmentVariables.JwtIssuer,
                ValidAudience = EnvironmentVariables.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSymmetricSecurityKey)
                )
            };

            options.MapInboundClaims = false;
        });

        services.AddAuthentication();
    }
}