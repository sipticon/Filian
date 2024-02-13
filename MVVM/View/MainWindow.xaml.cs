using System;
using System.Windows;
using Filian.MVVM.ViewModel;

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
            MainViewModel mvm = this.DataContext as MainViewModel;
            if (mvm != null)
            {
                mvm.Dispose();
                Application.Current.Shutdown();
            }
        }
    }
}