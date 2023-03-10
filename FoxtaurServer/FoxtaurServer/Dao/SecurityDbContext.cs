using FoxtaurServer.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao;

/// <summary>
/// Security DB context
/// </summary>
public class SecurityDbContext : IdentityDbContext<User>
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add custom stuff here
    }
}