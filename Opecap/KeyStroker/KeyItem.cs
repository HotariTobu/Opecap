using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Opecap
{
    public class KeyItem
    {
        public KeyItem(Key key, string text, Thickness margin)
        {
            Key = key;

            Text = text;
            Margin = margin;

            Flag = true;
        }

        public Key Key { get; }
        public bool IsMoved { get; private set; }

        public string Text { get; }
        public Thickness Margin { get; }

        private bool Flag;
        public void Move()
        {
            if (Flag)
            {
                IsMoved = true;
                Flag = false;
            }
        }
    }

    public class KeyStackItem
    {
        public KeyStackItem(KeyItem[] keyItems, Size size)
        {
            KeyItemList = keyItems;
            Size = size;
        }

        public KeyItem[] KeyItemList { get; }
        public Size Size { get; }
    }
}
