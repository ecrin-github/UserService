namespace UserService.Contracts.Access;

public class DataObjectResponse
{
    public int ObjectId { get; set; }
    public string SdOid { get; set; } = string.Empty;
    public string ObjectTitle { get; set; } = string.Empty;
    public string ObjectDescription { get; set; } = string.Empty;
    public IList<ObjectInstanceResponse> ObjectInstances { get; set; } = new List<ObjectInstanceResponse>();
}