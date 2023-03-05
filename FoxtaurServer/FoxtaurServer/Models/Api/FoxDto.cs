using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Fox
/// </summary>
public class FoxDto
{
    /// <summary>
    /// Fox ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Fox name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Fox frequency in Hz
    /// </summary>
    [JsonPropertyName("frequency")]
    public double Frequency { get; }

    /// <summary>
    /// Fox code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; }

    public FoxDto(
        Guid id,
        string name,
        double frequency,
        string code)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(nameof(code));
        }

        Id = id;
        Name = name;
        Frequency = frequency;
        Code = code;
    }
}