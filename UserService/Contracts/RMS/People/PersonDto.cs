using System.Text.Json.Serialization;

namespace UserService.Contracts.RMS.People;

public class PersonDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("orgName")]
    public string OrgName { get; set; }
    
    [JsonPropertyName("roleName")]
    public string RoleName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
}