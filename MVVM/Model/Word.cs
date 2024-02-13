using System.IO;

namespace Filian.MVVM.Model
{
    public class Word : DataBaseEntity
    {
        private string pronunciationPath;
        public int ThemeId { get; set; }
        public string PronunciationPath
    {
            get { return pronunciationPath; }
            set { pronunciationPath = Path.GetFullPath(value); }
        }
    }
}