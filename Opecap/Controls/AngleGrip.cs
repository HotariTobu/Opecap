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
    ///     <MyNamespace:AngleGrip/>
    ///
    /// </summary>
    public class AngleGrip : GripControl
    {
        static AngleGrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AngleGrip), new FrameworkPropertyMetadata(typeof(AngleGrip)));
        }

        #region == Angle ==

        public double Angle { get => (double)GetValue(AngleProperty); set => SetValue(AngleProperty, value); }
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(AngleGrip), new PropertyMetadata(0d,
            (d, e) => (d as AngleGrip)?.RaiseAngleChangedEvent()));

        #endregion
        #region == Radius ==

        public double Radius { get => (double)GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(AngleGrip), new PropertyMetadata(0d));

        #endregion

        #region == AngleChanged ==

        public static readonly RoutedEvent AngleChangedEvent = EventManager.RegisterRoutedEvent("AngleChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AngleGrip));
        public event RoutedEventHandler AngleChanged { add => AddHandler(AngleChangedEvent, value); remove => RemoveHandler(AngleChangedEvent, value); }
        private void RaiseAngleChangedEvent() => RaiseEvent(new RoutedEventArgs(AngleChangedEvent));

        #endregion

        protected override void UpdateValue()
        {
            Point point = Mouse.GetPosition(this);
            double angle = Math.Atan2(point.Y - 10, point.X - 10) * 180 / Math.PI;
            angle = angle < 0 ? angle + 360 : angle;
            Angle = angle;
        }
    }
}
