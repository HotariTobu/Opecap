using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Opecap
{
    /// <summary>
    /// PenCursor.xaml の相互作用ロジック
    /// </summary>
    public partial class PenCursor : Window
    {
        public PenCursor()
        {
            InitializeComponent();
        }

        private Setting Setting;

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Setting == null)
            {
                Setting = (Setting)DataContext;

                for (int i = 0; i < (int)PenType.Count; i++)
                {
                    object dataContext = (PenType)i switch
                    {
                        PenType.Left => Setting.CursorHighlighter.PenCursor.Left,
                        PenType.Middle => Setting.CursorHighlighter.PenCursor.Middle,
                        PenType.Right => Setting.CursorHighlighter.PenCursor.Right,
                        PenType.Stylus => Setting.CursorHighlighter.PenCursor.Stylus,
                        PenType.InvertedStylus => Setting.CursorHighlighter.PenCursor.InvertedStylus,
                        _ => null,
                    };

                    Binding brushBinding = new Binding("PenBrush");
                    brushBinding.Source = dataContext;

                    #region == Shadow ==

                    Path shadow = new Path();
                    shadow.Visibility = Visibility.Hidden;
                    C.Children.Add(Shadows[i] = shadow);

                    shadow.SetBinding(Shape.FillProperty, brushBinding);

                    #endregion

                    #region == Polyline ==

                    Polyline polyline = new Polyline();
                    polyline.StrokeThickness = 3;
                    polyline.StrokeDashArray = new DoubleCollection() { 3 };
                    polyline.StrokeDashCap = PenLineCap.Round;
                    C.Children.Add(Polylines[i] = polyline);

                    polyline.SetBinding(Shape.StrokeProperty, brushBinding);

                    #endregion

                    #region == StrokeEditBox ==

                    StrokeEditBox strokeEditBox = new StrokeEditBox();
                    strokeEditBox.Visibility = Visibility.Hidden;
                    C.Children.Add(StrokeEditBoxes[i] = strokeEditBox);

                    strokeEditBox.SetBinding(ForegroundProperty, brushBinding);

                    #endregion

                    BaseGrid.Children.Add(SelectedInkPresenters[i] = new InkPresenter());
                }

                Setting.CursorHighlighter.PenCursor.StrokeEditBoxes = StrokeEditBoxes;
            }

            if ((bool)e.NewValue)
            {
                Setting.InputHook.MouseHookLeftDown += MouseHookLeftDown;
                Setting.InputHook.MouseHookMiddleDown += MouseHookMiddleDown;
                Setting.InputHook.MouseHookRightDown += MouseHookRightDown;
                Setting.InputHook.MouseHookMove += MouseHookMove;
                Setting.InputHook.MouseHookLeftUp += MouseHookLeftUp;
                Setting.InputHook.MouseHookMiddleUp += MouseHookMiddleUp;
                Setting.InputHook.MouseHookRightUp += MouseHookRightUp;

                Stylus.Capture(this);
            }
            else
            {
                Setting.InputHook.MouseHookLeftDown -= MouseHookLeftDown;
                Setting.InputHook.MouseHookMiddleDown -= MouseHookMiddleDown;
                Setting.InputHook.MouseHookRightDown -= MouseHookRightDown;
                Setting.InputHook.MouseHookMove -= MouseHookMove;
                Setting.InputHook.MouseHookLeftUp -= MouseHookLeftUp;
                Setting.InputHook.MouseHookMiddleUp -= MouseHookMiddleUp;
                Setting.InputHook.MouseHookRightUp -= MouseHookRightUp;

                Stylus.Capture(null);
            }
        }

        #region == Events ==

        private bool[] IsDowns = new bool[(int)PenType.Count];
        private Path[] Shadows = new Path[(int)PenType.Count];

        private Stroke[] Strokes = new Stroke[(int)PenType.Count];

        private PenSelectMode[] SelectModes = new PenSelectMode[(int)PenType.Count];
        private Point[] EditStartPoints = new Point[(int)PenType.Count];
        private Polyline[] Polylines = new Polyline[(int)PenType.Count];
        private StrokeEditBox[] StrokeEditBoxes = new StrokeEditBox[(int)PenType.Count];
        private InkPresenter[] SelectedInkPresenters = new InkPresenter[(int)PenType.Count];

        private Point[] ErasePoints = new Point[(int)PenType.Count];

        private void PenDown(int type, ModifierKeys key, PenMode mode, DrawingAttributes attributes, double x, double y, out bool handled)
        {
            if ((Keyboard.Modifiers & key) == key)
            {
                handled = key != ModifierKeys.None;
                IsDowns[type] = true;

                double penWidth = attributes.Width;
                double penHeight = attributes.Height;
                Rect penRect = new Rect(0, 0, penWidth, penHeight);
                Shadows[type].Data = attributes.StylusTip switch
                {
                    StylusTip.Rectangle => new RectangleGeometry(penRect),
                    _ => new EllipseGeometry(penRect)
                };
                Shadows[type].SetValue(Canvas.LeftProperty, x - penWidth / 2);
                Shadows[type].SetValue(Canvas.TopProperty, y - penHeight / 2);

                switch (mode)
                {
                    case PenMode.Pen:
                        Setting.CursorHighlighter.PenCursor.Strokes.Add(Strokes[type] = new Stroke(new StylusPointCollection(new[] { new StylusPoint(x, y), }), attributes));
                        Shadows[type].Visibility = Visibility.Visible;
                        break;
                    case PenMode.Select:
                        {
                            StrokeEditBox strokeEditBox = StrokeEditBoxes[type];
                            switch (SelectModes[type] = strokeEditBox.GetSelectMode(EditStartPoints[type] = new Point(x, y)))
                            {
                                case PenSelectMode.Range:
                                    {
                                        foreach (Stroke stroke in strokeEditBox.Strokes)
                                        {
                                            DrawingAttributes clone = stroke.DrawingAttributes.Clone();
                                            Color color = clone.Color;
                                            color.A *= 2;
                                            clone.Color = color;
                                            stroke.DrawingAttributes = clone;
                                            Setting.CursorHighlighter.PenCursor.Strokes.Add(stroke);
                                        }
                                        strokeEditBox.ClearStrokes();
                                    }
                                    break;
                            }
                        }
                        break;
                    case PenMode.PointErase:
                        ErasePoints[type] = new Point(x, y);
                        Shadows[type].Visibility = Visibility.Visible;
                        break;
                    case PenMode.StrokeErase:
                        ErasePoints[type] = new Point(x, y);
                        Shadows[type].Visibility = Visibility.Visible;
                        break;
                }
            }
            else
            {
                handled = false;
            }
        }

        private int LastMoveTime;

        private void PenMove(int time, double x, double y)
        {
            if (time - LastMoveTime > 10)
            {
                LastMoveTime = time;
            }
            else
            {
                return;
            }

            int count = (int)PenType.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsDowns[i])
                {
                    Path shadow = Shadows[i];
                    if (shadow.IsVisible)
                    {
                        shadow.SetValue(Canvas.LeftProperty, x - shadow.ActualWidth / 2);
                        shadow.SetValue(Canvas.TopProperty, y - shadow.ActualHeight / 2);
                    }

                    PenMode mode = (PenType)i switch
                    {
                        PenType.Left => Setting.CursorHighlighter.PenCursor.Left.PenMode.Mode,
                        PenType.Middle => Setting.CursorHighlighter.PenCursor.Middle.PenMode.Mode,
                        PenType.Right => Setting.CursorHighlighter.PenCursor.Right.PenMode.Mode,
                        PenType.Stylus => Setting.CursorHighlighter.PenCursor.Stylus.PenMode.Mode,
                        PenType.InvertedStylus => Setting.CursorHighlighter.PenCursor.InvertedStylus.PenMode.Mode,
                        _ => PenMode.None
                    };

                    switch (mode)
                    {
                        case PenMode.Pen:
                            Strokes[i].StylusPoints.Add(new StylusPoint(x, y));
                            break;
                        case PenMode.Select:
                            {
                                StrokeEditBox strokeEditBox = StrokeEditBoxes[i];
                                Point editStartPoint = EditStartPoints[i];
                                switch (SelectModes[i])
                                {
                                    case PenSelectMode.Range:
                                        {
                                            PointCollection points = Polylines[i].Points;
                                            points.Add(new Point(x, y));

                                            StrokeCollection strokes = Setting.CursorHighlighter.PenCursor.Strokes;
                                            int strokeCount = strokes.Count;
                                            int j = 0;
                                            while (j < strokeCount)
                                            {
                                                Stroke stroke = strokes[j];
                                                if (stroke.HitTest(points, 80))
                                                {
                                                    strokes.RemoveAt(j);
                                                    strokeCount--;

                                                    DrawingAttributes clone = stroke.DrawingAttributes.Clone();
                                                    Color color = clone.Color;
                                                    color.A /= 2;
                                                    clone.Color = color;
                                                    stroke.DrawingAttributes = clone;
                                                    SelectedInkPresenters[i].Strokes.Add(stroke);
                                                }
                                                else
                                                {
                                                    j++;
                                                }
                                            }
                                        }
                                        break;
                                    case PenSelectMode.Move:
                                        strokeEditBox.OffsetX = x - editStartPoint.X;
                                        strokeEditBox.OffsetY = y - editStartPoint.Y;
                                        break;
                                    case PenSelectMode.Rotate:
                                        strokeEditBox.Angle = Math.Atan2(y - strokeEditBox.Top, x - strokeEditBox.Left) * 180 / Math.PI - 90;
                                        break;
                                    case PenSelectMode.Resize:
                                        strokeEditBox.ScaleX = (x - strokeEditBox.Left) / (editStartPoint.X - strokeEditBox.Left);
                                        strokeEditBox.ScaleY = (y - strokeEditBox.Top) / (editStartPoint.Y - strokeEditBox.Top);
                                        break;
                                }
                            }
                            break;
                        case PenMode.PointErase:
                            {
                                StylusShape shape = new EllipseStylusShape(shadow.ActualWidth, shadow.ActualHeight);
                                if (shadow.Data is RectangleGeometry)
                                {
                                    shape = new RectangleStylusShape(shadow.ActualWidth, shadow.ActualHeight);
                                }

                                EditStartPoints[i] = ErasePoints[i];
                                ErasePoints[i] = new Point(x, y);
                                Point[] points = { EditStartPoints[i], ErasePoints[i] };

                                StrokeCollection strokes = new StrokeCollection();
                                foreach (Stroke stroke in Setting.CursorHighlighter.PenCursor.Strokes)
                                {
                                    strokes.Add(stroke.GetEraseResult(points, shape));
                                }

                                Setting.CursorHighlighter.PenCursor.Strokes.Clear();
                                Setting.CursorHighlighter.PenCursor.Strokes.Add(strokes);
                            }
                            break;
                        case PenMode.StrokeErase:
                            {
                                StylusShape shape = new EllipseStylusShape(shadow.ActualWidth, shadow.ActualHeight);
                                if (shadow.Data is RectangleGeometry)
                                {
                                    shape = new RectangleStylusShape(shadow.ActualWidth, shadow.ActualHeight);
                                }

                                EditStartPoints[i] = ErasePoints[i];
                                ErasePoints[i] = new Point(x, y);
                                Point[] points = { EditStartPoints[i], ErasePoints[i] };

                                StrokeCollection strokes = Setting.CursorHighlighter.PenCursor.Strokes;
                                int strokeCount = strokes.Count;
                                int j = 0;
                                while (j < strokeCount)
                                {
                                    Stroke stroke = strokes[j];
                                    if (stroke.HitTest(points, shape))
                                    {
                                        strokes.RemoveAt(j);
                                        strokeCount--;
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void PenUp(int type, ModifierKeys key, PenMode mode, double x, double y, out bool handled)
        {
            if (IsDowns[type])
            {
                handled = key != ModifierKeys.None;
                IsDowns[type] = false;

                switch (mode)
                {
                    case PenMode.Pen:
                        Shadows[type].Visibility = Visibility.Hidden;
                        break;
                    case PenMode.Select:
                        {
                            StrokeEditBox strokeEditBox = StrokeEditBoxes[type];

                            switch (SelectModes[type])
                            {
                                case PenSelectMode.Range:
                                    Polylines[type].Points.Clear();
                                    strokeEditBox.Strokes = SelectedInkPresenters[type].Strokes;
                                    SelectedInkPresenters[type].Strokes.Clear();
                                    break;
                                case PenSelectMode.Delete:
                                    if (strokeEditBox.GetSelectMode(new Point(x, y)) == PenSelectMode.Delete)
                                    {
                                        strokeEditBox.ClearStrokes();
                                    }
                                    break;
                                case PenSelectMode.Move:
                                    strokeEditBox.NormalizeTranslation();
                                    break;
                                case PenSelectMode.Rotate:
                                    strokeEditBox.NormalizeRotation();
                                    break;
                                case PenSelectMode.Resize:
                                    strokeEditBox.NormalizeScale();
                                    break;
                            }
                        }
                        break;
                    case PenMode.PointErase:
                        Shadows[type].Visibility = Visibility.Hidden;
                        break;
                    case PenMode.StrokeErase:
                        Shadows[type].Visibility = Visibility.Hidden;
                        break;
                }
            }
            else
            {
                handled = false;
            }
        }

        #endregion

        #region == Handlers ==

        private void MouseHookLeftDown(MouseHookEventArgs args) => PenDown((int)PenType.Left, Setting.CursorHighlighter.PenCursor.Left.ACSW, Setting.CursorHighlighter.PenCursor.Left.PenMode.Mode, Setting.CursorHighlighter.PenCursor.Left.Attributes, args.X, args.Y, out args.Handled);
        private void MouseHookMiddleDown(MouseHookEventArgs args) => PenDown((int)PenType.Middle, Setting.CursorHighlighter.PenCursor.Middle.ACSW, Setting.CursorHighlighter.PenCursor.Middle.PenMode.Mode, Setting.CursorHighlighter.PenCursor.Middle.Attributes, args.X, args.Y, out args.Handled);
        private void MouseHookRightDown(MouseHookEventArgs args) => PenDown((int)PenType.Right, Setting.CursorHighlighter.PenCursor.Right.ACSW, Setting.CursorHighlighter.PenCursor.Right.PenMode.Mode, Setting.CursorHighlighter.PenCursor.Right.Attributes, args.X, args.Y, out args.Handled);

        private void MouseHookMove(MouseHookEventArgs args) => PenMove(args.Timestamp, args.X, args.Y);

        private void MouseHookLeftUp(MouseHookEventArgs args) => PenUp((int)PenType.Left, Setting.CursorHighlighter.PenCursor.Left.ACSW, Setting.CursorHighlighter.PenCursor.Left.PenMode.Mode, args.X, args.Y, out args.Handled);
        private void MouseHookMiddleUp(MouseHookEventArgs args) => PenUp((int)PenType.Middle, Setting.CursorHighlighter.PenCursor.Middle.ACSW, Setting.CursorHighlighter.PenCursor.Middle.PenMode.Mode, args.X, args.Y, out args.Handled);
        private void MouseHookRightUp(MouseHookEventArgs args) => PenUp((int)PenType.Right, Setting.CursorHighlighter.PenCursor.Right.ACSW, Setting.CursorHighlighter.PenCursor.Right.PenMode.Mode, args.X, args.Y, out args.Handled);

        private void Window_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            bool isHandled = false;
            if (e.Inverted)
            {
                PenDown((int)PenType.InvertedStylus, Setting.CursorHighlighter.PenCursor.InvertedStylus.ACSW, Setting.CursorHighlighter.PenCursor.InvertedStylus.PenMode.Mode, Setting.CursorHighlighter.PenCursor.InvertedStylus.Attributes, point.X, point.Y, out isHandled);
            }
            else
            {
                PenDown((int)PenType.Stylus, Setting.CursorHighlighter.PenCursor.Stylus.ACSW, Setting.CursorHighlighter.PenCursor.Stylus.PenMode.Mode, Setting.CursorHighlighter.PenCursor.Stylus.Attributes, point.X, point.Y, out isHandled);
            }
            e.Handled = isHandled;
        }

        private void Window_StylusButtonUp(object sender, StylusButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            PenMove(e.Timestamp, point.X, point.Y);
        }

        private void Window_StylusMove(object sender, StylusEventArgs e)
        {
            Point point = e.GetPosition(this);
            bool isHandled = false;
            if (e.Inverted)
            {
                PenUp((int)PenType.InvertedStylus, Setting.CursorHighlighter.PenCursor.Stylus.ACSW, Setting.CursorHighlighter.PenCursor.InvertedStylus.PenMode.Mode, point.X, point.Y, out isHandled);
            }
            else
            {
                PenUp((int)PenType.Stylus, Setting.CursorHighlighter.PenCursor.InvertedStylus.ACSW, Setting.CursorHighlighter.PenCursor.Stylus.PenMode.Mode, point.X, point.Y, out isHandled);
            }
            e.Handled = isHandled;
        }

        #endregion
    }

    public enum PenType
    {
        Left,
        Middle,
        Right,
        Stylus,
        InvertedStylus,
        Count
    }

    public enum PenMode
    {
        Pen,
        Select,
        PointErase,
        StrokeErase,
        None
    }

    public enum PenSelectMode
    {
        Range,
        Delete,
        Move,
        Rotate,
        Resize,
        None
    }
}
