using Log;
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
        private static Logfile log = new Logfile("Launcher");

        public static void SwitchToOrStartApp()
        {
            try
            {
                string appName = "SMS Search";
                string exeName = "SMS Search.exe";

                Process[] processes = Process.GetProcessesByName(appName);
                if (processes.Length > 0)
                {
                    log.Logger(LogLevel.Info, "AppSwitcher: Application is running. Switching focus.");
                    // Already running, bring to front
                    IntPtr handle = processes[0].MainWindowHandle;

                    if (handle == IntPtr.Zero)
                    {
                         log.Logger(LogLevel.Warning, "AppSwitcher: Main window handle is zero.");
                    }

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
                    log.Logger(LogLevel.Info, "AppSwitcher: Application not running. Starting: " + path);

                    if (File.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        log.Logger(LogLevel.Error, "AppSwitcher: Executable not found at " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "AppSwitcher: Error switching/starting app - " + ex.Message);
            }
        }
    }
}
