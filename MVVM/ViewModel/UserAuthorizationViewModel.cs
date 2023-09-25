using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Filian.Core;
using Filian.MVVM.View;

namespace Filian.MVVM.ViewModel
{
    public class UserAuthorizationViewModel
    {
        public RelayCommand OpenSingUpViewCommand { get; set; }
        public RelayCommand SignIn { get; set; }
        public MainWindow MainWindow { get; set; }
        public UserRegistrationView UserRegistrationView { get; set; }

        public UserAuthorizationViewModel()
        {
           SignIn = new RelayCommand(o=>OpenMainWindow());
           OpenSingUpViewCommand = new RelayCommand(o=> OpenUserRegistrationView());
        }

        private void OpenMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void OpenUserRegistrationView()
        {
            UserRegistrationView = new UserRegistrationView();
            UserRegistrationView.Show();
        }
    }
}
