namespace FoxtaurTracker.Services.Abstract.Enums;

/// <summary>
/// Possible statuses for location getting
/// </summary>
public enum GetLocationStatus
{
    Success,

    NotSupported,
    
    NotEnabled,
    
    NotPermitted,
    
    TimedOut
}