using SMS_Search;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Log
{
    public enum LogLevel
    {
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }

	public class Logfile
	{
		public void Logger(LogLevel level, string message)
		{
			string configPath = Path.Combine(Application.StartupPath, "SMS Search.json");
			ConfigManager config = new ConfigManager(configPath);
			if (config.GetValue("GENERAL", "DEBUG_LOG") == "1")
			{
                // Determine configured level
                string configLevelStr = config.GetValue("GENERAL", "LOG_LEVEL");
                LogLevel configLevel = LogLevel.Info; // Default
                if (!string.IsNullOrEmpty(configLevelStr))
                {
                    try
                    {
                        configLevel = (LogLevel)Enum.Parse(typeof(LogLevel), configLevelStr, true);
                    }
                    catch { }
                }

                // Check if we should log (Lower value = Higher importance)
                if ((int)level <= (int)configLevel)
                {
				    string arg = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
				    string arg2 = Application.StartupPath.ToString();

                    try
                    {
                        using (StreamWriter streamWriter = File.AppendText(string.Format("{0}\\SMS_Search_{1}.log", arg2, arg)))
                        {
                            streamWriter.WriteLine(string.Concat(new object[]
                            {
                                "ts=\"",
                                DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                "\" lvl=\"",
                                level.ToString().ToUpper(),
                                "\" msg=\"",
                                message,
                                "\" "
                            }));
                            streamWriter.Flush();
                        }
                    }
                    catch
                    {
                        // If logging fails (e.g. file locked), we swallow it to prevent crashing the app
                    }
                }
			}
		}

        public void CleanupLogs(int retentionDays)
        {
            try
            {
                string startupPath = Application.StartupPath;
                string[] files = Directory.GetFiles(startupPath, "SMS_Search_*.log");
                DateTime thresholdDate = DateTime.Now.Date.AddDays(-retentionDays);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    // Expected format: SMS_Search_yyyyMMdd.log
                    // Length: SMS_Search_ (11) + yyyyMMdd (8) + .log (4) = 23
                    if (fileName.Length == 23 && fileName.StartsWith("SMS_Search_") && fileName.EndsWith(".log"))
                    {
                        string datePart = fileName.Substring(11, 8);
                        DateTime logDate;
                        if (DateTime.TryParseExact(datePart, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out logDate))
                        {
                            if (logDate < thresholdDate)
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch { } // Best effort
                            }
                        }
                    }
                }
            }
            catch { }
        }
	}
}
