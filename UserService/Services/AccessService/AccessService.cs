using System.Net.Http.Headers;
using System.Text;
using UserService.Configs;
using UserService.Contracts.Access;
using UserService.Contracts.RMS.Metadata;
using UserService.Helpers.Adapters;
using UserService.Services.PeopleService;
using UserService.Services.RMS;
using UserService.Services.UserService;

namespace UserService.Services.AccessService;

public class AccessService : IAccessService
{
    private readonly IUserService _userService;

    private readonly IPeopleService _peopleService;
    private readonly IRmsService _rmsService;
    private readonly IAccessServiceAdapter _accessServiceAdapter;

    public AccessService(IUserService userService, 
        IPeopleService peopleService, 
        IRmsService rmsService, 
        IAccessServiceAdapter accessServiceAdapter)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _peopleService = peopleService ?? throw new ArgumentNullException(nameof(peopleService));
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
        _accessServiceAdapter = accessServiceAdapter ?? throw new ArgumentNullException(nameof(accessServiceAdapter));
    }

    public async Task<AccessResponse> GetUserAccessDataAsync(string id)
    {
        // var user = await _userService.GetUserAsync(id);

        var client = new HttpClient();
        var bytesArray = Encoding.ASCII.GetBytes($"{Users.AdminUser}:{Users.AdminPassword}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytesArray));
        
        var studyList = new List<StudyResponse>();
        
        var studies = await _rmsService.GetStudiesByOrgIdAsync(client, 100001);

        if (studies.Total == 0 || studies.Data.Count == 0)
        {
            return new AccessResponse()
            {
                Studies = studyList
            };
        }

        foreach (var study in studies.Data)
        {
            var dataObjectsList = new List<DataObjectResponse>();
            
            var dataObjectsQuery = await _rmsService.GetDataObjectBySdSidAsync(client, study.SdSid);
            if (dataObjectsQuery.Total == 0 || dataObjectsQuery.Data!.Count == 0)
            {
                continue;
            }

            foreach (var dataObject in dataObjectsQuery.Data)
            {
                var dataObjectResponseObject = _accessServiceAdapter.MapToDataObjectResponse(dataObject);
                
                var objectInstancesQuery = await _rmsService.GetObjectInstancesBySdOidAsync(client, dataObject.SdOid);
                if (objectInstancesQuery.Total != 0 || objectInstancesQuery.Data!.Count != 0)
                {
                    var objectInstances = objectInstancesQuery.Data!
                        .Select(objectInstance => _accessServiceAdapter.MapToObjectInstanceResponse(objectInstance))
                        .ToList();

                    dataObjectResponseObject.ObjectInstances = objectInstances;
                }
                
                dataObjectsList.Add(dataObjectResponseObject);
            }

            var studyResponseObject = _accessServiceAdapter.MapToStudyResponse(study);
            studyResponseObject.DataObjects = dataObjectsList;
            
            studyList.Add(studyResponseObject);
        }

        return new AccessResponse()
        {
            Studies = studyList
        };
    }
}