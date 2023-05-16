namespace LibAuxiliary.Abstract;

/// <summary>
/// Service to compress / decompress streams using zstd
/// </summary>
public interface ICompressionService
{
    /// <summary>
    /// Compress
    /// </summary>
    void Compress(Stream streamToCompress, Stream outputStream);

    /// <summary>
    /// Decompress
    /// </summary>
    Stream Decompress(Stream compressedStream);
}