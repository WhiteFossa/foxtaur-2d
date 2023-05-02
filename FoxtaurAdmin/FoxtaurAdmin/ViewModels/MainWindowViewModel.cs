using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FoxtaurAdmin.Models;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibFoxtaurAdmin.Services.Abstract;
using LibWebClient.Constants;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using LibWebClient.Services.Implementations;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;

namespace FoxtaurAdmin.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private MainModel _model;
 
    #region DI
    
    private readonly IWebClient _webClient = Program.Di.GetService<IWebClient>();
    private readonly IConfigurationService _configurationService = Program.Di.GetService<IConfigurationService>();
    private readonly IUserMessagesService _userMessagesService = Program.Di.GetService<IUserMessagesService>();
    
    #endregion
    
    #region Bound properties

    private string _login;

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }


    private string _password;

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string _serverUrl;

    public string ServerUrl
    {
        get => _serverUrl;
        set => this.RaiseAndSetIfChanged(ref _serverUrl, value);
    }

    private string _serverName;

    public string ServerName
    {
        get => _serverName;
        set => this.RaiseAndSetIfChanged(ref _serverName, value);
    }

    private int _protocolVersion;

    private int ProtocolVersion
    {
        get => _protocolVersion;
        set => this.RaiseAndSetIfChanged(ref _protocolVersion, value);
    }
    
    public bool IsLoggedIn
    {
        get => _model.IsLoggedIn;
        set => this.RaiseAndSetIfChanged(ref _model.IsLoggedIn, value);
    }

    #endregion
    
    #region Commands
    
    /// <summary>
    /// Login
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    
    /// <summary>
    /// Logout
    /// </summary>
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    
    #endregion
    
    #region Constructor

    public MainWindowViewModel(MainModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model), "Model have to be specified!");

        #region Commands
        
        var isCanLogin =
            Observable.CombineLatest
            (
                this.WhenAny(m => m.Login, l => l.Value),
                this.WhenAny(m => m.Password, p => p.Value),
                this.WhenAny(m => m.IsLoggedIn, ili => ili.Value),
                (login, password, isLoggedIn) => !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password) && !isLoggedIn
            );
        LoginCommand = ReactiveCommand.CreateFromTask(OnLoginCommandAsync, isCanLogin);
        
        var isCanLogout = this.WhenAny(m => m.IsLoggedIn, ili => ili.Value);
        LogoutCommand = ReactiveCommand.CreateFromTask(OnLogoutCommandAsync, isCanLogout);

        #endregion
        
        #region Initial state

        IsLoggedIn = false;

        #endregion
    }
    
    #endregion
    
    /// <summary>
    /// Logging in
    /// </summary>
    private async Task OnLoginCommandAsync()
    {
        IsLoggedIn = false;
        
        _model.ServerInfo = await _webClient.GetServerInfoAsync();
        
        ServerUrl = _configurationService.GetConfigurationString(ConfigConstants.ServerUrlSettingName);
        ServerName = _model.ServerInfo.Name;
        ProtocolVersion = _model.ServerInfo.ProtocolVersion;

        if (ProtocolVersion != WebClientConstants.ProtocolVersion)
        {
            await _userMessagesService.ShowMessageAsync("Error", $"Unsupported server protocol version!\nExpected {WebClientConstants.ProtocolVersion}, got {ProtocolVersion}.", Icon.Error);
            return;
        }

        var result = await _webClient.LoginAsync(new LoginRequest(Login, Password));
        if (!result.IsSuccessful)
        {
            await _userMessagesService.ShowMessageAsync("Error", $"Unable to log in. Are credentials correct?", Icon.Error);
            return;
        }

        IsLoggedIn = true;
    }

    /// <summary>
    /// Logging out
    /// </summary>
    private async Task OnLogoutCommandAsync()
    {
        await _webClient.LogoutAsync();

        IsLoggedIn = false;
    }
}