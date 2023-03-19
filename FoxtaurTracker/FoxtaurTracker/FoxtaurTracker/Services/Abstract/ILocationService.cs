using FoxtaurTracker.Services.Abstract.Models;

namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Delegate for OnLocationObtained() event
/// </summary>
public delegate Task OnLocationObtainedAsync(GetLocationArgs args);

/// <summary>
/// Service for getting device location
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Get current location
    /// </summary>
    Task GetCurrentLocationAsync(OnLocationObtainedAsync callback);
}