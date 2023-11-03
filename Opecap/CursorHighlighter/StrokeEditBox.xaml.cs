using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Opecap
{
    /// <summary>
    /// StrokeEditBox.xaml の相互作用ロジック
    /// </summary>
    public partial class StrokeEditBox : UserControl
    {
        private readonly StrokeEditBoxViewModel ViewModel;
        public StrokeEditBox()
        {
            InitializeComponent();
            ViewModel = (StrokeEditBoxViewModel)DataContext;
        }

        public double Left { get => (double)GetValue(Canvas.LeftProperty) + 50; set => SetValue(Canvas.LeftProperty, value - 50); }
        public double Top { get => (double)GetValue(Canvas.TopProperty) + 50; set => SetValue(Canvas.TopProperty, value - 50); }
        public double InternalWidth { get => ViewModel.Width; set => ViewModel.Width = value; }
        public double InternalHeight { get => ViewModel.Height; set => ViewModel.Height = value; }

        public double OffsetX { get => ViewModel.OffsetX; set => ViewModel.OffsetX = value; }
        public double OffsetY { get => ViewModel.OffsetY; set => ViewModel.OffsetY = value; }
        public double Angle { get => ViewModel.Angle; set => ViewModel.Angle = value; }
        public double ScaleX { get => ViewModel.ScaleX; set => ViewModel.ScaleX = value; }
        public double ScaleY { get => ViewModel.ScaleY; set => ViewModel.ScaleY = value; }
        public StrokeCollection Strokes
        {
            get
            {
                Matrix matrix = new Matrix();
                matrix.Translate(Left, Top);
                StrokeCollection strokes = new StrokeCollection(ViewModel.Strokes);
                strokes.Transform(matrix, false);
                return strokes;
            }
            set
            {
                int count = value.Count;
                if (count > 0)
                {
                    Rect bounds = value[0].GetBounds();
                    for (int i = 1; i < count; i++)
                    {
                        bounds.Union(value[i].GetBounds());
                    }

                    Left = bounds.Left;
                    Top = bounds.Top;
                    InternalWidth = bounds.Width;
                    InternalHeight = bounds.Height;

                    Visibility = Visibility.Visible;

                    Matrix matrix = new Matrix();
                    matrix.Translate(-Left, -Top);
                    ViewModel.Strokes = new StrokeCollection(value);
                    ViewModel.Strokes.Transform(matrix, false);
                }
            }
        }

        public PenSelectMode GetSelectMode(Point point)
        {
            IInputElement element = InputHitTest(PointFromScreen(point));
            if (element == LeftTop)
            {
                return PenSelectMode.Delete;
            }
            else if (element == RightTop)
            {
                return PenSelectMode.Move;
            }
            else if (element == LeftBottom)
            {
                return PenSelectMode.Rotate;
            }
            else if (element == RightBottom)
            {
                return PenSelectMode.Resize;
            }
            else
            {
                return PenSelectMode.Range;
            }
        }

        public void ClearStrokes()
        {
            ViewModel.Strokes.Clear();
            Visibility = Visibility.Hidden;
        }

        public void NormalizeTranslation()
        {
            Left += OffsetX;
            Top += OffsetY;
            OffsetX = 0;
            OffsetY = 0;
        }

        public void NormalizeRotation()
        {
            StrokeCollection strokes = ViewModel.Strokes;

            Matrix matrix = new Matrix();
            matrix.Rotate(Angle);
            strokes.Transform(matrix, false);

            int count = strokes.Count;
            if (count > 0)
            {
                Rect bounds = strokes[0].GetBounds();
                for (int i = 1; i < count; i++)
                {
                    bounds.Union(strokes[i].GetBounds());
                }

                Left += bounds.Left;
                Top += bounds.Top;
                InternalWidth = bounds.Width;
                InternalHeight = bounds.Height;

                matrix = new Matrix();
                matrix.Translate(-bounds.Left, -bounds.Top);
                strokes.Transform(matrix, false);
            }

            Angle = 0;
        }

        public void NormalizeScale()
        {
            double scaleX = ScaleX;
            double scaleY = ScaleY;

            Matrix matrix = new Matrix();
            matrix.Scale(scaleX, scaleY);
            if (scaleX < 0)
            {
                scaleX = -scaleX;
                matrix.Translate(InternalWidth * scaleX, 0);
            }
            if (scaleY < 0)
            {
                scaleY = -scaleY;
                matrix.Translate(0, InternalHeight * scaleY);
            }
            ViewModel.Strokes.Transform(matrix, false);

            InternalWidth *= scaleX;
            InternalHeight *= scaleY;
            ScaleX = 1;
            ScaleY = 1;
        }
    }
}
