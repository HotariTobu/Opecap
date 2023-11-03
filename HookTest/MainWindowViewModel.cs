using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HookTest
{
    public class MainWindowViewModel : SharedWPF.ViewModelBase
    {
        public List<Item> History = new List<Item>();
        #region == Items ==

        private readonly ObservableCollection<Item> _Items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items => _Items;

        #endregion

        public void Add(int timestamp, EventType type, string value)
        {
            Item item = null;
            if (Items[19].Timestamp == timestamp)
            {
                item = Items[19];
            }
            else
            {
                Items.RemoveAt(0);
                item = new Item(timestamp);
                Items.Add(item);
                History.Add(item);
            }

            switch (type)
            {
                case EventType.MouseHook:
                    item.MouseHookEvent = value;
                    break;
                case EventType.MouseCapture:
                    item.MouseCaptureEvent = value;
                    break;
                case EventType.TouchCapture:
                    item.TouchCaptureEvent = value;
                    break;
                case EventType.StylusCapture:
                    item.StylusCaptureEvent = value;
                    break;
                default:
                    break;
            }
        }

        public MainWindowViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                Items.Add(new Item(0));
            }
        }
    }

    public enum EventType
    {
        MouseHook,
        MouseCapture,
        TouchCapture,
        StylusCapture,
    }

    public class Item : SharedWPF.ViewModelBase
    {
        private static int Indexer = 0;

        public Item(int timestamp)
        {
            Index = Indexer;
            Indexer++;
            Timestamp = timestamp;
        }

        public int Index { get; }
        public int Timestamp { get; }

        #region == MouseHookEvent ==

        private string _MouseHookEvent;
        public string MouseHookEvent
        {
            get => _MouseHookEvent;
            set
            {
                if (_MouseHookEvent != value)
                {
                    _MouseHookEvent = value;
                    if (_MouseHookEvent == null)
                    {
                        SetError(nameof(MouseHookEvent), "Error: Null Reference");
                    }
                    else
                    {
                        ClearErrror(nameof(MouseHookEvent));
                    }
                    RaisePropertyChanged(nameof(MouseHookEvent));
                }
            }
        }

        #endregion

        #region == MouseCaptureEvent ==

        private string _MouseCaptureEvent;
        public string MouseCaptureEvent
        {
            get => _MouseCaptureEvent;
            set
            {
                if (_MouseCaptureEvent != value)
                {
                    _MouseCaptureEvent = value;
                    if (_MouseCaptureEvent == null)
                    {
                        SetError(nameof(MouseCaptureEvent), "Error: Null Reference");
                    }
                    else
                    {
                        ClearErrror(nameof(MouseCaptureEvent));
                    }
                    RaisePropertyChanged(nameof(MouseCaptureEvent));
                }
            }
        }

        #endregion

        #region == TouchCaptureEvent ==

        private string _TouchCaptureEvent;
        public string TouchCaptureEvent
        {
            get => _TouchCaptureEvent;
            set
            {
                if (_TouchCaptureEvent != value)
                {
                    _TouchCaptureEvent = value;
                    if (_TouchCaptureEvent == null)
                    {
                        SetError(nameof(TouchCaptureEvent), "Error: Null Reference");
                    }
                    else
                    {
                        ClearErrror(nameof(TouchCaptureEvent));
                    }
                    RaisePropertyChanged(nameof(TouchCaptureEvent));
                }
            }
        }

        #endregion

        #region == StylusCaptureEvent ==

        private string _StylusCaptureEvent;
        public string StylusCaptureEvent
        {
            get => _StylusCaptureEvent;
            set
            {
                if (_StylusCaptureEvent != value)
                {
                    _StylusCaptureEvent = value;
                    if (_StylusCaptureEvent == null)
                    {
                        SetError(nameof(StylusCaptureEvent), "Error: Null Reference");
                    }
                    else
                    {
                        ClearErrror(nameof(StylusCaptureEvent));
                    }
                    RaisePropertyChanged(nameof(StylusCaptureEvent));
                }
            }
        }

        #endregion
    }
}
