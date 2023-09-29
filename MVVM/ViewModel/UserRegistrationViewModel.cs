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

        public static string UserPassword
        {
            get => userPassword;
            set => userPassword = value;
        }

        public RelayCommand SignUp { get; set; }
        public MainWindow MainWindow { get; set; }
        
        public UserRegistrationViewModel()
        {
            SignUp = new RelayCommand(o => AddNewUserToDataBase());
        }

        private void AddNewUserToDataBase()
        {
            User newUser = CreateNewUser();
            if (newUser != null)
            {
                string sqlNewUser = "INSERT INTO users (username , email, user_status, user_password) VALUES (@username, @email, @status, @password)";

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                try
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlNewUser, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@username",newUser.UserName);
                    sqlCommand.Parameters.AddWithValue("@email",newUser.UserEmail);
                    sqlCommand.Parameters.AddWithValue("@status",newUser.UserStatus);
                    sqlCommand.Parameters.AddWithValue("@password", newUser.UserPassword);
                    sqlCommand.ExecuteNonQuery();

                    Log.Info("Successfully add new user to database.");

                    SetAccountData(newUser);
                    OpenMainWindow();

                }
                catch (Exception ex)
                {
                    Log.Error("Failed while trying to add new user to database: ", ex);
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
                }
            }

            return null;
        }

        private void OpenMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
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