using Filian.MVVM.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Filian.MVVM.View
{
    public partial class UserRegistrationView : Window
    {
        public UserRegistrationView()
        {
            InitializeComponent();
            EyeImage.Source = ChangeImageSource(@"Images\closed_eye.png");
        }

        private void SignUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserRegistrationViewModel.UserPassword = PasswordBox.Password;
        }

        private void ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e) => ShowPasswordFunction();
        private void ShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e) => HidePasswordFunction();
        private void ShowPassword_MouseLeave(object sender, MouseEventArgs e) => HidePasswordFunction();

        private void ShowPasswordFunction()
        {
            ShownPassword.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Hidden;
            ShownPassword.Text = PasswordBox.Password;
            EyeImage.Source = ChangeImageSource(@"Images\opened_eye.png");
        }

        private void HidePasswordFunction()
        {
            ShownPassword.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Visible;
            EyeImage.Source = ChangeImageSource(@"Images\closed_eye.png");
        }

        private BitmapImage ChangeImageSource(string sourcePath)
        {
            BitmapImage bitImg = new BitmapImage();
            bitImg.BeginInit();
            bitImg.UriSource = new Uri(Path.GetFullPath(sourcePath));
            bitImg.EndInit();
            return bitImg;
        }

        private void PasswordBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!Application.Current.Windows.OfType<MainWindow>().Any() && !Application.Current.Windows.OfType<UserAuthorizationView>().Any())
            {
                Application.Current.Shutdown();
            }
        }
    }
}