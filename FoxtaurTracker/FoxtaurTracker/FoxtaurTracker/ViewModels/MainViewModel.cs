using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class MainViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;
    
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
    /// Log in
    /// </summary>
    public ICommand EditProfileCommand { get; private set; }

    /// <summary>
    /// Log in
    /// </summary>
    public ICommand CreateTeamCommand { get; private set; }
    
    #endregion
    
    public MainViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        
        #region Commands binding

        EditProfileCommand = new Command(async() => await EditProfileAsync());
        CreateTeamCommand = new Command(async () => await CreateTeamAsync());

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

        await Shell.Current.GoToAsync("editProfilePage", navigationParameter);
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        // Getting current user info
        _userInfo = await _webClient.GetCurrentUserInfoAsync();
            
        // Reading profile
        var profileRequest = new ProfilesMassGetRequest(new List<Guid>() { _userInfo.Id });
        _profile = (await _webClient.MassGetProfilesAsync(profileRequest))
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

        await Shell.Current.GoToAsync("createTeamPage", navigationParameter);
    }
}