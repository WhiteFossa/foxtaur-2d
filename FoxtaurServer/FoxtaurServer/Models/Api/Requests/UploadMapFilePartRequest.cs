using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to upload a part of mapfile
/// </summary>
public class UploadMapFilePartRequest
{
    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// This part starts from this position in file
    /// </summary>
    [JsonPropertyName("startPosition")]
    public int StartPosition { get; set; }

    /// <summary>
    /// Data to upload (base64 encoded)
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; }
}