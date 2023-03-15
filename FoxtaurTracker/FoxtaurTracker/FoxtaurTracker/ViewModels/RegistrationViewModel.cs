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

        public RegistrationViewModel()
        {
            #region Commands binding

            SubmitRegistrationDataCommand = new AsyncRelayCommand(SubmitRegistrationDataAsync);

            #endregion
        }

        /// <summary>
        /// Submit registration data to server
        /// </summary>
        /// <returns></returns>
        private async Task SubmitRegistrationDataAsync()
        {
        }
    }
}
