using UserService.Contracts.Access;

namespace UserService.Services.AccessService;

public interface IAccessService
{
    Task<AccessResponse> GetUserAccessDataAsync(string id);
}