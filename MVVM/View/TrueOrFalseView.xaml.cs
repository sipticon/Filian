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
    public partial class TrueOrFalseView : UserControl
    {
        public TrueOrFalseView()
        {
            InitializeComponent();
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