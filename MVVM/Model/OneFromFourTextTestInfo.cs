namespace Filian.MVVM.Model
{
    public struct OneFromFourTextTestInfo
    {
        public string WordTranslation1;
        public string WordTranslation2;
        public string WordTranslation3;
        public string WordTranslation4;
        public string PicturePath;

        public OneFromFourTextTestInfo(string wordTranslation1, string wordTranslation2, string wordTranslation3, string wordTranslation4, string picturePath)
        {
            this.WordTranslation1 = wordTranslation1;
            this.WordTranslation2 = wordTranslation2;
            this.WordTranslation3 = wordTranslation3;
            this.WordTranslation4 = wordTranslation4;
            this.PicturePath = picturePath;
        }
    }
}