using Filian.Core;

namespace Filian.MVVM.Model
{
    public class Word : ObservableObject
    {
        private int id;
        private string name;
        private string picture_path;
        private int theme_id;
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

        public int ThemeId
        {
            get => theme_id;
            set => theme_id = value;
        }

        public string Translation
        {
            get => translation;
            set => translation = value;
        }
    }
}