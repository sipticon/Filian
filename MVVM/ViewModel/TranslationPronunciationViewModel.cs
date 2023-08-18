using System;
using System.Collections.Generic;
using System.Windows.Media;
using Filian.Core;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class TranslationPronunciationViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand PlayPronunciationCommand { get; set; }

        private static string _word;

        public Queue<Word> Words = TranslationPronunciationTest.Words;

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

        public TranslationPronunciationViewModel()
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
                        Log.Info("TranslationPronunciationTest successfully created.");
                        PlayPronunciationCommand = new RelayCommand(o => { PlayPronunciationForWord(currentWord.PronunciationPath); });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating TranslationPronunciationTest test: ", ex);
            }
        }

        private void PlayPronunciationForWord(string pathOfPronunciation)
        {
            var uri = new Uri(pathOfPronunciation, UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }
    }
}