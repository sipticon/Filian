﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Filian.MVVM.ViewModel;

namespace Filian.MVVM.View
{
    public partial class UserAccountView : UserControl
    {
        public UserAccountView()
        {
            InitializeComponent();
        }


        private void ChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            UserAccountViewModel.NewUserPassword = PasswordBox.Password;
            PasswordBox.Password = "";
        }

        private void ChangeUsername_OnClick(object sender, RoutedEventArgs e)
        {
            NewUsername.Text = "";
        }

        private void ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e) => ShowPasswordFunction();
        private void ShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e) => HidePasswordFunction();
        private void ShowPassword_MouseLeave(object sender, MouseEventArgs e) => HidePasswordFunction();

        private void ShowPasswordFunction()
        {
            ShownPassword.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Hidden;
            ShownPassword.Text = PasswordBox.Password;
            EyeImage.Source = ChangeImageSource(@"C:\Users\oleksandrm\materials\Filian\Images\opened_eye.png");
        }

        private void HidePasswordFunction()
        {
            ShownPassword.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Visible;
            EyeImage.Source = ChangeImageSource(@"C:\Users\oleksandrm\materials\Filian\Images\closed_eye.png");
        }

        private BitmapImage ChangeImageSource(string sourcePath)
        {
            BitmapImage bitImg = new BitmapImage();
            bitImg.BeginInit();
            bitImg.UriSource = new Uri(sourcePath);
            bitImg.EndInit();
            return bitImg;
        }
        private void passwordBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}