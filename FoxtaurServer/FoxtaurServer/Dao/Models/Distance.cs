using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

public class Distance
{
    /// <summary>
    /// Distance ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Associated map
    /// </summary>
    public Map Map { get; set; }

    /// <summary>
    /// Is someone running here?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Start location
    /// </summary>
    public Location StartLocation { get; set; }

    /// <summary>
    /// Finish corridor entrance location
    /// </summary>
    public Location FinishCorridorEntranceLocation { get; set; }

    /// <summary>
    /// Finish location
    /// </summary>
    public Location FinishLocation { get; set; }

    /// <summary>
    /// Foxes on distance
    /// </summary>
    public IList<Location> FoxesLocations { get; set; }

    /// <summary>
    /// Expected foxes taking order
    /// </summary>
    public IList<Location> ExpectedFoxesOrderLocations { get; set; }
    
    /// <summary>
    /// Hunters on distance
    /// </summary>
    public IList<Profile> Hunters { get; set; }

    /// <summary>
    /// First hunter start time (client will load hunters histories since this time)
    /// </summary>
    public DateTime FirstHunterStartTime { get; set; }
}