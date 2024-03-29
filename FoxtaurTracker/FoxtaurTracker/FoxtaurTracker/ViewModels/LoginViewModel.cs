﻿using System.ComponentModel;
using System.Windows.Input;
using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels
{
    public class LoginViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private readonly ILoginService _loginService;
        
        private bool _isFromRegistrationPage;

        private string _login;
        private string _password;
        private bool _isRememberMe;

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

        public bool IsRememberMe
        {
            get
            {
                return _isRememberMe;
            }
            set
            {
                _isRememberMe = value;
                RaisePropertyChanged(nameof(IsRememberMe));
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
            _loginService = App.ServicesProvider.GetService<ILoginService>();
            
            #region Commands binding

            LogInCommand = new Command(async () => await LogInAsync(),
                () =>
                {
                    return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
                });

            #endregion

            IsRememberMe = true;
        }

        private async Task LogInAsync()
        {
            var loginResult = await _loginService.LogInAsync(Login, Password, IsRememberMe);
            if (!loginResult.Item1)
            {
                await App.PopupsService.ShowAlertAsync("Error", "Failed to log in. Are credentials correct?");
                return;
            }

            _userModel = loginResult.Item2;
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "IsFromRegistrationPage", _isFromRegistrationPage },
                { "UserModel", _userModel }
            };

            await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("mainPage", navigationParameter));
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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                (LogInCommand as Command).ChangeCanExecute();
            });
        }
    }
}
