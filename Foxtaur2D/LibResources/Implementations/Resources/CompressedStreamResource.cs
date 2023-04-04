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
        try
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
                    else
                    {
                        // File may exist, but be outdated
                        var localETagPath = GenerateEtagPath(localPath);
                        string localETag;

                        if (File.Exists(localETagPath))
                        {
                            localETag = File.ReadAllText(localETagPath);
                        }
                        else
                        {
                            localETag = string.Empty;
                        }
                        
                        Uri remoteFileUri;
                        if (!Uri.TryCreate(ResourceName, UriKind.Absolute, out remoteFileUri))
                        {
                            throw new ArgumentException(nameof(ResourceName));
                        }
                        
                        var remoteETag = _webClient.GetHeadersAsync(remoteFileUri)
                            .Result
                            .Headers
                            .ETag
                            .Tag;

                        if (!localETag.Equals(remoteETag))
                        {
                            _logger.Info($"{ ResourceName } is outdated, reloading it...");
                            
                            // Local file is outdated
                            File.Delete(localETagPath);
                            File.Delete(localPath);
                            
                            LoadFromUrlToFile(ResourceName, onDownloadProgress);
                        }
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
        catch (ThreadInterruptedException)
        {
            // Download was aborted
            _logger.Error($"Compressed resource { ResourceName } download was aborted.");
        }
    }
    
    public override void Download(OnResourceLoadedDelegate onLoad, OnDownloadProgressDelegate onDownloadProgress = null)
    {
        throw new NotImplementedException("Call overloaded version.");
    }
}