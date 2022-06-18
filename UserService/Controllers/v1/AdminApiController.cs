using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Configs;

namespace UserService.Controllers.v1;

[Route($"admin/{ApiConfigs.ApiVersion}")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminApiController : ControllerBase
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }
}