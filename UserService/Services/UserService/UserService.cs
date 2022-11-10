using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Configs;
using UserService.Contracts.User;
using UserService.Helpers.Adapters;
using UserService.Models;
using UserService.Models.DbContext;
using UserService.Services.RoleService;

namespace UserService.Services.UserService;

public class UserService : IUserService
{
    private readonly UserDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IRoleService _roleService;
    private readonly IUserAdapter _userAdapter;
    
    public UserService(UserManager<User> userManager, IRoleService roleService, IUserAdapter userAdapter, UserDbContext dbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _userAdapter = userAdapter ?? throw new ArgumentNullException(nameof(userAdapter));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    private static string GeneratePassword()
    {
        const string chars = "PpQqSsVvXx0123456789!@#$%^&*()_+";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static bool IsAdminRoleEmail(string email)
    {
        return InternalRolesEmails.AdminEmailList.Contains(email);
    }

    private static bool IsInternalRoleEmail(string email)
    {
        if (InternalRolesEmails.ManagerEmailList.Contains(email))
        {
            return true;
        }

        var domain = email.Split('@')[1];

        if (domain.Contains("ecrin"))
        {
            return true;
        }

        return false;
    }
    
    private async Task<IdentityResult> SetRoleAsync(User user)
    {
        if (IsAdminRoleEmail(user.Email))
        {
            var adminRole = await _roleService.GetRoleByNameAsync(RoleConfigs.AdminRoleConfigs.RoleName);
            if (adminRole == null)
            {
                throw new Exception("Admin role not found");
            }

            var checkRole = await _userManager.IsInRoleAsync(user, adminRole.Name);
            
            if (!checkRole)
            {
                var res = await _userManager.AddToRoleAsync(user, adminRole.Name);
                if (!res.Succeeded)
                {
                    throw new Exception("Error while adding user to admin role");
                }
                return res;
            }
        }

        if (IsInternalRoleEmail(user.Email))
        {
            var managerRole = await _roleService.GetRoleByNameAsync(RoleConfigs.ManagerRoleConfigs.RoleName);
            if (managerRole == null)
            {
                throw new Exception("Manager role not found");
            }
            
            var res = await _userManager.AddToRoleAsync(user, managerRole.Name);
            if (!res.Succeeded)
            {
                throw new Exception("Error while adding user to manager role");
            }
            return res;
            
        }

        var userRole = await _roleService.GetRoleByNameAsync(RoleConfigs.UserRoleConfigs.RoleName);
        if (userRole == null)
        {
            throw new Exception("User role not found");
        }
        
        var resUser = await _userManager.AddToRoleAsync(user, userRole.Name);
        if (!resUser.Succeeded)
        {
            throw new Exception("Error while adding user to user role");
        }

        return resUser;
    }

    public async Task<IList<UserResponse>> GetUsersAsync()
    {
        var usersList = new List<UserResponse>();
        var users = await _userManager.Users.AsNoTracking().ToListAsync();
        
        if (users.Count == 0)
        {
            throw new Exception("No users found");
        }
        
        foreach (var user in users)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole.Count == 0)
            {
                continue;
            }

            var role = userRole[0];
            
            var userResponse = _userAdapter.MapUserResponse(user, role);
            usersList.Add(userResponse);
        }

        return usersList;
    }

    public async Task<IList<UserResponse>> GetUsersByOrganisationIdAsync(int orgId)
    {
        var usersList = new List<UserResponse>();
        var users = await _userManager.Users.AsNoTracking().Where(x => x.OrganisationId == orgId).ToListAsync();

        if (users.Count == 0)
        {
            throw new Exception("No users found");
        }
        
        foreach (var user in users)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole.Count == 0)
            {
                continue;
            }

            var role = userRole[0];
            
            var userResponse = _userAdapter.MapUserResponse(user, role);
            usersList.Add(userResponse);
        }

        return usersList;
    }

    public async Task<IList<UserResponse>> GetUsersByOrganisationNameAsync(string orgName)
    {
        var usersList = new List<UserResponse>();
        var users = await _userManager.Users.AsNoTracking().Where(x => x.Organisation.Equals(orgName)).ToListAsync();

        if (users.Count == 0)
        {
            throw new Exception("No users found");
        }
        
        foreach (var user in users)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole.Count == 0)
            {
                continue;
            }

            var role = userRole[0];
            
            var userResponse = _userAdapter.MapUserResponse(user, role);
            usersList.Add(userResponse);
        }

        return usersList;
    }

    public async Task<UserResponse> GetUserAsync(string id)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var role = await GetUserRoleAsync(user);
        return _userAdapter.MapUserResponse(user, role);
    }

    public async Task<UserResponse> GetUserByMailAsync(string email)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var role = await GetUserRoleAsync(user);
        return _userAdapter.MapUserResponse(user, role);
    }

    public async Task<string> GetUserRoleAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.Count > 0 ? roles[0] : string.Empty;
    }

    public async Task<UserResponse> GetOrCreateUserAsync(UserRequest userRequest)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.LsAaiId!.Equals(userRequest.sub) || x.Email.Equals(userRequest.email));
        if (user != null)
        {
            return _userAdapter.MapUserResponse(user, await GetUserRoleAsync(user));
        }

        return await CreateUserAsync(userRequest);
    }

    public async Task<UserResponse> CreateUserAsync(UserRequest userRequest)
    {
        var checkUser = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.LsAaiId!.Equals(userRequest.sub) || x.Email.Equals(userRequest.email));
        if (checkUser != null)
        {
            throw new Exception("User already exists");
        }
        
        var user = new User
        {
            LsAaiId = userRequest.sub,
            FullName = userRequest.name,
            FamilyName = userRequest.family_name,
            GivenName = userRequest.given_name,
            UserName = userRequest.email,
            Email = userRequest.email,
            OrganisationId = 10001,
            Organisation = "ECRIN",
        };
        
        var result = await _userManager.CreateAsync(user, GeneratePassword());
        
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed");
        }

        var setRoleResult = await SetRoleAsync(user);
        if (!setRoleResult.Succeeded)
        {
            throw new Exception("Role assignment failed");
        }

        return _userAdapter.MapUserResponse(user, await GetUserRoleAsync(user));
    }
    
    public async Task<UserResponse> UpdateUserAsync(UserResponse userResponse)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(userResponse.Id));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Email = userResponse.Email;
        user.UserName = userResponse.Email.Split('@')[0];

        user.LsAaiId = userResponse.LsAaiId;
        user.FullName = userResponse.FullName;
        user.FamilyName = userResponse.FamilyName;
        user.GivenName = userResponse.GivenName;
        user.OrganisationId = 10001;
        user.Organisation = "ECRIN";
        
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0)
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }
        
        var setRoleResult = await SetRoleAsync(user);
        if (!setRoleResult.Succeeded)
        {
            throw new Exception("Role assignment failed");
        }
        
        return _userAdapter.MapUserResponse(user, await GetUserRoleAsync(user));
    }

    public async Task DeleteUserByEmailAsync(string email)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0)
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteUserByIdAsync(string id)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0)
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserResponse> SetUserRoleAsync(string id, string role)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0)
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }
        
        var userRole = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Name.Equals(role));
        if (userRole == null)
        {
            throw new Exception("Role not found");
        }
        
        var setRoleResult = await _userManager.AddToRoleAsync(user, userRole.Name);
        
        if (!setRoleResult.Succeeded)
        {
            throw new Exception("Role assignment failed");
        }
        
        return _userAdapter.MapUserResponse(user, await GetUserRoleAsync(user));
    }
}