using LibWebClient.Services.Abstract;
using NLog;

namespace LibResources.Implementations.Resources;

/// <summary>
/// Delegate for decompression progress. Progress value is [0; 1]
/// </summary>
public delegate void OnDecompressionProgressDelegate(double progress);

public class CompressedStreamResource : DownloadableResourceBase
{
    private Logger _logger = LogManager.GetCurrentClassLogger();

    private bool _isLoading;
    private readonly Mutex _downloadLock = new Mutex();

    /// <summary>
    /// Decompressed stream - take data from here
    /// </summary>
    public Stream DecompressedStream = null;
    
    public CompressedStreamResource(string resourceName, bool isLocal, IWebClient webClient) : base(resourceName, isLocal, webClient)
    {
    }

    public void Download(OnResourceLoadedDelegate onLoad, OnDownloadProgressDelegate onDownloadProgress = null, OnDecompressionProgressDelegate onDecompressionProgress = null)
    {
        _downloadLock.WaitOne();

        try
        {
            OnLoad = onLoad ?? throw new ArgumentNullException(nameof(onLoad));
            
            if (_isLoading)
            {
                // Loading in progress
                return;
            }

            _isLoading = true;
        }
        finally
        {
            _downloadLock.ReleaseMutex();            
        }

        try
        {
            _logger.Info($"Loading compressed resource { ResourceName }...");
            
            if (!IsLocal)
            {
                // Do we have already downloaded file?
                var localPath = GetResourceLocalPath(ResourceName);
                if (!File.Exists(localPath))
                {
                    LoadFromUrlToFile(ResourceName, onDownloadProgress);    
                }
            }

            // Decompressing
            _logger.Info($"Decompressing resource { ResourceName }...");

            if (onDecompressionProgress != null)
            {
                onDecompressionProgress(0.0);
            }
            
            DecompressedStream = LoadZstdFile(GetLocalPath());
            
            // Done
            _logger.Info($"Decompression done: { ResourceName }...");
            
            if (onDecompressionProgress != null)
            {
                onDecompressionProgress(1.0);
            }

            OnLoad(this);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
            throw;
        }
    }
    
    public override void Download(OnResourceLoadedDelegate onLoad, OnDownloadProgressDelegate onDownloadProgress = null)
    {
        throw new NotImplementedException("Call overloaded version.");
    }
}