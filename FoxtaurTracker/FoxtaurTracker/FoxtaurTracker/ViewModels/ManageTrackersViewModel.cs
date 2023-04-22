using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;

namespace FoxtaurTracker.ViewModels;

public class ManageTrackersViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private User _userModel;
    
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

    #endregion

    public ManageTrackersViewModel()
    {
        #region Commands binding

        CreateNewTrackerCommand = new Command(async () => await CreateNewTrackerAsync());

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
        // TODO: Put load from server here
        TrackersItems.Add(new GsmGpsTrackerItem(0, "Yiffy tracker", "123456789"));
        TrackersItems.Add(new GsmGpsTrackerItem(1, "Yuffy tracker", "234567890"));
        TrackersItems.Add(new GsmGpsTrackerItem(2, "Yerfy tracker", "345678901"));
        
        TrackersItems = new List<GsmGpsTrackerItem>(TrackersItems); // Dirty way to force listview to update
    }

    private async Task CreateNewTrackerAsync()
    {
        
    }
}