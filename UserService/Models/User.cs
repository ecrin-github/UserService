using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UserService.Models;

[Table("users", Schema = "users")]
public class User : IdentityUser
{
    public string? LsAaiId { get; set; } = string.Empty;
    public string? GivenName { get; set; } = string.Empty;
    public string? FamilyName { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? Website { get; set; } = string.Empty;
    public string? Organisation { get; set; } = string.Empty;
    public int? OrganisationId { get; set; } = 0;
    public string? Address { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public int? PersonId { get; set; } = 0;
}