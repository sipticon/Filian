using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Filian.MVVM.ViewModel
{
    public class OneFromTwoViewModel : ObservableObject
    {
        private OneFromTwoTestInfo oneFromTwoTestInfo;
        private Random random = new Random();
        private static string word;
        private string picture_Path1;
        private string picture_Path2;
        private Queue<OneFromTwoTestInfo> oneFromTwoTestInfos = OneFromTwoTest.OneFromTwoTestInfos;
        private static string selectedImage = "";
        private static int countOftests;

        public int CountOftests
        {
            get => countOftests;
            set => countOftests = value;
        }

        public static string SelectedImage
        {
            get => selectedImage;
            set
            {
                selectedImage = value;
            }
        }

        public string Word
        {
            get => word;
            set
            { 
                word = value;
                OnPropertyChanged();
            } 
        }

        public string PicturePath1
        {
            get => picture_Path1;
            set
            {
                picture_Path1 = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath2
        {
            get => picture_Path2;
            set
            {
                picture_Path2 = value;
                OnPropertyChanged();
            }
        }

        public OneFromTwoViewModel()
        {
            if (oneFromTwoTestInfos != null)
            {
                CountOftests = oneFromTwoTestInfos.Count;
                if (CountOftests > 0)
                {
                    oneFromTwoTestInfo = oneFromTwoTestInfos.Dequeue();
                    Word = oneFromTwoTestInfo.word_Name;
                    var paths = new List<string> { oneFromTwoTestInfo.picture_Path1, oneFromTwoTestInfo.picture_Path2 };
                    int index = random.Next(0, paths.Count);
                    PicturePath1 = paths[index];
                    paths.RemoveAt(index);
                    PicturePath2 = paths.FirstOrDefault();
                }
            }
        }
    }
}