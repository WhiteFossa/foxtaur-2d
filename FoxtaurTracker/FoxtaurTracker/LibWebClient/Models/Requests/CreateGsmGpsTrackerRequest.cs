using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to create GSM-interfaced GPS tracker
/// </summary>
public class CreateGsmGpsTrackerRequest
{
    /// <summary>
    /// Tracker IMEI (we use it as credentials)
    /// </summary>
    [JsonPropertyName("imei")]
    public string Imei { get; }

    /// <summary>
    /// Tracker name (might be non-unique)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    public CreateGsmGpsTrackerRequest
    (
        string imei,
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("IMEI must not be empty!", nameof(imei));
        }
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be empty!", nameof(name));
        }

        Imei = imei;
        Name = name;
    }
}