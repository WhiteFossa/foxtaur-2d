using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create GSM-interfaced GPS tracker
/// </summary>
public class CreateGsmGpsTrackerRequest
{
    /// <summary>
    /// Tracker IMEI (we use as credentials)
    /// </summary>
    [JsonPropertyName("imei")]
    public string Imei { get; set; }
}