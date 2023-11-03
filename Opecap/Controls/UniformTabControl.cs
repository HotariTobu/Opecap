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
    ///     <MyNamespace:UniformTabControl/>
    ///
    /// </summary>
    public class UniformTabControl : TabControl
    {
        static UniformTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UniformTabControl), new FrameworkPropertyMetadata(typeof(UniformTabControl)));
        }

        #region == TabStripPlacement ==

        public new Dock TabStripPlacement { get => (Dock)GetValue(TabStripPlacementProperty); set => SetValue(TabStripPlacementProperty, value); }
        public static new readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(UniformTabControl), new PropertyMetadata(Dock.Top,
            (d, e) =>
            {
                if (d is UniformTabControl control)
                {
                    Dock dock = control.TabStripPlacement;
                    control.IsTop = dock == Dock.Top;
                    control.IsRight = dock == Dock.Right;
                    control.IsBottom = dock == Dock.Bottom;
                    control.IsLeft = dock == Dock.Left;
                }
            }));

        #endregion
        #region == IsTop ==

        public bool IsTop { get => (bool)GetValue(IsTopProperty); set => SetValue(IsTopProperty, value); }
        public static readonly DependencyProperty IsTopProperty = DependencyProperty.Register("IsTop", typeof(bool), typeof(UniformTabControl), new PropertyMetadata(true));

        #endregion
        #region == IsRight ==

        public bool IsRight { get => (bool)GetValue(IsRightProperty); set => SetValue(IsRightProperty, value); }
        public static readonly DependencyProperty IsRightProperty = DependencyProperty.Register("IsRight", typeof(bool), typeof(UniformTabControl), new PropertyMetadata(false));

        #endregion
        #region == IsBottom ==

        public bool IsBottom { get => (bool)GetValue(IsBottomProperty); set => SetValue(IsBottomProperty, value); }
        public static readonly DependencyProperty IsBottomProperty = DependencyProperty.Register("IsBottom", typeof(bool), typeof(UniformTabControl), new PropertyMetadata(false));

        #endregion
        #region == IsLeft ==

        public bool IsLeft { get => (bool)GetValue(IsLeftProperty); set => SetValue(IsLeftProperty, value); }
        public static readonly DependencyProperty IsLeftProperty = DependencyProperty.Register("IsLeft", typeof(bool), typeof(UniformTabControl), new PropertyMetadata(false));

        #endregion
    }
}
