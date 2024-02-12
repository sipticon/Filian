using System;
using System.Windows;
using System.Windows.Input;

namespace Filian.MVVM.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}