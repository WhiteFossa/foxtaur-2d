namespace LibRenderer.Abstractions.Layers;

/// <summary>
/// Interface for all layers
/// </summary>
public interface ILayer
{
    /// <summary>
    /// Layer order. Bigger number -> on top
    /// It's good to use sparse numbers, like 1000, 2000, 3000 and so on
    /// </summary>
    int Order { get; }
}