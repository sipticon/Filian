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
            if (string.IsNullOrEmpty(userName))
            {
                CreateUserNotificationBox("You entered incorrect username!", "Your username have to be between 2 and 20 characters.");
                return;
            }
            if (string.IsNullOrEmpty(userPassword))
            {
                CreateUserNotificationBox("You entered incorrect password!",
                    "Your password has to be between 5 and 25 characters, contains uppercase and lowercase letter and digits.");
                return;
            }

            User logInUser = new User(userName, userPassword);

            string sqlGetUser = $"SELECT * FROM users WHERE username = '{logInUser.UserName}'";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(sqlGetUser, sqlConnection);

                User gotUser = new User();

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (!sqlDataReader.HasRows)
                {
                    CreateUserNotificationBox("Current user does not exist!", "Please, check your credentials or create account.");
                    return;
                }

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
                    SetAccountData(gotUser);
                    OpenMainWindow();
                }
                else
                {
                    CreateUserNotificationBox("You entered incorrect password!",
                        "Please, check your credentials.");
                    return;
                }
                
                Log.Info("Successfully sign in.");

            }
            catch (Exception ex)
            {
                
                Log.Error("Failed while trying to sign in: ", ex);
                CreateUserNotificationBox("Something went wrong while tying to sign in!", "Please, check your credentials and connection to internet!");
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