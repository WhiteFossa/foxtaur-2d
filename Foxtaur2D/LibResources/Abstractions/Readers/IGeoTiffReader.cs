namespace LibResources.Abstractions.Readers;

/// <summary>
/// GeoTIFF reader
/// </summary>
public interface IGeoTiffReader
{
    /// <summary>
    /// Open file
    /// </summary>
    void Open(string path);

    /// <summary>
    /// Open stream
    /// </summary>
    void Open(Stream stream);

    /// <summary>
    /// Call this after opening to load the real raster data
    /// </summary>
    void LoadRasterData();

    /// <summary>
    /// Get pixel by planar coordinates. Result is normalized to [0; 1]
    /// </summary>
    double GetPixel(int band, int x, int y);

    /// <summary>
    /// Get pixel by planar coordinates (with bilinear interpolation). Result is normalized to [0; 1]
    /// </summary>
    double GetPixelWithInterpolation(int band, double x, double y); 

    /// <summary>
    /// Get image width
    /// </summary>
    int GetWidth();

    /// <summary>
    /// Get image height
    /// </summary>
    /// <returns></returns>
    int GetHeight();

    /// <summary>
    /// Get data size, occupied by reader (coarse)
    /// </summary>
    long GetDataSize();

    /// <summary>
    /// Get pixel coordinates by geo coordnates. Pixel coordinates may be wrong is geo coordinates points outside image
    /// </summary>
    void GetPixelCoordsByGeoCoords(double lat, double lon, out double x, out double y);
}