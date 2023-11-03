using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HookTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel ViewModel;
        private InputHook InputHook;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = (MainWindowViewModel)DataContext;

            InputHook = new InputHook();
            AddEvents();
            Mouse.Capture(this);
            Stylus.Capture(this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RemoveEvents();
            InputHook.Dispose();
            Mouse.Capture(null);
            Stylus.Capture(null);

            List<Item> items = ViewModel.History;
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                int end = i + 10;
                end = end > count ? count : end;

                Item item = items[i];
                int time = item.Timestamp;
                for (int j = i + 1; j < end; j++)
                {
                    Item target = items[j];
                    if (target.Timestamp == time)
                    {
                        item.MouseHookEvent += target.MouseHookEvent;
                        item.MouseCaptureEvent += target.MouseCaptureEvent;
                        item.TouchCaptureEvent += target.TouchCaptureEvent;
                        item.StylusCaptureEvent += target.StylusCaptureEvent;

                        items.RemoveAt(j);
                    }
                }

                count = items.Count;
            }

            File.WriteAllLines("result.txt", items.Select(item => $"{item.Index}\t{item.Timestamp}\t{item.MouseHookEvent}\t{item.MouseCaptureEvent}\t{item.TouchCaptureEvent}\t{item.StylusCaptureEvent}"));
        }

        #region MouseHook

        public void AddEvents()
        {
            InputHook.MouseHookMove += MouseHookMove;
            InputHook.MouseHookLeftDown += MouseHookLeftDown;
            InputHook.MouseHookLeftUp += MouseHookLeftUp;
            InputHook.MouseHookMiddleDown += MouseHookMiddleDown;
            InputHook.MouseHookMiddleUp += MouseHookMiddleUp;
            InputHook.MouseHookRightDown += MouseHookRightDown;
            InputHook.MouseHookRightUp += MouseHookRightUp;
            InputHook.MouseHookWheel += MouseHookWheel;
        }

        public void RemoveEvents()
        {
            InputHook.MouseHookMove -= MouseHookMove;
            InputHook.MouseHookLeftDown -= MouseHookLeftDown;
            InputHook.MouseHookLeftUp -= MouseHookLeftUp;
            InputHook.MouseHookMiddleDown -= MouseHookMiddleDown;
            InputHook.MouseHookMiddleUp -= MouseHookMiddleUp;
            InputHook.MouseHookRightDown -= MouseHookRightDown;
            InputHook.MouseHookRightUp -= MouseHookRightUp;
            InputHook.MouseHookWheel -= MouseHookWheel;
        }

        private Point LastMouseHookMove = new Point();

        private void MouseHookMove(MouseHookEventArgs args)
        {
            Point point = new Point(args.X, args.Y);
            if (LastMouseHookMove != point)
            {
                ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookMove");
                LastMouseHookMove = point;
            }
        }
        private void MouseHookLeftDown(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookLeftDown");
        private void MouseHookLeftUp(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookLeftUp");
        private void MouseHookMiddleDown(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookMiddleDown");
        private void MouseHookMiddleUp(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookMiddleDown");
        private void MouseHookRightDown(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookRightDown");
        private void MouseHookRightUp(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookRightUp");
        private void MouseHookWheel(MouseHookEventArgs args) => ViewModel.Add(args.Timestamp, EventType.MouseHook, "MouseHookWheel");

        #endregion

        #region MouseCapture

        private Point LastMouseCaptureMove = new Point();

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseDoubleClick");
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseDown");
        private void Window_MouseEnter(object sender, MouseEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseEnter");
        private void Window_MouseLeave(object sender, MouseEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseLeave");
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseLeftButtonDown");
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseLeftButtonUp");
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (LastMouseCaptureMove != point)
            {
                ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseMove");
                LastMouseCaptureMove = point;
            }
        }
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseRightButtonDown");
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseRightButtonUp");
        private void Window_MouseUp(object sender, MouseButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseUp");
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) => ViewModel.Add(e.Timestamp, EventType.MouseCapture, "MouseWheel");

        #endregion

        #region TouchCapture

        private Point LastTouchCaptureMove = new Point();

        private void Window_TouchDown(object sender, TouchEventArgs e) => ViewModel.Add(e.Timestamp, EventType.TouchCapture, "TouchDown");
        private void Window_TouchEnter(object sender, TouchEventArgs e) => ViewModel.Add(e.Timestamp, EventType.TouchCapture, "TouchEnter");
        private void Window_TouchLeave(object sender, TouchEventArgs e) => ViewModel.Add(e.Timestamp, EventType.TouchCapture, "TouchLeave");
        private void Window_TouchMove(object sender, TouchEventArgs e)
        {
            Point point = e.GetTouchPoint(this).Position;
            if (LastTouchCaptureMove != point)
            {
                ViewModel.Add(e.Timestamp, EventType.TouchCapture, "TouchMove");
                LastTouchCaptureMove = point;
            }
        }
        private void Window_TouchUp(object sender, TouchEventArgs e) => ViewModel.Add(e.Timestamp, EventType.TouchCapture, "TouchUp");

        #endregion

        #region StylusCapture

        private Point LastStylusCaptureMove = new Point();

        private void Window_StylusButtonDown(object sender, StylusButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusButtonDown");
        private void Window_StylusButtonUp(object sender, StylusButtonEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusButtonUp");
        private void Window_StylusDown(object sender, StylusDownEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusDown");
        private void Window_StylusEnter(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusEnter");
        private void Window_StylusInAirMove(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusInAirMove");
        private void Window_StylusInRange(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusInRange");
        private void Window_StylusLeave(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusLeave");
        private void Window_StylusMove(object sender, StylusEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (LastStylusCaptureMove != point)
            {
                ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusMove");
                LastStylusCaptureMove = point;
            }
        }
        private void Window_StylusOutOfRange(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusOutOfRange");
        private void Window_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusSystemGesture");
        private void Window_StylusUp(object sender, StylusEventArgs e) => ViewModel.Add(e.Timestamp, EventType.StylusCapture, "StylusUp");

        #endregion
    }
}
