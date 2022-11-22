using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        return services;
    }
}