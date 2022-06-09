using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.v1;

public class UserApiController : BaseUserApiController
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        var claims = User.Claims.Select(claim => $"Claim type: {claim.Type} - Claim value: {claim.Value}").ToList();
        return Ok(claims);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }
}