using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create GSM-interfaced GPS tracker
/// </summary>
public class CreateGsmGpsTrackerRequest
{
    /// <summary>
    /// Tracker IMEI (we use it as credentials)
    /// </summary>
    [JsonPropertyName("imei")]
    public string Imei { get; set; }

    /// <summary>
    /// Tracker name (might be non-unique)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}