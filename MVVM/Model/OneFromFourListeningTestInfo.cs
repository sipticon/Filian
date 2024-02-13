using System.IO;

namespace Filian.MVVM.Model
{
    public struct OneFromFourListeningTestInfo
    {
        public string WordName;
        public string WordPronunciation;
        public string PicturePath1;
        public string PicturePath2;
        public string PicturePath3;
        public string PicturePath4;

        public OneFromFourListeningTestInfo(string wordName, string wordPronunciation, string picturePath1, string picturePath2, string picturePath3, string picturePath4)
        {
            this.WordName = wordName;
            this.WordPronunciation = wordPronunciation;
            this.PicturePath1 = Path.GetFullPath(picturePath1);
            this.PicturePath2 = Path.GetFullPath(picturePath2);
            this.PicturePath3 = Path.GetFullPath(picturePath3);
            this.PicturePath4 = Path.GetFullPath(picturePath4);
        }
    }
}