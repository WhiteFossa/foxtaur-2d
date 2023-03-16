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

    #endregion
    
    public MainViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        
        #region Commands binding

        EditProfileCommand = new Command(async () => await EditProfileAsync());

        #endregion
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _isFromRegistrationPage = (bool)query["IsFromRegistrationPage"];
        _userModel = (User)query["UserModel"];
        
        // Getting current user info (TODO: Move to async code)
        _userInfo = _webClient.GetCurrentUserInfoAsync().Result;
            
        // Reading profile (TODO: Move to async code)
        var profileRequest = new ProfilesMassGetRequest(new List<Guid>() { _userInfo.Id });
        _profile = _webClient.MassGetProfilesAsync(profileRequest)
            .Result
            .Single();
        
        FormatUsernameToDisplay();

        if (_isFromRegistrationPage)
        {
            EditProfileAsync().RunSynchronously();
        }
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
            { "Profile", _profile }
        };

        await Shell.Current.GoToAsync("editProfilePage", navigationParameter);
    }
}