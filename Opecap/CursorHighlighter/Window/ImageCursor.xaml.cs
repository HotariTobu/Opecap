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
    /// ImageCursor.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageCursor : Window
    {
        public ImageCursor()
        {
            InitializeComponent();
        }

        private Setting Setting;

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Setting = (Setting)DataContext;

            if ((bool)e.NewValue)
            {
                Setting.InputHook.MouseHookMove += MouseHookMove;
            }
            else
            {
                Setting.InputHook.MouseHookMove -= MouseHookMove;
            }
        }

        private void MouseHookMove(MouseHookEventArgs args)
        {
            Left = args.X - Setting.CursorHighlighter.ImageCursor.InternalX;
            Top = args.Y - Setting.CursorHighlighter.ImageCursor.InternalY;
        }
    }
}
