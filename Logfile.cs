// using Ini;
using SMS_Search;
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
			string configPath = ".\\SMS Search.json";
			ConfigManager config = new ConfigManager(configPath);
			if (config.GetValue("GENERAL", "DEBUG_LOG") == "1")
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
