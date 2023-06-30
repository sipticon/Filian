namespace Filian.MVVM.Model
{
    public struct OneFromTwoStruct
    {
        public string wordName;
        public string picturePath1;
        public string picturePath2;

        public OneFromTwoStruct(string wordName, string picturePath1, string picturePath2)
        {
            this.wordName = wordName;
            this.picturePath1 = picturePath1;
            this.picturePath2 = picturePath2;
        }
    }
}