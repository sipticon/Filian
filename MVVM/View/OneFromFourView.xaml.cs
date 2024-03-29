﻿using Filian.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Filian.MVVM.View
{
    public partial class OneFromFourView : UserControl
    {
        public OneFromFourView()
        {
            InitializeComponent();
            OneFromFourViewModel.Grid = MainGrid;
        }

        private bool _isSelected1;
        private bool _isSelected2;
        private bool _isSelected3;
        private bool _isSelected4;

        private void Select1(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected2 && !_isSelected3 && !_isSelected4)
                SelectAndMarkImage(sender, ref _isSelected1);
        }

        private void Select2(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected3 && !_isSelected4)
                SelectAndMarkImage(sender, ref _isSelected2);
        }

        private void Select3(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected2 && !_isSelected4)
                SelectAndMarkImage(sender, ref _isSelected3);
        }
        private void Select4(object sender, MouseButtonEventArgs e)
        {
            if (!_isSelected1 && !_isSelected2 && !_isSelected3)
                SelectAndMarkImage(sender, ref _isSelected4);
        }

        private void SelectAndMarkImage(object sender, ref bool isSelected)
        {
            Image selectedImage = (Image)sender;
            if (!isSelected)
            {
                selectedImage.Opacity = 0.6;
                OneFromFourViewModel.SelectedImage = ((Image)sender).Source.ToString();
                OneFromFourViewModel.Column = Grid.GetColumn(selectedImage);
                OneFromFourViewModel.Row = Grid.GetRow(selectedImage);
                isSelected = true;
            }
            else
            {
                selectedImage.Opacity = 1;
                OneFromFourViewModel.SelectedImage = "";
                isSelected = false;
            }
        }
    }
}