using Microsoft.AspNetCore.Identity;

namespace UserService.Services.RoleService;

public interface IRoleService
{
    Task<IList<IdentityRole>> GetRoles();
    Task<IdentityRole> GetRoleByNameAsync(string roleName);
}