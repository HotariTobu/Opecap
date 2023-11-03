using SharedWPF;
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
    /// KeyStrokerTab.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyStrokerTab : UserControl
    {
        private KeyStrokerHolder KeyStrokerHolder { get; }
        private KeyStroker KeyStroker { get; }

        public KeyStrokerTab()
        {
            InitializeComponent();
            FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies;
            FontStyleComboBox.ItemsSource = new[] { FontStyles.Normal, FontStyles.Oblique, FontStyles.Italic };
            FontFamilyComboBox.SelectedIndex = 0;
            FontStyleComboBox.SelectedIndex = 0;

            KeyStrokerHolder = new KeyStrokerHolder();
            KeyStrokerHolder.Show();
            KeyStrokerHolder.Hide();

            KeyStroker = new KeyStroker();
            KeyStroker.Isolate();
            KeyStroker.Owner = KeyStrokerHolder;
        }

        private Setting Setting;

        public void AfterInitialize()
        {
            Setting = (Setting)DataContext;

            KeyStrokerHolder.DataContext = Setting;
            KeyStroker.DataContext = Setting;

            Setting.KeyStroker.Initialize();
            DirectionAngleGrip.Angle = Setting.KeyStroker.Direction;

            KeyStroker.Show();
        }

        public void ShowHolder()
        {
            KeyStroker.RemoveEvents();
            Setting.InputHook.KeyHookDown += InputHook_KeyHookDown;

            KeyStrokerHolder.Show();
        }

        public void HideHolder()
        {
            KeyStroker.AddEvents();
            Setting.InputHook.KeyHookDown -= InputHook_KeyHookDown;

            KeyStrokerHolder.Hide();
        }

        private void InputHook_KeyHookDown(KeyHookEventArgs args) => Setting.KeyStroker.DownKeyName = KeyStroker.ResetKeyName(args.Key) ?? args.KeyName;

        private void DirectionAngleGrip_CaptureEnded(object sender, RoutedEventArgs e)
        {
            Setting.KeyStroker.Direction = DirectionAngleGrip.Angle;
            DirectionAngleGrip.Angle = Setting.KeyStroker.Direction;
        }
    }
}
