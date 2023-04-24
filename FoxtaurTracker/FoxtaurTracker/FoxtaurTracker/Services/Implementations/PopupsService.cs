using FoxtaurTracker.Services.Abstract;

namespace FoxtaurTracker.Services.Implementations;

public class PopupsService : IPopupsService
{
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        await MainThread.InvokeOnMainThreadAsync(async () => Application.Current.MainPage.DisplayAlert(title, message, cancel));
    }

    public async Task<bool> ShowQuestionAsync(string title, string message, string yesButton = "Yes", string noButton = "No")
    {
        return await MainThread.InvokeOnMainThreadAsync(async () => Application.Current.MainPage.DisplayAlert(title, message, yesButton, noButton)).Result;
    }
}