using Filian.MVVM.ViewModel;
using System.Windows.Controls;

namespace Filian.MVVM.View
{
    public partial class TranslationPronunciationView : UserControl
    {
        public TranslationPronunciationView()
        {
            InitializeComponent();
            Answer.Text = "";
            TranslationPronunciationViewModel.Grid = MainGrid;
            TranslationPronunciationViewModel.Column = Grid.GetColumn(Answer);
            TranslationPronunciationViewModel.Row = Grid.GetRow(Answer);
        }
    }
}