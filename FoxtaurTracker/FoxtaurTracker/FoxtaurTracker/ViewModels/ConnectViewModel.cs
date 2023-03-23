using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxtaurTracker.Models;
using System.Windows.Input;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class ConnectViewModel : INotifyPropertyChanged
    {
        private MainModel _mainModel;

        private readonly IWebClient _webClient;
        private readonly ILoginService _loginService;

        private bool _isConnected;
        
        private string _serverUrl;
        private string _serverName;
        private string _serverProtocolVersion;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        #region Bound properties

        /// <summary>
        /// Server address
        /// </summary>
        public string ServerUrl
        {
            get
            {
                return _serverUrl;
            }
            
            set
            {
                _serverUrl = value;
                RaisePropertyChanged(nameof(ServerUrl));
            }
        }

        /// <summary>
        ///  Server name
        /// </summary>
        public string ServerName
        {
            get
            {
                return _serverName;
            }
            set
            {
                _serverName = value;
                RaisePropertyChanged(nameof(ServerName));
            }
        }

        /// <summary>
        /// Server protocol version
        /// </summary>
        public string ServerProtocolVersion
        {
            get
            {
                return _serverProtocolVersion;
            }
            set
            {
                _serverProtocolVersion = value;
                RaisePropertyChanged(nameof(ServerProtocolVersion));
            }
        }

        #endregion
        
        #region Commands

        /// <summary>
        /// Connect to server
        /// </summary>
        public ICommand ConnectCommand { get; private set; }
        
        /// <summary>
        /// Login
        /// </summary>
        public ICommand LogInCommand { get; private set; }

        /// <summary>
        /// Registration
        /// </summary>
        public ICommand RegisterCommand { get; private set; }

        #endregion
        
        public ConnectViewModel()
        {
            _webClient = App.ServicesProvider.GetService<IWebClient>();
            _loginService = App.ServicesProvider.GetService<ILoginService>();
            
            _mainModel = new MainModel();

            #region Initializing values

            ServerUrl = "-";
            ServerName = "-";
            ServerProtocolVersion = "-";

            #endregion

            #region Commands binding

            ConnectCommand = new Command(async () => await ConnectToServerAsync(), () => !_isConnected);
            LogInCommand = new Command(async () => await ShowLoginPageAsync(), () => _isConnected);
            RegisterCommand = new Command(async () => ShowRegistrationPageAsync(), () => _isConnected);

            #endregion
        }

        private async Task ConnectToServerAsync()
        {
            // Setting up server (TODO: Move URL to config)
            ServerUrl = @"https://api.foxtaur.me";

            await _webClient.ConnectAsync();
            
            var serverInfo = await _webClient.GetServerInfoAsync();
            ServerName = serverInfo.Name;
            ServerProtocolVersion = serverInfo.ProtocolVersion.ToString();

            _isConnected = true;
            RefreshCanExecutes();

            await TryToPerformAutologinAsync();
        }

        private async Task TryToPerformAutologinAsync()
        {
            var autologinResult = await _loginService.TryPerformAutologinAsync();

            if (!autologinResult.Item1)
            {
                return;
            }
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "IsFromRegistrationPage", false },
                { "UserModel", autologinResult.Item2 }
            };

            await Shell.Current.GoToAsync("mainPage", navigationParameter);
        }
        
        private async Task ShowLoginPageAsync()
        {
            _mainModel.User.Login = String.Empty;

            var navigationParameter = new Dictionary<string, object>
            {
                { "IsFromRegistrationPage", false },
                { "UserModel", _mainModel.User }
            };
            
            await Shell.Current.GoToAsync("loginPage", navigationParameter);
        }

        private async Task ShowRegistrationPageAsync()
        {
            _mainModel.User.Login = string.Empty; // Clearing login before registration
            var navigationParameter = new Dictionary<string, object>
            {
                { "UserModel", _mainModel.User }
            };
            
            await Shell.Current.GoToAsync("registrationPage", navigationParameter);
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void RefreshCanExecutes()
        {
            (ConnectCommand as Command).ChangeCanExecute();
            (LogInCommand as Command).ChangeCanExecute();
            (RegisterCommand as Command).ChangeCanExecute();
        }
    }
}
