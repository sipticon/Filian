﻿using Filian.MVVM.Model;
using System;
using System.Collections.Generic;
using Filian.Core;

namespace Filian.MVVM.ViewModel
{
    public class FindPairTranslationViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly Random _random = new Random();

        private string _word1;
        private string _word2;
        private string _word3;
        private string _word4;

        private string _translation;
        private static string _correctWord;

        private readonly Queue<FindPairTranslationTestInfo> _findPairTranslationTestInfos = FindPairTranslationTest.FindPairTranslationTestInfos;

        private static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static string SelectedWord { get; set; } = "";

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

        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }

        public string CorrectWord
        {
            get => _correctWord;
            set => _correctWord = value;
        }


        public FindPairTranslationViewModel()
        {
            try
            {
                if (_findPairTranslationTestInfos != null)
                {
                    CountOfTests = _findPairTranslationTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        FindPairTranslationTestInfo findPairTranslationTestInfo = _findPairTranslationTestInfos.Dequeue();
                        Translation = findPairTranslationTestInfo.CorrectTranslation;
                        CorrectWord = findPairTranslationTestInfo.CorrectWord;
                        var words = new List<string> { findPairTranslationTestInfo.CorrectWord, findPairTranslationTestInfo.FalseWord1, findPairTranslationTestInfo.FalseWord2, findPairTranslationTestInfo.FalseWord3 };
                        GetRandomWord(words, ref _word1);
                        GetRandomWord(words, ref _word2);
                        GetRandomWord(words, ref _word3);
                        GetRandomWord(words, ref _word4);
                        Log.Info("FindPairTranslationTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating FindPairTranslationTest: ", ex);
            }
        }

        private void GetRandomWord(List<string> words, ref string word)
        {
            int index = _random.Next(0, words.Count);
            word = words[index];
            words.RemoveAt(index);
        }
    }
}