using Log;
using System;
using System.Windows.Forms;
using System.IO;
using SMS_Search;

namespace SMS_Search_Launcher
{
    static class Program
    {
        private static Logfile log;

        [STAThread]
        static void Main()
        {
            log = new Logfile("Launcher");
            log.Logger(LogLevel.Info, "Launcher starting...");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // We do not show a form. We run a hidden context.
                // Using a hidden window to handle messages.
                Application.Run(new HiddenWindow());
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "Fatal Error in Launcher Main: " + ex.ToString());
            }
            finally
            {
                log.Logger(LogLevel.Info, "Launcher terminating");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
             log.Logger(LogLevel.Error, "Launcher Unhandled Domain Exception: " + (e.ExceptionObject as Exception)?.ToString());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
             log.Logger(LogLevel.Error, "Launcher Unhandled Thread Exception: " + e.Exception.ToString());
        }
    }
}
