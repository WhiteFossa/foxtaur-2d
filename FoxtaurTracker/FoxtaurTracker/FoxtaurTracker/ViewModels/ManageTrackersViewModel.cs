using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class ManageTrackersViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private User _userModel;
    private readonly IWebClient _webClient;
    private readonly IPopupsService _popupsService;

    private IReadOnlyCollection<GsmGpsTracker> _trackers;
    private List<GsmGpsTrackerItem> _trackersItems = new List<GsmGpsTrackerItem>();
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    /// <summary>
    /// Trackers items
    /// </summary>
    public List<GsmGpsTrackerItem> TrackersItems
    {
        get
        {
            return _trackersItems;
        }
        set
        {
            _trackersItems = value;
            RaisePropertyChanged(nameof(TrackersItems));
        }
    }
    
    #region Commands

    /// <summary>
    /// Create new tracker
    /// </summary>
    public ICommand CreateNewTrackerCommand { get; private set; }
    
    /// <summary>
    /// Claim the tracker
    /// </summary>
    public ICommand ClaimTrackerCommand { get; private set; }
    
    /// <summary>
    /// Delete the tracker
    /// </summary>
    public ICommand DeleteTrackerCommand { get; private set; }

    #endregion

    public ManageTrackersViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        _popupsService = App.ServicesProvider.GetService<IPopupsService>();
        
        #region Commands binding

        CreateNewTrackerCommand = new Command(async () => await CreateNewTrackerAsync());
        ClaimTrackerCommand = new Command<Guid>(async tId => await ClaimTrackerAsync(tId));
        DeleteTrackerCommand = new Command<Guid>(async tId => await DeleteTrackerAsync(tId));

        #endregion
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _userModel = (User)query["UserModel"];
    }
    
    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        _trackers = await _webClient.GetAllGsmGpsTrackersAsync().ConfigureAwait(false);
        
        var trackersAsList = _trackers.ToList();
        TrackersItems = new List<GsmGpsTrackerItem>();
        for (int index = 0; index < trackersAsList.Count; index++)
        {
            TrackersItems.Add(new GsmGpsTrackerItem(trackersAsList[index], index));
        }
        
        TrackersItems = new List<GsmGpsTrackerItem>(TrackersItems); // Dirty way to force listview to update
    }

    private async Task CreateNewTrackerAsync()
    {
        
    }
    
    private async Task ClaimTrackerAsync(Guid trackerId)
    {
        if (!await _popupsService.ShowQuestionAsync("Are you sure?", "Do you want to claim this tracker?"))
        {
            return;
        }

        var claimedTracker = await _webClient.ClaimGsmGpsTrackerAsync(new ClaimGsmGpsTrackerRequest(trackerId));
        await _popupsService.ShowAlertAsync("Success", "Tracker successfully claimed.");
    }
    
    private async Task DeleteTrackerAsync(Guid trackerId)
    {
    }
}