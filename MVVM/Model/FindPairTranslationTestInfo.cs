namespace Filian.MVVM.Model
{
    public struct FindPairTranslationTestInfo
    {
        public string CorrectTranslation;
        public string CorrectWord;
        public string FalseWord1;
        public string FalseWord2;
        public string FalseWord3;

        public FindPairTranslationTestInfo(string correctTranslation, string correctWord, string falseWord1, string falseWord2, string falseWord3)
        {
            this.CorrectTranslation = correctTranslation;
            this.CorrectWord = correctWord;
            this.FalseWord1 = falseWord1;
            this.FalseWord2 = falseWord2;
            this.FalseWord3 = falseWord3;
        }
    }
}