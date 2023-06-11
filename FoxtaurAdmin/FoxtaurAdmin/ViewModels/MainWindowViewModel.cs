using System;
using System.Collections.Generic;
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
using LibAuxiliary.Helpers;
using LibFoxtaurAdmin.Services.Abstract;
using LibWebClient.Constants;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
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

    private string _mapName;

    public string MapName
    {
        get => _mapName;
        set => this.RaiseAndSetIfChanged(ref _mapName, value);
    }

    private string _mapNorthLat;

    public string MapNorthLat
    {
        get => _mapNorthLat;
        set => this.RaiseAndSetIfChanged(ref _mapNorthLat, value);
    }
    
    private string _mapSouthLat;

    public string MapSouthLat
    {
        get => _mapSouthLat;
        set => this.RaiseAndSetIfChanged(ref _mapSouthLat, value);
    }
    
    private string _mapWestLon;
    
    public string MapWestLon
    {
        get => _mapWestLon;
        set => this.RaiseAndSetIfChanged(ref _mapWestLon, value);
    }
    
    private string _mapEastLon;
    
    public string MapEastLon
    {
        get => _mapEastLon;
        set => this.RaiseAndSetIfChanged(ref _mapEastLon, value);
    }
    

    private int _selectedMapFIleIndex = -1;

    public int SelectedMapFileIndex
    {
        get => _selectedMapFIleIndex;
        set { this.RaiseAndSetIfChanged(ref _selectedMapFIleIndex, value); }
    }

    private IList<MapFile> _mapFiles = new List<MapFile>();
    
    public IList<MapFile> MapFiles
    {
        get => _mapFiles;
        set
        {
            this.RaiseAndSetIfChanged(ref _mapFiles, value);
        }
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
    
    /// <summary>
    /// Create new map on server
    /// </summary>
    public ReactiveCommand<Unit, Unit> CreateNewMapCommand { get; }

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
        UploadMapFileCommand = ReactiveCommand.CreateFromTask(OnUploadMapFileCommandAsync); // TODO: Add enable/disable button logic

        var isCanCreateMap =
            Observable.CombineLatest
            (
                this.WhenAny(m => m.MapName, mn => mn.Value),
                this.WhenAny(m => m.MapNorthLat, mnl => mnl.Value),
                this.WhenAny(m => m.MapSouthLat, msl => msl.Value),
                this.WhenAny(m => m.MapWestLon, mwl => mwl.Value),
                this.WhenAny(m => m.MapEastLon, mel => mel.Value),
                this.WhenAny(m => m.SelectedMapFileIndex, smfi => smfi.Value),
                (mapName, mapNorthLat, mapSouthLat, mapWestLon, mapEastLon, selectedMapFileIndex) =>
                {
                    return !string.IsNullOrWhiteSpace(mapName)
                           &&
                           !string.IsNullOrWhiteSpace(mapNorthLat)
                           &&
                           !string.IsNullOrWhiteSpace(mapSouthLat)
                           &&
                           !string.IsNullOrWhiteSpace(mapWestLon)
                           &&
                           !string.IsNullOrWhiteSpace(mapEastLon)
                           &&
                           selectedMapFileIndex != -1;
                }
            );
        CreateNewMapCommand = ReactiveCommand.CreateFromTask(OnCreateNewMapCommandAsync, isCanCreateMap);

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

        #region After login business-logic

        await ReloadMapFilesAsync();
        
        #endregion
        
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
    private async Task OnUploadMapFileCommandAsync()
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

        await _webClient.MarkMapFileAsReadyAsync(new MarkMapFileAsReadyRequest(mapFile.Id));
        
        MapFileName = string.Empty;
        MapFilePath = string.Empty;
        MapFileUploadProgress = 0;
        
        await ReloadMapFilesAsync();
    }

    /// <summary>
    /// Reload map files in create new map section
    /// </summary>
    public async Task ReloadMapFilesAsync()
    {
        MapFiles = (await _webClient.GetAllMapFilesAsync().ConfigureAwait(false)).ToList();
    }

    /// <summary>
    /// Create new map on server
    /// </summary>
    private async Task OnCreateNewMapCommandAsync()
    {
        var northLat = DegreesRadiansHelper.ToRadians(double.Parse(MapNorthLat));
        var southLat = DegreesRadiansHelper.ToRadians(double.Parse(MapSouthLat));
        var westLon = DegreesRadiansHelper.ToRadians(double.Parse(MapWestLon));
        var eastLon = DegreesRadiansHelper.ToRadians(double.Parse(MapEastLon));
        
        await _webClient.CreateMapAsync
        (
            new CreateMapRequest(MapName, northLat, southLat, eastLon, westLon, MapFiles[SelectedMapFileIndex].Id)
        ).ConfigureAwait(false);
    }
}