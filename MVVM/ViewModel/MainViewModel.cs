using System.Collections.Generic;
using Filian.Core;
using System.Windows;
using Filian.MVVM.Model;
using Filian.MVVM.View;

namespace Filian.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand TestsViewCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public TestsViewModel TestsVm { get; set; }
        public ThemesViewModel ThemesVm { get; set; }
        public UnderThemesViewModel UnderThemesVm { get; set; }
        public WelcomeViewModel WelcomeVm { get; set; }
        public OneFromTwoViewModel OneFromTwoVm { get; set; }

        private object _currentView;
        private object _previousView;
        private static int _themeId;

        private static List<int> _underthemesId = new List<int>();
        private Visibility labelVisibility = Visibility.Hidden;

        private int countOfTests;
        private int countOfCorrectAnswers = 0;

        public int CountOfTests
        {
            get => countOfTests;
            set
            {
                countOfTests = value;
                OnPropertyChanged("CountOfTests");
            } 
        }

        public Visibility LabelVisibility
        {
            get => labelVisibility;
            set
            {
                labelVisibility = value;
                OnPropertyChanged("LabelVisibility");
            }
        }

        public List<int> UnderthemesId
        {
            get => _underthemesId;
            set => _underthemesId = value;
        }

        public int ThemeId
        {
            get => _themeId;
            set => _themeId = value;
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public object PreviousView
        {
            get => _previousView;
            set
            {
                _previousView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TestsVm = new TestsViewModel();
            WelcomeVm = new WelcomeViewModel();
            ThemesVm = new ThemesViewModel();

            CurrentView = WelcomeVm;

            TestsViewCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = TestsVm;
            });

            ApplyCommand = new RelayCommand(o => { Apply_Click(); });
            ExitCommand = new RelayCommand(o => { Exit_Click(); });
            BackCommand = new RelayCommand(o => { Back_Click(); });
        }

        private void Apply_Click()
        {
            if (CurrentView == TestsVm)
            {
                var test = TestsVm.SelectedTest;
                if (test != null)
                {
                    ViewChange(ThemesVm);
                }
            }
            else if (CurrentView == ThemesVm)
            {
                var theme = ThemesVm.SelectedTheme;
                if (theme != null)
                {
                    ThemeId = theme.Id;
                    UnderThemesVm = new UnderThemesViewModel();
                    ViewChange(UnderThemesVm);
                }
            }
            else if (CurrentView == UnderThemesVm)
            {
                var underThemes = UnderThemesVm.SelectedItems;
                if (underThemes != null)
                {
                    foreach (UnderTheme ut in underThemes)
                    {
                        UnderthemesId.Add(ut.Id);
                    }
                   
                    OneFromTwoTest oneFromTwoTest = new OneFromTwoTest();
                    OneFromTwoVm = new OneFromTwoViewModel();
                    CountOfTests = OneFromTwoVm.CountOftests - 1;
                    LabelVisibility = Visibility.Visible;
                    ViewChange(OneFromTwoVm);
                }
            }
            else
            {
                string currentWord = OneFromTwoVm.Word;
                string selectedImageAnswer = OneFromTwoViewModel.SelectedImage;
                if (CountOfTests == 1)
                {
                    if (selectedImageAnswer == "" || selectedImageAnswer == null)
                        MessageBox.Show("Please, select answer!");
                    else if (selectedImageAnswer.Contains(currentWord))
                    {
                        countOfCorrectAnswers++;
                        MessageBox.Show("Correct");
                    }
                    else
                        MessageBox.Show("Incorrect");

                    MessageBox.Show($"That's all! \n Count of correct answers - {countOfCorrectAnswers}");

                    EndOfTest();
                }
                else
                {
                    if (selectedImageAnswer == "" || selectedImageAnswer == null)
                        MessageBox.Show("Please, select answer!");
                    else if (selectedImageAnswer.Contains(currentWord))
                    {
                        countOfCorrectAnswers++;
                        CheckResultOfChoise("Correct");
                    }
                    else
                        CheckResultOfChoise("Incorrect");
                }
            }
        }

        private void Exit_Click()
        {
            Application.Current.Shutdown();
        }

        private void Back_Click()
        {
            if (PreviousView != null)
            {
                CurrentView = PreviousView;
                LabelVisibility = Visibility.Hidden;
            }
        }

        private void ViewChange(object newView)
        {
            PreviousView = CurrentView;
            CurrentView = newView;
        }

        private void CheckResultOfChoise(string messageResultOfSelection)
        {
            MessageBox.Show(messageResultOfSelection);
            OneFromTwoView OneFromTwoV = new OneFromTwoView();
            ViewChange(OneFromTwoV);
            CountOfTests--;
        }

        private void EndOfTest()
        {
            CurrentView = WelcomeVm;
            LabelVisibility = Visibility.Hidden;
            PreviousView = WelcomeVm;
            countOfCorrectAnswers = 0;
        }
    }
}