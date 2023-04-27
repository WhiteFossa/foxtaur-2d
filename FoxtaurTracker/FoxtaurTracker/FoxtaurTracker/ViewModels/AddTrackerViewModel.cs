using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class AddTrackerViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;
    private readonly IPopupsService _popupsService;
    
    private string _name;
    private string _imei;
    
    /// <summary>
    /// Tracker name
    /// </summary>
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            RaisePropertyChanged(nameof(Name));
            RefreshCanExecutes();
        }
    }
    
    /// <summary>
    /// Tracker IMEI
    /// </summary>
    public string Imei
    {
        get
        {
            return _imei;
        }
        set
        {
            _imei = value;
            RaisePropertyChanged(nameof(Imei));
            RefreshCanExecutes();
        }
    }
    
    #region Commands

    /// <summary>
    /// Log in
    /// </summary>
    public ICommand AddCommand { get; private set; }

    #endregion
    
    public event PropertyChangedEventHandler PropertyChanged;

    public AddTrackerViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        _popupsService = App.ServicesProvider.GetService<IPopupsService>();
        
        #region Commands binding

        AddCommand = new Command(async () => await AddTrackerAsync(),
            () =>
            {
                return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Imei);
            });

        #endregion
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }
    
    private async Task AddTrackerAsync()
    {
        try
        {
            await _webClient.CreateGsmGpsTrackerAsync(new CreateGsmGpsTrackerRequest(Imei, Name));
        }
        catch (Exception)
        {
            await _popupsService.ShowAlertAsync("Failure", "Failed to create new tracker!");
            return;
        }
        
        await _popupsService.ShowAlertAsync("Success", "New tracker successfully created!");
        
        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(".."));
    }
    
    private void RefreshCanExecutes()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            (AddCommand as Command).ChangeCanExecute();
        });
    }
}