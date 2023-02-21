using LibGeo.Abstractions;

namespace LibRenderer.Abstractions;

/// <summary>
/// Abstract layer, which can be rendered
/// </summary>
public interface ILayer
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
    /// Layer's geo provider
    /// </summary>
    IGeoProvider GeoProvider { get; }

    /// <summary>
    /// Force pixels array regeneration
    /// </summary>
    void RegeneratePixelsArray();
    
    /// <summary>
    /// Return pixels array (RGBA format)
    /// </summary>
    byte[] GetPixelsArray();

    /// <summary>
    /// Is pixel with given coordinates exist in layer
    /// </summary>
    bool IsPixelExist(double lat, double lon);
}