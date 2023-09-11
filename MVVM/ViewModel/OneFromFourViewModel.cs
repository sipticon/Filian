using System;
using System.Collections.Generic;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class OneFromFourViewModel : ViewModel
    {
        private static string _word;
        private static string _translation;
        private string _picturePath1;
        private string _picturePath2;
        private string _picturePath3;
        private string _picturePath4;

        private readonly Queue<OneFromFourTestInfo> _oneFromFourTestInfos = OneFromFourTest.OneFromFourTestInfos;

        public string Word
        {
            get => _word;
            set => _word = value;
        }

        public static string SelectedImage { get; set; }

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

        public string PicturePath3
        {
            get => _picturePath3;
            set
            {
                _picturePath3 = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath4
        {
            get => _picturePath4;
            set
            {
                _picturePath4 = value;
                OnPropertyChanged();
            }
        }

        public OneFromFourViewModel()
        {
            try
            {
                if (_oneFromFourTestInfos != null)
                {
                    CountOfTests = _oneFromFourTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        OneFromFourTestInfo oneFromFourTestInfo = _oneFromFourTestInfos.Dequeue();
                        Word = oneFromFourTestInfo.WordName;
                        _translation = oneFromFourTestInfo.WordTranslation;
                        var paths = new List<string> { oneFromFourTestInfo.PicturePath1, oneFromFourTestInfo.PicturePath2, oneFromFourTestInfo.PicturePath3, oneFromFourTestInfo.PicturePath4 };
                        GetRandomImage(paths, ref _picturePath1);
                        GetRandomImage(paths, ref _picturePath2);
                        GetRandomImage(paths, ref _picturePath3);
                        GetRandomImage(paths, ref _picturePath4);
                        Log.Info("OneFromFourTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating OneFromFourTest: ", ex);
            }
        }

        private void GetRandomImage(List<string> paths, ref string picturePath)
        {
            int index = _random.Next(0, paths.Count);
            picturePath = paths[index];
            paths.RemoveAt(index);
        }
    }
}