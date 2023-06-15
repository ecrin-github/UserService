using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS.Metadata;

public class DataObjectDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("sdOid")]
    public string SdOid { get; set; }
    
    [JsonPropertyName("sdSid")]
    public string SdSid { get; set; }
    
    [JsonPropertyName("displayTitle")]
    public string DisplayTitle { get; set; }
    
    [JsonPropertyName("studyName")]
    public string StudyName { get; set; }
    
    [JsonPropertyName("typeName")]
    public string TypeName { get; set; }
}