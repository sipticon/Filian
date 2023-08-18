using Filian.MVVM.Model;
using System;
using System.Collections.Generic;
using Filian.Core;

namespace Filian.MVVM.ViewModel
{
    public class OneFromFourTextViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);


        private readonly Random _random = new Random();

        private string _word1;
        private string _word2;
        private string _word3;
        private string _word4;

        private string _picturePath;

        private readonly Queue<OneFromFourTextTestInfo> _oneFromFourTextTestInfos = OneFromFourTextTest.OneFromFourTextTestInfos;

        private static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static string SelectedWord { get; set; } = "";

        public static string CorrectTranslation { get; set; } = "";

        public string Word1
        {
            get => _word1;
            set
            {
                _word1 = value;
                OnPropertyChanged();
            }
        }

        public string Word2
        {
            get => _word2;
            set
            {
                _word2 = value;
                OnPropertyChanged();
            }
        }

        public string Word3
        {
            get => _word3;
            set
            {
                _word3 = value;
                OnPropertyChanged();
            }
        }

        public string Word4
        {
            get => _word4;
            set
            {
                _word4 = value;
                OnPropertyChanged();
            }
        }

        public string PicturePath
        {
            get => _picturePath;
            set
            {
                _picturePath = value;
                OnPropertyChanged();
            }
        }

        public OneFromFourTextViewModel()
        {
            try
            {
                if (_oneFromFourTextTestInfos != null)
                {
                    CountOfTests = _oneFromFourTextTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        OneFromFourTextTestInfo oneFromFourTextTestInfo = _oneFromFourTextTestInfos.Dequeue();
                        PicturePath = oneFromFourTextTestInfo.PicturePath;
                        CorrectTranslation = oneFromFourTextTestInfo.WordTranslation1;
                        var words = new List<string> { oneFromFourTextTestInfo.WordTranslation1, oneFromFourTextTestInfo.WordTranslation2, oneFromFourTextTestInfo.WordTranslation3, oneFromFourTextTestInfo.WordTranslation4 };
                        GetRandomImage(words, ref _word1);
                        GetRandomImage(words, ref _word2);
                        GetRandomImage(words, ref _word3);
                        GetRandomImage(words, ref _word4);
                        Log.Info("OneFromFourTextTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating OneFromFourTextTest: ", ex);
            }
        }

        private void GetRandomImage(List<string> words, ref string word)
        {
            int index = _random.Next(0, words.Count);
            word = words[index];
            words.RemoveAt(index);
        }
    }
}