using System;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading;
using Log;
using SingleInstance;

namespace SMS_Search
{
	internal class Program
	{
		public static string[] Params;

		[STAThread]
		private static void Main(string[] args)
		{
            try
            {
                Program.Params = args;
                AppBootstrapper.Run(args);
            }
            catch (Exception ex)
            {
                // This block catches exceptions when dependencies (e.g. Serilog.dll) are missing.
                // Since Program.Main does not reference external types directly, it can execute
                // and catch the TypeInitializationException or FileNotFoundException thrown when loading AppBootstrapper.
                MessageBox.Show("A fatal startup error occurred.\n\n" +
                                "It appears some dependencies are missing or the application is corrupted.\n" +
                                "Details:\n" + ex.ToString(),
                                "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}
	}

    internal static class AppBootstrapper
    {
        private static Logfile log;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Run(string[] args)
        {
            // Initialization Logic moved here to isolate dependencies
            bool isListener = args.Length > 0 && args[0] == "--listener";
            log = new Logfile(isListener ? "Listener" : "App");

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (isListener)
                {
                    if (SingleApplication.Run("SMSSearchListener"))
                    {
                        Application.Run(new SMS_Search.Listener.HiddenWindow());
                    }
                    return;
                }

                ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMSSearch_settings.json"));
			    if (config.GetValue("GENERAL", "MULTI_INSTANCE") == "1")
			    {
				    Application.Run(new frmMain(args));
				    return;
			    }
			    SingleApplication.Run(new frmMain(args));
            }
            catch (Exception ex)
            {
                if (log != null)
                    log.Logger(LogLevel.Critical, "Fatal Error in AppBootstrapper.Run: " + ex.ToString());
                MessageBox.Show("A fatal error occurred. Check logs for details.\n" + ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (log != null)
                log.Logger(LogLevel.Critical, "Unhandled Thread Exception: " + e.Exception.ToString());
            MessageBox.Show("An unexpected error occurred. \n" + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (log != null)
                log.Logger(LogLevel.Critical, "Unhandled Domain Exception: " + (e.ExceptionObject as Exception).ToString());
            MessageBox.Show("A fatal error occurred. \n" + (e.ExceptionObject as Exception).Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
