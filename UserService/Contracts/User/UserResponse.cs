namespace UserService.Contracts.User;

public class UserResponse : Models.User
{
    public string Role { get; set; } = string.Empty;
}