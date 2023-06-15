using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserService.Configs;
using UserService.Contracts.RMS;
using UserService.Contracts.RMS.People;

namespace UserService.Services.PeopleService;

public class PeopleService : IPeopleService
{
    public async Task<RmsResponseDto<PersonDto>> GetPeopleAsync(HttpClient client)
    {
        var res = await client.GetAsync("https://api-v2.ecrin-rms.org/api/people/list");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<PersonDto>>(responseString)!;
    }

    public async Task<RmsResponseDto<PersonDto>> GetPeopleFromOrganisationAsync(HttpClient client, string orgName)
    {
        var res = await client.GetAsync("https://api-v2.ecrin-rms.org/api/people/list");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<PersonDto>>(responseString)!;
    }

    public async Task<RmsResponseDto<OrganisationDto>> GetOrganisationsByNameAsync(HttpClient client, string orgName)
    {
        var res = await client.GetAsync($"https://api-v2.ecrin-rms.org/api/context/orgs-table/{orgName}");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<OrganisationDto>>(responseString)!;
    }
}