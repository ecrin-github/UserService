using UserService.Contracts.RMS;
using UserService.Contracts.RMS.Metadata;

namespace UserService.Services.RMS;

public interface IRmsService
{
    Task<RmsResponseDto<StudyDto>> GetStudiesByOrgIdAsync(HttpClient client, int orgId);

    Task<RmsResponseDto<DataObjectDto>> GetDataObjectBySdSidAsync(HttpClient client, string sdSid);

    Task<RmsResponseDto<ObjectInstanceDto>> GetObjectInstancesBySdOidAsync(HttpClient client, string sdOid);
}