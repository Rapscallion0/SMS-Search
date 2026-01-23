// using Ini;
using SingleInstance;
using System;
using System.IO;
using System.Windows.Forms;
namespace SMS_Search
{
	internal class Program
	{
		public static string[] Params;
		[STAThread]
		private static void Main(string[] args)
		{
			Program.Params = args;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMS Search.json"));
			if (config.GetValue("GENERAL", "MULTI_INSTANCE") == "1")
			{
				Application.Run(new frmMain(Program.Params));
				return;
			}
			SingleApplication.Run(new frmMain(Program.Params));
		}
	}
}
