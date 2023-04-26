namespace UserService.Contracts.Access;

public class ObjectInstanceResponse
{
    public int Id { get; set; }
    public string SdOid { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool UrlAccessible { get; set; }
}