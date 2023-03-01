namespace LibWebClient.Models.DTOs;

/// <summary>
/// Distance
/// </summary>
public class DistanceDto
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
    public Guid MapId { get; }

    /// <summary>
    /// Is someone running here?
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Start location ID
    /// </summary>
    public Guid StartLocationId { get; }

    /// <summary>
    /// Finish corridor entrance location ID
    /// </summary>
    public Guid FinishCorridorEntranceLocationId { get; }

    /// <summary>
    /// Finish location ID
    /// </summary>
    public Guid FinishLocationId { get; }

    /// <summary>
    /// Foxes on distance
    /// </summary>
    public IReadOnlyCollection<Guid> FoxesLocationsIds { get; }

    /// <summary>
    /// Expected foxes taking order (points to locations)
    /// </summary>
    public IReadOnlyCollection<Guid> ExpectedFoxesOrderLocationsIds { get; }
    
    /// <summary>
    /// Hunters on distance
    /// </summary>
    public IReadOnlyCollection<Guid> HuntersIds { get; }

    public DistanceDto(
        Guid id,
        string name,
        Guid mapId,
        bool isActive,
        Guid startLocationId,
        Guid finishCorridorEntranceLocationId,
        Guid finishLocationId,
        IReadOnlyCollection<Guid> foxesLocationsIds,
        IReadOnlyCollection<Guid> expectedFoxesOrderLocationsIds,
        IReadOnlyCollection<Guid> huntersIds)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        FoxesLocationsIds = foxesLocationsIds ?? throw new ArgumentNullException(nameof(foxesLocationsIds));
        ExpectedFoxesOrderLocationsIds = expectedFoxesOrderLocationsIds ?? throw new ArgumentNullException(nameof(expectedFoxesOrderLocationsIds));
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        Id = id;
        Name = name;
        MapId = mapId;
        IsActive = isActive;
        StartLocationId = startLocationId;
        FinishCorridorEntranceLocationId = finishCorridorEntranceLocationId;
        FinishLocationId = finishLocationId;
    }
}