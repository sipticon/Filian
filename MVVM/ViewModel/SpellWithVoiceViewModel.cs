using System;
using System.Collections.Generic;
using System.Windows.Media;
using Filian.Core;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class SpellWithVoiceViewModel : ViewModel
    {        public RelayCommand PlayPronunciationCommand { get; set; }

        private static string _word;
        private static string _translation;
        private string _pronunciationPath;

        public Queue<Word> Words = SpellWithVoiceTest.Words;

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
            set
            {
                _word = value;
            }
        }

        public string Translation
        {
            get => _translation;
            set => _translation = value;
        }

        public string PronunciationPath
        {
            get => _pronunciationPath;
            set
            {
                _pronunciationPath = value;
                OnPropertyChanged();
            }
        }

        public SpellWithVoiceViewModel()
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
                        Log.Info("SpellWithVoiceTest successfully created.");
                        PlayPronunciationCommand = new RelayCommand(o => { PlayPronunciationForWord(currentWord.PronunciationPath); });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating SpellWithVoiceTest test: ", ex);
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