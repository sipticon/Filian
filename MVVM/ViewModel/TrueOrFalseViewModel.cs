using Filian.MVVM.Model;
using System;
using System.Collections.Generic;
using Filian.Core;

namespace Filian.MVVM.ViewModel
{
    public class TrueOrFalseViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly Random _random = new Random();

        private static string _shownTranslation;
        private static string _correctTranslation;
        private string _picturePath;

        private readonly Queue<TrueOrFalseTestInfo> _trueOrFalseTestInfos = TrueOrFalseTest.TrueOrFalseTestInfos;

        private static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static Nullable<bool> IsCurrentWordRight { get; set; }

        public string CorrectTranslation
        {
            get => _correctTranslation;
            set => _correctTranslation = value;
        }

        public string ShownTranslation
        {
            get => _shownTranslation;
            set
            {
                _shownTranslation = value;
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

        public TrueOrFalseViewModel()
        {
            try
            {
                if (_trueOrFalseTestInfos != null)
                {
                    CountOfTests = _trueOrFalseTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        TrueOrFalseTestInfo trueOrFalseTestInfo = _trueOrFalseTestInfos.Dequeue();
                        string[] correctAndFalseWords = { trueOrFalseTestInfo.WordTranslation, trueOrFalseTestInfo.FalseWord };
                        _shownTranslation = GetRandomWord(correctAndFalseWords);
                        PicturePath = trueOrFalseTestInfo.PicturePath;
                        _correctTranslation = trueOrFalseTestInfo.WordTranslation;
                        Log.Info("TrueOrFalseTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating TrueOrFalseTest: ", ex);
            }
        }

        private string GetRandomWord(string[] words)
        {
            int index = _random.Next(0, words.Length);
            return words[index];
        }
    }
}