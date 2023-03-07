using Avalonia.Media;

namespace LibWebClient.Models;

/// <summary>
/// Hunter
/// </summary>
public class Hunter
{
    /// <summary>
    /// Hunter Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// True if hunter is on distance now
    /// </summary>
    public bool IsRunning { get; }

    /// <summary>
    /// Team, may be null if hunter is teamless
    /// </summary>
    public Team Team { get; }

    /// <summary>
    /// Hunter locations history, from old to new
    /// </summary>
    public IReadOnlyCollection<HunterLocation> LocationsHistory { get; }
    
    /// <summary>
    /// Color
    /// </summary>
    public Color Color { get; }

    public Hunter(
        Guid id,
        string name,
        bool isRunning,
        Team team,
        IReadOnlyCollection<HunterLocation> locationsHistory,
        Color color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        IsRunning = isRunning;
        Team = team;
        LocationsHistory = locationsHistory ?? throw new ArgumentNullException(nameof(locationsHistory));
        Color = color;
    }
}