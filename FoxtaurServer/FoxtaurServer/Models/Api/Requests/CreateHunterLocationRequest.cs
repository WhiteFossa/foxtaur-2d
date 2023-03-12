using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create hunter location
/// </summary>
public class CreateHunterLocationRequest
{
    /// <summary>
    /// Hunter locations
    /// </summary>
    [JsonPropertyName("hunterLocations")]
    public IReadOnlyCollection<HunterLocationDto> HunterLocations { get; set; }
}