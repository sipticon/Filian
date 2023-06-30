using Filian.Core;

namespace Filian.MVVM.Model
{
    public class Theme : ObservableObject
    {
        private int id;
        private string name;
        private string picture_path;
        private string translation;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string PicturePath
        {
            get => picture_path;
            set => picture_path = value;
        }

        public string Translation
        {
            get => translation;
            set => translation = value;
        }
    }
}