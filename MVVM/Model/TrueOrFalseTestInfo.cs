namespace Filian.MVVM.Model
{
    public struct TrueOrFalseTestInfo
    {
        public string WordName;
        public string WordTranslation;
        public string PicturePath;
        public string FalseWord;

        public TrueOrFalseTestInfo(string wordName, string wordTranslation, string picturePath, string falseWord)
        {
            this.WordName = wordName;
            this.WordTranslation = wordTranslation;
            this.PicturePath = picturePath;
            this.FalseWord = falseWord;
        }
    }
}