using UserService.Contracts.Access;
using UserService.Contracts.RMS.Metadata;

namespace UserService.Helpers.Adapters;

public class AccessServiceAdapter : IAccessServiceAdapter
{
    public StudyResponse MapToStudyResponse(StudyDto study)
    {
        return new StudyResponse()
        {
            StudyId = study.Id,
            SdSid = study.SdSid,
            StudyTitle = study.DisplayTitle
        };
    }

    public DataObjectResponse MapToDataObjectResponse(DataObjectDto dataObject)
    {
        return new DataObjectResponse()
        {
            ObjectId = dataObject.Id,
            SdOid = dataObject.SdOid,
            ObjectTitle = dataObject.DisplayTitle,
            ObjectDescription = $"Description of the {dataObject.SdOid} data object"
        };
    }

    public ObjectInstanceResponse MapToObjectInstanceResponse(ObjectInstanceDto objectInstance)
    {
        return new ObjectInstanceResponse()
        {
            Id = objectInstance.Id,
            SdOid = objectInstance.SdOid,
            Url = objectInstance.Url!,
            UrlAccessible = objectInstance.UrlAccessible,
            RepositoryOrgId = objectInstance.RepositoryOrgId,
            RepositoryOrg = objectInstance.RepositoryOrg
        };
    }
}