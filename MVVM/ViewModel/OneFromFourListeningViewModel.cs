using System;
using System.Collections.Generic;
using System.Windows.Media;
using Filian.Core;
using Filian.MVVM.Model;

namespace Filian.MVVM.ViewModel
{
    public class OneFromFourListeningViewModel : ViewModel
    {
        public RelayCommand PlayPronunciationCommand { get; set; }
        
        private static string _word;
        private static string _pronunciationPath;
        private string _picturePath1;
        private string _picturePath2;
        private string _picturePath3;
        private string _picturePath4;

        private readonly Queue<OneFromFourListeningTestInfo> _oneFromFourListeningTestInfos = OneFromFourListeningTest.OneFromFourListeningTestInfos;

        public string Word
        {
            get => _word;
            set => _word = value;
        }

        public static string SelectedImage { get; set; }

        public string PronunciationPath
        {
            get => _pronunciationPath;
            set
            {
                _pronunciationPath = value;
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

        public OneFromFourListeningViewModel()
        {
            try
            {
                if (_oneFromFourListeningTestInfos != null)
                {
                    CountOfTests = _oneFromFourListeningTestInfos.Count;
                    if (CountOfTests > 0)
                    {
                        OneFromFourListeningTestInfo oneFromFourTextTestInfo = _oneFromFourListeningTestInfos.Dequeue();
                        Word = oneFromFourTextTestInfo.WordName;
                        var paths = new List<string> { oneFromFourTextTestInfo.PicturePath1, oneFromFourTextTestInfo.PicturePath2, oneFromFourTextTestInfo.PicturePath3, oneFromFourTextTestInfo.PicturePath4 };
                        GetRandomImage(paths, ref _picturePath1);
                        GetRandomImage(paths, ref _picturePath2);
                        GetRandomImage(paths, ref _picturePath3);
                        GetRandomImage(paths, ref _picturePath4);
                        PlayPronunciationCommand = new RelayCommand(o => { PlayPronunciationForWord(oneFromFourTextTestInfo.WordPronunciation); });
                        Log.Info("OneFromFourListeningTest successfully created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while creating OneFromFourListeningTest: ", ex);
            }
        }

        private void GetRandomImage(List<string> paths, ref string picturePath)
        {
            int index = _random.Next(0, paths.Count);
            picturePath = paths[index];
            paths.RemoveAt(index);
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