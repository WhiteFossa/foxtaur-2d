using System.ComponentModel;
using System.Windows.Input;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class LoginViewModel  : IQueryAttributable, INotifyPropertyChanged
    {
        private IWebClient _webClient;
        
        private bool _isFromRegistrationPage;

        private string _login;
        private string _password;
        
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
                await App.PopupsService.ShowAlertAsync("Error", "Login failed. Is credentials correct?");
                return;
            }
            
            await App.PopupsService.ShowAlertAsync("Success", $"Token: { result.Token }, Expiration: { result.ExpirationTime }");
        }
        
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Login = (string)query["Login"];
            _isFromRegistrationPage = (bool)query["IsFromRegistrationPage"];
        }
        
        private void RefreshCanExecutes()
        {
            (LogInCommand as Command).ChangeCanExecute();
        }
    }
}
