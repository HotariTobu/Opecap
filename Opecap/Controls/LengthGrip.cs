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
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap;assembly=Opecap"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:LengthGrip/>
    ///
    /// </summary>
    public class LengthGrip : GripControl
    {
        static LengthGrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LengthGrip), new FrameworkPropertyMetadata(typeof(LengthGrip)));
        }

        #region == MaxLength ==

        public double MaxLength { get => (double)GetValue(MaxLengthProperty); set => SetValue(MaxLengthProperty, value); }
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(double), typeof(LengthGrip), new PropertyMetadata(0d));

        #endregion
        #region == MinLength ==

        public double MinLength { get => (double)GetValue(MinLengthProperty); set => SetValue(MinLengthProperty, value); }
        public static readonly DependencyProperty MinLengthProperty = DependencyProperty.Register("MinLength", typeof(double), typeof(LengthGrip), new PropertyMetadata(0d));

        #endregion
        #region == Length ==

        public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(LengthGrip), new PropertyMetadata(0d,
            (d, e) => (d as LengthGrip)?.RaiseLengthChangedEvent(),
            (d, baseValue) =>
            {
                double result = (double)baseValue;
                if (d is LengthGrip lengthGrip)
                {
                    return CoerceLength(result, lengthGrip.MinLength, lengthGrip.MaxLength);
                }
                return result;
            }));

        private static double CoerceLength(double value, double min, double max)
        {
            if (min < max)
            {
                return value < min ? min : value > max ? max : value;
            }
            else
            {
                return value < min ? min : value;
            }
        }

        #endregion

        #region == LengthChanged ==

        public static readonly RoutedEvent LengthChangedEvent = EventManager.RegisterRoutedEvent("LengthChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LengthGrip));
        public event RoutedEventHandler LengthChanged { add => AddHandler(LengthChangedEvent, value); remove => RemoveHandler(LengthChangedEvent, value); }
        private void RaiseLengthChangedEvent() => RaiseEvent(new RoutedEventArgs(LengthChangedEvent));

        #endregion

        protected override void UpdateValue()
        {
            Length = CoerceLength(((Vector)Mouse.GetPosition(this)).Length - 16, MinLength, MaxLength);
        }
    }
}
