using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SMS_Search_Launcher
{
    public static class HotKeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void RegisterHotKey(IntPtr handle, int id, KeyModifiers modifiers, Keys key)
        {
            if (!RegisterHotKey(handle, id, (uint)modifiers, (uint)key))
            {
                // Handle error if needed, or silent fail
            }
        }

        public static void UnregisterHotKey(IntPtr handle, int id)
        {
            UnregisterHotKey(handle, id);
        }

        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }
    }
}
