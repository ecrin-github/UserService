using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.v1;

[ApiController]
[Authorize(Roles = "admin")]
public class AdminApiController : ControllerBase
{
    [HttpGet("admin/claims")]
    public IActionResult GetClaims()
    {
        var claims = User.Claims.Select(claim => $"Claim type: {claim.Type} - Claim value: {claim.Value}").ToList();
        return Ok(claims);
    }
}