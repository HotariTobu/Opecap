using Microsoft.Win32;
using SharedWPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Opecap
{
    /// <summary>
    /// ImageCursorTab.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageCursorTab : UserControl
    {
        public ImageCursorTab()
        {
            InitializeComponent();
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Image File|*.png;*.jpg;*.gif|All Files|*.*";
        }

        private readonly OpenFileDialog OpenFileDialog;

        private Setting Setting;

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Setting == null)
            {
                Setting = (Setting)DataContext;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFileDialog.ShowDialog(this.FindAncestor<Window>()) ?? false)
            {
                Setting.CursorHighlighter.ImageCursor.ImagePath = OpenFileDialog.FileName;
            }
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Setting.CursorHighlighter.ImageCursor.ImagePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            }
        }

        private Point LastMousePosition;
        private Point LastCursorOffset;

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LastMousePosition = e.GetPosition(this);
                LastCursorOffset = new Point(Setting.CursorHighlighter.ImageCursor.OffsetX, Setting.CursorHighlighter.ImageCursor.OffsetY);
                Mouse.Capture((IInputElement)sender);
            }
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point newOffset = e.GetPosition(this) - LastMousePosition + LastCursorOffset;
                Setting.CursorHighlighter.ImageCursor.OffsetX = newOffset.X;
                Setting.CursorHighlighter.ImageCursor.OffsetY = newOffset.Y;
            }
        }

        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
