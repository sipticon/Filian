using Filian.Core;

namespace Filian.MVVM.Model
{
    public class Test : ObservableObject
    {
        private int id;
        private string name;
        private string image_path;
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

        public string ImagePath
        {
            get => image_path;
            set => image_path = value;
        }

        public string Translation
        {
            get => translation;
            set => translation = value;
        }
    }
}