namespace LibRenderer.Abstractions.Layers;

/// <summary>
/// Abstract layer, which can be rendered
/// </summary>
public interface IRasterLayer : ILayer
{
    /// <summary>
    /// Layer width
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Layer height
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Force pixels array regeneration
    /// </summary>
    void RegeneratePixelsArray();
    
    /// <summary>
    /// Return pixels array (RGBA format)
    /// </summary>
    byte[] GetPixelsArray();

    /// <summary>
    /// Get pixel coordinates
    /// </summary>
    /// <returns>True if given coordinates exist within layer</returns>
    bool GetPixelCoordinates(double lat, double lon, out double x, out double y);

    /// <summary>
    /// If false, then layer is not ready and renderer MUST NOT call any other methods / read properties
    /// </summary>
    bool IsReady();
}