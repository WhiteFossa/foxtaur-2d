using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create new fox
/// </summary>
public class CreateFoxRequest
{
    /// <summary>
    /// Fox name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Fox frequency in Hz
    /// </summary>
    [JsonPropertyName("frequency")]
    public double Frequency { get; set; }

    /// <summary>
    /// Fox code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; }
}