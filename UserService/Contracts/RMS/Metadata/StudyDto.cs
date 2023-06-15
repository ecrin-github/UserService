using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS.Metadata;

public class StudyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("sdSid")]
    public string SdSid { get; set; }
    
    [JsonPropertyName("displayTitle")]
    public string DisplayTitle { get; set; }
    
    [JsonPropertyName("typeName")]
    public string TypeName { get; set; }
    
    [JsonPropertyName("statusName")]
    public string StatusName { get; set; }
}