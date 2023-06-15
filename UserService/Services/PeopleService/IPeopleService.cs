using UserService.Contracts.RMS;
using UserService.Contracts.RMS.People;

namespace UserService.Services.PeopleService;

public interface IPeopleService
{
    Task<RmsResponseDto<PersonDto>> GetPeopleAsync(HttpClient client);

    Task<RmsResponseDto<PersonDto>> GetPeopleFromOrganisationAsync(HttpClient client, string orgName);

    Task<RmsResponseDto<OrganisationDto>> GetOrganisationsByNameAsync(HttpClient client, string orgName);
}