using FoxtaurTracker.Services.Abstract;

namespace FoxtaurTracker.Services.Implementations;

public class PopupsService : IPopupsService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }
}