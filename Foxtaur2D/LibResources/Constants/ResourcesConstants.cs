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
    /// Download files using this download chunk size
    /// </summary>
    public const int DownloadChunkSize = 1000000;
}