using System.Text.Json;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibAuxiliary.Models;

namespace LibAuxiliary.Implementations;

public class SettingsService : ISettingsService
{
    private string _configFilePath;

    private double _huntersLocationsRefreshInterval;

    public SettingsService()
    {
        _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SettingsConstants.ConfigFileName);
        
        LoadSettings();
    }
    
    public void SetHuntersLocationsRefreshInterval(double interval)
    {
        _huntersLocationsRefreshInterval = interval;
        SaveSettings();
    }

    public double GetHuntersLocationsRefreshInterval()
    {
        return _huntersLocationsRefreshInterval;
    }

    private void LoadSettings()
    {
        if (!File.Exists(_configFilePath))
        {
            // First run, we need to create config file
            SetHuntersLocationsRefreshInterval(SettingsConstants.DefaultHuntersLocationsRefreshInterval);

            SaveSettings();
        }
        
        var settings = JsonSerializer.Deserialize<SerializableSettings>(File.ReadAllText(_configFilePath));

        _huntersLocationsRefreshInterval = settings.HuntersLocationsRefreshInterval;
    }
    
    private void SaveSettings()
    {
        var settings = new SerializableSettings()
        {
            HuntersLocationsRefreshInterval = GetHuntersLocationsRefreshInterval()
        };
        
        var serializedSettings = JsonSerializer.Serialize(settings);
        
        File.WriteAllText(_configFilePath, serializedSettings);
    }
}