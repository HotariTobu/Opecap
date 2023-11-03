using SharedWPF;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Opecap
{
    /// <summary>
    /// KeyStroker.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyStroker : Window
    {
        public KeyStroker()
        {
            InitializeComponent();
        }

        private Setting Setting;

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Setting == null)
            {
                Setting = (Setting)DataContext;
            }

            if ((bool)e.NewValue)
            {
                AddEvents();
            }
            else
            {
                RemoveEvents();
            }
        }

        public void AddEvents()
        {
            Setting.InputHook.KeyHookDown += KeyHookDown;
            Setting.InputHook.KeyHookUp += KeyHookUp;
        }

        public void RemoveEvents()
        {
            Setting.InputHook.KeyHookDown -= KeyHookDown;
            Setting.InputHook.KeyHookUp -= KeyHookUp;
        }

        #region == KeyHookEvent ==

        private readonly bool[] KeyboardState = new bool[256];
        private static char SpaceChar => ' ';

        private void KeyHookDown(KeyHookEventArgs args)
        {
            Key key = args.Key;
            if (!KeyboardState[(int)key])
            {
                string keyName = ResetKeyName(key) ?? args.KeyName;
                StringBuilder stringBuilder = new StringBuilder();
                foreach (char c in keyName)
                {
                    if (c != SpaceChar)
                    {
                        stringBuilder.Append(c);
                    }
                }

                Setting.KeyStroker.DownList.Add(new KeyItem(key, stringBuilder.ToString(), Setting.KeyStroker.KeyMargin));
                KeyboardState[(int)key] = true;
            }
        }

        private void KeyHookUp(KeyHookEventArgs args)
        {
            Key key = args.Key;
            if (KeyboardState[(int)key])
            {
                KeyboardState[(int)key] = false;
            }
            else
            {
                return;
            }

            System.Collections.ObjectModel.ObservableCollection<KeyItem> downList = Setting.KeyStroker.DownList;
            System.Collections.ObjectModel.ObservableCollection<KeyStackItem> upList = Setting.KeyStroker.UpList;
            KeyItem keyItem = downList.First(keyItem => keyItem.Key == key);
            int index = downList.IndexOf(keyItem);
            downList.Remove(keyItem);

            if (!keyItem.IsMoved)
            {
                IEnumerable<KeyItem> keyItems = downList.Take(index).Append(keyItem);

                double potentialWidth = 0;
                double potentialHeight = 0;

                foreach (KeyItem item in keyItems)
                {
                    item.Move();
                    Size size = this.MeasureText(item.Text);
                    potentialWidth = potentialWidth > size.Width ? potentialWidth : size.Width;
                    potentialHeight = potentialHeight > size.Height ? potentialHeight : size.Height;
                }

                potentialWidth += Setting.KeyStroker.Interval1 * 2;
                potentialHeight += Setting.KeyStroker.Interval2 * 2;

                KeyStackItem keyStackItem = new KeyStackItem(keyItems.ToArray(), new Size(potentialWidth, potentialHeight));

                if (Setting.KeyStroker.DownListIsVisible)
                {
                    potentialWidth += Setting.KeyStroker.DownListWidth;
                    potentialHeight += Setting.KeyStroker.DownListHeight;
                }

                int count = upList.Count;
                if (Setting.KeyStroker.IsHorizontal)
                {
                    List<double> widths = upList.Select(item => item.Size.Width).ToList();
                    while (count > 0 && ActualWidth < potentialWidth + widths.Sum())
                    {
                        if (Setting.KeyStroker.IsLeftToRight)
                        {
                            upList.RemoveAt(count - 1);
                            widths.RemoveAt(count - 1);
                        }
                        else
                        {
                            upList.RemoveAt(0);
                            widths.RemoveAt(0);
                        }

                        count--;
                    }
                }
                else
                {
                    List<double> heights = upList.Select(item => item.Size.Height).ToList();
                    while (count > 0 && ActualHeight < potentialHeight + heights.Sum())
                    {
                        if (Setting.KeyStroker.IsBottomToTop)
                        {
                            upList.RemoveAt(0);
                            heights.RemoveAt(0);
                        }
                        else
                        {
                            upList.RemoveAt(count - 1);
                            heights.RemoveAt(count - 1);
                        }

                        count--;
                    }
                }

                if (Setting.KeyStroker.IsBottomToTop || Setting.KeyStroker.IsRightToLeft)
                {
                    upList.Add(keyStackItem);
                }
                else if (Setting.KeyStroker.IsLeftToRight || Setting.KeyStroker.IsTopToBottom)
                {
                    upList.Insert(0, keyStackItem);
                }
            }
        }

        #endregion

        public static string ResetKeyName(Key key) => key switch
        {
            Key.LeftCtrl => "Ctrl",
            Key.RightCtrl => "Ctrl",
            Key.LeftShift => "Shift",
            Key.RightShift => "Shift",
            Key.LeftAlt => "Alt",
            Key.RightAlt => "Alt",
            Key.LWin => "Windows",
            Key.RWin => "Windows",
            Key.Snapshot => "PrintScreen",
            _ => null,
        };
    }
}
