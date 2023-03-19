using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using LibWebClient.Models;
using LibWebClient.Models.Enums;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class RegisterOnDistanceViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;
    private User _userModel;

    private IReadOnlyCollection<Distance> _distances;
    
    private int _distanceIndex { get; set; }
    private List<DistanceItem> _distanceItems = new List<DistanceItem>();
    
    /// <summary>
    /// Distance index
    /// </summary>
    public int DistanceIndex
    {
        get
        {
            return _distanceIndex;
        }
        set
        {
            _distanceIndex = value;
            RaisePropertyChanged(nameof(DistanceIndex));
            RefreshCanExecutes();
        }
    }
    
    /// <summary>
    /// Distances
    /// </summary>
    public List<DistanceItem> DistanceItems
    {
        get
        {
            return _distanceItems;
        }
        set
        {
            _distanceItems = value;
            RaisePropertyChanged(nameof(DistanceItems));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #region Commands

    /// <summary>
    /// Create team
    /// </summary>
    public ICommand RegisterOnDistanceCommand { get; private set; }

    #endregion

    public RegisterOnDistanceViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();

        #region Commands binding

        RegisterOnDistanceCommand = new Command(async () => await RegisterOnDistanceAsync(),
            () =>
            {
                return DistanceIndex != -1;
            });

        #endregion
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        DistanceIndex = -1;
        
        _distances = await _webClient.GetDistancesWithoutIncludeAsync();

        var distancesAsList = _distances.ToList();
        
        DistanceItems = new List<DistanceItem>();
        
        for (int index = 0; index < distancesAsList.Count; index++)
        {
            DistanceItems.Add(new DistanceItem(distancesAsList[index], index));
        }

        DistanceItems = new List<DistanceItem>(DistanceItems); // Dirty way to force picker to update
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _userModel = (User)query["UserModel"];
    }

    private async Task RegisterOnDistanceAsync()
    {
        var request = new RegisterOnDistanceRequest(DistanceItems.Single(d => d.Index == DistanceIndex).Distance.Id);
        
        try
        {
            var result = await _webClient.RegisterOnDistanceAsync(request);

            switch (result.Result)
            {
                case RegistrationOnDistanceResult.Success:
                    await App.PopupsService.ShowAlertAsync("Success", "You are registered on the distance.");
                    break;
                
                case RegistrationOnDistanceResult.AlreadyRegistered:
                    await App.PopupsService.ShowAlertAsync("Error", "You are already registered on this distance.");
                    return;

                case RegistrationOnDistanceResult.DistanceNotFound:
                    await App.PopupsService.ShowAlertAsync("Error", "Distance not found. Very strange.");
                    return;

                case RegistrationOnDistanceResult.Failure:
                    await App.PopupsService.ShowAlertAsync("Error", "Generic error during registration on the distance.");
                    return;
                
                default:
                    throw new InvalidOperationException("Unknown registration on distance response.");
            }
        }
        catch (Exception)
        {
            await App.PopupsService.ShowAlertAsync("Error", "Unknown error during registration on distance.");
            return;
        }
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "IsFromRegistrationPage", false },
            { "UserModel", _userModel }
        };

        await Shell.Current.GoToAsync("mainPage", navigationParameter);
    }
    
    private void RefreshCanExecutes()
    {
        (RegisterOnDistanceCommand as Command).ChangeCanExecute();
    }
}