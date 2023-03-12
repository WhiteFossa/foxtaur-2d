using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;
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

    /// <summary>
    /// Foxes
    /// </summary>
    public DbSet<Fox> Foxes { get; set; }

    /// <summary>
    /// Locations
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Hunters locations
    /// </summary>
    public DbSet<HunterLocation> HuntersLocations { get; set; }

    /// <summary>
    /// Distances
    /// </summary>
    public DbSet<Distance> Distances { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Distances
        modelBuilder
            .Entity<Distance>()
            .HasOne(d => d.StartLocation)
            .WithMany(l => l.AsStartInDistances);

        modelBuilder
            .Entity<Distance>()
            .HasOne(d => d.FinishCorridorEntranceLocation)
            .WithMany(l => l.AsFinishCorridorEntranceInDistances);
        
        modelBuilder
            .Entity<Distance>()
            .HasOne(d => d.FinishLocation)
            .WithMany(l => l.AsFinishLocationInDistances);
        
        modelBuilder
            .Entity<Distance>()
            .HasMany(d => d.FoxesLocations)
            .WithMany(l => l.AsFoxLocationInDistances)
            .UsingEntity(join => join.ToTable("DistancesToFoxesLocations"));
        
        modelBuilder
            .Entity<Distance>()
            .HasMany(d => d.ExpectedFoxesOrderLocations)
            .WithMany(l => l.AsExpectedFoxOrderLocationInDistances)
            .UsingEntity(join => join.ToTable("DistancesToExpectedFoxesOrderLocations"));

        modelBuilder
            .Entity<Distance>()
            .HasMany(d => d.Hunters)
            .WithMany(h => h.ParticipatedInDistances);
    }
}