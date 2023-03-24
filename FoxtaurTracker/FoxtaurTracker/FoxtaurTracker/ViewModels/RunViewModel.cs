using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using FoxtaurTracker.Services.Abstract.Models;
using LibAuxiliary.Helpers;
using LibWebClient.Models;

namespace FoxtaurTracker.ViewModels;

public class RunViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly ILocationsProcessingService _locationsProcessingService;

    private User _userModel;

    #region Commands

    /// <summary>
    /// Start tracking
    /// </summary>
    public ICommand StartTrackingCommand { get; private set; }

    /// <summary>
    /// Stop tracking
    /// </summary>
    public ICommand StopTrackingCommand { get; private set; }

    /// <summary>
    /// Exit this page
    /// </summary>
    public ICommand ExitCommand { get; private set; }
    
    #endregion
    
    public event PropertyChangedEventHandler PropertyChanged;

    private string _lastGpsFixString;

    public string LastGpxFixString
    {
        get
        {
            return _lastGpsFixString;
        }

        set
        {
            _lastGpsFixString = value;
            RaisePropertyChanged(nameof(LastGpxFixString));
        }
    }

    private string _lastDataSubmissionString;

    public string LastDataSubmissionString
    {
        get
        {
            return _lastDataSubmissionString;
        }

        set
        {
            _lastDataSubmissionString = value;
            RaisePropertyChanged(nameof(LastDataSubmissionString));
        }
    }

    private int _positionsSent;

    public int PositionsSent
    {
        get
        {
            return _positionsSent;
        }

        set
        {
            _positionsSent = value;
            RaisePropertyChanged(nameof(PositionsSent));
        }
    }

    private int _positionsToSend;

    public int PositionsToSend
    {
        get
        {
            return _positionsToSend;
        }

        set
        {
            _positionsToSend = value;
            RaisePropertyChanged(nameof(PositionsToSend));
        }
    }

    public RunViewModel()
    {
        _locationsProcessingService = App.ServicesProvider.GetService<ILocationsProcessingService>();
        
        #region Commands binding
        
        StartTrackingCommand = new Command(async () => await StartTrackingAsync(),
            () =>
            {
                return !_locationsProcessingService.IsTrackingOn();
            });
        
        
        StopTrackingCommand = new Command(async () => await StopTrackingAsync(),
            () =>
            {
                return _locationsProcessingService.IsTrackingOn();
            });
        
        
        ExitCommand = new Command(async () => await ExitAsync(),
            () =>
            {
                return !_locationsProcessingService.IsTrackingOn();
            });
        
        #endregion
    }
    
    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private async Task StartTrackingAsync()
    {
        await _locationsProcessingService.StartTrackingAsync(OnStatisticsUpdate);
        RefreshCanExecutes();
    }

    private async Task StopTrackingAsync()
    {
        await _locationsProcessingService.StopTrackingAsync();
        RefreshCanExecutes();
    }

    private async Task ExitAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "IsFromRegistrationPage", false },
            { "UserModel", _userModel }
        };

        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("mainPage", navigationParameter));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _userModel = (User)query["UserModel"];
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        LastGpxFixString = "N/A";
        LastDataSubmissionString = "N/A";
        PositionsSent = 0;
        PositionsToSend = 0;
    }
    
    private void RefreshCanExecutes()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            (StartTrackingCommand as Command).ChangeCanExecute();
            (StopTrackingCommand as Command).ChangeCanExecute();
            (ExitCommand as Command).ChangeCanExecute();
        });
    }

    private void OnStatisticsUpdate(LocationsServiceStatistics statistics)
    {
        LastGpxFixString = statistics.LastGpsFix.AsTimeAgo();
        LastDataSubmissionString = statistics.LastDataSendTime.AsTimeAgo();
        PositionsSent = statistics.LocationsSent;
        PositionsToSend = statistics.LocationsToSend;
    }
}