using LibWebClient.Models;

namespace Foxtaur2D.Models;

/// <summary>
/// Main model. Common data are stored here
/// </summary>
public class MainModel
{
    /// <summary>
    /// Selected distance
    /// </summary>
    public Distance Distance { get; set; }

    /// <summary>
    /// Displayed hunter (for single hunter mode)
    /// </summary>
    public Hunter DisplayedHunter { get; set; }

    /// <summary>
    /// Displayed team (for single team mode)
    /// </summary>
    public Team DisplayedTeam { get; set; }
}