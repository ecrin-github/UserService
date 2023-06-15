using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS;

public class RmsResponseDto<T>
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
    
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }
    
    [JsonPropertyName("messages")]
    public IList<string>? Messages { get; set; }
    
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }
}