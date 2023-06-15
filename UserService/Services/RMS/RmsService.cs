using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserService.Configs;
using UserService.Contracts.RMS;
using UserService.Contracts.RMS.Metadata;
using UserService.Contracts.RMS.People;

namespace UserService.Services.RMS;

public class RmsService : IRmsService
{
    public async Task<RmsResponseDto<StudyDto>> GetStudiesByOrgIdAsync(HttpClient client, int orgId)
    {
        var res = await client.GetAsync($"https://api-v2.ecrin-rms.org/api/studies/list/by-org/{orgId}");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<StudyDto>>(responseString)!;
    }

    public async Task<RmsResponseDto<DataObjectDto>> GetDataObjectBySdSidAsync(HttpClient client, string sdSid)
    {
        var res = await client.GetAsync($"https://api-v2.ecrin-rms.org/api/studies/{sdSid}/objects");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<DataObjectDto>>(responseString)!;
    }

    public async Task<RmsResponseDto<ObjectInstanceDto>> GetObjectInstancesBySdOidAsync(HttpClient client, string sdOid)
    {
        var res = await client.GetAsync($"https://api-v2.ecrin-rms.org/api/data-objects/{sdOid}/instances");
        var responseString = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RmsResponseDto<ObjectInstanceDto>>(responseString)!;
    }
}