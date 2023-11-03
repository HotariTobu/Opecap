using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Opecap
{
    public class InputHook : IDisposable
    {
        private event HookProc MouseProcProperty;
        private event HookProc KeyboardProcProperty;

        private readonly IntPtr MouseHandle;
        private readonly IntPtr KeyboardHandle;

        public InputHook()
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);
            MouseHandle = SetWindowsHookEx(14, MouseProcProperty = MouseProc, hInstance, IntPtr.Zero);
            KeyboardHandle = SetWindowsHookEx(13, KeyboardProcProperty = KeyboardProc, hInstance, IntPtr.Zero);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(MouseHandle);
            UnhookWindowsHookEx(KeyboardHandle);
        }

        public delegate void MouseHookEvent(MouseHookEventArgs args);
        public event MouseHookEvent MouseHookMove;
        public event MouseHookEvent MouseHookLeftDown;
        public event MouseHookEvent MouseHookLeftUp;
        public event MouseHookEvent MouseHookRightDown;
        public event MouseHookEvent MouseHookRightUp;
        public event MouseHookEvent MouseHookMiddleDown;
        public event MouseHookEvent MouseHookMiddleUp;
        public event MouseHookEvent MouseHookWheel;

        private IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(MouseHandle, nCode, wParam, lParam);
            }

            MSLLHOOKSTRUCT data = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            MouseHookEventArgs args = new MouseHookEventArgs(data.pt.x, data.pt.y, (short)((data.mouseData >> 16) & 0xFFFF), data.time);

            switch (wParam.ToInt32())
            {
                case 0x0200:
                    MouseHookMove?.Invoke(args);
                    break;
                case 0x0201:
                    MouseHookLeftDown?.Invoke(args);
                    break;
                case 0x0202:
                    MouseHookLeftUp?.Invoke(args);
                    break;
                case 0x0204:
                    MouseHookRightDown?.Invoke(args);
                    break;
                case 0x0205:
                    MouseHookRightUp?.Invoke(args);
                    break;
                case 0x0207:
                    MouseHookMiddleDown?.Invoke(args);
                    break;
                case 0x0208:
                    MouseHookMiddleUp?.Invoke(args);
                    break;
                case 0x020A:
                    MouseHookWheel?.Invoke(args);
                    break;
            }

            if (args.Handled)
            {
                return new IntPtr(1);
            }

            return CallNextHookEx(MouseHandle, nCode, wParam, lParam);
        }

        public delegate void KeyHookEvent(KeyHookEventArgs args);
        public event KeyHookEvent KeyHookDown;
        public event KeyHookEvent KeyHookUp;

        private IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(KeyboardHandle, nCode, wParam, lParam);
            }

            KBDLLHOOKSTRUCT data = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            uint vkCode = data.vkCode;
            uint scanCode = data.scanCode;
            
            Key key = KeyInterop.KeyFromVirtualKey((int)vkCode);

            bool canCallDownEvent = ((((uint)data.flags) >> 7) & 1) == 0;
            bool canCallUpEvent;

            switch (key)
            {
                case Key.DbeDbcsChar:
                    return CallNextHookEx(KeyboardHandle, nCode, wParam, lParam);
                case Key.DbeSbcsChar:
                    canCallDownEvent = true;
                    canCallUpEvent = true;
                    break;

                case Key.DbeAlphanumeric:
                case Key.DbeKatakana:
                case Key.DbeHiragana:
                case Key.DbeRoman:
                case Key.DbeNoRoman:
                    if (canCallDownEvent)
                    {
                        canCallUpEvent = true;
                    }
                    else
                    {
                        return CallNextHookEx(KeyboardHandle, nCode, wParam, lParam);
                    }
                    break;
                default:
                    canCallUpEvent = !canCallDownEvent;
                    break;
            }

            StringBuilder keyNameBuffer = new StringBuilder(null, 256);
            GetKeyNameText(1 + ((int)scanCode << 16) + ((int)data.flags << 24), keyNameBuffer, 255);

            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            StringBuilder unicodeBuffer = new StringBuilder(5);
            ToUnicodeEx(vkCode, scanCode, keyboardState, unicodeBuffer, 4, 0, GetKeyboardLayout(0));

            KeyHookEventArgs args = new KeyHookEventArgs(key, keyNameBuffer.ToString(), unicodeBuffer.ToString(), data.time);

            if (canCallDownEvent)
            {
                KeyHookDown?.Invoke(args);
            }

            if (canCallUpEvent)
            {
                KeyHookUp?.Invoke(args);
            }

            if (args.Handled)
            {
                return new IntPtr(1);
            }

            return CallNextHookEx(KeyboardHandle, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        private enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_LOWER_IL_INJECTED = 0x02,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        [StructLayout(LayoutKind.Sequential)]
        private class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, IntPtr dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString, int cchSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int ToUnicodeEx(uint virtualKeyCode, uint scanCode, byte[] keyboardState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder receivingBuffer, int bufferSize, uint flags, IntPtr dwhkl);
    }

    public class MouseHookEventArgs
    {
        public MouseHookEventArgs(int x, int y, short delta, uint time)
        {
            X = x;
            Y = y;
            Delta = delta;
            Timestamp = (int)time;
        }

        public readonly int X;
        public readonly int Y;
        public readonly short Delta;
        public readonly int Timestamp;

        public bool Handled;
    }

    public class KeyHookEventArgs
    {
        public KeyHookEventArgs(Key key, string keyName, string unicode, uint time)
        {
            Key = key;
            KeyName = keyName;
            Unicode = unicode;
            Timestamp = (int)time;
        }

        public readonly Key Key;
        public readonly string KeyName;
        public readonly string Unicode;
        public readonly int Timestamp;

        public bool Handled;
    }
}
