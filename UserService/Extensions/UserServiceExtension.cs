using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Configs;
using UserService.Helpers.Adapters;
using UserService.Models;
using UserService.Models.DbContext;
using UserService.Services.RoleService;
using UserService.Services.UserService;

namespace UserService.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<UserDbContext>(options =>
            options
                .UseNpgsql(DbConfigs.ConnectionString)
                .UseSnakeCaseNamingConvention());
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserAdapter, UserAdapter>();
        
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, Services.UserService.UserService>();
        
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