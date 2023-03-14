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
    /// Put downloaded resources here (relative to executable)
    /// </summary>
    public const string DownloadedDirectory = "Downloaded/";

    /// <summary>
    /// HTTP client timeout in seconds
    /// </summary>
    public const int HttpClientTimeout = 3600;
}