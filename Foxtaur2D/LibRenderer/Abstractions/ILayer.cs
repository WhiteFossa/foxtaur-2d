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
    /// Return pixels array (RGBA format)
    /// </summary>
    byte[] GetPixelsArray();
}