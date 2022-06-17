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
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminRole = roleMgr.FindByNameAsync(RoleConfigs.AdminRoleConfigs.RoleName).Result;
        if (adminRole == null)
        {
            adminRole = new IdentityRole
            {
                Id = RoleConfigs.AdminRoleConfigs.RoleId,
                Name = RoleConfigs.AdminRoleConfigs.RoleName,
                NormalizedName = RoleConfigs.AdminRoleConfigs.RoleNormalizedName
            };
            var res = roleMgr.CreateAsync(adminRole).Result;
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.First().Description);
            }
        }
        
        var managerRole = roleMgr.FindByNameAsync(RoleConfigs.ManagerRoleConfigs.RoleName).Result;
        if (managerRole == null)
        {
            managerRole = new IdentityRole
            {
                Id = RoleConfigs.ManagerRoleConfigs.RoleId,
                Name = RoleConfigs.ManagerRoleConfigs.RoleName,
                NormalizedName = RoleConfigs.ManagerRoleConfigs.RoleNormalizedName
            };
            var res = roleMgr.CreateAsync(managerRole).Result;
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.First().Description);
            }
        }
        
        var userRole = roleMgr.FindByNameAsync(RoleConfigs.UserRoleConfigs.RoleName).Result;
        if (userRole == null)
        {
            userRole = new IdentityRole
            {
                Id = RoleConfigs.UserRoleConfigs.RoleId,
                Name = RoleConfigs.UserRoleConfigs.RoleName,
                NormalizedName = RoleConfigs.UserRoleConfigs.RoleNormalizedName
            };
            var res = roleMgr.CreateAsync(userRole).Result;
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.First().Description);
            }
        }
        
        var admin = userMgr.FindByNameAsync(UserConfigs.Admin.UserName).Result;
        if (admin == null)
        {
            admin = new User
            {
                Id = UserConfigs.Admin.Id,
                UserName = UserConfigs.Admin.UserName,
                Email = "admin@email.com",
                EmailConfirmed = true,
                GivenName = "Sergei",
                FirstName = "Gorianin",
                FullName = "Sergei Gorianin",
                Website = "https://sg.org",
                Organisation = "ECRIN",
                OrganisationId = 12345,
                Address = "Bakinskaya st. 4/2",
                Location = "Astrakhan, Russia",
                PersonId = 11313
            };
            var result = userMgr.CreateAsync(admin, UserConfigs.Admin.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            var addedRole = userMgr.AddToRoleAsync(admin, RoleConfigs.AdminRoleConfigs.RoleName).Result;
            if (addedRole == null)
            {
                throw new Exception(addedRole?.Errors.First().Description);
            }            
            Log.Debug("admin created");
        }
        else
        {
            Log.Debug("admin already exists");
        }
        
        
        var manager = userMgr.FindByNameAsync(UserConfigs.Manager.UserName).Result;
        if (manager == null)
        {
            manager = new User
            {
                Id = UserConfigs.Manager.Id,
                UserName = UserConfigs.Manager.UserName,
                Email = "manager@email.com",
                EmailConfirmed = true,
                GivenName = "John",
                FirstName = "Doe",
                FullName = "John Doe",
                Website = "https://jd.org",
                Organisation = "Organisation name",
                OrganisationId = 54321,
                Address = "5 st. Watt",
                Location = "Paris, France",
                PersonId = 1231
            };
            var result = userMgr.CreateAsync(manager, UserConfigs.Manager.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            var addedRole = userMgr.AddToRoleAsync(manager, RoleConfigs.ManagerRoleConfigs.RoleName).Result;
            if (addedRole == null)
            {
                throw new Exception(addedRole?.Errors.First().Description);
            }
            
            Log.Debug("manager created");
        }
        else
        {
            Log.Debug("manager already exists");
        }
        
        var user = userMgr.FindByNameAsync(UserConfigs.User.UserName).Result;
        if (user == null)
        {
            user = new User
            {
                Id = UserConfigs.User.Id,
                UserName = UserConfigs.User.UserName,
                Email = "user@email.com",
                EmailConfirmed = true,
                GivenName = "Chris",
                FirstName = "Own",
                FullName = "Chris Own",
                Website = "https://co.org",
                Organisation = "CO corp",
                OrganisationId = 123123,
                Address = "4 st. of Freedom",
                Location = "California, USA",
                PersonId = 1121
            };
            var result = userMgr.CreateAsync(user, UserConfigs.User.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            var addedRole = userMgr.AddToRoleAsync(user, RoleConfigs.UserRoleConfigs.RoleName).Result;
            if (addedRole == null)
            {
                throw new Exception(addedRole?.Errors.First().Description);
            }            
            Log.Debug("user created");
        }
        else
        {
            Log.Debug("user already exists");
        }
        
        
    }
}