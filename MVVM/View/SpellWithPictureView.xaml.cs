using System.Windows.Controls;
using Filian.MVVM.ViewModel;

namespace Filian.MVVM.View
{
    public partial class SpellWithPictureView : UserControl
    {
        public SpellWithPictureView()
        {
            InitializeComponent();
            Answer.Text = "";
            SpellWithPictureViewModel.Grid = MainGrid;
            SpellWithPictureViewModel.Column = Grid.GetColumn(Answer);
            SpellWithPictureViewModel.Row = Grid.GetRow(Answer);
        }
    }
}