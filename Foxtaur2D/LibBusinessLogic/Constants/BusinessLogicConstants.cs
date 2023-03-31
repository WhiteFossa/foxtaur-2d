using Avalonia.Media;

namespace LibBusinessLogic.Constants;

/// <summary>
/// Various constants, used in business logic
/// </summary>
public static class BusinessLogicConstants
{
    /// <summary>
    /// Team ID for hunters who have no team
    /// </summary>
    public static readonly Guid TeamlessTeamId = new Guid("fdbc8f37-a03b-469e-bdb7-acc247e9e3ed");
    
    /// <summary>
    /// Team name for hunters who have no team
    /// </summary>
    public static string TeamlessTeamName = "--- No team ---";

    /// <summary>
    /// Team color for hunters who have no team
    /// </summary>
    public static Color TeamlessTeamColor = Colors.White;
}