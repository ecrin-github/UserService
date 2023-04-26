using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BasicAuth;
using UserService.Contracts.User;
using UserService.Services.UserService;

namespace UserService.Controllers.v1;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer"), BasicAuthorization]
[Route("api/v1/[controller]/[action]")]
public class UserApiController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserApiController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersByOrganisationId(int orgId)
    {
        var users = await _userService.GetUsersByOrganisationIdAsync(orgId);
        return Ok(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersByOrganisationName(string name)
    {
        var users = await _userService.GetUsersByOrganisationNameAsync(name);
        return Ok(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUserAsync(id);
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserByLsAai(string id)
    {
        var user = await _userService.GetUserByLsAaiAsync(id);
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByMailAsync(email);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRole(string email)
    {
        var user = await _userService.GetUserByMailAsync(email);
        var role = await _userService.GetUserRoleAsync(user);
        
        return Ok(role);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetOrCreateUser([FromBody] UserRequest userRequest)
    {
        var user = await _userService.GetOrCreateUserAsync(userRequest);
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserResponse userRequest)
    {
        var user = await _userService.UpdateUserAsync(userRequest);
        return Ok(user);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUserByEmail(string email)
    {
        await _userService.DeleteUserByEmailAsync(email);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUserByIdAsync(id);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> SetRoleToUser(string id, string role)
    {
        var res = await _userService.SetUserRoleAsync(id, role);
        return Ok(res);
    }
}