using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class CreateTeamViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;
    private User _userModel;
    
    #region Team fields
        
    private string _name { get; set; }
    private Color _color { get; set; }

    /// <summary>
    /// Name
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
    /// Hunter color
    /// </summary>
    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            RaisePropertyChanged(nameof(Color));
        }
    }
    
    #endregion
    
    #region Commands

    /// <summary>
    /// Create team
    /// </summary>
    public ICommand CreateTeamCommand { get; private set; }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    public CreateTeamViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();

        #region Commands binding

        CreateTeamCommand = new Command(async () => await CreateTeamAsync(),
            () =>
            {
                return !string.IsNullOrWhiteSpace(Name);
            });

        #endregion
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _userModel = (User)query["UserModel"];
    }

    private async Task CreateTeamAsync()
    {
        Color.ToRgba(out var teamColorR, out var teamColorG, out var teamColorB, out var teamColorA);

        var request = new CreateTeamRequest(Name, new ColorDto() { A = teamColorA, R = teamColorR, G = teamColorG, B = teamColorB });

        try
        {
            await _webClient.CreateTeamAsync(request).ConfigureAwait(false);
        }
        catch (Exception)
        {
            await App.PopupsService.ShowAlertAsync("Error", "Failed to create the team.");
            return;
        }

        await App.PopupsService.ShowAlertAsync("Success", "Team created.");
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "IsFromRegistrationPage", false },
            { "UserModel", _userModel }
        };

        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("mainPage", navigationParameter));
    }
    
    private void RefreshCanExecutes()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            (CreateTeamCommand as Command).ChangeCanExecute();
        });
    }
}