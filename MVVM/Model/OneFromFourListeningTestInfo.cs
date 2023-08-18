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
            this.PicturePath1 = picturePath1;
            this.PicturePath2 = picturePath2;
            this.PicturePath3 = picturePath3;
            this.PicturePath4 = picturePath4;
        }
    }
}
