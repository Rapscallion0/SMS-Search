using Ini;
using SingleInstance;
using System;
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
			IniFile iniFile = new IniFile(".\\SMS Search.ini");
			if (iniFile.IniReadValue("GENERAL", "MULTI_INSTANCE") == "1")
			{
				Application.Run(new frmMain(Program.Params));
				return;
			}
			SingleApplication.Run(new frmMain(Program.Params));
		}
	}
}
