using Filian.MVVM.ViewModel;
using System.Windows.Controls;

namespace Filian.MVVM.View
{
    public partial class TranslationTextView : UserControl
    {
        public TranslationTextView()
        {
            InitializeComponent();
            Answer.Text = "";
            TranslationTextViewModel.Grid = MainGrid;
            TranslationTextViewModel.Column = Grid.GetColumn(Answer);
            TranslationTextViewModel.Row = Grid.GetRow(Answer);
        }
    }
}