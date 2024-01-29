using Filian.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Filian.MVVM.View
{
    public partial class TrueOrFalseView : UserControl
    {
        public TrueOrFalseView()
        {
            InitializeComponent();
            TrueOrFalseViewModel.Grid = MainGrid;
        }

        private bool _isSelectedTrue;
        private bool _isSelectedFalse;

        private void SelectTrue(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelectedFalse)
                SelectAndMarkWord(sender, true, ref _isSelectedTrue);
        }

        private void SelectFalse(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelectedTrue)
                SelectAndMarkWord(sender, false, ref _isSelectedFalse);
        }

        private void SelectAndMarkWord(object sender,bool answer, ref bool isSelected)
        {
            Label selectedWord = (Label)sender;
            if (!isSelected)
            {
                selectedWord.Opacity = 0.6;
                TrueOrFalseViewModel.IsCurrentWordRight = answer;
                TrueOrFalseViewModel.Column = Grid.GetColumn(selectedWord);
                TrueOrFalseViewModel.Row = Grid.GetRow(selectedWord);
                isSelected = true;
            }
            else
            {
                selectedWord.Opacity = 1;
                TrueOrFalseViewModel.IsCurrentWordRight = null;
                isSelected = false;
            }
        }
    }
}