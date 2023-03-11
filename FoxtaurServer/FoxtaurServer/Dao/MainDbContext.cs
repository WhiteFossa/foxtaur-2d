using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao;

/// <summary>
/// Main DB context
/// </summary>
public class MainDbContext : DbContext
{
    /// <summary>
    /// Teams
    /// </summary>
    public DbSet<Team> Teams { get; set; }

    /// <summary>
    /// Hunters profiles
    /// </summary>
    public DbSet<Profile> Profiles { get; set; }

    /// <summary>
    /// Maps
    /// </summary>
    public DbSet<Map> Maps { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }
}