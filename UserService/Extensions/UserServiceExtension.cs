using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using UserService.Configs;

namespace UserService.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = IdentityConfigs.AuthorityUrl;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = IdentityConfigs.AuthorityUrl;
                options.RequireHttpsMetadata = false;

                options.ClientId = IdentityConfigs.RmsClientConfigs.ClientId;
                options.ClientSecret = IdentityConfigs.RmsClientConfigs.ClientSecret;

                options.ClaimActions.MapUniqueJsonKey("roles", "role");

                options.ResponseType = IdentityConfigs.RmsClientConfigs.ResponseType;

                options.Scope.Add("roles");

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.TokenValidationParameters.RoleClaimType = "role";
            });


        return services;
    }
}