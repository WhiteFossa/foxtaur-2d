using System.ComponentModel;
using System.Windows.Input;

namespace FoxtaurTracker.ViewModels
{
    public class LoginViewModel  : IQueryAttributable, INotifyPropertyChanged
    {
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
