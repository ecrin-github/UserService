using UserService.Contracts.Access;
using UserService.Services.UserService;

namespace UserService.Services.AccessService;

public class AccessService : IAccessService
{
    private readonly IUserService _userService;

    public AccessService(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<AccessResponse> GetUserAccessDataAsync(string id)
    {
        var user = await _userService.GetUserAsync(id);

        return new AccessResponse
        {
            Studies = new List<StudyResponse>
            {
                new()
                {
                    StudyId = 10049,
                    SdSid = "RMS-NCT00005129",
                    StudyTitle = "Bogalusa Heart Study",
                    DataObjects = new List<DataObjectResponse>
                    {
                        new()
                        {
                            ObjectId = 300066,
                            SdOid = "RMS-NCT00005129::12::29730626",
                            ObjectTitle = "Cohort profile: The MULTI sTUdy Diabetes rEsearch (MULTITUDE) consortium.",
                            ObjectDescription = "Object description 1",
                            ObjectInstances = new List<ObjectInstanceResponse>
                            {
                                new()
                                {
                                    Id = 400536,
                                    SdOid = "RMS-NCT00005129::12::29730626",
                                    UrlAccessible = false,
                                    Url = ""
                                }
                            }
                        }
                    }
                },
            }
        };
    }
}