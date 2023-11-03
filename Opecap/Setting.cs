using SharedWPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Opecap
{
    public class Setting : ViewModelBase
    {
        private string Path { get; }

        public Setting() { }
        public Setting(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException();
            }

            Path = path;

            try
            {
                if (XDocument.Load(path)?.Root is XElement root)
                {
                    XMLToTree(root, ___Opecap);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool Save() => Save(Path);
        public bool Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                XElement root = new XElement("Opecap");
                TreeToXML(root, ___Opecap);
                new XDocument(root).Save(path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        private void XMLToTree(XElement element, ISettingContainer container)
        {
            foreach (string key in container.Containers.Keys)
            {
                XElement child = element.Element(key);
                if (child != null)
                {
                    XMLToTree(child, container.Containers[key]);
                }
            }

            foreach (string key in container.Items.Keys)
            {
                XElement child = element.Element(key);
                if (child != null)
                {
                    container.Items[key].Str = child.Value;
                }
            }
        }

        private void TreeToXML(XElement element, ISettingContainer container)
        {
            foreach (string key in container.Containers.Keys)
            {
                XElement child = new XElement(key);
                element.Add(child);
                TreeToXML(child, container.Containers[key]);
            }

            foreach (string key in container.Items.Keys)
            {
                XElement child = new XElement(key);
                element.Add(child);
                child.Value = container.Items[key].Str;
            }
        }

            public _CursorHighlighter CursorHighlighter => ((__CursorHighlighter)___Opecap.Containers["CursorHighlighter"])._CursorHighlighter;
            public _KeyStroker KeyStroker => ((__KeyStroker)___Opecap.Containers["KeyStroker"])._KeyStroker;
            public _MainWindow MainWindow => ((__MainWindow)___Opecap.Containers["MainWindow"])._MainWindow;

        public InputHook InputHook;


        private readonly __Opecap ___Opecap = new __Opecap();


        private class __Opecap : ISettingContainer
        {
            public __Opecap()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "CursorHighlighter", new __CursorHighlighter() },
                    { "KeyStroker", new __KeyStroker() },
                    { "MainWindow", new __MainWindow() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {

                };
            }
        }

        public class _CursorHighlighter : ViewModelBase
        {
            public _CursorHighlighter(object obj) => ___CursorHighlighter = (__CursorHighlighter)obj;
            private readonly __CursorHighlighter ___CursorHighlighter;

            public _CircleCursor CircleCursor => ((__CircleCursor)___CursorHighlighter.Containers["CircleCursor"])._CircleCursor;
            public _ImageCursor ImageCursor => ((__ImageCursor)___CursorHighlighter.Containers["ImageCursor"])._ImageCursor;
            public _SpotCursor SpotCursor => ((__SpotCursor)___CursorHighlighter.Containers["SpotCursor"])._SpotCursor;
            public _PenCursor PenCursor => ((__PenCursor)___CursorHighlighter.Containers["PenCursor"])._PenCursor;

            public int Index { get => (int)___CursorHighlighter.Items["Index"].Obj; set { ___CursorHighlighter.Items["Index"].Obj = value; RaisePropertyChanged("Index"); } }
        }

        private class __CursorHighlighter : ISettingContainer
        {
            public __CursorHighlighter()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "CircleCursor", new __CircleCursor() },
                    { "ImageCursor", new __ImageCursor() },
                    { "SpotCursor", new __SpotCursor() },
                    { "PenCursor", new __PenCursor() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "Index", new SettingValueInt(0) },
                };

                _CursorHighlighter = new _CursorHighlighter(this);
            }

            public readonly _CursorHighlighter _CursorHighlighter;
        }

        public class _CircleCursor : ViewModelBase
        {
            public _CircleCursor(object obj) => ___CircleCursor = (__CircleCursor)obj;
            private readonly __CircleCursor ___CircleCursor;


            public Color BackgroundColor { get => (Color)___CircleCursor.Items["BackgroundColor"].Obj; set { ___CircleCursor.Items["BackgroundColor"].Obj = value; RaisePropertyChanged("BackgroundColor"); } }
            public Color LeftBrushColor { get => (Color)___CircleCursor.Items["LeftBrushColor"].Obj; set { ___CircleCursor.Items["LeftBrushColor"].Obj = value; RaisePropertyChanged("LeftBrushColor"); } }
            public Color MiddleBrushColor { get => (Color)___CircleCursor.Items["MiddleBrushColor"].Obj; set { ___CircleCursor.Items["MiddleBrushColor"].Obj = value; RaisePropertyChanged("MiddleBrushColor"); } }
            public Color RightBrushColor { get => (Color)___CircleCursor.Items["RightBrushColor"].Obj; set { ___CircleCursor.Items["RightBrushColor"].Obj = value; RaisePropertyChanged("RightBrushColor"); } }
            public double Radius { get => (double)___CircleCursor.Items["Radius"].Obj; set { ___CircleCursor.Items["Radius"].Obj = value; RaisePropertyChanged("Radius"); UpdateRadius(); } }
            public double LeftAngle { get => (double)___CircleCursor.Items["LeftAngle"].Obj; set { ___CircleCursor.Items["LeftAngle"].Obj = value; RaisePropertyChanged("LeftAngle"); UpdateLeftAngle(); } }
            public double RightAngle { get => (double)___CircleCursor.Items["RightAngle"].Obj; set { ___CircleCursor.Items["RightAngle"].Obj = value; RaisePropertyChanged("RightAngle"); UpdateRightAngle(); } }

            #region == Width ==

            private double _Width;
            public double Width
            {
                get => _Width;
                set
                {
                    if (_Width != value)
                    {
                        _Width = value;
                        RaisePropertyChanged(nameof(Width));
                    }
                }
            }

            #endregion
            #region == Height ==

            private double _Height;
            public double Height
            {
                get => _Height;
                set
                {
                    if (_Height != value)
                    {
                        _Height = value;
                        RaisePropertyChanged(nameof(Height));
                    }
                }
            }

            #endregion

            #region == LeftDown ==

            private bool _LeftDown;
            public bool LeftDown
            {
                get => _LeftDown;
                set
                {
                    if (_LeftDown != value)
                    {
                        _LeftDown = value;
                        RaisePropertyChanged(nameof(LeftDown));
                    }
                }
            }

            #endregion
            #region == LeftPoint ==

            private Point _LeftPoint;
            public Point LeftPoint
            {
                get => _LeftPoint;
                set
                {
                    if (_LeftPoint != value)
                    {
                        _LeftPoint = value;
                        RaisePropertyChanged(nameof(LeftPoint));
                    }
                }
            }

            #endregion
            #region == LeftIsLarge ==

            private bool _LeftIsLarge;
            public bool LeftIsLarge
            {
                get => _LeftIsLarge;
                set
                {
                    if (_LeftIsLarge != value)
                    {
                        _LeftIsLarge = value;
                        RaisePropertyChanged(nameof(LeftIsLarge));
                    }
                }
            }

            #endregion

            #region == MiddleDown ==

            private bool _MiddleDown;
            public bool MiddleDown
            {
                get => _MiddleDown;
                set
                {
                    if (_MiddleDown != value)
                    {
                        _MiddleDown = value;
                        RaisePropertyChanged(nameof(MiddleDown));
                    }
                }
            }

            #endregion
            #region == MiddleIsLarge ==

            private bool _MiddleIsLarge;
            public bool MiddleIsLarge
            {
                get => _MiddleIsLarge;
                set
                {
                    if (_MiddleIsLarge != value)
                    {
                        _MiddleIsLarge = value;
                        RaisePropertyChanged(nameof(MiddleIsLarge));
                    }
                }
            }

            #endregion
            #region == MiddleIsClockwise ==

            private bool _MiddleIsClockwise;
            public bool MiddleIsClockwise
            {
                get => _MiddleIsClockwise;
                set
                {
                    if (_MiddleIsClockwise != value)
                    {
                        _MiddleIsClockwise = value;
                        RaisePropertyChanged(nameof(MiddleIsClockwise));
                    }
                }
            }

            #endregion

            #region == RightDown ==

            private bool _RightDown;
            public bool RightDown
            {
                get => _RightDown;
                set
                {
                    if (_RightDown != value)
                    {
                        _RightDown = value;
                        RaisePropertyChanged(nameof(RightDown));
                    }
                }
            }

            #endregion
            #region == RightPoint ==

            private Point _RightPoint;
            public Point RightPoint
            {
                get => _RightPoint;
                set
                {
                    if (_RightPoint != value)
                    {
                        _RightPoint = value;
                        RaisePropertyChanged(nameof(RightPoint));
                    }
                }
            }

            #endregion
            #region == RightIsLarge ==

            private bool _RightIsLarge;
            public bool RightIsLarge
            {
                get => _RightIsLarge;
                set
                {
                    if (_RightIsLarge != value)
                    {
                        _RightIsLarge = value;
                        RaisePropertyChanged(nameof(RightIsLarge));
                    }
                }
            }

            #endregion

            #region == WheelSign ==

            private double _WheelSign;
            public double WheelSign
            {
                get => _WheelSign;
                set
                {
                    if (_WheelSign != value)
                    {
                        _WheelSign = value;
                        RaisePropertyChanged(nameof(WheelSign));
                    }
                }
            }

            #endregion
            #region == WheelRotate ==

            private bool _WheelRotate;
            public bool WheelRotate
            {
                get => _WheelRotate;
                set
                {
                    if (_WheelRotate != value)
                    {
                        _WheelRotate = value;
                        RaisePropertyChanged(nameof(WheelRotate));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                UpdateRadius();
                UpdateLeftAngle();
                UpdateRightAngle();
            }

            private void UpdateRadius() => Width = Height = Radius * 2;

            private void UpdateLeftAngle()
            {
                double degree = LeftAngle;
                double radian = degree * Math.PI / 180;
                LeftPoint = new Point(Math.Cos(radian), Math.Sin(radian));
                LeftIsLarge = degree < 90 || degree > 270;
                UpdateMiddle();
            }

            private void UpdateRightAngle()
            {
                double degree = RightAngle;
                double radian = degree * Math.PI / 180;
                RightPoint = new Point(Math.Cos(radian), Math.Sin(radian));
                RightIsLarge = degree > 90 && degree < 270;
                UpdateMiddle();
            }

            private void UpdateMiddle()
            {
                double dif = RightAngle - LeftAngle;
                bool reverse = LeftAngle < 90 ^ RightAngle < 90;
                MiddleIsLarge = (Math.Abs(dif) > 180) ^ reverse;
                MiddleIsClockwise = (dif < 0) ^ reverse;
            }

            #endregion
        }

        private class __CircleCursor : ISettingContainer
        {
            public __CircleCursor()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "BackgroundColor", new SettingValueColor(Color.FromArgb(128, 255, 255, 0)) },
                    { "LeftBrushColor", new SettingValueColor(Color.FromArgb(255, 255, 0, 0)) },
                    { "MiddleBrushColor", new SettingValueColor(Color.FromArgb(255, 0, 255, 0)) },
                    { "RightBrushColor", new SettingValueColor(Color.FromArgb(255, 0, 0, 255)) },
                    { "Radius", new SettingValueDouble(50) },
                    { "LeftAngle", new SettingValueDouble(239.27416538962) },
                    { "RightAngle", new SettingValueDouble(300.72583456764) },
                };

                _CircleCursor = new _CircleCursor(this);
            }

            public readonly _CircleCursor _CircleCursor;
        }

        public class _ImageCursor : ViewModelBase
        {
            public _ImageCursor(object obj) => ___ImageCursor = (__ImageCursor)obj;
            private readonly __ImageCursor ___ImageCursor;


            public string ImagePath { get => (string)___ImageCursor.Items["ImagePath"].Obj; set { ___ImageCursor.Items["ImagePath"].Obj = value; RaisePropertyChanged("ImagePath"); UpdateImagePath(); } }
            public double RadiusX { get => (double)___ImageCursor.Items["RadiusX"].Obj; set { ___ImageCursor.Items["RadiusX"].Obj = value; RaisePropertyChanged("RadiusX"); UpdateLength(); } }
            public double RadiusY { get => (double)___ImageCursor.Items["RadiusY"].Obj; set { ___ImageCursor.Items["RadiusY"].Obj = value; RaisePropertyChanged("RadiusY"); UpdateLength(); } }
            public double OffsetX { get => (double)___ImageCursor.Items["OffsetX"].Obj; set { ___ImageCursor.Items["OffsetX"].Obj = value; RaisePropertyChanged("OffsetX"); UpdateOffset(); } }
            public double OffsetY { get => (double)___ImageCursor.Items["OffsetY"].Obj; set { ___ImageCursor.Items["OffsetY"].Obj = value; RaisePropertyChanged("OffsetY"); UpdateOffset(); } }
            public double Angle { get => (double)___ImageCursor.Items["Angle"].Obj; set { ___ImageCursor.Items["Angle"].Obj = value; RaisePropertyChanged("Angle"); } }
            public bool IsFill { get => (bool)___ImageCursor.Items["IsFill"].Obj; set { ___ImageCursor.Items["IsFill"].Obj = value; RaisePropertyChanged("IsFill"); UpdateStretch(); } }
            public bool IsNotFillNorUniform { get => (bool)___ImageCursor.Items["IsNotFillNorUniform"].Obj; set { ___ImageCursor.Items["IsNotFillNorUniform"].Obj = value; RaisePropertyChanged("IsNotFillNorUniform"); UpdateStretch(); } }
            public bool IsUniform { get => (bool)___ImageCursor.Items["IsUniform"].Obj; set { ___ImageCursor.Items["IsUniform"].Obj = value; RaisePropertyChanged("IsUniform"); UpdateStretch(); } }
            public bool IsUniformToFill { get => (bool)___ImageCursor.Items["IsUniformToFill"].Obj; set { ___ImageCursor.Items["IsUniformToFill"].Obj = value; RaisePropertyChanged("IsUniformToFill"); UpdateStretch(); } }

            #region == CursorImage ==

            private ImageSource _CursorImage;
            public ImageSource CursorImage
            {
                get => _CursorImage;
                set
                {
                    if (_CursorImage != value)
                    {
                        _CursorImage = value;
                        if (_CursorImage == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(CursorImage));
                    }
                }
            }

            #endregion

            #region == Width ==

            private double _Width;
            public double Width
            {
                get => _Width;
                set
                {
                    if (_Width != value)
                    {
                        _Width = value;
                        RaisePropertyChanged(nameof(Width));
                    }
                }
            }

            #endregion
            #region == Height ==

            private double _Height;
            public double Height
            {
                get => _Height;
                set
                {
                    if (_Height != value)
                    {
                        _Height = value;
                        RaisePropertyChanged(nameof(Height));
                    }
                }
            }

            #endregion
            #region == WidthEx ==

            private double _WidthEx;
            public double WidthEx
            {
                get => _WidthEx;
                set
                {
                    if (_WidthEx != value)
                    {
                        _WidthEx = value;
                        RaisePropertyChanged(nameof(WidthEx));
                    }
                }
            }

            #endregion
            #region == HeightEx ==

            private double _HeightEx;
            public double HeightEx
            {
                get => _HeightEx;
                set
                {
                    if (_HeightEx != value)
                    {
                        _HeightEx = value;
                        RaisePropertyChanged(nameof(HeightEx));
                    }
                }
            }

            #endregion
            #region == InternalX ==

            private double _InternalX;
            public double InternalX
            {
                get => _InternalX;
                set
                {
                    if (_InternalX != value)
                    {
                        _InternalX = value;
                        RaisePropertyChanged(nameof(InternalX));
                    }
                }
            }

            #endregion
            #region == InternalY ==

            private double _InternalY;
            public double InternalY
            {
                get => _InternalY;
                set
                {
                    if (_InternalY != value)
                    {
                        _InternalY = value;
                        RaisePropertyChanged(nameof(InternalY));
                    }
                }
            }

            #endregion

            #region == Stretch ==

            private Stretch _Stretch;
            public Stretch Stretch
            {
                get => _Stretch;
                set
                {
                    if (_Stretch != value)
                    {
                        _Stretch = value;
                        RaisePropertyChanged(nameof(Stretch));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                UpdateImagePath();
                UpdateLength();
                UpdateStretch();
            }

            private void UpdateImagePath()
            {
                try
                {
                    CursorImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(System.IO.Path.GetFullPath(ImagePath)));
                }
                catch { }
            }

            private void UpdateLength()
            {
                double w = Width = RadiusX * 2;
                double h = Height = RadiusY * 2;
                double r = WidthEx = HeightEx = Math.Sqrt(w * w + h * h);
                UpdateOffset();
            }

            private void UpdateOffset()
            {
                InternalX = WidthEx / 2 + OffsetX;
                InternalY = HeightEx / 2 + OffsetY;
            }

            private void UpdateStretch()
            {
                if (IsFill)
                {
                    Stretch = Stretch.Fill;
                }
                else if (IsUniform)
                {
                    Stretch = Stretch.Uniform;
                }
                else if (IsUniformToFill)
                {
                    Stretch = Stretch.UniformToFill;
                }
                else
                {
                    Stretch = Stretch.None;
                }
            }

            #endregion
        }

        private class __ImageCursor : ISettingContainer
        {
            public __ImageCursor()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "ImagePath", new SettingValueString("Pen.png") },
                    { "RadiusX", new SettingValueDouble(50) },
                    { "RadiusY", new SettingValueDouble(50) },
                    { "OffsetX", new SettingValueDouble(-50) },
                    { "OffsetY", new SettingValueDouble(-50) },
                    { "Angle", new SettingValueDouble(0) },
                    { "IsFill", new SettingValueBool(true) },
                    { "IsNotFillNorUniform", new SettingValueBool(false) },
                    { "IsUniform", new SettingValueBool(false) },
                    { "IsUniformToFill", new SettingValueBool(false) },
                };

                _ImageCursor = new _ImageCursor(this);
            }

            public readonly _ImageCursor _ImageCursor;
        }

        public class _SpotCursor : ViewModelBase
        {
            public _SpotCursor(object obj) => ___SpotCursor = (__SpotCursor)obj;
            private readonly __SpotCursor ___SpotCursor;


            public Color BackgroundColor { get => (Color)___SpotCursor.Items["BackgroundColor"].Obj; set { ___SpotCursor.Items["BackgroundColor"].Obj = value; RaisePropertyChanged("BackgroundColor"); } }
            public double Radius { get => (double)___SpotCursor.Items["Radius"].Obj; set { ___SpotCursor.Items["Radius"].Obj = value; RaisePropertyChanged("Radius"); } }

            public Rect ScreenRect => new Rect(0, 0, SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);

            #region == Pos ==

            private Point _Pos;
            public Point Pos
            {
                get => _Pos;
                set
                {
                    if (_Pos != value)
                    {
                        _Pos = value;
                        RaisePropertyChanged(nameof(Pos));
                    }
                }
            }

            #endregion
        }

        private class __SpotCursor : ISettingContainer
        {
            public __SpotCursor()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "BackgroundColor", new SettingValueColor(Color.FromArgb(100, 0, 0, 0)) },
                    { "Radius", new SettingValueDouble(50) },
                };

                _SpotCursor = new _SpotCursor(this);
            }

            public readonly _SpotCursor _SpotCursor;
        }

        public class _PenCursor : ViewModelBase
        {
            public _PenCursor(object obj) => ___PenCursor = (__PenCursor)obj;
            private readonly __PenCursor ___PenCursor;

            public _Left Left => ((__Left)___PenCursor.Containers["Left"])._Left;
            public _Middle Middle => ((__Middle)___PenCursor.Containers["Middle"])._Middle;
            public _Right Right => ((__Right)___PenCursor.Containers["Right"])._Right;
            public _Stylus Stylus => ((__Stylus)___PenCursor.Containers["Stylus"])._Stylus;
            public _InvertedStylus InvertedStylus => ((__InvertedStylus)___PenCursor.Containers["InvertedStylus"])._InvertedStylus;

            public int Index { get => (int)___PenCursor.Items["Index"].Obj; set { ___PenCursor.Items["Index"].Obj = value; RaisePropertyChanged("Index"); } }

            #region == Strokes ==

            private readonly StrokeCollection _Strokes = new StrokeCollection();
            public StrokeCollection Strokes => _Strokes;

            #endregion
            public StrokeEditBox[] StrokeEditBoxes;

            #region == Update ==

            public void Initialize()
            {
                Left.Initialize();
                Middle.Initialize();
                Right.Initialize();
                Stylus.Initialize();
                InvertedStylus.Initialize();
            }

            #endregion
        }

        private class __PenCursor : ISettingContainer
        {
            public __PenCursor()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "Left", new __Left() },
                    { "Middle", new __Middle() },
                    { "Right", new __Right() },
                    { "Stylus", new __Stylus() },
                    { "InvertedStylus", new __InvertedStylus() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "Index", new SettingValueInt(0) },
                };

                _PenCursor = new _PenCursor(this);
            }

            public readonly _PenCursor _PenCursor;
        }

        public class _Left : ViewModelBase
        {
            public _Left(object obj) => ___Left = (__Left)obj;
            private readonly __Left ___Left;

            public _PenMode PenMode => ((__PenMode)___Left.Containers["PenMode"])._PenMode;
            public _PenTip PenTip => ((__PenTip)___Left.Containers["PenTip"])._PenTip;

            public Color PenColor { get => (Color)___Left.Items["PenColor"].Obj; set { ___Left.Items["PenColor"].Obj = value; RaisePropertyChanged("PenColor"); UpdateAttributes(); } }
            public double Width { get => (double)___Left.Items["Width"].Obj; set { ___Left.Items["Width"].Obj = value; RaisePropertyChanged("Width"); UpdateAttributes(); } }
            public double Height { get => (double)___Left.Items["Height"].Obj; set { ___Left.Items["Height"].Obj = value; RaisePropertyChanged("Height"); UpdateAttributes(); } }
            public bool IsAlt { get => (bool)___Left.Items["IsAlt"].Obj; set { ___Left.Items["IsAlt"].Obj = value; RaisePropertyChanged("IsAlt"); UpdateKey(); } }
            public bool IsCtrl { get => (bool)___Left.Items["IsCtrl"].Obj; set { ___Left.Items["IsCtrl"].Obj = value; RaisePropertyChanged("IsCtrl"); UpdateKey(); } }
            public bool IsShift { get => (bool)___Left.Items["IsShift"].Obj; set { ___Left.Items["IsShift"].Obj = value; RaisePropertyChanged("IsShift"); UpdateKey(); } }
            public bool IsWin { get => (bool)___Left.Items["IsWin"].Obj; set { ___Left.Items["IsWin"].Obj = value; RaisePropertyChanged("IsWin"); UpdateKey(); } }

            #region == PenBrush ==

            private Brush _PenBrush;
            public Brush PenBrush
            {
                get => _PenBrush;
                set
                {
                    if (_PenBrush != value)
                    {
                        _PenBrush = value;
                        if (_PenBrush == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(PenBrush));
                    }
                }
            }

            #endregion
            #region == Attributes ==

            private DrawingAttributes _Attributes = new DrawingAttributes();
            public DrawingAttributes Attributes
            {
                get => _Attributes;
                set
                {
                    if (_Attributes != value)
                    {
                        _Attributes = value;
                        if (_Attributes == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(Attributes));
                    }
                }
            }

            #endregion
            #region == ACSW ==

            private ModifierKeys _ACSW;
            public ModifierKeys ACSW
            {
                get => _ACSW;
                set
                {
                    if (_ACSW != value)
                    {
                        _ACSW = value;
                        RaisePropertyChanged(nameof(ACSW));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                PenMode.Initialize();
                PenTip.Initialize();
                UpdateAttributes();
                UpdateKey();

                if (PenMode.Mode == Opecap.PenMode.None)
                {
                    PenMode.IsPen = true;
                }
            }

            private void UpdateAttributes()
            {
                PenBrush = new SolidColorBrush(PenColor);
                Attributes = new DrawingAttributes()
                {
                    Color = PenColor,
                    FitToCurve = true,
                    StylusTip = PenTip.Tip,
                    Width = Width,
                    Height = Height,
                };
            }

            private void UpdateKey()
            {
                ModifierKeys acsw = ModifierKeys.None;
                acsw = IsAlt ? acsw | ModifierKeys.Alt : acsw;
                acsw = IsCtrl ? acsw | ModifierKeys.Control : acsw;
                acsw = IsShift ? acsw | ModifierKeys.Shift : acsw;
                acsw = IsWin ? acsw | ModifierKeys.Windows : acsw;
                ACSW = acsw;
            }

            #endregion
        }

        private class __Left : ISettingContainer
        {
            public __Left()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "PenMode", new __PenMode() },
                    { "PenTip", new __PenTip() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "PenColor", new SettingValueColor(Color.FromArgb(255, 255, 0, 0)) },
                    { "Width", new SettingValueDouble(3) },
                    { "Height", new SettingValueDouble(3) },
                    { "IsAlt", new SettingValueBool(true) },
                    { "IsCtrl", new SettingValueBool(false) },
                    { "IsShift", new SettingValueBool(false) },
                    { "IsWin", new SettingValueBool(false) },
                };

                _Left = new _Left(this);
            }

            public readonly _Left _Left;
        }

        public class _Middle : ViewModelBase
        {
            public _Middle(object obj) => ___Middle = (__Middle)obj;
            private readonly __Middle ___Middle;

            public _PenMode PenMode => ((__PenMode)___Middle.Containers["PenMode"])._PenMode;
            public _PenTip PenTip => ((__PenTip)___Middle.Containers["PenTip"])._PenTip;

            public Color PenColor { get => (Color)___Middle.Items["PenColor"].Obj; set { ___Middle.Items["PenColor"].Obj = value; RaisePropertyChanged("PenColor"); UpdateAttributes(); } }
            public double Width { get => (double)___Middle.Items["Width"].Obj; set { ___Middle.Items["Width"].Obj = value; RaisePropertyChanged("Width"); UpdateAttributes(); } }
            public double Height { get => (double)___Middle.Items["Height"].Obj; set { ___Middle.Items["Height"].Obj = value; RaisePropertyChanged("Height"); UpdateAttributes(); } }
            public bool IsAlt { get => (bool)___Middle.Items["IsAlt"].Obj; set { ___Middle.Items["IsAlt"].Obj = value; RaisePropertyChanged("IsAlt"); UpdateKey(); } }
            public bool IsCtrl { get => (bool)___Middle.Items["IsCtrl"].Obj; set { ___Middle.Items["IsCtrl"].Obj = value; RaisePropertyChanged("IsCtrl"); UpdateKey(); } }
            public bool IsShift { get => (bool)___Middle.Items["IsShift"].Obj; set { ___Middle.Items["IsShift"].Obj = value; RaisePropertyChanged("IsShift"); UpdateKey(); } }
            public bool IsWin { get => (bool)___Middle.Items["IsWin"].Obj; set { ___Middle.Items["IsWin"].Obj = value; RaisePropertyChanged("IsWin"); UpdateKey(); } }

            #region == PenBrush ==

            private Brush _PenBrush;
            public Brush PenBrush
            {
                get => _PenBrush;
                set
                {
                    if (_PenBrush != value)
                    {
                        _PenBrush = value;
                        if (_PenBrush == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(PenBrush));
                    }
                }
            }

            #endregion
            #region == Attributes ==

            private DrawingAttributes _Attributes = new DrawingAttributes();
            public DrawingAttributes Attributes
            {
                get => _Attributes;
                set
                {
                    if (_Attributes != value)
                    {
                        _Attributes = value;
                        if (_Attributes == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(Attributes));
                    }
                }
            }

            #endregion
            #region == ACSW ==

            private ModifierKeys _ACSW;
            public ModifierKeys ACSW
            {
                get => _ACSW;
                set
                {
                    if (_ACSW != value)
                    {
                        _ACSW = value;
                        RaisePropertyChanged(nameof(ACSW));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                PenMode.Initialize();
                PenTip.Initialize();
                UpdateAttributes();
                UpdateKey();

                if (PenMode.Mode == Opecap.PenMode.None)
                {
                    PenMode.IsSelect = true;
                }
            }

            private void UpdateAttributes()
            {
                PenBrush = new SolidColorBrush(PenColor);
                Attributes = new DrawingAttributes()
                {
                    Color = PenColor,
                    FitToCurve = true,
                    StylusTip = PenTip.Tip,
                    Width = Width,
                    Height = Height,
                };
            }

            private void UpdateKey()
            {
                ModifierKeys acsw = ModifierKeys.None;
                acsw = IsAlt ? acsw | ModifierKeys.Alt : acsw;
                acsw = IsCtrl ? acsw | ModifierKeys.Control : acsw;
                acsw = IsShift ? acsw | ModifierKeys.Shift : acsw;
                acsw = IsWin ? acsw | ModifierKeys.Windows : acsw;
                ACSW = acsw;
            }

            #endregion
        }

        private class __Middle : ISettingContainer
        {
            public __Middle()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "PenMode", new __PenMode() },
                    { "PenTip", new __PenTip() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "PenColor", new SettingValueColor(Color.FromArgb(255, 0, 255, 0)) },
                    { "Width", new SettingValueDouble(3) },
                    { "Height", new SettingValueDouble(3) },
                    { "IsAlt", new SettingValueBool(true) },
                    { "IsCtrl", new SettingValueBool(false) },
                    { "IsShift", new SettingValueBool(false) },
                    { "IsWin", new SettingValueBool(false) },
                };

                _Middle = new _Middle(this);
            }

            public readonly _Middle _Middle;
        }

        public class _Right : ViewModelBase
        {
            public _Right(object obj) => ___Right = (__Right)obj;
            private readonly __Right ___Right;

            public _PenMode PenMode => ((__PenMode)___Right.Containers["PenMode"])._PenMode;
            public _PenTip PenTip => ((__PenTip)___Right.Containers["PenTip"])._PenTip;

            public Color PenColor { get => (Color)___Right.Items["PenColor"].Obj; set { ___Right.Items["PenColor"].Obj = value; RaisePropertyChanged("PenColor"); UpdateAttributes(); } }
            public double Width { get => (double)___Right.Items["Width"].Obj; set { ___Right.Items["Width"].Obj = value; RaisePropertyChanged("Width"); UpdateAttributes(); } }
            public double Height { get => (double)___Right.Items["Height"].Obj; set { ___Right.Items["Height"].Obj = value; RaisePropertyChanged("Height"); UpdateAttributes(); } }
            public bool IsAlt { get => (bool)___Right.Items["IsAlt"].Obj; set { ___Right.Items["IsAlt"].Obj = value; RaisePropertyChanged("IsAlt"); UpdateKey(); } }
            public bool IsCtrl { get => (bool)___Right.Items["IsCtrl"].Obj; set { ___Right.Items["IsCtrl"].Obj = value; RaisePropertyChanged("IsCtrl"); UpdateKey(); } }
            public bool IsShift { get => (bool)___Right.Items["IsShift"].Obj; set { ___Right.Items["IsShift"].Obj = value; RaisePropertyChanged("IsShift"); UpdateKey(); } }
            public bool IsWin { get => (bool)___Right.Items["IsWin"].Obj; set { ___Right.Items["IsWin"].Obj = value; RaisePropertyChanged("IsWin"); UpdateKey(); } }

            #region == PenBrush ==

            private Brush _PenBrush;
            public Brush PenBrush
            {
                get => _PenBrush;
                set
                {
                    if (_PenBrush != value)
                    {
                        _PenBrush = value;
                        if (_PenBrush == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(PenBrush));
                    }
                }
            }

            #endregion
            #region == Attributes ==

            private DrawingAttributes _Attributes = new DrawingAttributes();
            public DrawingAttributes Attributes
            {
                get => _Attributes;
                set
                {
                    if (_Attributes != value)
                    {
                        _Attributes = value;
                        if (_Attributes == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(Attributes));
                    }
                }
            }

            #endregion
            #region == ACSW ==

            private ModifierKeys _ACSW;
            public ModifierKeys ACSW
            {
                get => _ACSW;
                set
                {
                    if (_ACSW != value)
                    {
                        _ACSW = value;
                        RaisePropertyChanged(nameof(ACSW));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                PenMode.Initialize();
                PenTip.Initialize();
                UpdateAttributes();
                UpdateKey();

                if (PenMode.Mode == Opecap.PenMode.None)
                {
                    PenMode.IsPointErase = true;
                }
            }

            private void UpdateAttributes()
            {
                PenBrush = new SolidColorBrush(PenColor);
                Attributes = new DrawingAttributes()
                {
                    Color = PenColor,
                    FitToCurve = true,
                    StylusTip = PenTip.Tip,
                    Width = Width,
                    Height = Height,
                };
            }

            private void UpdateKey()
            {
                ModifierKeys acsw = ModifierKeys.None;
                acsw = IsAlt ? acsw | ModifierKeys.Alt : acsw;
                acsw = IsCtrl ? acsw | ModifierKeys.Control : acsw;
                acsw = IsShift ? acsw | ModifierKeys.Shift : acsw;
                acsw = IsWin ? acsw | ModifierKeys.Windows : acsw;
                ACSW = acsw;
            }

            #endregion
        }

        private class __Right : ISettingContainer
        {
            public __Right()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "PenMode", new __PenMode() },
                    { "PenTip", new __PenTip() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "PenColor", new SettingValueColor(Color.FromArgb(255, 0, 0, 255)) },
                    { "Width", new SettingValueDouble(50) },
                    { "Height", new SettingValueDouble(50) },
                    { "IsAlt", new SettingValueBool(true) },
                    { "IsCtrl", new SettingValueBool(false) },
                    { "IsShift", new SettingValueBool(false) },
                    { "IsWin", new SettingValueBool(false) },
                };

                _Right = new _Right(this);
            }

            public readonly _Right _Right;
        }

        public class _Stylus : ViewModelBase
        {
            public _Stylus(object obj) => ___Stylus = (__Stylus)obj;
            private readonly __Stylus ___Stylus;

            public _PenMode PenMode => ((__PenMode)___Stylus.Containers["PenMode"])._PenMode;
            public _PenTip PenTip => ((__PenTip)___Stylus.Containers["PenTip"])._PenTip;

            public Color PenColor { get => (Color)___Stylus.Items["PenColor"].Obj; set { ___Stylus.Items["PenColor"].Obj = value; RaisePropertyChanged("PenColor"); UpdateAttributes(); } }
            public double Width { get => (double)___Stylus.Items["Width"].Obj; set { ___Stylus.Items["Width"].Obj = value; RaisePropertyChanged("Width"); UpdateAttributes(); } }
            public double Height { get => (double)___Stylus.Items["Height"].Obj; set { ___Stylus.Items["Height"].Obj = value; RaisePropertyChanged("Height"); UpdateAttributes(); } }
            public bool IsAlt { get => (bool)___Stylus.Items["IsAlt"].Obj; set { ___Stylus.Items["IsAlt"].Obj = value; RaisePropertyChanged("IsAlt"); UpdateKey(); } }
            public bool IsCtrl { get => (bool)___Stylus.Items["IsCtrl"].Obj; set { ___Stylus.Items["IsCtrl"].Obj = value; RaisePropertyChanged("IsCtrl"); UpdateKey(); } }
            public bool IsShift { get => (bool)___Stylus.Items["IsShift"].Obj; set { ___Stylus.Items["IsShift"].Obj = value; RaisePropertyChanged("IsShift"); UpdateKey(); } }
            public bool IsWin { get => (bool)___Stylus.Items["IsWin"].Obj; set { ___Stylus.Items["IsWin"].Obj = value; RaisePropertyChanged("IsWin"); UpdateKey(); } }

            #region == PenBrush ==

            private Brush _PenBrush;
            public Brush PenBrush
            {
                get => _PenBrush;
                set
                {
                    if (_PenBrush != value)
                    {
                        _PenBrush = value;
                        if (_PenBrush == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(PenBrush));
                    }
                }
            }

            #endregion
            #region == Attributes ==

            private DrawingAttributes _Attributes = new DrawingAttributes();
            public DrawingAttributes Attributes
            {
                get => _Attributes;
                set
                {
                    if (_Attributes != value)
                    {
                        _Attributes = value;
                        if (_Attributes == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(Attributes));
                    }
                }
            }

            #endregion
            #region == ACSW ==

            private ModifierKeys _ACSW;
            public ModifierKeys ACSW
            {
                get => _ACSW;
                set
                {
                    if (_ACSW != value)
                    {
                        _ACSW = value;
                        RaisePropertyChanged(nameof(ACSW));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                PenMode.Initialize();
                PenTip.Initialize();
                UpdateAttributes();
                UpdateKey();

                if (PenMode.Mode == Opecap.PenMode.None)
                {
                    PenMode.IsPen = true;
                }
            }

            private void UpdateAttributes()
            {
                PenBrush = new SolidColorBrush(PenColor);
                Attributes = new DrawingAttributes()
                {
                    Color = PenColor,
                    FitToCurve = true,
                    StylusTip = PenTip.Tip,
                    Width = Width,
                    Height = Height,
                };
            }

            private void UpdateKey()
            {
                ModifierKeys acsw = ModifierKeys.None;
                acsw = IsAlt ? acsw | ModifierKeys.Alt : acsw;
                acsw = IsCtrl ? acsw | ModifierKeys.Control : acsw;
                acsw = IsShift ? acsw | ModifierKeys.Shift : acsw;
                acsw = IsWin ? acsw | ModifierKeys.Windows : acsw;
                ACSW = acsw;
            }

            #endregion
        }

        private class __Stylus : ISettingContainer
        {
            public __Stylus()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "PenMode", new __PenMode() },
                    { "PenTip", new __PenTip() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "PenColor", new SettingValueColor(Color.FromArgb(255, 255, 0, 0)) },
                    { "Width", new SettingValueDouble(3) },
                    { "Height", new SettingValueDouble(3) },
                    { "IsAlt", new SettingValueBool(true) },
                    { "IsCtrl", new SettingValueBool(false) },
                    { "IsShift", new SettingValueBool(false) },
                    { "IsWin", new SettingValueBool(false) },
                };

                _Stylus = new _Stylus(this);
            }

            public readonly _Stylus _Stylus;
        }

        public class _InvertedStylus : ViewModelBase
        {
            public _InvertedStylus(object obj) => ___InvertedStylus = (__InvertedStylus)obj;
            private readonly __InvertedStylus ___InvertedStylus;

            public _PenMode PenMode => ((__PenMode)___InvertedStylus.Containers["PenMode"])._PenMode;
            public _PenTip PenTip => ((__PenTip)___InvertedStylus.Containers["PenTip"])._PenTip;

            public Color PenColor { get => (Color)___InvertedStylus.Items["PenColor"].Obj; set { ___InvertedStylus.Items["PenColor"].Obj = value; RaisePropertyChanged("PenColor"); UpdateAttributes(); } }
            public double Width { get => (double)___InvertedStylus.Items["Width"].Obj; set { ___InvertedStylus.Items["Width"].Obj = value; RaisePropertyChanged("Width"); UpdateAttributes(); } }
            public double Height { get => (double)___InvertedStylus.Items["Height"].Obj; set { ___InvertedStylus.Items["Height"].Obj = value; RaisePropertyChanged("Height"); UpdateAttributes(); } }
            public bool IsAlt { get => (bool)___InvertedStylus.Items["IsAlt"].Obj; set { ___InvertedStylus.Items["IsAlt"].Obj = value; RaisePropertyChanged("IsAlt"); UpdateKey(); } }
            public bool IsCtrl { get => (bool)___InvertedStylus.Items["IsCtrl"].Obj; set { ___InvertedStylus.Items["IsCtrl"].Obj = value; RaisePropertyChanged("IsCtrl"); UpdateKey(); } }
            public bool IsShift { get => (bool)___InvertedStylus.Items["IsShift"].Obj; set { ___InvertedStylus.Items["IsShift"].Obj = value; RaisePropertyChanged("IsShift"); UpdateKey(); } }
            public bool IsWin { get => (bool)___InvertedStylus.Items["IsWin"].Obj; set { ___InvertedStylus.Items["IsWin"].Obj = value; RaisePropertyChanged("IsWin"); UpdateKey(); } }

            #region == PenBrush ==

            private Brush _PenBrush;
            public Brush PenBrush
            {
                get => _PenBrush;
                set
                {
                    if (_PenBrush != value)
                    {
                        _PenBrush = value;
                        if (_PenBrush == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(PenBrush));
                    }
                }
            }

            #endregion
            #region == Attributes ==

            private DrawingAttributes _Attributes = new DrawingAttributes();
            public DrawingAttributes Attributes
            {
                get => _Attributes;
                set
                {
                    if (_Attributes != value)
                    {
                        _Attributes = value;
                        if (_Attributes == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(Attributes));
                    }
                }
            }

            #endregion
            #region == ACSW ==

            private ModifierKeys _ACSW;
            public ModifierKeys ACSW
            {
                get => _ACSW;
                set
                {
                    if (_ACSW != value)
                    {
                        _ACSW = value;
                        RaisePropertyChanged(nameof(ACSW));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                PenMode.Initialize();
                PenTip.Initialize();
                UpdateAttributes();
                UpdateKey();

                if (PenMode.Mode == Opecap.PenMode.None)
                {
                    PenMode.IsPointErase = true;
                }
            }

            private void UpdateAttributes()
            {
                PenBrush = new SolidColorBrush(PenColor);
                Attributes = new DrawingAttributes()
                {
                    Color = PenColor,
                    FitToCurve = true,
                    StylusTip = PenTip.Tip,
                    Width = Width,
                    Height = Height,
                };
            }

            private void UpdateKey()
            {
                ModifierKeys acsw = ModifierKeys.None;
                acsw = IsAlt ? acsw | ModifierKeys.Alt : acsw;
                acsw = IsCtrl ? acsw | ModifierKeys.Control : acsw;
                acsw = IsShift ? acsw | ModifierKeys.Shift : acsw;
                acsw = IsWin ? acsw | ModifierKeys.Windows : acsw;
                ACSW = acsw;
            }

            #endregion
        }

        private class __InvertedStylus : ISettingContainer
        {
            public __InvertedStylus()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {
                    { "PenMode", new __PenMode() },
                    { "PenTip", new __PenTip() },
                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "PenColor", new SettingValueColor(Color.FromArgb(255, 0, 0, 255)) },
                    { "Width", new SettingValueDouble(50) },
                    { "Height", new SettingValueDouble(50) },
                    { "IsAlt", new SettingValueBool(true) },
                    { "IsCtrl", new SettingValueBool(false) },
                    { "IsShift", new SettingValueBool(false) },
                    { "IsWin", new SettingValueBool(false) },
                };

                _InvertedStylus = new _InvertedStylus(this);
            }

            public readonly _InvertedStylus _InvertedStylus;
        }

        public class _PenMode : ViewModelBase
        {
            public _PenMode(object obj) => ___PenMode = (__PenMode)obj;
            private readonly __PenMode ___PenMode;


            public bool IsPen { get => (bool)___PenMode.Items["IsPen"].Obj; set { ___PenMode.Items["IsPen"].Obj = value; RaisePropertyChanged("IsPen"); Update(); } }
            public bool IsSelect { get => (bool)___PenMode.Items["IsSelect"].Obj; set { ___PenMode.Items["IsSelect"].Obj = value; RaisePropertyChanged("IsSelect"); Update(); } }
            public bool IsPointErase { get => (bool)___PenMode.Items["IsPointErase"].Obj; set { ___PenMode.Items["IsPointErase"].Obj = value; RaisePropertyChanged("IsPointErase"); Update(); } }
            public bool IsStrokeErase { get => (bool)___PenMode.Items["IsStrokeErase"].Obj; set { ___PenMode.Items["IsStrokeErase"].Obj = value; RaisePropertyChanged("IsStrokeErase"); Update(); } }

            #region == Mode ==

            private PenMode _Mode;
            public PenMode Mode
            {
                get => _Mode;
                set
                {
                    if (_Mode != value)
                    {
                        _Mode = value;
                        RaisePropertyChanged(nameof(Mode));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                Update();
            }

            private void Update()
            {
                if (IsPen)
                {
                    Mode = PenMode.Pen;
                }
                else if (IsSelect)
                {
                    Mode = PenMode.Select;
                }
                else if (IsPointErase)
                {
                    Mode = PenMode.PointErase;
                }
                else if (IsStrokeErase)
                {
                    Mode = PenMode.StrokeErase;
                }
                else
                {
                    Mode = PenMode.None;
                }
            }

            #endregion
        }

        private class __PenMode : ISettingContainer
        {
            public __PenMode()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "IsPen", new SettingValueBool(false) },
                    { "IsSelect", new SettingValueBool(false) },
                    { "IsPointErase", new SettingValueBool(false) },
                    { "IsStrokeErase", new SettingValueBool(false) },
                };

                _PenMode = new _PenMode(this);
            }

            public readonly _PenMode _PenMode;
        }

        public class _PenTip : ViewModelBase
        {
            public _PenTip(object obj) => ___PenTip = (__PenTip)obj;
            private readonly __PenTip ___PenTip;


            public bool IsEllipse { get => (bool)___PenTip.Items["IsEllipse"].Obj; set { ___PenTip.Items["IsEllipse"].Obj = value; RaisePropertyChanged("IsEllipse"); Update(); } }
            public bool IsRectangle { get => (bool)___PenTip.Items["IsRectangle"].Obj; set { ___PenTip.Items["IsRectangle"].Obj = value; RaisePropertyChanged("IsRectangle"); Update(); } }

            #region == Tip ==

            private StylusTip _Tip;
            public StylusTip Tip
            {
                get => _Tip;
                set
                {
                    if (_Tip != value)
                    {
                        _Tip = value;
                        RaisePropertyChanged(nameof(Tip));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                Update();
            }

            private void Update()
            {
                if (IsRectangle)
                {
                    Tip = StylusTip.Rectangle;
                }
                else
                {
                    Tip = StylusTip.Ellipse;
                }
            }

            #endregion
        }

        private class __PenTip : ISettingContainer
        {
            public __PenTip()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "IsEllipse", new SettingValueBool(true) },
                    { "IsRectangle", new SettingValueBool(false) },
                };

                _PenTip = new _PenTip(this);
            }

            public readonly _PenTip _PenTip;
        }

        public class _KeyStroker : ViewModelBase
        {
            public _KeyStroker(object obj) => ___KeyStroker = (__KeyStroker)obj;
            private readonly __KeyStroker ___KeyStroker;


            public Color ForegroundColor { get => (Color)___KeyStroker.Items["ForegroundColor"].Obj; set { ___KeyStroker.Items["ForegroundColor"].Obj = value; RaisePropertyChanged("ForegroundColor"); } }
            public Color BackgroundColor { get => (Color)___KeyStroker.Items["BackgroundColor"].Obj; set { ___KeyStroker.Items["BackgroundColor"].Obj = value; RaisePropertyChanged("BackgroundColor"); } }
            public bool DownListIsVisible { get => (bool)___KeyStroker.Items["DownListIsVisible"].Obj; set { ___KeyStroker.Items["DownListIsVisible"].Obj = value; RaisePropertyChanged("DownListIsVisible"); } }
            public double Interval { get => (double)___KeyStroker.Items["Interval"].Obj; set { ___KeyStroker.Items["Interval"].Obj = value; RaisePropertyChanged("Interval"); UpdateInterval(); } }
            public double FontSize { get => (double)___KeyStroker.Items["FontSize"].Obj; set { ___KeyStroker.Items["FontSize"].Obj = value; RaisePropertyChanged("FontSize"); UpdateDownListSize(); } }
            public string FontFamily { get => (string)___KeyStroker.Items["FontFamily"].Obj; set { ___KeyStroker.Items["FontFamily"].Obj = value; RaisePropertyChanged("FontFamily"); RaisePropertyChanged(nameof(FontFamilyInstance)); UpdateDownListSize(); } }
            public int FontStyle { get => (int)___KeyStroker.Items["FontStyle"].Obj; set { ___KeyStroker.Items["FontStyle"].Obj = value; RaisePropertyChanged("FontStyle"); RaisePropertyChanged(nameof(FontStyleInstance)); UpdateDownListSize(); } }
            public double Direction { get => (double)___KeyStroker.Items["Direction"].Obj; set { ___KeyStroker.Items["Direction"].Obj = value; RaisePropertyChanged("Direction"); UpdateDirection(); } }
            public double Left { get => (double)___KeyStroker.Items["Left"].Obj; set { ___KeyStroker.Items["Left"].Obj = value; RaisePropertyChanged("Left"); } }
            public double Top { get => (double)___KeyStroker.Items["Top"].Obj; set { ___KeyStroker.Items["Top"].Obj = value; RaisePropertyChanged("Top"); } }
            public double Width { get => (double)___KeyStroker.Items["Width"].Obj; set { ___KeyStroker.Items["Width"].Obj = value; RaisePropertyChanged("Width"); } }
            public double Height { get => (double)___KeyStroker.Items["Height"].Obj; set { ___KeyStroker.Items["Height"].Obj = value; RaisePropertyChanged("Height"); } }

            public FontFamily FontFamilyInstance => new FontFamily(FontFamily);
            public FontStyle FontStyleInstance => FontStyle switch { 1 => FontStyles.Oblique, 2 => FontStyles.Italic, _ => FontStyles.Normal, };

            public double Zero => 0;
            public double Fifty => 50;

            #region == DownListWidth ==

            private double _DownListWidth;
            public double DownListWidth
            {
                get => _DownListWidth;
                set
                {
                    if (_DownListWidth != value)
                    {
                        _DownListWidth = value;
                        RaisePropertyChanged(nameof(DownListWidth));
                    }
                }
            }

            #endregion
            #region == DownListHeight ==

            private double _DownListHeight;
            public double DownListHeight
            {
                get => _DownListHeight;
                set
                {
                    if (_DownListHeight != value)
                    {
                        _DownListHeight = value;
                        RaisePropertyChanged(nameof(DownListHeight));
                    }
                }
            }

            #endregion

            #region == DownList ==

            private readonly ObservableCollection<KeyItem> _DownList = new ObservableCollection<KeyItem>();
            public ObservableCollection<KeyItem> DownList => _DownList;

            #endregion
            #region == UpList ==

            private readonly ObservableCollection<KeyStackItem> _UpList = new ObservableCollection<KeyStackItem>();
            public ObservableCollection<KeyStackItem> UpList => _UpList;

            #endregion

            #region == IsRightToLeft ==

            private bool _IsRightToLeft;
            public bool IsRightToLeft
            {
                get => _IsRightToLeft;
                set
                {
                    if (_IsRightToLeft != value)
                    {
                        _IsRightToLeft = value;
                        RaisePropertyChanged(nameof(IsRightToLeft));
                    }
                }
            }

            #endregion
            #region == IsBottomToTop ==

            private bool _IsBottomToTop;
            public bool IsBottomToTop
            {
                get => _IsBottomToTop;
                set
                {
                    if (_IsBottomToTop != value)
                    {
                        _IsBottomToTop = value;
                        RaisePropertyChanged(nameof(IsBottomToTop));
                    }
                }
            }

            #endregion
            #region == IsLeftToRight ==

            private bool _IsLeftToRight;
            public bool IsLeftToRight
            {
                get => _IsLeftToRight;
                set
                {
                    if (_IsLeftToRight != value)
                    {
                        _IsLeftToRight = value;
                        RaisePropertyChanged(nameof(IsLeftToRight));
                    }
                }
            }

            #endregion
            #region == IsTopToBottom ==

            private bool _IsTopToBottom;
            public bool IsTopToBottom
            {
                get => _IsTopToBottom;
                set
                {
                    if (_IsTopToBottom != value)
                    {
                        _IsTopToBottom = value;
                        RaisePropertyChanged(nameof(IsTopToBottom));
                    }
                }
            }

            #endregion

            public bool IsHorizontal { get; set; }
            public double Interval1 { get; set; }
            public double Interval2 { get; set; }
            public Thickness KeyMargin { get; set; }

            #region == DownKeyName ==

            private string _DownKeyName = "ABC";
            public string DownKeyName
            {
                get => _DownKeyName;
                set
                {
                    if (_DownKeyName != value)
                    {
                        _DownKeyName = value;
                        if (_DownKeyName == null)
                        {
                            AddError("Error: Null Reference");
                        }
                        else
                        {
                            ClearErrors();
                        }
                        RaisePropertyChanged(nameof(DownKeyName));
                    }
                }
            }

            #endregion

            #region == Update ==

            public void Initialize()
            {
                UpdateInterval();
                UpdateDirection(true);
                UpdateDownListSize();
            }

            private void ClearList()
            {
                DownList.Clear();
                UpList.Clear();
            }

            public void UpdateInterval()
            {
                double interval = Interval;

                ClearList();
                Interval1 = interval;
                Interval2 = interval / 2;
                KeyMargin = new Thickness(Interval1, Interval2, Interval1, Interval2);
                UpdateDownListSize();
            }

            public void UpdateDirection(bool coerce = false)
            {
                double angle = Direction;
                int direction = (int)Math.Round(angle / 90) % 4;
                if (!coerce && (int)angle == direction * 90)
                {
                    return;
                }
                else
                {
                    Direction = direction * 90;
                }

                ClearList();
                IsRightToLeft = direction == 0;
                IsBottomToTop = direction == 1;
                IsLeftToRight = direction == 2;
                IsTopToBottom = direction == 3;
                IsHorizontal = IsLeftToRight || IsRightToLeft;
            }

            private void UpdateDownListSize()
            {
                ClearList();
                Size size = Utilities.MeasureText("PrintScreen", FontFamilyInstance, FontStyleInstance, FontWeights.Normal, FontStretches.Normal, FontSize);
                DownListWidth = size.Width + Interval1;
                DownListHeight = size.Height + Interval1;
            }

            #endregion
        }

        private class __KeyStroker : ISettingContainer
        {
            public __KeyStroker()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "ForegroundColor", new SettingValueColor(Color.FromArgb(255, 255, 255, 255)) },
                    { "BackgroundColor", new SettingValueColor(Color.FromArgb(128, 0, 0, 0)) },
                    { "DownListIsVisible", new SettingValueBool(true) },
                    { "Interval", new SettingValueDouble(10) },
                    { "FontSize", new SettingValueDouble(20) },
                    { "FontFamily", new SettingValueString("Yu Gothic UI") },
                    { "FontStyle", new SettingValueInt(0) },
                    { "Direction", new SettingValueDouble(90) },
                    { "Left", new SettingValueDouble(50) },
                    { "Top", new SettingValueDouble(50) },
                    { "Width", new SettingValueDouble(350) },
                    { "Height", new SettingValueDouble(150) },
                };

                _KeyStroker = new _KeyStroker(this);
            }

            public readonly _KeyStroker _KeyStroker;
        }

        public class _MainWindow : ViewModelBase
        {
            public _MainWindow(object obj) => ___MainWindow = (__MainWindow)obj;
            private readonly __MainWindow ___MainWindow;


            public double Left { get => (double)___MainWindow.Items["Left"].Obj; set { ___MainWindow.Items["Left"].Obj = value; RaisePropertyChanged("Left"); } }
            public double Top { get => (double)___MainWindow.Items["Top"].Obj; set { ___MainWindow.Items["Top"].Obj = value; RaisePropertyChanged("Top"); } }
            public double Width { get => (double)___MainWindow.Items["Width"].Obj; set { ___MainWindow.Items["Width"].Obj = value; RaisePropertyChanged("Width"); } }
            public double Height { get => (double)___MainWindow.Items["Height"].Obj; set { ___MainWindow.Items["Height"].Obj = value; RaisePropertyChanged("Height"); } }
        }

        private class __MainWindow : ISettingContainer
        {
            public __MainWindow()
            {
                Containers = new Dictionary<string, ISettingContainer>()
                {

                };

                Items = new Dictionary<string, ISettingValue>()
                {
                    { "Left", new SettingValueDouble(100) },
                    { "Top", new SettingValueDouble(200) },
                    { "Width", new SettingValueDouble(650) },
                    { "Height", new SettingValueDouble(600) },
                };

                _MainWindow = new _MainWindow(this);
            }

            public readonly _MainWindow _MainWindow;
        }


        private class ISettingContainer
        {
            public Dictionary<string, ISettingContainer> Containers { get; protected set; }
            public Dictionary<string, ISettingValue> Items { get; protected set; }
        }

        private interface ISettingValue
        {
            public object Obj { get; set; }
            public string Str { get; set; }
        }

        private class SettingValueColor : ISettingValue
        {
            public SettingValueColor(Color value) => Value = value;
            private Color Value;

            public object Obj { get => Value; set => Value = (Color)value; }
            public string Str
            {
                get => $"{Value.A}, {Value.R}, {Value.G}, {Value.B}";
                set
                {
                    string[] vs = value?.Split(", ");
                    if (vs != null && vs.Length == 4 && byte.TryParse(vs[0], out byte a) && byte.TryParse(vs[1], out byte r) && byte.TryParse(vs[2], out byte g) && byte.TryParse(vs[3], out byte b))
                    {
                        Value = Color.FromArgb(a, r, g, b);
                    }
                }
            }
        }

        private class SettingValueDouble : ISettingValue
        {
            public SettingValueDouble(double value = 0) => Value = value;
            private double Value;

            public object Obj { get => Value; set => Value = (double)value; }
            public string Str { get => Value.ToString(); set => double.TryParse(value, out Value); }
        }

        private class SettingValueString : ISettingValue
        {
            public SettingValueString(string value = "") => Value = value;
            private string Value;

            public object Obj { get => Value; set => Value = (string)value; }
            public string Str { get => Value; set => Value = value; }
        }

        private class SettingValueInt : ISettingValue
        {
            public SettingValueInt(int value = 0) => Value = value;
            private int Value;

            public object Obj { get => Value; set => Value = (int)value; }
            public string Str { get => Value.ToString(); set => int.TryParse(value, out Value); }
        }

        private class SettingValueBool : ISettingValue
        {
            public SettingValueBool(bool value = false) => Value = value;
            private bool Value;

            public object Obj { get => Value; set => Value = (bool)value; }
            public string Str { get => Value.ToString(); set => bool.TryParse(value, out Value); }
        }
    }
}