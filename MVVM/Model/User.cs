using System.ComponentModel.DataAnnotations;

namespace Filian.MVVM.Model
{
    public class User
    {
        private int userId;
        private string userPassword;
        private string userName;
        private string userEmail;
        
        private static int userIdIncrement = 1;
        public int UserId
        {
            get => userId;
            set
            {
                if(value > 0)
                    userId = value;
            }
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
            set => UserEmail = value;
        }

        public User()
        {
            this.userName = "test" + userIdIncrement;
            this.userEmail = $"usertest{userIdIncrement}@gmail.com";
            this.userPassword = "filiantestiser" + userIdIncrement;
            userIdIncrement++;
        }
        public User(string userName, string userEmail, string userPassword)
        {
            this.userName = userName;
            this.userEmail = userEmail;
            this.userPassword = userPassword;
        }
    }
}
