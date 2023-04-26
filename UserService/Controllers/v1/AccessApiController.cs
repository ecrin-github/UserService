using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BasicAuth;
using UserService.Services.AccessService;

namespace UserService.Controllers.v1;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer"), BasicAuthorization]
[Route("api/v1/[controller]/[action]")]
public class AccessApiController : ControllerBase
{
    private readonly IAccessService _accessService;
    
    public AccessApiController(IAccessService accessService)
    {
        _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserAccessData(string id)
    {
        var accesses = await _accessService.GetUserAccessDataAsync(id);
        return Ok(accesses);
    }
}