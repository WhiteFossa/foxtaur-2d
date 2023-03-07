using Avalonia.Media;

namespace LibWebClient.Models;

/// <summary>
/// Team
/// </summary>
public class Team
{
    /// <summary>
    /// Team Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Color
    /// </summary>
    public Color Color { get; }

    public Team(
        Guid id,
        string name,
        Color color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        Color = color;
    }
}