using System.IO;
using System.Linq;
using System.Windows;
using Filian.Core;
using Filian.MVVM.View;

namespace Filian.MVVM.ViewModel
{
    public class UserNotificationViewModel : ViewModel
    {
        private static string notificationMessage;
        private static string notificationAdvice;

        private static string imagePath = Path.GetFullPath(@"Images\owl_exclamation.png");

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = Path.GetFullPath(value);
                OnPropertyChanged("ImagePath");
            }
        }

        public  string NotificationMessage
        {
            get => notificationMessage;
            set
            {
                notificationMessage = value;
                OnPropertyChanged("NotificationMessage");
            }
        }

        public string NotificationAdvice
        {
            get => notificationAdvice;
            set
            {
                notificationAdvice = value;
                OnPropertyChanged("NotificationAdvice");
            }
        }

        public RelayCommand CloseNotificationBoxCommand { get; set;}

        public UserNotificationViewModel()
        {
            CloseNotificationBoxCommand = new RelayCommand(o => CloseNotificationBox());
        }

        private void CloseNotificationBox()
        {
            Application.Current.Windows.OfType<UserNotificationView>().First().Close();
        }
    }
}