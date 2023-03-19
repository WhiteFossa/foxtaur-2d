namespace LibWebClient.Models;

/// <summary>
/// Distance
/// </summary>
public class Distance
{
    /// <summary>
    /// Distance ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Associated map
    /// </summary>
    public Map Map { get; }

    /// <summary>
    /// Is someone running here?
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Start location
    /// </summary>
    public Location StartLocation { get; }

    /// <summary>
    /// Finish corridor entrance location
    /// </summary>
    public Location FinishCorridorEntranceLocation { get; }

    /// <summary>
    /// Finish location
    /// </summary>
    public Location FinishLocation { get; }

    /// <summary>
    /// Foxes on distance
    /// </summary>
    public IReadOnlyCollection<Location> Foxes { get; }

    /// <summary>
    /// Hunters on distance
    /// </summary>
    public IReadOnlyCollection<Hunter> Hunters { get; }
    
    /// <summary>
    /// First hunter start time (we will load hunters histories since this time)
    /// </summary>
    public DateTime FirstHunterStartTime { get; }

    public Distance(
        Guid id,
        string name,
        Map map,
        bool isActive,
        Location startLocation,
        Location finishCorridorEntranceLocation,
        Location finishLocation,
        IReadOnlyCollection<Location> foxes,
        IReadOnlyCollection<Hunter> hunters,DateTime firstHunterStartTime)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        Map = map ?? throw new ArgumentNullException(nameof(map));
        IsActive = isActive;
        StartLocation = startLocation ?? throw new ArgumentNullException(nameof(startLocation));
        FinishCorridorEntranceLocation = finishCorridorEntranceLocation ?? throw new ArgumentNullException(nameof(finishCorridorEntranceLocation));
        FinishLocation = finishLocation ?? throw new ArgumentNullException(nameof(finishLocation));
        Foxes = foxes ?? throw new ArgumentNullException(nameof(foxes));
        Hunters = hunters ?? throw new ArgumentNullException(nameof(hunters));
        FirstHunterStartTime = firstHunterStartTime;
    }
}