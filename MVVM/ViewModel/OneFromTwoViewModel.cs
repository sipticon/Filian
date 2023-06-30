using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Filian.MVVM.ViewModel
{
    public class OneFromTwoViewModel : ObservableObject
    {
        private OneFromTwoStruct oneFromTwoStruct;
        private Random random = new Random();
        private static string word;
        private string picture_path_1;
        private string picture_path_2;
        private Queue<OneFromTwoStruct> queueOfStructs = OneFromTwoTest.OneFromTwoStructs;
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
            get => picture_path_1;
            set
            {
                picture_path_1 = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath2
        {
            get => picture_path_2;
            set
            {
                picture_path_2 = value;
                OnPropertyChanged();
            }
        }

        public OneFromTwoViewModel()
        {
            if (queueOfStructs != null)
            {
                CountOftests = queueOfStructs.Count;
                if (CountOftests > 0)
                {
                    oneFromTwoStruct = queueOfStructs.Dequeue();
                    Word = oneFromTwoStruct.wordName;
                    var paths = new List<string> { oneFromTwoStruct.picturePath1, oneFromTwoStruct.picturePath2 };
                    int index = random.Next(0, paths.Count);
                    PicturePath1 = paths[index];
                    paths.RemoveAt(index);
                    PicturePath2 = paths.FirstOrDefault();
                }
            }
        }
    }
}