using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Filian.Core;
using System.Windows;
using System.Windows.Controls;
using Filian.MVVM.Model;
using Filian.MVVM.View;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Filian.MVVM.ViewModel
{
    public class MainViewModel :  ViewModel
    {
        public RelayCommand OpenLanguagesViewCommand { get; set; }
        public RelayCommand OpenTestsViewCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public LanguagesViewModel LanguagesVm { get; set; }
        public TestsViewModel TestsVm { get; set; }
        public ThemesViewModel ThemesVm { get; set; }
        public UnderThemesViewModel UnderThemesVm { get; set; }
        public WelcomeViewModel WelcomeVm { get; set; }
        public OneFromTwoViewModel OneFromTwoVm { get; set; }
        public OneFromFourViewModel OneFromFourVm { get; set; }
        public OneFromFourTextViewModel OneFromFourTextVm { get; set; }
        public OneFromFourListeningViewModel OneFromFourListeningVm { get; set; }
        public TrueOrFalseViewModel TrueOrFalseVm { get; set; }
        public SpellWithPictureViewModel SpellWithPictureVm { get; set; }
        public SpellWithVoiceViewModel SpellWithVoiceVm { get; set; }
        public TranslationTextViewModel TranslationTextVm { get; set; }
        public TranslationPronunciationViewModel TranslationPronunciationVm { get; set; }
        public FindPairTranslationViewModel FindPairTranslationVm { get; set; }

        public static Grid gr;

        private object _currentView;

        private static int _themeId;
        private int _testId;
        private int _countOfTests = 0;
        private int _countOfCorrectAnswers = 0;

        private bool _backButtonActive = true;
        private bool _navigatePanelButtonsActive = true;

        private bool isAnswerShown = false;

        private string _countOfTestsLabel;

        private static List<int> _underThemeIds = new List<int>();
        private Stack _previousViews;

        private Visibility _visibilityOfCountOfTestsLabel = Visibility.Hidden;

        public static readonly string _correctAnswerImagesource = @"C:\Users\oleksandrm\materials\Test_Icons\grey\Checkbox_Yes.png";
        public static readonly string _wrongAnswerImagesource = @"C:\Users\oleksandrm\materials\Test_Icons\grey\Checkbox_No.png";


        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public int ThemeId
        {
            get => _themeId;
            set => _themeId = value;
        }
        public static int LanguageId { get; set; } = 2;

        public string CountOfTestsLabel
        {
            get => _countOfTestsLabel;
            set
            {
                _countOfTestsLabel = value;
                OnPropertyChanged("CountOfTestsLabel");
            }
        }


        public List<int> UnderThemeIds
        {
            get => _underThemeIds;
            set => _underThemeIds = value;
        }

        public Visibility VisibilityOfCountOfTestsLabel
        {
            get => _visibilityOfCountOfTestsLabel;
            set
            {
                _visibilityOfCountOfTestsLabel = value;
                OnPropertyChanged("VisibilityOfCountOfTestsLabel");
            }
        }

        public MainViewModel()
        {
            Log.Info("Application started.");

            LanguagesVm = new LanguagesViewModel();
            WelcomeVm = new WelcomeViewModel();
            TestsVm = new TestsViewModel();

            _previousViews = new Stack();

            ChangeView(LanguagesVm);

            
            OpenLanguagesViewCommand = new RelayCommand(o => { NavigateToLanguagesView(); });
            OpenTestsViewCommand = new RelayCommand(o => { NavigateToTestsView(); });
            ApplyCommand = new RelayCommand(o => { Apply_Click(); });
            ExitCommand = new RelayCommand(o => { Exit_Click(); });
            BackCommand = new RelayCommand(o => { Back_Click(); });
        }

        private void Apply_Click()
        {
            if (CurrentView == LanguagesVm)
            {
                var languageId = LanguagesVm.SelectedLanguage;
                if (languageId != null)
                {
                    LanguageId = languageId.Id;
                    TestsVm = new TestsViewModel();
                    ChangeView(TestsVm);
                }
            }
            else if (CurrentView == TestsVm)
            {
                var test = TestsVm.SelectedTest;
                if (test != null)
                {
                    _testId = test.Id;
                    ThemesVm = new ThemesViewModel();
                    ChangeView(ThemesVm);
                }
            }
            else if (CurrentView == ThemesVm)
            {
                var theme = ThemesVm.SelectedTheme;
                if (theme != null)
                {
                    ThemeId = theme.Id;
                    UnderThemesVm = new UnderThemesViewModel();
                    ChangeView(UnderThemesVm);
                }
            }
            else if (CurrentView == UnderThemesVm)
            {
                var underThemes = UnderThemesVm.SelectedItems;
                if (underThemes != null)
                {
                    foreach (Theme underTheme in underThemes)
                    {
                        UnderThemeIds.Add(underTheme.Id);
                    }

                    switch (_testId)
                    {
                        case 1 :
                            OneFromTwoTest oneFromTwoTest = new OneFromTwoTest(UnderThemeIds);
                            OneFromTwoVm = new OneFromTwoViewModel();
                            _countOfTests = OneFromTwoVm.CountOfTests - 1;
                            ChangeView(OneFromTwoVm);
                            break;
                        case 2:
                            OneFromFourTest oneFromFourTest = new OneFromFourTest(UnderThemeIds);
                            OneFromFourVm = new OneFromFourViewModel();
                            _countOfTests = OneFromFourVm.CountOfTests - 1;
                            ChangeView(OneFromFourVm);
                            break;
                        case 3:
                            OneFromFourTextTest oneFromFourTextTest = new OneFromFourTextTest(UnderThemeIds);
                            OneFromFourTextVm = new OneFromFourTextViewModel();
                            _countOfTests = OneFromFourTextVm.CountOfTests - 1;
                            ChangeView(OneFromFourTextVm);
                            break;
                        case 4:
                            OneFromFourListeningTest oneFromFourListeningTest = new OneFromFourListeningTest(UnderThemeIds);
                            OneFromFourListeningVm = new OneFromFourListeningViewModel();
                            _countOfTests = OneFromFourListeningVm.CountOfTests - 1;
                            ChangeView(OneFromFourListeningVm);
                            break;
                        case 5:
                            TrueOrFalseTest trueOrFalseTest = new TrueOrFalseTest(UnderThemeIds);
                            TrueOrFalseVm = new TrueOrFalseViewModel();
                            _countOfTests = TrueOrFalseVm.CountOfTests - 1;
                            ChangeView(TrueOrFalseVm);
                            break;
                        case 6:
                            SpellWithPictureTest spellWithPictureTest = new SpellWithPictureTest(UnderThemeIds);
                            SpellWithPictureVm = new SpellWithPictureViewModel();
                            _countOfTests = SpellWithPictureVm.CountOfTests - 1;
                            ChangeView(SpellWithPictureVm);
                            break;
                        case 7:
                            SpellWithVoiceTest spellWithVoiceTest = new SpellWithVoiceTest(UnderThemeIds);
                            SpellWithVoiceVm = new SpellWithVoiceViewModel();
                            _countOfTests = SpellWithVoiceVm.CountOfTests - 1;
                            ChangeView(SpellWithVoiceVm);
                            break;
                        case 8:
                            TranslationTextTest translationTextTest = new TranslationTextTest(UnderThemeIds);
                            TranslationTextVm = new TranslationTextViewModel();
                            _countOfTests = TranslationTextVm.CountOfTests - 1;
                            ChangeView(TranslationTextVm);
                            break;
                        case 9:
                            TranslationPronunciationTest translationPronunciationTest = new TranslationPronunciationTest(UnderThemeIds);
                            TranslationPronunciationVm = new TranslationPronunciationViewModel();
                            _countOfTests = TranslationPronunciationVm.CountOfTests - 1;
                            ChangeView(TranslationPronunciationVm);
                            break;
                        case 10:
                            FindPairTranslationTest findPairTranslationTest = new FindPairTranslationTest(UnderThemeIds);
                            FindPairTranslationVm = new FindPairTranslationViewModel();
                            _countOfTests = FindPairTranslationVm.CountOfTests - 1;
                            ChangeView(FindPairTranslationVm);
                            break;
                    }
                    CountOfTestsLabel = $"Tests left: {_countOfTests}";
                    VisibilityOfCountOfTestsLabel = Visibility.Visible;
                    _backButtonActive = false;
                    _navigatePanelButtonsActive = false;
                }
            }
            else
            {
                if (_testId == 1)
                {
                    MoveToTheNextStep(OneFromTwoVm.Word, OneFromTwoViewModel.SelectedImage);
                }
                else if (_testId == 2)
                {
                    CheckResultOfChoice(OneFromFourVm.Word, OneFromFourViewModel.SelectedImage);
                }
                else if (_testId == 3)
                {
                    CheckResultOfChoice(OneFromFourTextViewModel.CorrectTranslation, OneFromFourTextViewModel.SelectedWord);
                }
                else if (_testId == 4)
                {
                    CheckResultOfChoice(OneFromFourListeningVm.Word, OneFromFourListeningViewModel.SelectedImage);
                }
                else if (_testId == 5)
                {
                    CheckResultOfChoice((TrueOrFalseVm.ShownTranslation == TrueOrFalseVm.CorrectTranslation).ToString(), TrueOrFalseViewModel.IsCurrentWordRight.ToString());
                }
                else if (_testId == 6)
                {
                    CheckResultOfChoice(SpellWithPictureVm.Translation.ToLower(), SpellWithPictureVm.Answer.ToLower());
                }
                else if (_testId == 7)
                {
                    CheckResultOfChoice(SpellWithVoiceVm.Translation.ToLower(), SpellWithVoiceVm.Answer.ToLower());
                }
                else if (_testId == 8)
                {
                    CheckResultOfChoice(TranslationTextVm.Word.ToLower(), TranslationTextVm.Answer.ToLower());
                }
                else if (_testId == 9)
                {
                    CheckResultOfChoice(TranslationPronunciationVm.Word.ToLower(), TranslationPronunciationVm.Answer.ToLower());
                }
                else if (_testId == 10)
                {
                    CheckResultOfChoice(FindPairTranslationVm.CorrectWord, FindPairTranslationViewModel.SelectedWord);
                }
            }
        }

        private void Exit_Click()
        {
            try
            {
                Application.Current.Shutdown();
                Log.Info("Application successfully closed.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while closing application",ex);
            }
        }

        private void Back_Click()
        {
            try
            {
                if (_backButtonActive && _previousViews.Count > 1)
                {
                    CurrentView = _previousViews.Pop();
                    Log.Info("Successfully returning to the previous page.");
                }
                else
                {
                    MessageBox.Show("Can't go back!");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while return to the previous page: ",ex);
            }
        }

        private void NavigateToLanguagesView()
        {
            if (_navigatePanelButtonsActive)
                ChangeView(LanguagesVm);
            else
                MessageBox.Show("You are in the test!");
        }

        private void NavigateToTestsView()
        {
            if (_navigatePanelButtonsActive)
                ChangeView(TestsVm);
            else
                MessageBox.Show("You are in the test!");
        }

        private void ChangeView(object newView)
        {
            try
            {
                if(_backButtonActive)
                    _previousViews.Push(CurrentView);
                CurrentView = newView;
                Log.Info($"View successfully changed to {newView}");
            }
            catch (Exception ex)
            {
                Log.Error("Exception while changing view: ",ex);
            }
        }

        private void CheckResultOfChoice(string correctAnswer, string selectedAnswer)
        {
            if (selectedAnswer == "")
                MessageBox.Show("Please, select answer!");
            else if (selectedAnswer.Contains(correctAnswer + ".") || selectedAnswer == correctAnswer)
            {
                _countOfCorrectAnswers++;
                Grid.Children.Add(CreateImage(_correctAnswerImagesource));
            }
            else
            {
                Grid.Children.Add(CreateImage(_wrongAnswerImagesource));
            }
            CountOfTestsLabel = $"Tests left: {_countOfTests}";
        }

        private void MoveToTheNextStep(string correctAnswer, string selectedAnswer)
        {
            if (_countOfTests == 1)
            {
                CheckResultOfChoice(correctAnswer, selectedAnswer);
                MessageBox.Show($"That's all! \n Count of correct answers - {_countOfCorrectAnswers}");
                EndOfTest();
            }
            else
            {
                if (!isAnswerShown)
                {
                    CheckResultOfChoice(correctAnswer, selectedAnswer);
                    isAnswerShown = true;
                }
                else
                    MoveToTheNextTest();
            }
        }
        private void MoveToTheNextTest()
        {
            object newView = null;
            switch (_testId)
            {
                case 1:
                    newView = new OneFromTwoView();
                    break;
                case 2:
                    newView = new OneFromFourView();
                    break;
                case 3:
                    newView = new OneFromFourTextView();
                    break;
                case 4:
                    newView = new OneFromFourListeningView();
                    break;
                case 5:
                    newView = new TrueOrFalseView();
                    break;
                case 6:
                    newView = new SpellWithPictureView();
                    break;
                case 7:
                    newView = new SpellWithVoiceView();
                    break;
                case 8:
                    newView = new TranslationTextView();
                    break;
                case 9:
                    newView = new TranslationPronunciationView();
                    break;
                case 10:
                    newView = new FindPairTranslationView();
                    break;
            }
            ChangeView(newView);
            _countOfTests--;
            isAnswerShown = false;
        }

        private Image CreateImage(string pathToImage)
        {
            Image myImage = new Image();
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(pathToImage);
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.Width = 100;
            myImage.Height = 100;
            Grid.SetColumn(myImage,Column);
            Grid.SetRow(myImage, Row);
            return myImage;
        }    

        private void EndOfTest()
        {
            Log.Info($"Test {CurrentView} successfully finished.");
            ChangeView(WelcomeVm);
            _previousViews.Clear();
            VisibilityOfCountOfTestsLabel = Visibility.Hidden;
            _countOfCorrectAnswers = 0;
            _countOfTests = 0;
            _backButtonActive = true;
            _navigatePanelButtonsActive = true;
        }
    }
}