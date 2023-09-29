using System;
using System.Data.SqlClient;
using System.Linq;
using Filian.Core;
using Filian.MVVM.Model;
using Filian.MVVM.View;
namespace Filian.MVVM.ViewModel
{
    public class UserAuthorizationViewModel : ViewModel
    {
        private string userName;
        private static string userPassword;
        public static bool IsDataCorrect;
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

        public static string UserPassword
        {
            get => userPassword;
            set => userPassword = value;
        }

        public RelayCommand OpenSingUpViewCommand { get; set; }
        public RelayCommand SignIn { get; set; }
        public MainWindow MainWindow { get; set; }
        public UserRegistrationView UserRegistrationView { get; set; }

        public UserAuthorizationViewModel()
        {
           SignIn = new RelayCommand(o=> LogIn());
           OpenSingUpViewCommand = new RelayCommand(o=> OpenUserRegistrationView());
        }

        private void LogIn()
        {
            User logInUser = new User(userName, userPassword);

            string sqlGetUser = $"SELECT * FROM users WHERE username = '{logInUser.UserName}'";

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlGetUser, sqlConnection);

                User gotUser = new User();

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    gotUser.UserName = sqlDataReader.GetString(1);
                    gotUser.UserEmail = sqlDataReader.GetString(2);
                    gotUser.UserStatus = sqlDataReader.GetString(3);
                    gotUser.CountOfCorrectAnswers = sqlDataReader.GetInt32(4);
                    gotUser.UserPassword = sqlDataReader.GetString(5);
                }

                if (gotUser.UserPassword == logInUser.UserPassword)
                {
                    IsDataCorrect = true;
                    SetAccountData(gotUser);
                    OpenMainWindow();
                }
                
                Log.Info("Successfully sign in.");

            }
            catch (Exception ex)
            {
                IsDataCorrect = false;
                Log.Error("Failed while trying to sign in: ", ex);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void OpenMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
            System.Windows.Application.Current.Windows.OfType<UserAuthorizationView>().First().Close();

        }

        private void OpenUserRegistrationView()
        {
            UserRegistrationView = new UserRegistrationView();
            UserRegistrationView.Show();
            System.Windows.Application.Current.Windows.OfType<UserAuthorizationView>().First().Close();
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
