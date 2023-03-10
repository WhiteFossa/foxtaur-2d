using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Dictionary with hunters IDs and hunters locations histories
/// </summary>
public class HuntersLocationsDictionaryDto
{
    /// <summary>
    /// Locations dictionary
    /// </summary>
    [JsonPropertyName("huntersLocations")]
    public Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>> HuntersLocations { get; set; }
}