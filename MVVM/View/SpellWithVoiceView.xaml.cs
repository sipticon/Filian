using Filian.MVVM.ViewModel;
using System.Windows.Controls;

namespace Filian.MVVM.View
{
    public partial class SpellWithVoiceView : UserControl
    {
        public SpellWithVoiceView()
        {
            InitializeComponent();
            Answer.Text = "";
            SpellWithVoiceViewModel.Grid = MainGrid;
            SpellWithVoiceViewModel.Column = Grid.GetColumn(Answer);
            SpellWithVoiceViewModel.Row = Grid.GetRow(Answer);
        }
    }
}