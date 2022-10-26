using UserService.Contracts.User;
using UserService.Models;

namespace UserService.Helpers.Adapters;

public class UserAdapter : IUserAdapter
{
    public UserResponse MapUserResponse(User user, string role)
    {
        return new UserResponse
        {
            Id = user.Id,
            LsAaiId = user.LsAaiId,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
            FullName = user.FullName,
            Website = user.Website,
            Email = user.Email,
            Role = role,
            Organisation = user.Organisation,
            OrganisationId = user.OrganisationId,
            Location = user.Location,
            Address = user.Address,
            PersonId = user.PersonId
        };
    }
}