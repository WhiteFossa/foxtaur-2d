using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Distance
/// </summary>
public class DistanceDto
{
    /// <summary>
    /// Distance ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Associated map
    /// </summary>
    [JsonPropertyName("mapId")]
    public Guid MapId { get; }

    /// <summary>
    /// Is someone running here?
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; }

    /// <summary>
    /// Start location ID
    /// </summary>
    [JsonPropertyName("startLocationId")]
    public Guid StartLocationId { get; }

    /// <summary>
    /// Finish corridor entrance location ID
    /// </summary>
    [JsonPropertyName("finishCorridorEntranceLocationId")]
    public Guid FinishCorridorEntranceLocationId { get; }

    /// <summary>
    /// Finish location ID
    /// </summary>
    [JsonPropertyName("finishLocationId")]
    public Guid FinishLocationId { get; }

    /// <summary>
    /// Foxes on distance
    /// </summary>
    [JsonPropertyName("foxesLocationsIds")]
    public IReadOnlyCollection<Guid> FoxesLocationsIds { get; }

    /// <summary>
    /// Hunters on distance
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; }

    /// <summary>
    /// First hunter start time (client will load hunters histories since this time)
    /// </summary>
    [JsonPropertyName("firstHunterStartTime")]
    public DateTime FirstHunterStartTime { get; }
    
    /// <summary>
    /// Distance close time (client will load hunters histories till this this)
    /// </summary>
    [JsonPropertyName("closeTime")]
    public DateTime CloseTime { get; }

    public DistanceDto(
        Guid id,
        string name,
        Guid mapId,
        bool isActive,
        Guid startLocationId,
        Guid finishCorridorEntranceLocationId,
        Guid finishLocationId,
        IReadOnlyCollection<Guid> foxesLocationsIds,
        IReadOnlyCollection<Guid> huntersIds,
        DateTime firstHunterStartTime,
        DateTime closeTime)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        FoxesLocationsIds = foxesLocationsIds ?? throw new ArgumentNullException(nameof(foxesLocationsIds));
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        if (closeTime <= firstHunterStartTime)
        {
            throw new ArgumentOutOfRangeException(nameof(closeTime), "Close time must be greater than first hunter start time.");
        }

        Id = id;
        Name = name;
        MapId = mapId;
        IsActive = isActive;
        StartLocationId = startLocationId;
        FinishCorridorEntranceLocationId = finishCorridorEntranceLocationId;
        FinishLocationId = finishLocationId;
        FirstHunterStartTime = firstHunterStartTime;
        CloseTime = closeTime;
    }
}