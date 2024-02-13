using Filian.Core;
using Filian.MVVM.Model;
using Filian.MVVM.View;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace Filian.MVVM.ViewModel
{
    public class UserRegistrationViewModel : ViewModel
    {
        private string userName;
        private string userEmail;
        private static string userPassword;

        public string UserName
        {
            get => userName;
            set
            {
                userName = (value.Length >= 2 && value.Length <= 20) ? value : null;
                OnPropertyChanged();
            }
        }

        public static string UserPassword
        {
            get => userPassword;
            set
            {
                if (value.Length >= 5 && value.Any(char.IsUpper) && value.Any(char.IsLower) && value.Any(char.IsDigit) &&
                    value.Length <= 25 && !value.Contains("'"))
                    userPassword = value;
                else
                    userPassword = null;
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

        public RelayCommand SignUp { get; set; }
        public RelayCommand OpenSingInViewCommand { get; set; }
        public MainWindow MainWindow { get; set; }
        public UserAuthorizationView UserAuthorizationView { get; set; }
        
        public UserRegistrationViewModel()
        {
            SignUp = new RelayCommand(o => AddNewUserToDataBase());
            OpenSingInViewCommand = new RelayCommand(o => OpenUserAuthorizationView());
        }

        private void AddNewUserToDataBase()
        {
            User newUser = CreateNewUser();
            if (newUser != null)
            {
                string sqlNewUser = 
                    $"INSERT INTO users (username , email, user_status, user_password) VALUES ('{newUser.UserName}', '{newUser.UserEmail}', '{newUser.UserStatus}', '{newUser.UserPassword}')";

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(sqlNewUser, sqlConnection);
                    sqlCommand.ExecuteNonQuery();

                    Log.Info("Successfully add new user to database.");

                    SetAccountData(newUser);
                    OpenMainWindow();

                }
                catch (Exception ex)
                {
                    CreateUserNotificationBox("User with current username or email address already exists!",
                        "Create new username or sign in.");
                    Log.Error("Failed while trying to add new user to database: ", ex);
                    CreateUserNotificationBox("Something went wrong while tying to register you!", "Please, check your connection to internet!");
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

        }

        private User CreateNewUser()
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    if (!string.IsNullOrEmpty(userPassword))
                    {
                        User newUser = new User(userName, userEmail, "Owlet", UserPassword);
                        return newUser;
                    }
                    CreateUserNotificationBox("You entered incorrect password!",
                        "Your password has to be between 5 and 25 characters, contains uppercase and lowercase letter and digits.");
                    return null;
                }
                CreateUserNotificationBox("You entered incorrect email!", "Please, enter correct email address.");
                return null;
            }
            CreateUserNotificationBox("You entered incorrect username!", "Your username have to be between 2 and 20 characters.");
            return null;
        }

        private void OpenMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
            Application.Current.Windows.OfType<UserRegistrationView>().First().Close();
        }

        private void OpenUserAuthorizationView()
        {
            UserAuthorizationView = new UserAuthorizationView();
            UserAuthorizationView.Show();
            Application.Current.Windows.OfType<UserRegistrationView>().First().Close();
        }

        private void SetAccountData(User user)
        {
            MainViewModel.userName = user.UserName;
            MainViewModel.userEmail = user.UserEmail;
            MainViewModel.userStatus = user.UserStatus;
            MainViewModel.countOfCorrectAnswers = user.CountOfCorrectAnswers.ToString();
        }
    }
}