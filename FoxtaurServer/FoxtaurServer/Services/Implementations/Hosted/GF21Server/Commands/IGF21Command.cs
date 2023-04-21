using FoxtaurServer.Models.Trackers;

namespace FoxtaurServer.Services.Implementations.Hosted.Commands;

/// <summary>
/// Command, what will be executed by GF-21
/// </summary>
public interface IGF21Command
{
    /// <summary>
    /// Send command to tracker
    /// </summary>
    Task<string> SendCommandAsync(TrackerContext context);
}