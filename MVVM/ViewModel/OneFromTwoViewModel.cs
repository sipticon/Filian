using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Filian.MVVM.ViewModel
{
    public class OneFromTwoViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly Random _random = new Random();

        private static string _word;
        private string _translation;
        private string _picturePath1;
        private string _picturePath2;

        private readonly Queue<OneFromTwoTestInfo> _oneFromTwoTestInfos = OneFromTwoTest.OneFromTwoTestInfos;

        private static string _selectedImage = "";
        private static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static string SelectedImage
        {
            get => _selectedImage;
            set => _selectedImage = value;
        }

        public string Word
        {
            get => _word;
            set => _word = value;
        }

        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath1
        {
            get => _picturePath1;
            set
            {
                _picturePath1 = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath2
        {
            get => _picturePath2;
            set
            {
                _picturePath2 = value;
                OnPropertyChanged();
            }
        }

        public OneFromTwoViewModel()
        {
            try
            {
                if (_oneFromTwoTestInfos != null)
                {
                    CountOfTests = _oneFromTwoTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        OneFromTwoTestInfo oneFromTwoTestInfo = _oneFromTwoTestInfos.Dequeue();
                        Word = oneFromTwoTestInfo.WordName;
                        Translation = oneFromTwoTestInfo.WordTranslation;
                        var paths = new List<string>
                            { oneFromTwoTestInfo.PicturePath1, oneFromTwoTestInfo.PicturePath2 };
                        int index = _random.Next(0, paths.Count);
                        PicturePath1 = paths[index];
                        paths.RemoveAt(index);
                        PicturePath2 = paths.FirstOrDefault();
                        Log.Info("OneFromTwoTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating OneFromTwoTest: ",ex);
            }
        }
    }
}