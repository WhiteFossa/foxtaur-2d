using System.IO.Compression;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using ZstdSharp;

namespace LibAuxiliary.Implementations;

public class CompressionService : ICompressionService
{
    public void Compress(Stream streamToCompress, Stream outputStream)
    {
        using (var compressionStream = new CompressionStream(outputStream, CompressionConstants.CompressionLevel))
        {
            streamToCompress.CopyTo(compressionStream);
        }
    }

    public Stream Decompress(Stream compressedStream)
    {
        return new DecompressionStream(compressedStream);
    }
}