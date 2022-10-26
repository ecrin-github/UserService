using Microsoft.AspNetCore.Mvc;
using UserService.Services.RoleService;

namespace UserService.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class RoleApiController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleApiController(IRoleService roleService)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetRoles();
        return Ok(roles);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoleByName(string name)
    {
        var role = await _roleService.GetRoleByNameAsync(name);
        return Ok(role);
    }
}