namespace UserService.Contracts.User;

public class UserRequest
{
    public string sub { get; set; }
    public string name { get; set; }
    public string given_name { get; set; }
    public string family_name { get; set; }
    public string email { get; set; }
}