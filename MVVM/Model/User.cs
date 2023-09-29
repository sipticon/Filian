using System.ComponentModel.DataAnnotations;

namespace Filian.MVVM.Model
{
    public class User
    {
        private int userId;
        private string userPassword;
        private string userName;
        private string userEmail;
        private string userStatus;
        private int countOfCorrectAnswers;

        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        public string UserPassword
        {
            get => userPassword;
            set => userPassword = value;
        }

        public string UserName
        {
            get => userName;
            set => userName = value;
        }

        public string UserEmail
        {
            get => userEmail;
            set => userEmail = value;
        }

        public string UserStatus
        {
            get => userStatus;
            set => userStatus = value;
        }

        public int CountOfCorrectAnswers
        {
            get => countOfCorrectAnswers;
            set => countOfCorrectAnswers = value;
        }

        public User()
        {
        }
        public User(string userName, string userEmail, string userStatus, string userPassword)
        {
            this.userName = userName;
            this.userEmail = userEmail;
            this.userStatus = userStatus;
            this.userPassword = userPassword;
        }

        public User(string userName, string userPassword)
        {
            this.userName = userName;
            this.userPassword = userPassword;
        }
    }
}
