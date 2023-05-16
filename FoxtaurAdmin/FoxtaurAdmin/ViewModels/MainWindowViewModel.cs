using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using FoxtaurAdmin.Constants;
using FoxtaurAdmin.Models;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibFoxtaurAdmin.Services.Abstract;
using LibWebClient.Constants;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ZstdSharp;

namespace FoxtaurAdmin.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private MainModel _model;
 
    #region DI
    
    private readonly IWebClient _webClient = Program.Di.GetService<IWebClient>();
    private readonly IConfigurationService _configurationService = Program.Di.GetService<IConfigurationService>();
    private readonly IUserMessagesService _userMessagesService = Program.Di.GetService<IUserMessagesService>();
    private readonly ICompressionService _compressionService = Program.Di.GetService<ICompressionService>();
    
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

    private string _mapFileName;

    public string MapFileName
    {
        get => _mapFileName;
        set => this.RaiseAndSetIfChanged(ref _mapFileName, value);
    }

    private string _mapFilePath;

    public string MapFilePath
    {
        get => _mapFilePath;
        set => this.RaiseAndSetIfChanged(ref _mapFilePath, value);
    }

    private double _mapFileUploadProgress;

    public double MapFileUploadProgress
    {
        get => _mapFileUploadProgress;
        set => this.RaiseAndSetIfChanged(ref _mapFileUploadProgress, value);
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
    
    /// <summary>
    /// Set map file to upload (not upload, just set a path)
    /// </summary>
    public ReactiveCommand<Unit, Unit> SetMapFileCommand { get; }
    
    /// <summary>
    /// Upload selected map file to server
    /// </summary>
    public ReactiveCommand<Unit, Unit> UploadMapFileCommand { get; }

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

        SetMapFileCommand = ReactiveCommand.CreateFromTask(OnSetMapFileCommandAsync); // TODO: Add enable/disable button logic
        UploadMapFileCommand = ReactiveCommand.CreateFromTask(OnUploadMapFileCommand); // TODO: Add enable/disable button logic

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
        await _webClient.LogoutAsync().ConfigureAwait(false);

        IsLoggedIn = false;
    }
    
    /// <summary>
    /// Setting map file to upload path
    /// </summary>
    private async Task OnSetMapFileCommandAsync()
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "GeoTIFF", Extensions = { "tiff", "TIFF" } });
        dialog.AllowMultiple = false;
        
        var dialogResult = await dialog.ShowAsync(Program.GetMainWindow());
        if (dialogResult == null)
        {
            return;
        }
            
        MapFilePath = dialogResult.FirstOrDefault();
    }
    
    /// <summary>
    /// Perform map file upload
    /// </summary>
    private async Task OnUploadMapFileCommand()
    {
        // Compressing map file
        using var mapFileStream = File.OpenRead(MapFilePath);
        using var compressedStream = new MemoryStream();
        _compressionService.Compress(mapFileStream, compressedStream);

        // Creating empty map file first
        var mapFile = await _webClient.CreateMapFileAsync
        (
            new CreateMapFileRequest(MapFileName, (int)compressedStream.Length)
        );
        
        // Uploading chunks
        using (var mapFileReader = new BinaryReader(compressedStream))
        {
            var uploaded = 0;
            MapFileUploadProgress = 0;
            while (uploaded < compressedStream.Length)
            {
                var remaining = (int)compressedStream.Length - uploaded;
                var toUploadSize = Math.Min(GlobalConstants.MapFileUploadChunkSize, remaining);

                // Reading chunk from file
                var data = new byte[toUploadSize];
                mapFileReader.BaseStream.Seek(uploaded, SeekOrigin.Begin);
                mapFileReader.Read(data, 0, toUploadSize);

                var uploadRequest = new UploadMapFilePartRequest
                (
                    mapFile.Id,
                    uploaded,
                    Convert.ToBase64String(data)
                );

                await _webClient.UploadMapFilePartAsync(uploadRequest);

                uploaded += toUploadSize;
                MapFileUploadProgress = uploaded / (double)compressedStream.Length;
            }
        }

        MapFileName = string.Empty;
        MapFilePath = string.Empty;
        MapFileUploadProgress = 0;
    }
}