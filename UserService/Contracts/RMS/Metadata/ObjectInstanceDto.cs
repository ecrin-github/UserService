using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS.Metadata;

public class ObjectInstanceDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("sdOid")]
    public string SdOid { get; set; }
    
    [JsonPropertyName("instanceTypeId")]
    public int? InstanceTypeId { get; set; }
    
    [JsonPropertyName("repositoryOrgId")]
    public int? RepositoryOrgId { get; set; }
    
    [JsonPropertyName("repositoryOrg")]
    public string? RepositoryOrg { get; set; }
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("urlAccessible")]
    public bool? UrlAccessible { get; set; }
    
    [JsonPropertyName("urlLastChecked")]
    public DateTime? UrlLastChecked { get; set; }
    
    [JsonPropertyName("resourceTypeId")]
    public int? ResourceTypeId { get; set; }
    
    [JsonPropertyName("resourceSize")]
    public string? ResourceSize { get; set; }
    
    [JsonPropertyName("resourceSizeUnits")]
    public string? ResourceSizeUnits { get; set; }
    
    [JsonPropertyName("resourceComments")]
    public string? ResourceComments { get; set; }
}