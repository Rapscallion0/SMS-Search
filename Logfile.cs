using Ini;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
namespace Log
{
	public class Logfile
	{
		public void Logger(int level, string message)
		{
			string iNIPath = ".\\SMS Search.ini";
			IniFile iniFile = new IniFile(iNIPath);
			if (iniFile.IniReadValue("GENERAL", "DEBUG_LOG") == "1")
			{
				string arg = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
				string arg2 = Application.StartupPath.ToString();
				StreamWriter streamWriter = File.AppendText(string.Format("{0}\\SMS_Search_{1}.log", arg2, arg));
				streamWriter.WriteLine(string.Concat(new object[]
				{
					"ts=\"",
					DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture),
					"\" lvl=\"",
					level,
					"\" msg=\"",
					message,
					"\" "
				}));
				streamWriter.Flush();
				streamWriter.Close();
			}
		}
	}
}
