using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Models.DbContext;

namespace UserService.Services.RoleService;

public class RoleService : IRoleService
{
    private readonly UserDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public RoleService(RoleManager<IdentityRole> roleManager, UserDbContext dbContext)
    {
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<IList<IdentityRole>> GetRoles()
    {
        return await _dbContext.Roles.ToListAsync();
    }
    
    public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new ArgumentException("Role not found");
        }
        return role;
    }
}