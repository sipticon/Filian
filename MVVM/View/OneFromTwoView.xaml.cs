﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Filian.MVVM.ViewModel;

namespace Filian.MVVM.View
{
    public partial class OneFromTwoView : UserControl
    {
        private bool _isSelected1;
        private bool _isSelected2;
        public OneFromTwoView()
        {
            InitializeComponent();
        }

        private void Select1(object sender, MouseButtonEventArgs e)
        {
            if(!_isSelected2)
                SelectAndMarkImage(sender, ref _isSelected1);
        }

        private void Select2(object sender, MouseButtonEventArgs e)
        {
            if(!_isSelected1)
                SelectAndMarkImage(sender, ref _isSelected2);
        }

        private void SelectAndMarkImage(object sender, ref bool isSelected)
        {
            Image selectedImage = (Image)sender;
            if (!isSelected)
            {
                selectedImage.Opacity = 0.6;
                OneFromTwoViewModel.SelectedImage = ((Image)sender).Source.ToString();
                isSelected = true;
            }
            else
            {
                selectedImage.Opacity = 1;
                OneFromTwoViewModel.SelectedImage = "";
                isSelected = false;
            }
        }
    }
}