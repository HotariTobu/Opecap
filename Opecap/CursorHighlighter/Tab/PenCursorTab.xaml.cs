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
    /// PenCursorTab.xaml の相互作用ロジック
    /// </summary>
    public partial class PenCursorTab : UserControl
    {
        public PenCursorTab()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = ((Setting)DataContext);
            setting.CursorHighlighter.PenCursor.Strokes.Clear();
            foreach (StrokeEditBox strokeEditBox in setting.CursorHighlighter.PenCursor.StrokeEditBoxes)
            {
                strokeEditBox.ClearStrokes();
            }
        }
    }
}
