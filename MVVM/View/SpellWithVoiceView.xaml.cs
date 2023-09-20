using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
