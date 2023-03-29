using System.Net.Http.Headers;
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
    public abstract void Download(OnResourceLoadedDelegate onLoad, OnDownloadProgressDelegate onDownloadProgress = null);

    /// <summary>
    /// Load resource as a stream from URL
    /// </summary>
    protected MemoryStream LoadFromUrl(string url, OnDownloadProgressDelegate onDownloadProgress = null)
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
            if (onDownloadProgress != null)
            {
                onDownloadProgress(0.0);
            }

            _downloadMutex.WaitOne();

            try
            {
                // Downloading piece-by-piece
                var headersResponse = GetHeaders(uriResult);
                var downloadSize = headersResponse
                    .Content
                    .Headers
                    .ContentLength
                    .Value;

                long downloaded = 0;
                var resultStream = new MemoryStream();
                
                while (downloaded < downloadSize)
                {
                    var currentChunkSize = downloadSize - downloaded;
                    if (currentChunkSize > ResourcesConstants.DownloadChunkSize)
                    {
                        currentChunkSize = ResourcesConstants.DownloadChunkSize;
                    }

                    var downloadedChunk = DownloadWithRange(uriResult, downloaded, downloaded + currentChunkSize);
                    using (var downloadedChunkStream = downloadedChunk.Content.ReadAsStream())
                    {
                        using (var downloadedChunkMemoryStream = new MemoryStream())
                        {
                            downloadedChunkStream.CopyTo(downloadedChunkMemoryStream);
                            downloadedChunkMemoryStream.WriteTo(resultStream);
                        }
                    }

                    downloaded += downloadedChunk
                        .Content
                        .Headers
                        .ContentLength
                        .Value;
                    
                    if (onDownloadProgress != null)
                    {
                        onDownloadProgress(downloaded / (double)downloadSize);
                    }
                }

                return resultStream;
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

    /// <summary>
    /// Download content from given URI from given range
    /// </summary>
    private HttpResponseMessage DownloadWithRange(Uri uri, long start, long end)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));

        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), "Start must be non-negative.");
        }

        if (end <= start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), "End must be greater than start.");
        }
        
        var httpClient = new HttpClient();
        httpClient.Timeout = new TimeSpan(0, 0, ResourcesConstants.HttpClientTimeout);
        using var webRequest = new HttpRequestMessage(HttpMethod.Get, uri);
        webRequest.Headers.Range = new RangeHeaderValue(start, end);

        return httpClient.Send(webRequest);
    }

    private HttpResponseMessage GetHeaders(Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));
        
        var httpClient = new HttpClient();
        using var webRequest = new HttpRequestMessage(HttpMethod.Head, uri);
        
        return httpClient.Send(webRequest);
    }
    
    protected void LoadFromUrlToFile(string url, OnDownloadProgressDelegate onDownloadProgress = null)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException(nameof(url));
        }

        using (var downloadStream = LoadFromUrl(url, onDownloadProgress))
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