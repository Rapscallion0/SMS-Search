using SMS_Search.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SMS_Search.Listener
{
    /// <summary>
    /// Utility class to handle focusing an existing application instance or starting a new one.
    /// </summary>
    public static class AppSwitcher
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        private static Logfile log = new Logfile("Listener");

        /// <summary>
        /// Checks if the main GUI application is running. If so, brings it to foreground.
        /// If not, starts a new instance.
        /// </summary>
        public static void SwitchToOrStartApp()
        {
            try
            {
                string currentProcessName = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(currentProcessName);

                Process guiProcess = null;
                int currentId = Process.GetCurrentProcess().Id;

                foreach (Process p in processes)
                {
                    if (p.Id != currentId)
                    {
                        // Check if it has a window handle. The GUI should have one.
                        if (p.MainWindowHandle != IntPtr.Zero)
                        {
                            guiProcess = p;
                            break;
                        }
                    }
                }

                if (guiProcess != null)
                {
                    log.Logger(LogLevel.Info, "AppSwitcher: GUI Application is running. Switching focus.");
                    IntPtr handle = guiProcess.MainWindowHandle;

                    if (handle != IntPtr.Zero)
                    {
                        if (IsIconic(handle))
                        {
                            ShowWindow(handle, SW_RESTORE);
                        }
                        SetForegroundWindow(handle);
                    }
                }
                else
                {
                    // Not running, start it (Normal GUI mode, no args)
                    string path = Application.ExecutablePath;
                    log.Logger(LogLevel.Info, "AppSwitcher: Application not running. Starting: " + path);

                    Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "AppSwitcher: Error switching/starting app - " + ex.Message);
            }
        }
    }
}
