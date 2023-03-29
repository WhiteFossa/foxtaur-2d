namespace LibRenderer.Enums;

/// <summary>
/// Possible states for map
/// </summary>
public enum MapState
{
    NotRequested,
    
    Downloading,
    
    Decompressing,
    
    Processing,
    
    Ready
}