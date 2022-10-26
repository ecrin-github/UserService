using UserService.Contracts.User;
using UserService.Models;

namespace UserService.Helpers.Adapters;

public interface IUserAdapter
{
    UserResponse MapUserResponse(User user, string role);
}