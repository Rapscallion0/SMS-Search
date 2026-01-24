using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SMS_Search_Launcher
{
    public static class AppSwitcher
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        public static void SwitchToOrStartApp()
        {
            string appName = "SMS Search";
            string exeName = "SMS Search.exe";

            Process[] processes = Process.GetProcessesByName(appName);
            if (processes.Length > 0)
            {
                // Already running, bring to front
                IntPtr handle = processes[0].MainWindowHandle;

                // If the handle is zero, it might be hiding in the tray or just starting up.
                // However, for WinForms apps in tray, MainWindowHandle might still be valid or we might need to find the window differently.
                // Assuming standard behavior for now.

                if (IsIconic(handle))
                {
                    ShowWindow(handle, SW_RESTORE);
                }
                SetForegroundWindow(handle);
            }
            else
            {
                // Not running, start it
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exeName);
                if (File.Exists(path))
                {
                    Process.Start(path);
                }
            }
        }
    }
}
