using System;
using System.Data.SqlClient;
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
                if(value.Length > 3)
                    newUsername = value;
                OnPropertyChanged();
            }
        }

        public static string NewUserPassword { get; set; }

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
            if(string.IsNullOrEmpty(NewUsername)) return;

            string sqlChangeUsername = $"UPDATE users SET username='{NewUsername}' WHERE username = '{UserName}'";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlChangeUsername, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                UserNameLabel = $"Username: {NewUsername}";
                NewUsername = "";
            }
            catch (Exception ex)
            {
                Log.Error("Error while trying to change username: "+ex);
            }
            finally
            {
                sqlConnection.Close();
            }
            
        }

        private void CreateNewPassword()
        {
            if (string.IsNullOrEmpty(NewUserPassword)) return;

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
                Log.Error("Error while trying to change user password: " + ex);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

    }
}
