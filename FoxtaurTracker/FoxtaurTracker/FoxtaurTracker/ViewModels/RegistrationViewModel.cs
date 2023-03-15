using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace FoxtaurTracker.ViewModels
{
    public class RegistrationViewModel
    {
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

        public RegistrationViewModel()
        {
            #region Commands binding

            SubmitRegistrationDataCommand = new AsyncRelayCommand(SubmitRegistrationDataAsync);

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
            }
        }
    }
}
