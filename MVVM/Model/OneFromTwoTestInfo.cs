namespace Filian.MVVM.Model
{
    public struct OneFromTwoTestInfo
    {
        public string word_Name;
        public string picture_Path1;
        public string picture_Path2;

        public OneFromTwoTestInfo(string word_Name, string picture_Path1, string picture_Path2)
        {
            this.word_Name = word_Name;
            this.picture_Path1 = picture_Path1;
            this.picture_Path2 = picture_Path2;
        }
    }
}