namespace LibAuxiliary.Abstract;

/// <summary>
/// Service to read configuration
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Get configuration string (with check is it set)
    /// </summary>
    string GetConfigurationString(string key);
}