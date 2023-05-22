namespace FoxtaurServer.Models;

/// <summary>
/// File to download
/// </summary>
public class FileToDownload
{
    /// <summary>
    /// Is this file successfully found and ready to be downloaded
    /// </summary>
    public bool IsFound { get; private set; }

    /// <summary>
    /// File content
    /// </summary>
    public byte[] Content { get; private set; }
    
    /// <summary>
    /// Last modified date
    /// </summary>
    public DateTime LastModified { get; private set; }

    /// <summary>
    /// SHA-512 hash of file, to use as ETag
    /// </summary>
    public string Hash { get; private set; }

    public FileToDownload(bool isFound, byte[] content, DateTime lastModified, string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException(nameof(hash));
        }

        IsFound = isFound;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        LastModified = lastModified;
        Hash = hash;
    }
}