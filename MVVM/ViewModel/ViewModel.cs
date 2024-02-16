using System;
using System.Windows.Controls;
using Filian.Core;
using Filian.MVVM.View;

namespace Filian.MVVM.ViewModel
{
    public class ViewModel : ObservableObject
    {
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public readonly string sqlConnectionString = $"Data Source=filian-database.c5ce82k0wnfb.us-west-2.rds.amazonaws.com,1433;Initial Catalog=filian_db;{CredentialsDecryptor.DecryptCredentials()}";

        public readonly Random _random = new Random();

        protected static string _answer = "";

        protected static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static Grid Grid
        {
            get;
            set;
        }

        public static int Column { get; set; }
        public static int Row { get; set; }

        public void CreateUserNotificationBox(string notification, string advice)
        {
            UserNotificationViewModel userNotificationViewModel = new UserNotificationViewModel
            {
             NotificationMessage = notification,
             NotificationAdvice = advice
            };
            UserNotificationView userNotificationView = new UserNotificationView();
            userNotificationView.Show();
        }
    }
}