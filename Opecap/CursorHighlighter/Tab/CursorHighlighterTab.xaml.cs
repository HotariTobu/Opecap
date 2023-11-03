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
    /// CursorHighlighterTab.xaml の相互作用ロジック
    /// </summary>
    public partial class CursorHighlighterTab : UserControl
    {
        private readonly CircleCursor CircleCursor;
        private readonly ImageCursor ImageCursor;
        //private readonly ZoomCursor ZoomCursor;
        private readonly SpotCursor SpotCursor;
        private readonly PenCursor PenCursor;

        private Window CurrentCursor;

        private Window GetCursor(int index)
        {
            return index switch
            {
                1 => ImageCursor,
                2 => SpotCursor,
                3 => PenCursor,
                _ => CircleCursor,
            };
        }

        public CursorHighlighterTab()
        {
            InitializeComponent();

            CircleCursor = new CircleCursor();
            ImageCursor = new ImageCursor();
            //ZoomCursor = new ZoomCursor();
            SpotCursor = new SpotCursor();
            PenCursor = new PenCursor();
            CircleCursor.Isolate();
            ImageCursor.Isolate();
            //ZoomCursor.Isolate();
            SpotCursor.Isolate();
            PenCursor.Isolate();
        }

        private Setting Setting;

        public void AfterInitialize()
        {
            Setting = (Setting)DataContext;

            CircleCursor.DataContext = Setting;
            ImageCursor.DataContext = Setting;
            //ZoomCursor.DataContext = Setting;
            SpotCursor.DataContext = Setting;
            PenCursor.DataContext = Setting;

            Setting.CursorHighlighter.CircleCursor.Initialize();
            Setting.CursorHighlighter.ImageCursor.Initialize();
            //Setting.CursorHighlighter.SpotCursor.Initialize();
            Setting.CursorHighlighter.PenCursor.Initialize();

            CurrentCursor = GetCursor(Setting.CursorHighlighter.Index);
            CurrentCursor.Show();
        }

        public void ShowCursor() => CurrentCursor.Show();
        public void HideCursor() => CurrentCursor.Hide();

        private void UniformTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Setting != null)
            {
                CurrentCursor = GetCursor(Setting.CursorHighlighter.Index);
            }
        }
    }
}
