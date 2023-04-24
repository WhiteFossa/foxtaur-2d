using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// GSM-interfaced GPS tracker DTO
/// </summary>
public class GsmGpsTrackerDto
{
    /// <summary>
    /// Tracker ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Tracker IMEI (we use as credentials)
    /// </summary>
    [JsonPropertyName("imei")]
    public string Imei { get; set; }
    
    /// <summary>
    /// Tracker name (may be non-unique)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// For now tracker is being used by this user
    /// </summary>
    [JsonPropertyName("usedBy")]
    public Guid? UsedBy { get; set; }
}