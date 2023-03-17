using LibWebClient.Models;

namespace FoxtaurTracker.Models;

/// <summary>
/// Team item (for dropdown)
/// </summary>
public class TeamItem
{
    /// <summary>
    /// Team
    /// </summary>
    public Team Team { get; }

    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }

    public TeamItem(Team team, int index)
    {
        Team = team ?? throw new ArgumentNullException(nameof(team));
        Index = index;
    }
}