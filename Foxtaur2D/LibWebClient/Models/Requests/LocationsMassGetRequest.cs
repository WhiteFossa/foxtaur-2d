using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for mass getting locations
/// </summary>
public class LocationsMassGetRequest
{
    /// <summary>
    /// List of locations IDs to get locations
    /// </summary>
    [JsonPropertyName("locationsIds")]
    public IReadOnlyCollection<Guid> LocationsIds { get; }

    public LocationsMassGetRequest(IReadOnlyCollection<Guid> locationsIds)
    {
        LocationsIds = locationsIds ?? throw new ArgumentNullException(nameof(locationsIds));
    }
}