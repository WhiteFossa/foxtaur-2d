namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Configuration service
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Get configuration string (with check if it set)
    /// </summary>
    Task<string> GetConfigurationString(string key);
}