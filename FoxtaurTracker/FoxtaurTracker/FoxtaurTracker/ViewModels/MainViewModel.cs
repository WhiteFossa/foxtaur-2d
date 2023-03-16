using System.ComponentModel;
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

    public MainViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
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
    }

    
}