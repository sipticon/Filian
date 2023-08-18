using System;
using System.Collections.Generic;
using Filian.Core;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class TranslationTextViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private static string _word;
        private static string _translation;

        public Queue<Word> Words = TranslationTextTest.Words;

        private static string _answer = "";
        private static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public string Answer
        {
            get => _answer;
            set
            {
                _answer = value;
                OnPropertyChanged();
            }
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
        
        public TranslationTextViewModel()
        {
            try
            {
                if (Words != null)
                {
                    CountOfTests = Words.Count;
                    if (CountOfTests > 0)
                    {
                        Word currentWord = Words.Dequeue();
                        Word = currentWord.Name;
                        Translation = currentWord.Translation;
                        Log.Info("TranslationTextTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating TranslationTextTest test: ", ex);
            }
        }
    }
}