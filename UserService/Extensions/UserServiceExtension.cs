using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using UserService.Configs;

namespace UserService.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication
            (options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.IsEssential = true;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = IdentityConfigs.AuthorityUrl;
                options.ClientId = IdentityConfigs.LsAaiConfigs.ClientId;
                options.ClientSecret = IdentityConfigs.LsAaiConfigs.ClientSecret;
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.UsePkce = true;
                options.CallbackPath = "/signin-oidc";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("orcid");
            });

        return services;
    }
}