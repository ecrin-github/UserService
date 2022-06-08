using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace UserService.Controllers.v1;

public class UserApiController : BaseUserApiController
{
    [HttpGet("info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var claimsData = User.Claims.Select(claim => $"Claim type: {claim.Type} - Claim value: {claim.Value}").ToList();
        
        var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
        if (identityToken != null) claimsData.Add($"Identity token is: {identityToken}");

        return Ok(claimsData);
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> UserLogout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        return Ok("Logged out");
    }
}