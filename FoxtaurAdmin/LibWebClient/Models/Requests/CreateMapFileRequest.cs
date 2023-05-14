using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to create map file
/// </summary>
public class CreateMapFileRequest
{
    /// <summary>
    /// User-friendly file name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Size, in bytes
    /// </summary>
    [JsonPropertyName("size")]
    public int Size { get; }

    public CreateMapFileRequest(string name, int size)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be empty.", nameof(name));
        }

        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException("Size must be greater than 0.", nameof(size));
        }

        Name = name;
        Size = size;
    }
}