using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Configs;

namespace UserService.Controllers.v1;

[Route($"{ApiConfigs.ApiUrl}/{ApiConfigs.ApiVersion}")]
[ApiController]
[Authorize]
public class BaseUserApiController : ControllerBase
{
    
}