using UserService.Contracts.Access;
using UserService.Contracts.RMS.Metadata;

namespace UserService.Helpers.Adapters;

public interface IAccessServiceAdapter
{
    StudyResponse MapToStudyResponse(StudyDto study);

    DataObjectResponse MapToDataObjectResponse(DataObjectDto dataObject);

    ObjectInstanceResponse MapToObjectInstanceResponse(ObjectInstanceDto objectInstance);
}