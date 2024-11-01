using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using System.Security;
using System.Text;

namespace ForumPOF.Extensions;

public static class ApiExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Secretkey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];

                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void DynamicLog(this ILogger logger, string message, int statusCode)
    {
        switch (statusCode)
        {
            case 200:
            case 204:
            case 201:
                logger.LogInformation(message);
                break;
            case 400:
            case 401:
            case 403:
            case 404:
            case 500:
                logger.LogError(message);
                break;
            default:
                break;
        }
    }
}
