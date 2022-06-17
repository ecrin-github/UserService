using System.Security.Claims;
using AuthorizationServer.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationServer.Services;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
    private readonly UserManager<User> _userManager;

    public ProfileService(
        UserManager<User> userManager, 
        IUserClaimsPrincipalFactory<User> claimsFactory)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _claimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(claimsFactory));
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        var roles = await _userManager.GetRolesAsync(user);
        var principal = await _claimsFactory.CreateAsync(user);

        var roleName = roles.Count > 0 ? roles[0] : string.Empty;

        var claims = principal.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        // Add custom claims in token here based on user properties or any other source
        claims.Add(new Claim("role", roleName));
        claims.Add(new Claim("given_name", user.GivenName ?? string.Empty));
        claims.Add(new Claim("first_name", user.FirstName ?? string.Empty));
        claims.Add(new Claim("full_name", user.FullName ?? string.Empty));
        claims.Add(new Claim("website", user.Website ?? string.Empty));
        claims.Add(new Claim("organisation", user.Organisation ?? string.Empty));
        claims.Add(new Claim("organisation_id", user.OrganisationId.ToString() ?? string.Empty));
        claims.Add(new Claim("person_id", user.PersonId.ToString() ?? string.Empty));
        claims.Add(new Claim("address", user.Address ?? string.Empty));
        claims.Add(new Claim("location", user.Location ?? string.Empty));
        
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}