using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using FoxtaurTracker.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class RegistrationViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private readonly IWebClient _webClient;

        private User _userModel;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        #region Commands

        /// <summary>
        /// Submit registration data
        /// </summary>
        public ICommand SubmitRegistrationDataCommand { get; private set; }

        #endregion

        #region Bound properties

        /// <summary>
        ///  User login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation
        /// </summary>
        public string PasswordConfirmation { get; set; }

        #endregion

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public RegistrationViewModel()
        {
            _webClient = App.ServicesProvider.GetService<IWebClient>();
            
            #region Commands binding

            SubmitRegistrationDataCommand = new Command(async () => SubmitRegistrationDataAsync());

            #endregion
        }

        /// <summary>
        /// Submit registration data to server
        /// </summary>
        private async Task SubmitRegistrationDataAsync()
        {
            if (!Password.Equals(PasswordConfirmation))
            {
                await App.PopupsService.ShowAlertAsync("Error", "Password and confirmation don't match.");
                return;
            }

            var request = new RegistrationRequest(Login, Email, Password);
            var isSuccessful = await _webClient.RegisterUserAsync(request);

            if (!isSuccessful)
            {
                await App.PopupsService.ShowAlertAsync("Error", "Failed to register.");
                return;
            }

            await App.PopupsService.ShowAlertAsync("Success", "Registration successful, log in please.");

            _userModel.Login = Login;
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "IsFromRegistrationPage", true },
                { "UserModel", _userModel }
            };
            
            await Shell.Current.GoToAsync("loginPage", navigationParameter);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _userModel = (User)query["UserModel"];
        }
    }
}
