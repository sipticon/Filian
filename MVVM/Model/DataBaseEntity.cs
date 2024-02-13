using System.IO;
using Filian.Core;

namespace Filian.MVVM.Model
{
    public abstract class DataBaseEntity : ObservableObject
    {
        private string picturePath;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }

        public string PicturePath
        {
            get { return picturePath;}
            set { picturePath = Path.GetFullPath(value); }
        }
    }
}