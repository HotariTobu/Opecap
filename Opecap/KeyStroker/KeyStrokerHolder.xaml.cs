using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Opecap
{
    /// <summary>
    /// KeyStrokerHolder.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyStrokerHolder : Window
    {
        public KeyStrokerHolder()
        {
            InitializeComponent();
        }

        private Point LastMousePosition;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastMousePosition = e.GetPosition(this);
            Mouse.Capture(this);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(this) - (Vector)LastMousePosition;
                Left += point.X;
                Top += point.Y;
            }
        }
    }
}
