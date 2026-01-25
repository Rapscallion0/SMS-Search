// using Ini;
using Log;
using SingleInstance;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace SMS_Search
{
	internal class Program
	{
		public static string[] Params;
        private static Logfile log;

		[STAThread]
		private static void Main(string[] args)
		{
            log = new Logfile();
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			Program.Params = args;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMS Search.json"));
			    if (config.GetValue("GENERAL", "MULTI_INSTANCE") == "1")
			    {
				    Application.Run(new frmMain(Program.Params));
				    return;
			    }
			    SingleApplication.Run(new frmMain(Program.Params));
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "Fatal Error in Main: " + ex.ToString());
                MessageBox.Show("A fatal error occurred. Check logs for details.\n" + ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.Logger(LogLevel.Error, "Unhandled Thread Exception: " + e.Exception.ToString());
            MessageBox.Show("An unexpected error occurred. \n" + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Logger(LogLevel.Error, "Unhandled Domain Exception: " + (e.ExceptionObject as Exception).ToString());
            MessageBox.Show("A fatal error occurred. \n" + (e.ExceptionObject as Exception).Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
	}
}
