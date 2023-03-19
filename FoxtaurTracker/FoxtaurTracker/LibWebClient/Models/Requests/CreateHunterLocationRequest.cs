using System.Text.Json.Serialization;
using LibWebClient.Models.DTOs;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to create hunter locations
/// </summary>
public class CreateHunterLocationsRequest
{
    /// <summary>
    /// Hunter locations
    /// </summary>
    [JsonPropertyName("hunterLocations")]
    public IReadOnlyCollection<HunterLocationDto> HunterLocations { get; }

    public CreateHunterLocationsRequest(IReadOnlyCollection<HunterLocationDto> hunterLocations)
    {
        HunterLocations = hunterLocations ?? throw new ArgumentNullException(nameof(hunterLocations));
    }
}