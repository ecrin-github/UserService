using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Models.DbContext;

public class UserDbContext : IdentityDbContext<User>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("users");
        
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .ToTable("users");

        builder.Entity<IdentityRole>()
            .ToTable("roles");

        builder.Entity<IdentityUserLogin<string>>()
            .ToTable("user_login");
        
        builder.Entity<IdentityUserClaim<string>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserRole<string>>()
            .ToTable("user_roles");
        
        builder.Entity<IdentityUserToken<string>>()
            .ToTable("user_token");
        
        builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("role_claims");
    }
}