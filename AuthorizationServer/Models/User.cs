using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationServer.Models;

[Table("users", Schema = "users")]
public class User : IdentityUser
{
    
}