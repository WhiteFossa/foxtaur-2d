using LibAuxiliary.Abstract;
using Microsoft.Extensions.Configuration;

namespace LibAuxiliary.Implementations;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GetConfigurationString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(nameof(key));
        }
        
        var value = _configuration[key];

        if (value == null)
        {
            throw new ArgumentException(nameof(key));
        }

        return value;
    }
}