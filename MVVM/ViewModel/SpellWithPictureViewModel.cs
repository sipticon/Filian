using System;
using System.Collections.Generic;
using Filian.Core;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class SpellWithPictureViewModel : ViewModel
    {
        private static string _word;
        private static string _translation;
        private string _picturePath;

        public Queue<Word> Words = SpellWithPictureTest.Words;

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
            set => _translation = value;
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


        public SpellWithPictureViewModel()
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
                        PicturePath = currentWord.PicturePath;
                        Log.Info("SpellWithPictureTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating SpellWithPictureTest: ", ex);
            }
        }
    }
}