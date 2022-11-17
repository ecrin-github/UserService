namespace UserService.Contracts.User;

public class UserRequest
{
    public string sub { get; set; }
    public string? name { get; set; } = string.Empty;
    public string? given_name { get; set; } = string.Empty;
    public string? family_name { get; set; } = string.Empty;
    public string? email { get; set; } = string.Empty;
}