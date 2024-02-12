using System.IO;

namespace Filian.MVVM.Model
{
    public struct OneFromTwoTestInfo
    {
        public string WordName;
        public string WordTranslation;
        public string PicturePath1;
        public string PicturePath2;

        public OneFromTwoTestInfo(string wordName, string wordTranslation, string picturePath1, string picturePath2)
        {
            this.WordName = wordName;
            this.WordTranslation = wordTranslation;
            this.PicturePath1 = Path.GetFullPath(picturePath1);
            this.PicturePath2 = Path.GetFullPath(picturePath2);
        }
    }
}