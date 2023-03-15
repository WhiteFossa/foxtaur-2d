using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxtaurTracker.Models;
using System.Windows.Input;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel _mainModel;

        private IWebClient _webClient;

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
        
        public MainViewModel()
        {
            _webClient = App.ServicesProvider.GetService<IWebClient>();
            
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
        }
        
        private async Task ShowLoginPageAsync()
        {
            await Shell.Current.GoToAsync("loginPage");
        }

        private async Task ShowRegistrationPageAsync()
        {
            await Shell.Current.GoToAsync("registrationPage");
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
