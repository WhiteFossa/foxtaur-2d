namespace LibResources.Constants;

/// <summary>
/// Constants, related to resources
/// </summary>
public static class ResourcesConstants
{
    /// <summary>
    /// How many active downloading threads can be
    /// </summary>
    public const int MaxActiveDownloadingThreads = 5;
    
    /// <summary>
    /// Base URL for resources
    /// </summary>
    public const string ResourcesBaseUrl = "https://static.foxtaur.me/";
    
    /// <summary>
    /// Put downloaded resources here (relative to executable)
    /// </summary>
    public const string DownloadedDirectory = "Downloaded/";
}