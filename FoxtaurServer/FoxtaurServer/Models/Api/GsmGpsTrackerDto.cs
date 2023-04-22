using System.Text.Json.Serialization;
using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// GSM-interfaced GPS tracker DTO
/// </summary>
public class GsmGpsTrackerDto
{
    /// <summary>
    /// Tracker ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Tracker IMEI (we use as credentials)
    /// </summary>
    [JsonPropertyName("imei")]
    public string Imei { get; }
    
    /// <summary>
    /// Tracker name (may be non-unique)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// For now tracker is being used by this user
    /// </summary>
    [JsonPropertyName("usedBy")]
    public Guid? UsedBy { get; }

    public GsmGpsTrackerDto
    (
        Guid id,
        string imei,
        string name,
        Guid? usedBy
    )
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("IMEI must be not empty.", nameof(imei));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be not empty.", nameof(name));
        }
            
        Id = id;
        Imei = imei;
        Name = name;
        UsedBy = usedBy;
    }
}