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
        /*
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = IdentityConfigs.AuthorityUrl;
            options.ClientId = IdentityConfigs.RmsClientConfigs.ClientId;
            options.ClientSecret = IdentityConfigs.RmsClientConfigs.ClientSecret;
            options.ResponseType = IdentityConfigs.RmsClientConfigs.ResponseType;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            
            options.ClaimActions.DeleteClaim("sid");
            options.ClaimActions.DeleteClaim("idp");
            
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("offline_access");
            options.Scope.Add("address");
            options.Scope.Add("email");
            options.Scope.Add("phone");
            options.Scope.Add("roles");
            
            options.ClaimActions.MapUniqueJsonKey("roles", "role");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                RoleClaimType = "role"
            };
        });
        */
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:7011";
                    
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
            
        // adds an authorization policy to make sure the token is for scope 'api1'
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "openid");
            });
        });
        

        return services;
    }
}