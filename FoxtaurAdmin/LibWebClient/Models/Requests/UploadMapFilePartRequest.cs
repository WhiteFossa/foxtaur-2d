using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to upload a part of mapfile
/// </summary>
public class UploadMapFilePartRequest
{
    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// This part starts from this position in file
    /// </summary>
    [JsonPropertyName("startPosition")]
    public int StartPosition { get; }

    /// <summary>
    /// Data to upload (base64 encoded)
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; }

    public UploadMapFilePartRequest
    (
        Guid id,
        int startPosition,
        string data
    )
    {
        if (startPosition < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startPosition), "Start position mustn't be negative.");
        }

        if (string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentException("Data mustn't be empty.", nameof(data));
        }

        Id = id;
        StartPosition = startPosition;
        Data = data;
    }
}