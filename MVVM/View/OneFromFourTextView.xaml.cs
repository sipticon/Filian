using Filian.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Filian.MVVM.View
{
    public partial class OneFromFourTextView : UserControl
    {
        public OneFromFourTextView()
        {
            InitializeComponent();
        }
        private bool _isSelected1;
        private bool _isSelected2;
        private bool _isSelected3;
        private bool _isSelected4;

        private void Select1(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected2 && !_isSelected3 && !_isSelected4)
                SelectAndMarkWord(sender, ref _isSelected1);
        }

        private void Select2(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected3 && !_isSelected4)
                SelectAndMarkWord(sender, ref _isSelected2);
        }

        private void Select3(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected2 && !_isSelected4)
                SelectAndMarkWord(sender, ref _isSelected3);
        }
        private void Select4(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected2 && !_isSelected3)
                SelectAndMarkWord(sender, ref _isSelected4);
        }

        private void SelectAndMarkWord(object sender, ref bool isSelected)
        {
            Label selectedWord = (Label)sender;
            if (!isSelected)
            {
                selectedWord.Opacity = 0.6;
                OneFromFourTextViewModel.SelectedWord = ((Label)sender).Content.ToString();
                isSelected = true;
            }
            else
            {
                selectedWord.Opacity = 1;
                OneFromFourTextViewModel.SelectedWord = "";
                isSelected = false;
            }
        }
    }
}