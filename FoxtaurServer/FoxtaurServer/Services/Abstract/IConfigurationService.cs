namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Configuration service
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Get configuration string (with check is it set)
    /// </summary>
    Task<string> GetConfigurationString(string key);
}