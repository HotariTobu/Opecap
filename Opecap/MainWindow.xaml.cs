using SharedWPF;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Opecap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Setting Setting;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = Setting = new Setting("Setting.xml");
            Setting.InputHook = new InputHook();

            Tab0.AfterInitialize();
            Tab1.AfterInitialize();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Tab0.ShowCursor();
                Tab1.HideHolder();
            }
            else
            {
                Tab0.HideCursor();
                Tab1.ShowHolder();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Setting.Save();
            Setting.InputHook.Dispose();
        }
    }
}
