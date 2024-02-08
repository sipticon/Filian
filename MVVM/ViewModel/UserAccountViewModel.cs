using System;
using System.Data.SqlClient;
using System.Linq;
using Filian.Core;

namespace Filian.MVVM.ViewModel
{
    public class UserAccountViewModel : ViewModel
    {
        private static string userName ;
        private static string userEmail;
        private static string userStatus;
        private static string countOfCorrectAnswers;

        private string userNameLabel;
        private string userEmailLabel;
        private string userStatusLabel;
        private string countOfCorrectAnswersLabel;

        private string newUsername;
        private static string newUserPassword;

        public string UserName
        {
            get => userName;
            set => userName = value;
        }

        public string UserEmail
        {
            get => userEmail;
            set => userEmail =value;
        }

        public string UserStatus
        {
            get => userStatus;
            set => userStatus = value;
        }

        public string CountOfCorrectAnswers
        {
            get => countOfCorrectAnswers;
            set => countOfCorrectAnswers = value;
        }

        public string UserNameLabel
        {
            get => userNameLabel;
            set
            {
                userNameLabel = value;
                OnPropertyChanged("UserNameLabel");
            }
        }


        public string UserEmailLabel
        {
            get => userEmailLabel;
            set
            {
                userEmailLabel = value;
                OnPropertyChanged("UserEmailLabel");
            }
        }

        public string UserStatusLabel
        {
            get => userStatusLabel;
            set
            {
                userStatusLabel = value;
                OnPropertyChanged("UserStatusLabel");
            }
        }

        public string CountOfCorrectAnswersLabel
        {
            get => countOfCorrectAnswersLabel;
            set
            {
                countOfCorrectAnswersLabel = value;
                OnPropertyChanged("CountOfCorrectAnswersLabel");
            }
        }

        public string NewUsername
        {
            get => newUsername;
            set
            {
                newUsername = (value.Length >= 5 && value.Length <= 10) ? value : null;
                OnPropertyChanged();
            }
        }

        public static string NewUserPassword
        {
            get => newUserPassword;
            set
            {
                if (value.Length >= 5 && value.Any(char.IsUpper) && value.Any(char.IsLower) && value.Any(char.IsDigit) &&
                    value.Length <= 20 && !value.Contains("'"))
                    newUserPassword = value;
                else
                    newUserPassword = null;
            }
        }

        public RelayCommand ChangeUsername { get; set; }
        public RelayCommand ChangePassword { get; set; }

        public UserAccountViewModel()
        {
            UserNameLabel = $"Username: {UserName}";
            UserEmailLabel = $"Email: {UserEmail}";
            UserStatusLabel = $"Status: {UserStatus}";
            CountOfCorrectAnswersLabel = $"Count of correct answers: {CountOfCorrectAnswers}";
            ChangeUsername = new RelayCommand(o => CreateNewUsername());
            ChangePassword = new RelayCommand(o => CreateNewPassword());
        }

        private void CreateNewUsername()
        {
            if (string.IsNullOrEmpty(NewUsername))
            {
                CreateUserNotificationBox("You entered incorrect new username!", "Your username have to be between 5 and 10 characters.");
                return;
            }

            string sqlChangeUsername = $"UPDATE users SET username='{NewUsername}' WHERE username = '{UserName}'";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlChangeUsername, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                UserNameLabel = $"Username: {NewUsername}";
                UserName = NewUsername;
                NewUsername = "";
            }
            catch (Exception ex)
            {
                CreateUserNotificationBox("User with current username already exists!",
                    "Please, select other username.");
                Log.Error("Error while trying to change username: "+ex);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void CreateNewPassword()
        {
            if (string.IsNullOrEmpty(NewUserPassword))
            {
                CreateUserNotificationBox("You entered incorrect new password!",
                    "Your password has to be between 5 and 20 characters, contains uppercase and lowercase letter and digits.");
                return;
            }

            string sqlChangeUserPassword = $"UPDATE users SET user_password='{NewUserPassword}' WHERE username = '{UserName}'";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlChangeUserPassword, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                CreateUserNotificationBox("Something went wrong while tying to change password!", "Please, check your connection to internet!");
            }
            finally
            {
                sqlConnection.Close();
            }
        }

    }
}