using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

public class HuntersLocationsDictionaryDto
{
    /// <summary>
    /// Locations dictionary
    /// </summary>
    [JsonPropertyName("huntersLocations")]
    public Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>> HuntersLocations { get; set; }

    public HuntersLocationsDictionaryDto(Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>> huntersLocations)
    {
        HuntersLocations = huntersLocations ?? throw new ArgumentNullException(nameof(huntersLocations));
    }
}