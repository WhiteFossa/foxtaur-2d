using LibResources.Constants;
using NLog;
using ZstdNet;

namespace LibResources.Implementations.Resources;

/// <summary>
/// Delegate for OnLoaded() event
/// </summary>
public delegate void OnResourceLoadedDelegate(DownloadableResourceBase resourceBase);

/// <summary>
/// Delegate for download progress. Progress value is [0; 1]
/// </summary>
public delegate void OnDownloadProgressDelegate(double progress);

/// <summary>
/// Resource, downloadable from network
/// </summary>
public abstract class DownloadableResourceBase
{
    /// <summary>
    /// Unique resource name
    /// </summary>
    public string ResourceName { get; private set; }

    /// <summary>
    /// If true, then RemotePath points to a local file, so no need to download anything
    /// </summary>
    public bool IsLocal { get; private set; }

    /// <summary>
    /// Call this when resource load is completed
    /// </summary>
    public OnResourceLoadedDelegate OnLoad;

    /// <summary>
    /// Semaphore for limit the number of active downloading threads
    /// </summary>
    protected static Semaphore DownloadThreadsLimiter = new Semaphore(ResourcesConstants.MaxActiveDownloadingThreads, ResourcesConstants.MaxActiveDownloadingThreads); 
    
    private Logger _logger = LogManager.GetCurrentClassLogger();

    private static Mutex _downloadMutex = new Mutex();

    public DownloadableResourceBase(string resourceName,
        bool isLocal)
    {
        if (string.IsNullOrEmpty(resourceName))
        {
            throw new ArgumentException(nameof(resourceName));
        }
        
        ResourceName = resourceName;
        IsLocal = isLocal;
    }

    /// <summary>
    /// After download resource can be found here
    /// </summary>
    public virtual string GetLocalPath()
    {
        if (IsLocal)
        {
            return ResourceName; // For local resources local path == remote path
        }

        return GetResourceLocalPath(ResourceName);
    }
    
    /// <summary>
    /// Download resource
    /// </summary>
    public abstract void Download(OnResourceLoadedDelegate onLoad);

    /// <summary>
    /// Load resource as a stream from URL
    /// </summary>
    protected MemoryStream LoadFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException(nameof(url));
        }
        
        Uri uriResult;
        if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult))
        {
            throw new ArgumentException(nameof(url));
        }

        try
        {
            _logger.Info($"Waiting for download from {uriResult}");

            DownloadThreadsLimiter.WaitOne();

            _logger.Info($"Downloading from {uriResult}");

            _downloadMutex.WaitOne();

            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, ResourcesConstants.HttpClientTimeout);
                using (var webRequest = new HttpRequestMessage(HttpMethod.Get, uriResult))
                {
                    using (var downloadStream = httpClient.Send(webRequest).Content.ReadAsStream())
                    {
                        var resultStream = new MemoryStream();
                        downloadStream.CopyTo(resultStream);

                        return resultStream;                        
                    }
                }
            }
            finally
            {
                _downloadMutex.ReleaseMutex();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw;
        }
        finally
        {
            DownloadThreadsLimiter.Release();
        }
    }

    protected void LoadFromUrlToFile(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException(nameof(url));
        }

        using (var downloadStream = LoadFromUrl(url))
        {
            var localPath = GetResourceLocalPath(url);
            
            _logger.Info($"Saving { url } to { localPath }");
            SaveStreamAsFile(downloadStream, localPath);
        }
    }

    /// <summary>
    /// Return path, where downloaded resource have to be stored
    /// </summary>
    protected string GetResourceLocalPath(string relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
        {
            throw new ArgumentException(nameof(relativeUrl));
        }
        
        return (ResourcesConstants.DownloadedDirectory + relativeUrl)
            .Replace(@"://", @"_")
            .Replace(@":", "_");
    }

    /// <summary>
    /// Save stream as a file
    /// </summary>
    protected void SaveStreamAsFile(MemoryStream stream, string path)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException(nameof(path));
        }
        
        // Do target directory exist?
        var targetDirectory = Path.GetDirectoryName(path);
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        
        using (var fileStream = File.Create(path))
        {
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
        }
    }
    
    /// <summary>
    /// Load ZSTD file to stream
    /// </summary>
    protected Stream LoadZstdFile(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException(nameof(path));
        }

        return new DecompressionStream(File.OpenRead(path));
    }
}