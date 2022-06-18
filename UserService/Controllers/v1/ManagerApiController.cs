using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Configs;

namespace UserService.Controllers.v1;

[Route($"manager/{ApiConfigs.ApiVersion}")]
[ApiController]
[Authorize(Roles = "Admin, Manager")]
public class ManagerApiController : ControllerBase
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }
}