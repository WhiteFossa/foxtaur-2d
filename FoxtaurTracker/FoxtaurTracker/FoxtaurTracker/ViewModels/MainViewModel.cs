using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class MainViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;
    private readonly ILoginService _loginService;

    private bool _isFromRegistrationPage;

    private User _userModel;
    private UserInfo _userInfo;
    private Profile _profile;
    
    private string _usernameToDisplay;

    /// <summary>
    ///  Username to display
    /// </summary>
    public string UsernameToDisplay
    {
        get
        {
            return _usernameToDisplay;
        }
        set
        {
            _usernameToDisplay = value;
            RaisePropertyChanged(nameof(UsernameToDisplay));
        }
    }

    #region Commands

    /// <summary>
    /// Edit profile
    /// </summary>
    public ICommand EditProfileCommand { get; private set; }

    /// <summary>
    /// Create a team
    /// </summary>
    public ICommand CreateTeamCommand { get; private set; }
    
    /// <summary>
    /// Manage trackers
    /// </summary>
    public ICommand ManageTrackersCommand { get; private set; }
    
    /// <summary>
    /// Register on distance
    /// </summary>
    public ICommand RegisterOnDistanceCommand { get; private set; }
    
    /// <summary>
    /// Start tracking
    /// </summary>
    public ICommand RunCommand { get; private set; }

    /// <summary>
    /// Log out
    /// </summary>
    public ICommand LogOutCommand { get; private set; }

    #endregion
    
    public MainViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        _loginService = App.ServicesProvider.GetService<ILoginService>();

        #region Commands binding

        EditProfileCommand = new Command(async () => await EditProfileAsync());
        CreateTeamCommand = new Command(async () => await CreateTeamAsync());
        ManageTrackersCommand = new Command(async () => await ManageTrackersAsync());
        RegisterOnDistanceCommand = new Command(async () => await RegisterOnDistanceAsync());
        RunCommand = new Command(async () => await RunAsync());
        LogOutCommand = new Command(async() => await LogOutAsync());

        #endregion
    }

    public event PropertyChangedEventHandler PropertyChanged;
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _isFromRegistrationPage = (bool)query["IsFromRegistrationPage"];
        _userModel = (User)query["UserModel"];
    }

    private void FormatUsernameToDisplay()
    {
        UsernameToDisplay = $"{ _profile.FirstName } { _profile.MiddleName } { _profile.LastName }";
    }
    
    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task EditProfileAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "UserModel", _userModel },
            { "Profile", _profile }
        };

        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("editProfilePage", navigationParameter));
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        // Getting current user info
        _userInfo = await _webClient.GetCurrentUserInfoAsync().ConfigureAwait(false);
            
        // Reading profile
        var profileRequest = new ProfilesMassGetRequest(new List<Guid>() { _userInfo.Id });
        _profile = (await _webClient.MassGetProfilesAsync(profileRequest).ConfigureAwait(false))
            .Single();
        
        FormatUsernameToDisplay();

        if (_isFromRegistrationPage)
        {
            await EditProfileAsync();
        }
    }
    
    private async Task CreateTeamAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "UserModel", _userModel },
        };

        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("createTeamPage", navigationParameter));
    }

    private async Task ManageTrackersAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "UserModel", _userModel },
        };
        
        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("manageTrackersPage", navigationParameter));
    }

    private async Task RegisterOnDistanceAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "UserModel", _userModel },
        };

        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("registerOnDistancePage", navigationParameter));
    }

    private async Task RunAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "UserModel", _userModel }
        };
        
        await MainThread.InvokeOnMainThreadAsync(async () => await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("runPage", navigationParameter)));
    }

    private async Task LogOutAsync()
    {
        await _loginService.LogOutAsync();
        
        await Shell.Current.GoToAsync("connectPage");
    }
}