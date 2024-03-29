using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

public class CreateDistanceRequest
{
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
    /// First hunter start time (client will load hunters histories since this time)
    /// </summary>
    [JsonPropertyName("firstHunterStartTime")]
    public DateTime FirstHunterStartTime { get; set; }
    
    /// <summary>
    /// Distance close time (client will load hunters histories till this this)
    /// </summary>
    [JsonPropertyName("closeTime")]
    public DateTime CloseTime { get; set; }
}