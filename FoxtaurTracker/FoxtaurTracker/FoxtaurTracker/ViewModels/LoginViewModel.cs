using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class LoginViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private readonly IWebClient _webClient;
        private readonly ISettingsService _settingsService;
        
        private bool _isFromRegistrationPage;

        private string _login;
        private string _password;

        private User _userModel;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        ///  User login
        /// </summary>
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                RaisePropertyChanged(nameof(Login));
                RefreshCanExecutes();   
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
                RefreshCanExecutes();
            }
        }
        
        #region Commands

        /// <summary>
        /// Log in
        /// </summary>
        public ICommand LogInCommand { get; private set; }

        #endregion

        public LoginViewModel()
        {
            _webClient = App.ServicesProvider.GetService<IWebClient>();
            _settingsService = App.ServicesProvider.GetService<ISettingsService>();
            
            #region Commands binding

            LogInCommand = new Command(async () => await LogInAsync(),
                () =>
                {
                    return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
                });

            #endregion
        }

        private async Task LogInAsync()
        {
            var request = new LoginRequest(Login, Password);
            var result = await _webClient.LoginAsync(request);

            if (!result.IsSuccessful)
            {
                await App.PopupsService.ShowAlertAsync("Error", "Login failed. Are credentials correct?");
                return;
            }
            
            // Saving login and password for autologin
            _settingsService.SaveLogin(Login);
            await _settingsService.SavePasswordAsync(Password);

            _userModel.Token = result.Token;
            _userModel.TokenExpirationTime = result.ExpirationTime;
            
            // Setting token to client
            await _webClient.SetAuthentificationTokenAsync(_userModel.Token);
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "IsFromRegistrationPage", _isFromRegistrationPage },
                { "UserModel", _userModel }
            };

            await Shell.Current.GoToAsync("mainPage", navigationParameter);
        }
        
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _isFromRegistrationPage = (bool)query["IsFromRegistrationPage"];
            
            _userModel = (User)query["UserModel"];
            Login = _userModel.Login;
        }
        
        private void RefreshCanExecutes()
        {
            (LogInCommand as Command).ChangeCanExecute();
        }
    }
}
