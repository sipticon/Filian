using System;
using System.Collections;
using System.Collections.Generic;
using Filian.Core;
using System.Windows;
using System.Windows.Controls;
using Filian.MVVM.Model;
using Filian.MVVM.View;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.Linq;
using Application = System.Windows.Application;
using System.Data.SqlClient;
using System.IO;

namespace Filian.MVVM.ViewModel
{
    public class MainViewModel :  ViewModel, IDisposable
    {
        public RelayCommand OpenLanguagesViewCommand { get; set; }
        public RelayCommand OpenTestsViewCommand { get; set; }
        public RelayCommand OpenUserAccountViewCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }

        public LanguagesViewModel LanguagesVm { get; set; }
        public TestsViewModel TestsVm { get; set; }
        public ThemesViewModel ThemesVm { get; set; }
        public UnderThemesViewModel UnderThemesVm { get; set; }
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
        public UserAccountViewModel UserAccountVm { get; set; }

        public UserAuthorizationView UserAuthorizationView { get; set; }

        private object _currentView;

        private static int _themeId;
        private int _testId;
        private new int _countOfTests = 0;
        private int _countOfCorrectAnswers = 0;

        private bool _backButtonActive = true;
        private bool _navigatePanelButtonsActive = true;

        private bool _isAnswerShown = false;

        private string _pageHeader = "Select language to test";
        private string _textOnMainButton = "Apply";

        private static List<int> _underThemeIds = new List<int>();
        private readonly Stack _previousViews;
        
        public static readonly string _correctAnswerImagesource = Path.GetFullPath(@"FilianFiles\Test_Icons\grey\Checkbox_Yes.png");
        public static readonly string _wrongAnswerImagesource = Path.GetFullPath(@"FilianFiles\Test_Icons\grey\Checkbox_No.png");

        private string backButtonImagePath = Path.GetFullPath(@"Images\back.png");
        private string closeButtonImagePath = Path.GetFullPath(@"Images\close.png");

        public string BackButtonImagePath
        {
            get => backButtonImagePath;
            set
            {
                backButtonImagePath = Path.GetFullPath(value);
                OnPropertyChanged("BackButtonImagePath");
            }
        }

        public string CloseButtonImagePath
        {
            get => closeButtonImagePath;
            set
            {
                closeButtonImagePath = Path.GetFullPath(value);
                OnPropertyChanged("CloseButtonImagePath");
            }
        }

        public static string userName;
        public static string userEmail;
        public static string userStatus;
        public static string countOfCorrectAnswers;

        private int _userCountOfCorrectAnswers;

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

        public string PageHeader
        {
            get => _pageHeader;
            set
            {
                _pageHeader = value;
                OnPropertyChanged("PageHeader");
            }
        }

        public string TextOnMainButton
        {
            get => _textOnMainButton;
            set
            {
                _textOnMainButton = value;
                OnPropertyChanged("TextOnMainButton");
            }
        }

        public List<int> UnderThemeIds
        {
            get => _underThemeIds;
            set => _underThemeIds = value;
        }
        
        public MainViewModel()
        {
            Log.Info("Application started.");

            LanguagesVm = new LanguagesViewModel();
            TestsVm = new TestsViewModel();
            
            _userCountOfCorrectAnswers = Convert.ToInt32(countOfCorrectAnswers);

            _previousViews = new Stack();

            ChangeView(LanguagesVm);

            OpenLanguagesViewCommand = new RelayCommand(o => { NavigateToLanguagesView(); });
            OpenTestsViewCommand = new RelayCommand(o => { NavigateToTestsView(); });
            OpenUserAccountViewCommand = new RelayCommand(o => { NavigateToUserAccountView(); });
            ApplyCommand = new RelayCommand(o => { Apply_Click(); });
            ExitCommand = new RelayCommand(o => { Exit_Click(); });
            BackCommand = new RelayCommand(o => { Back_Click(); });
            SignOutCommand = new RelayCommand(o => { SignOut(); });
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
                    PageHeader = "Select test type";
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
                    PageHeader = "Select theme to test";
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
                    PageHeader = "Select underthemes to test";
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
                    PageHeader = $"Tests left: {_countOfTests}";
                    _backButtonActive = false;
                    _navigatePanelButtonsActive = false;
                    TextOnMainButton = "Check";
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
                    MoveToTheNextStep(OneFromFourVm.Word, OneFromFourViewModel.SelectedImage);
                }
                else if (_testId == 3)
                {
                    MoveToTheNextStep(OneFromFourTextViewModel.CorrectTranslation, OneFromFourTextViewModel.SelectedWord);
                }
                else if (_testId == 4)
                {
                    MoveToTheNextStep(OneFromFourListeningVm.Word, OneFromFourListeningViewModel.SelectedImage);
                }
                else if (_testId == 5)
                {
                    MoveToTheNextStep((TrueOrFalseVm.ShownTranslation == TrueOrFalseVm.CorrectTranslation).ToString(), TrueOrFalseViewModel.IsCurrentWordRight.ToString());
                }
                else if (_testId == 6)
                {
                    MoveToTheNextStep(SpellWithPictureVm.Translation.ToLower(), SpellWithPictureVm.Answer.ToLower());
                }
                else if (_testId == 7)
                {
                    MoveToTheNextStep(SpellWithVoiceVm.Translation.ToLower(), SpellWithVoiceVm.Answer.ToLower());
                }
                else if (_testId == 8)
                {
                    MoveToTheNextStep(TranslationTextVm.Word.ToLower(), TranslationTextVm.Answer.ToLower());
                }
                else if (_testId == 9)
                {
                    MoveToTheNextStep(TranslationPronunciationVm.Word.ToLower(), TranslationPronunciationVm.Answer.ToLower());
                }
                else if (_testId == 10)
                {
                    MoveToTheNextStep(FindPairTranslationVm.CorrectWord, FindPairTranslationViewModel.SelectedWord);
                }
            }
        }

        private void Exit_Click()
        {
           Dispose();
           Application.Current.Shutdown();
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
                    CreateUserNotificationBox("Can't go back!", "You can't navigate to back!");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed while return to the previous page: ",ex);
            }
        }

        private void SignOut()
        {
            if (_navigatePanelButtonsActive)
            {
                UpdateUserInfo();
                UserAuthorizationView = new UserAuthorizationView();
                UserAuthorizationView.Show();
                Application.Current.Windows.OfType<MainWindow>().First().Close();
            }
            else
                CreateUserNotificationBox("You are in the test!", "You can't sign out while test is running!");
        }

        private void NavigateToLanguagesView()
        {
            if (_navigatePanelButtonsActive)
            {
                ChangeView(LanguagesVm);
                PageHeader = "Select language to test";
            }
            else
                CreateUserNotificationBox("You are in the test!", "You can't navigate to other tabs while test is running!");
        }

        private void NavigateToTestsView()
        {
            if (_navigatePanelButtonsActive)
            {
                ChangeView(TestsVm);
                PageHeader = "Select test type";
            }
            else
                CreateUserNotificationBox("You are in the test!", "You can't navigate to other tabs while test is running!");
        }

        private void NavigateToUserAccountView()
        {
            if (_navigatePanelButtonsActive)
            {
                UserAccountVm = new UserAccountViewModel()
                {
                    UserName = userName,
                    UserEmail = userEmail,
                    UserStatus = userStatus,
                    CountOfCorrectAnswers = _userCountOfCorrectAnswers.ToString()
                };
                ChangeView(UserAccountVm);
                PageHeader = "Account profile";
            }
            else
                CreateUserNotificationBox("You are in the test!", "You can't navigate to other tabs while test is running!");
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
            if (selectedAnswer.Contains(correctAnswer + ".") || selectedAnswer == correctAnswer)
            {
                _countOfCorrectAnswers++;
                Grid.Children.Add(CreateImage(_correctAnswerImagesource));
            }
            else
                Grid.Children.Add(CreateImage(_wrongAnswerImagesource));
        }

        private void MoveToTheNextStep(string correctAnswer, string selectedAnswer)
        {
            if (String.IsNullOrEmpty(selectedAnswer))
                CreateUserNotificationBox("You didn't select the answer!", "Please, select answer!");
            else if (_countOfTests == 1)
            {
                CheckResultOfChoice(correctAnswer, selectedAnswer);
                CreateUserNotificationBox("The test is completed!", $"Count of correct answers - {_countOfCorrectAnswers}");
                EndOfTest();
            }
            else
            {
                if (!_isAnswerShown)
                {
                    CheckResultOfChoice(correctAnswer, selectedAnswer);
                    _isAnswerShown = true;
                    TextOnMainButton = "Next";
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
                    OneFromTwoViewModel.SelectedImage = "";
                    break;
                case 2:
                    newView = new OneFromFourView();
                    OneFromFourViewModel.SelectedImage = "";
                    break;
                case 3:
                    newView = new OneFromFourTextView();
                    OneFromFourTextViewModel.SelectedWord = "";
                    break;
                case 4:
                    newView = new OneFromFourListeningView();
                    OneFromFourListeningViewModel.SelectedImage = "";
                    break;
                case 5:
                    newView = new TrueOrFalseView();
                    TrueOrFalseViewModel.IsCurrentWordRight = null;
                    break;
                case 6:
                    newView = new SpellWithPictureView();
                    SpellWithPictureVm.Answer = "";
                    break;
                case 7:
                    newView = new SpellWithVoiceView();
                    SpellWithVoiceVm.Answer = "";
                    break;
                case 8:
                    newView = new TranslationTextView();
                    TranslationTextVm.Answer = "";
                    break;
                case 9:
                    newView = new TranslationPronunciationView();
                    TranslationPronunciationVm.Answer = "";
                    break;
                case 10:
                    newView = new FindPairTranslationView();
                    FindPairTranslationViewModel.SelectedWord = "";
                    break;
            }
            _countOfTests--;
            PageHeader = $"Tests left: {_countOfTests}";
            ChangeView(newView);
            _isAnswerShown = false;
            TextOnMainButton = "Check";
        }

        private Image CreateImage(string pathToImage)
        {
            Image myImage = new Image();
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(Path.GetFullPath(pathToImage));
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.Width = 70;
            myImage.Height = 70;
            Grid.SetColumn(myImage,Column);
            Grid.SetRow(myImage, Row);
            return myImage;
        }    

        private void EndOfTest()
        {
            Log.Info($"Test {CurrentView} successfully finished.");
            ChangeView(TestsVm);
            _previousViews.Clear();
            _userCountOfCorrectAnswers += _countOfCorrectAnswers;
            _countOfCorrectAnswers = 0;
            _countOfTests = 0;
            _backButtonActive = true;
            _navigatePanelButtonsActive = true;
            UnderThemeIds = new List<int>();
            TextOnMainButton = "Apply";
            PageHeader = "Select test type";
        }

        public void UpdateUserInfo()
        {
            string newUserStatus = "";
            string sqlChangeUserInfo = "";
            if (_userCountOfCorrectAnswers >= 200 && _countOfCorrectAnswers < 500)
                newUserStatus = "Owl";
            else if (_userCountOfCorrectAnswers >= 500 && _countOfCorrectAnswers < 1000)
                newUserStatus = "Old owl";
            else if (_userCountOfCorrectAnswers >= 1000)
                newUserStatus = "Legend owl";
            else
                newUserStatus = "";

            sqlChangeUserInfo = !string.IsNullOrEmpty(newUserStatus) 
                ? $"UPDATE users SET correct_answers='{_userCountOfCorrectAnswers}', user_status='{newUserStatus}' WHERE username = '{userName}'" 
                : $"UPDATE users SET correct_answers='{_userCountOfCorrectAnswers}' WHERE username = '{userName}'";

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlChangeUserInfo, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error("Error while trying to change username: " + ex);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void Dispose()
        {
            try
            {
                UpdateUserInfo();
                Log.Info("Application successfully closed.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while closing application", ex);
            }
        }
    }
}