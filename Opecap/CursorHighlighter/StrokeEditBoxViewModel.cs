using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Ink;

namespace Opecap
{
    public class StrokeEditBoxViewModel : SharedWPF.ViewModelBase
    {
        #region == Strokes ==

        private StrokeCollection _Strokes;
        public StrokeCollection Strokes
        {
            get => _Strokes;
            set
            {
                if (_Strokes != value)
                {
                    _Strokes = value;
                    if (_Strokes == null)
                    {
                        AddError("Error: Null Reference");
                    }
                    else
                    {
                        ClearErrors();
                    }
                    RaisePropertyChanged(nameof(Strokes));
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

        #region == OffsetX ==

        private double _OffsetX;
        public double OffsetX
        {
            get => _OffsetX;
            set
            {
                if (_OffsetX != value)
                {
                    _OffsetX = value;
                    RaisePropertyChanged(nameof(OffsetX));
                }
            }
        }

        #endregion
        #region == OffsetY ==

        private double _OffsetY;
        public double OffsetY
        {
            get => _OffsetY;
            set
            {
                if (_OffsetY != value)
                {
                    _OffsetY = value;
                    RaisePropertyChanged(nameof(OffsetY));
                }
            }
        }

        #endregion
        #region == Angle ==

        private double _Angle;
        public double Angle
        {
            get => _Angle;
            set
            {
                if (_Angle != value)
                {
                    _Angle = value;
                    RaisePropertyChanged(nameof(Angle));
                }
            }
        }

        #endregion
        #region == ScaleX ==

        private double _ScaleX;
        public double ScaleX
        {
            get => _ScaleX;
            set
            {
                if (_ScaleX != value)
                {
                    _ScaleX = value;
                    RaisePropertyChanged(nameof(ScaleX));
                }
            }
        }

        #endregion
        #region == ScaleY ==

        private double _ScaleY;
        public double ScaleY
        {
            get => _ScaleY;
            set
            {
                if (_ScaleY != value)
                {
                    _ScaleY = value;
                    RaisePropertyChanged(nameof(ScaleY));
                }
            }
        }

        #endregion

        public StrokeEditBoxViewModel()
        {
            Strokes = new StrokeCollection();

            ScaleX = 1;
            ScaleY = 1;
        }
    }
}
