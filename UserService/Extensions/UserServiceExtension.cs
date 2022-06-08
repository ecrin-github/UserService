using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using UserService.Configs;

namespace UserService.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = IdentityConfigs.AuthorityUrl;
                options.ClientId = "rms";
                options.ClientSecret = "fbGLQslZ6gCm3SlYRRhQ5tkSDtQHm7F1kmVjAsPvStLObXWxMnozXAmoqYtd";
                options.ResponseType = "code";
                
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.SaveTokens = true;

                options.GetClaimsFromUserInfoEndpoint = true;
            });

        return services;
    }
}