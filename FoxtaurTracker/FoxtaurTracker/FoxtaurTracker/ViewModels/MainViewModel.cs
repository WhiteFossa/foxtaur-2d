using CommunityToolkit.Mvvm.Input;
using FoxtaurTracker.Models;
using System.Windows.Input;

namespace FoxtaurTracker.ViewModels
{
    public class MainViewModel
    {
        /// <summary>
        /// Main model
        /// </summary>
        private MainModel _mainModel;

        #region Commands

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
            _mainModel = new MainModel();

            #region Commands binding

            LogInCommand = new AsyncRelayCommand(ShowLoginPage);
            RegisterCommand = new AsyncRelayCommand(ShowRegistrationPage);

            #endregion
        }

        private async Task ShowLoginPage()
        {
            await Shell.Current.GoToAsync("loginPage");
        }

        private async Task ShowRegistrationPage()
        {
            await Shell.Current.GoToAsync("registrationPage");
        }
    }
}
