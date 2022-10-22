using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.v1;

public class UserApiController : BaseUserApiController
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        Console.WriteLine(User.Identity.AuthenticationType.ToString());
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }
}