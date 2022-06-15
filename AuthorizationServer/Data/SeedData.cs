using System.Security.Claims;
using AuthorizationServer.Configs;
using AuthorizationServer.Models;
using AuthorizationServer.Models.DbContext;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AuthorizationServer.Data;

public static class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<UserDbContext>();
        context?.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var admin = userMgr.FindByNameAsync("admin").Result;
        if (admin == null)
        {
            admin = new User
            {
                UserName = UserConfigs.InternalUser.UserName,
                Email = "admin@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(admin, UserConfigs.InternalUser.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(admin, new Claim[]{
                new (JwtClaimTypes.Name, "Sergei Gorianin"),
                new (JwtClaimTypes.GivenName, "Sergei"),
                new (JwtClaimTypes.FamilyName, "Gorianin"),
                new (JwtClaimTypes.WebSite, "https://sg.com"),
                new (JwtClaimTypes.Role, RoleConfigs.InternalUserRoleConfigs.RoleName),
                new ("organisation", "ECRIN"),
                new (JwtClaimTypes.PhoneNumber, "1234567890"),
                new (JwtClaimTypes.Address, "Astrakhan, Russia"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("admin created");
        }
        else
        {
            Log.Debug("admin already exists");
        }

        var user = userMgr.FindByNameAsync("user").Result;
        if (user == null)
        {
            user = new User
            {
                UserName = UserConfigs.ExternalUser.UserName,
                Email = "user@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(user, UserConfigs.ExternalUser.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(user, new Claim[]{
                new (JwtClaimTypes.Name, "John Doe"),
                new (JwtClaimTypes.GivenName, "John"),
                new (JwtClaimTypes.FamilyName, "Doe"),
                new (JwtClaimTypes.WebSite, "https://jd.com"),
                new (JwtClaimTypes.Role, RoleConfigs.ExternalUserRoleConfigs.RoleName),
                new ("organisation", "Organisation name"),
                new (JwtClaimTypes.PhoneNumber, "0987654321"),
                new (JwtClaimTypes.Address, "Paris, France"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("user created");
        }
        else
        {
            Log.Debug("user already exists");
        }
    }
}