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
    /// Circle.xaml の相互作用ロジック
    /// </summary>
    public partial class CircleCursor : Window
    {
        public CircleCursor()
        {
            InitializeComponent();
        }

        private Setting Setting;

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Setting == null)
            {
                Setting = (Setting)DataContext;
            }

            if ((bool)e.NewValue)
            {
                Setting.InputHook.MouseHookMove += MouseHookMove;
                Setting.InputHook.MouseHookLeftDown += MouseHookLeftDown;
                Setting.InputHook.MouseHookLeftUp += MouseHookLeftUp;
                Setting.InputHook.MouseHookMiddleDown += MouseHookMiddleDown;
                Setting.InputHook.MouseHookMiddleUp += MouseHookMiddleUp;
                Setting.InputHook.MouseHookRightDown += MouseHookRightDown;
                Setting.InputHook.MouseHookRightUp += MouseHookRightUp;
                Setting.InputHook.MouseHookWheel += MouseHookWheel;
            }
            else
            {
                Setting.InputHook.MouseHookMove -= MouseHookMove;
                Setting.InputHook.MouseHookLeftDown -= MouseHookLeftDown;
                Setting.InputHook.MouseHookLeftUp -= MouseHookLeftUp;
                Setting.InputHook.MouseHookMiddleDown -= MouseHookMiddleDown;
                Setting.InputHook.MouseHookMiddleUp -= MouseHookMiddleUp;
                Setting.InputHook.MouseHookRightDown -= MouseHookRightDown;
                Setting.InputHook.MouseHookRightUp -= MouseHookRightUp;
                Setting.InputHook.MouseHookWheel -= MouseHookWheel;
            }
        }

        public void SetLocation(Point point)
        {
            double radius = Setting.CursorHighlighter.CircleCursor.Radius;
            Left = point.X - radius;
            Top = point.Y - radius;
        }

        #region == MouseHookEvent ==

        public void MouseHookMove(MouseHookEventArgs args) => SetLocation(new Point(args.X, args.Y));
        private void MouseHookLeftDown(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.LeftDown = true;
        private void MouseHookLeftUp(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.LeftDown = false;
        private void MouseHookMiddleDown(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.MiddleDown = true;
        private void MouseHookMiddleUp(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.MiddleDown = false;
        private void MouseHookRightDown(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.RightDown = true;
        private void MouseHookRightUp(MouseHookEventArgs args) => Setting.CursorHighlighter.CircleCursor.RightDown = false;
        private async void MouseHookWheel(MouseHookEventArgs args)
        {
            Setting.CursorHighlighter.CircleCursor.WheelSign = Math.Sign(args.Delta);
            Setting.CursorHighlighter.CircleCursor.WheelRotate = true;
            await System.Threading.Tasks.Task.Delay(200);
            Setting.CursorHighlighter.CircleCursor.WheelRotate = false;
        }

        #endregion
    }
}
