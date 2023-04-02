using System.Text.Json.Serialization;

namespace LibAuxiliary.Models;

/// <summary>
/// Application settings, which can be serialized and stored as JSON
/// </summary>
public class SerializableSettings
{
    [JsonPropertyName("HuntersLocationsRefreshInterval")]
    public double HuntersLocationsRefreshInterval { get; set; }
}