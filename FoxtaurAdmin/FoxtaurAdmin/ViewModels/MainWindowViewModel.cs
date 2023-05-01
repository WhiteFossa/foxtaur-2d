using System;
using System.Reactive;
using FoxtaurAdmin.Models;
using ReactiveUI;

namespace FoxtaurAdmin.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private MainModel _model;
    
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

    #endregion
    
    #region Commands
    
    /// <summary>
    /// Login
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    
    #endregion
    
    #region Constructor

    public MainWindowViewModel(MainModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model), "Model have to be specified!");

        LoginCommand = ReactiveCommand.Create(OnLoginCommand);
    }
    
    #endregion
    
    /// <summary>
    /// Logging in
    /// </summary>
    private void OnLoginCommand()
    {
        ServerUrl = "Test URL";
        ServerName = "Test Name";
        ProtocolVersion = 1337;
    }
}