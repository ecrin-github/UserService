namespace UserService.Contracts.Access;

public class StudyResponse
{
    public int StudyId { get; set; }
    public string SdSid { get; set; } = string.Empty;
    public string StudyTitle { get; set; } = string.Empty;
    public IList<DataObjectResponse> DataObjects { get; set; } = new List<DataObjectResponse>();
}