using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.BasicAuth;
using UserService.Configs;
using UserService.Helpers.Adapters;
using UserService.Models.DbContext;
using UserService.Services.AccessService;
using UserService.Services.PeopleService;
using UserService.Services.RMS;
using UserService.Services.RoleService;
using UserService.Services.UserService;
using User = UserService.Models.User;

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
        services.AddScoped<IAccessService, AccessService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPeopleService, PeopleService>();

        services.AddScoped<IRmsService, RmsService>();

        services.AddScoped<IAccessServiceAdapter, AccessServiceAdapter>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = IdentityConfigs.AuthorityUrl;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.RequireHttpsMetadata = false;
            });

        services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, null);
        
        return services;
    }
}