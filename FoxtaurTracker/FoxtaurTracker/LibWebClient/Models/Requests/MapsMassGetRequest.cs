using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for mass getting the maps
/// </summary>
public class MapsMassGetRequest
{
    /// <summary>
    /// List of maps IDs to get maps
    /// </summary>
    [JsonPropertyName("mapsIds")]
    public IReadOnlyCollection<Guid> MapsIds { get; }

    public MapsMassGetRequest(IReadOnlyCollection<Guid> mapsIds)
    {
        MapsIds = mapsIds ?? throw new ArgumentNullException(nameof(mapsIds));
    }
}