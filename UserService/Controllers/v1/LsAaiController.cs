using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UserService.Configs;

namespace UserService.Controllers.v1;

[Route("api/v1/[controller]")]
public class LsAaiController : ControllerBase
{
    [HttpGet("generate-access-code-url")]
    public IActionResult GenerateAuthUrl()
    {
        var ru = new RequestUrl(IdentityConfigs.AuthorityAuthUrl);

        var url = ru.CreateAuthorizeUrl(
            clientId: IdentityConfigs.LsAaiConfigs.ClientId,
            responseType: "code id_token",
            redirectUri: "http://localhost:7220/callback",
            scope: "openid");

        return Ok(url);
    }
}