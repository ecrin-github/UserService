using AuthorizationServer.Configs;
using AuthorizationServer.Models;
using AuthorizationServer.Models.DbContext;
using AuthorizationServer.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Extensions;

public static class AuthorizationServerExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllersWithViews();
        
        services.AddDbContext<UserDbContext>(options =>
            options
                .UseNpgsql(DbConfigs.ConnectionString)
                .UseSnakeCaseNamingConvention());
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(IdentityServerConfigs.IdentityResources)
            .AddInMemoryClients(IdentityServerConfigs.Clients)
            .AddAspNetIdentity<User>()
            .AddDeveloperSigningCredential();

        services.AddScoped<IProfileService, ProfileService>();

        return services;
    }
}