using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Opecap
{
    public abstract class GripControl : Control
    {
        protected abstract void UpdateValue();

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Mouse.Capture(this);
            UpdateValue();
            RaiseCaptureStartedEvent();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed || e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                UpdateValue();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Mouse.Capture(null);
            RaiseCaptureEndedEvent();
        }

        #region == CaptureStarted ==

        public static readonly RoutedEvent CaptureStartedEvent = EventManager.RegisterRoutedEvent("CaptureStarted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GripControl));
        public event RoutedEventHandler CaptureStarted { add => AddHandler(CaptureStartedEvent, value); remove => RemoveHandler(CaptureStartedEvent, value); }
        private void RaiseCaptureStartedEvent() => RaiseEvent(new RoutedEventArgs(CaptureStartedEvent));

        #endregion
        #region == CaptureEnded ==

        public static readonly RoutedEvent CaptureEndedEvent = EventManager.RegisterRoutedEvent("CaptureEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GripControl));
        public event RoutedEventHandler CaptureEnded { add => AddHandler(CaptureEndedEvent, value); remove => RemoveHandler(CaptureEndedEvent, value); }
        private void RaiseCaptureEndedEvent() => RaiseEvent(new RoutedEventArgs(CaptureEndedEvent));

        #endregion
    }
}
