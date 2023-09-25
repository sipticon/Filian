using Filian.Core;
using Filian.MVVM.Model;
using Filian.MVVM.View;
using System.ComponentModel.DataAnnotations;

namespace Filian.MVVM.ViewModel
{
    public class UserRegistrationViewModel : ObservableObject
    {
        private string userName;
        private string userEmail;
        private string userPassword;

        public string UserName
        {
            get => userName;
            set
            {
                if (value.Length > 3)
                    userName = value;
                OnPropertyChanged();
            }
        }

        public string UserEmail
        {
            get => userEmail;
            set
            {
                var email = new EmailAddressAttribute();
                if (email.IsValid(value))
                    userEmail = value;
                OnPropertyChanged();
            }
        }

        public string UserPassword
        {
            get => userPassword;
            set
            {
                userPassword = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SignUp { get; set; }
        public MainWindow MainWindow { get; set; }

        public UserRegistrationViewModel()
        {
            SignUp = new RelayCommand(o => OpenMainWindow());
        }

        private void OpenMainWindow()
        {
            User newUser = new User(userName, userEmail, userPassword);
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}