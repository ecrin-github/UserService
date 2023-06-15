using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS.People;

public class OrganisationDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("defaultName")]
    public string DefaultName { get; set; }
}