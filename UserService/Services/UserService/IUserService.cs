using UserService.Contracts.User;
using UserService.Models;

namespace UserService.Services.UserService;

public interface IUserService
{
    Task<IList<UserResponse>> GetUsersAsync();
    Task<IList<UserResponse>> GetUsersByOrganisationIdAsync(int orgId);
    Task<IList<UserResponse>> GetUsersByOrganisationNameAsync(string orgName);
    Task<UserResponse> GetUserAsync(string id);
    Task<UserResponse> GetUserByLsAaiAsync(string id);
    Task<UserResponse> GetUserByMailAsync(string email);
    Task<string> GetUserRoleAsync(User user);
    Task<UserResponse> GetOrCreateUserAsync(UserRequest userRequest);
    Task<UserResponse> CreateUserAsync(UserRequest userRequest);
    Task<UserResponse> UpdateUserAsync(UserResponse userResponse);
    Task DeleteUserByEmailAsync(string email);
    Task DeleteUserByIdAsync(string id);
    Task<UserResponse> SetUserRoleAsync(string id, string role);
}