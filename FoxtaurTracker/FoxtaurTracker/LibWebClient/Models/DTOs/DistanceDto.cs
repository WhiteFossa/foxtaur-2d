using System.Text.Json.Serialization;
using LibWebClient.Models.Abstract;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Distance
/// </summary>
public class DistanceDto : IIdedDto
{
    /// <summary>
    /// Distance ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Associated map
    /// </summary>
    [JsonPropertyName("mapId")]
    public Guid MapId { get; set; }

    /// <summary>
    /// Is someone running here?
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Start location ID
    /// </summary>
    [JsonPropertyName("startLocationId")]
    public Guid StartLocationId { get; set; }

    /// <summary>
    /// Finish corridor entrance location ID
    /// </summary>
    [JsonPropertyName("finishCorridorEntranceLocationId")]
    public Guid FinishCorridorEntranceLocationId { get; set; }

    /// <summary>
    /// Finish location ID
    /// </summary>
    [JsonPropertyName("finishLocationId")]
    public Guid FinishLocationId { get; set; }

    /// <summary>
    /// Foxes on distance
    /// </summary>
    [JsonPropertyName("foxesLocationsIds")]
    public IReadOnlyCollection<Guid> FoxesLocationsIds { get; set; }

    /// <summary>
    /// Hunters on distance
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; set; }

    /// <summary>
    /// First hunter start time (client will load hunters histories since this time)
    /// </summary>
    [JsonPropertyName("firstHunterStartTime")]
    public DateTime FirstHunterStartTime { get; set; }
}