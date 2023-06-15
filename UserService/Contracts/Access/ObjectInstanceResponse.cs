namespace UserService.Contracts.Access;

public class ObjectInstanceResponse
{
    public int Id { get; set; }
    public string SdOid { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool? UrlAccessible { get; set; }
    
    public int? RepositoryOrgId { get; set; }
    
    public string? RepositoryOrg { get; set; }
}