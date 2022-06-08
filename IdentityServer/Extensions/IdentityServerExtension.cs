using IdentityServer.Configs;

namespace IdentityServer.Extensions;

public static class IdentityServerExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllersWithViews();
        
        services.AddIdentityServer()
            .AddInMemoryClients(IdentityServerConfigs.Clients)
            .AddInMemoryApiResources(IdentityServerConfigs.ApiResources)
            .AddInMemoryApiScopes(IdentityServerConfigs.ApiScopes)
            .AddInMemoryIdentityResources(IdentityServerConfigs.IdentityResources)
            .AddTestUsers(IdentityServerConfigs.TestUsers)
            .AddDeveloperSigningCredential();
        
        return services;
    }
}