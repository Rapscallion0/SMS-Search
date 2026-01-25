using SMS_Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        private static readonly object _lock = new object();
        private bool _isConfigLoaded = false;
        private bool _debugLogEnabled = false;
        private LogLevel _configuredLogLevel = LogLevel.Info;
        private string _source = "App";
        private int _retentionDays = 14;

        public Logfile(string source = "App")
        {
            _source = source;
        }

        public void ReloadConfig()
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "SMS Search.json");
                ConfigManager config = new ConfigManager(configPath);

                _debugLogEnabled = config.GetValue("GENERAL", "DEBUG_LOG") == "1";

                string configLevelStr = config.GetValue("GENERAL", "LOG_LEVEL");
                _configuredLogLevel = LogLevel.Info; // Default
                if (!string.IsNullOrEmpty(configLevelStr))
                {
                    try
                    {
                        _configuredLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), configLevelStr, true);
                    }
                    catch { }
                }

                string retentionStr = config.GetValue("GENERAL", "LOG_RETENTION");
                if (!int.TryParse(retentionStr, out _retentionDays))
                {
                    _retentionDays = 14;
                }

                _isConfigLoaded = true;
            }
            catch
            {
                // Fallback defaults
                _debugLogEnabled = false;
                _configuredLogLevel = LogLevel.Info;
                _retentionDays = 14;
            }
        }

		public void Logger(LogLevel level, string message)
		{
            if (!_isConfigLoaded)
            {
                ReloadConfig();
            }

            // Always log Errors, or if Debug Log is enabled and level is within range
            if (level == LogLevel.Error || (_debugLogEnabled && (int)level <= (int)_configuredLogLevel))
            {
                string dateStr = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                string startupPath = Application.StartupPath;
                string logFilePath = Path.Combine(startupPath, string.Format("SMS_Search_{0}.log", dateStr));

                lock (_lock)
                {
                    try
                    {
                        bool isNewFile = !File.Exists(logFilePath);

                        using (StreamWriter streamWriter = File.AppendText(logFilePath))
                        {
                            if (isNewFile)
                            {
                                WriteLogHeader(streamWriter);
                            }

                            string sourceTag = string.IsNullOrEmpty(_source) ? "" : "[" + _source + "] ";
                            streamWriter.WriteLine(string.Concat(new object[]
                            {
                                "ts=\"",
                                DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                "\" lvl=\"",
                                level.ToString().ToUpper(),
                                "\" msg=\"",
                                sourceTag,
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

        private void WriteLogHeader(StreamWriter writer)
        {
            try
            {
                writer.WriteLine(new string('=', 50));
                writer.WriteLine("SMS Search Log File");
                writer.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine("Version: " + Application.ProductVersion);
                writer.WriteLine("Machine: " + Environment.MachineName);
                writer.WriteLine("User: " + Environment.UserName);
                writer.WriteLine(new string('-', 50));
                writer.WriteLine("Configuration Snapshot:");

                string configPath = Path.Combine(Application.StartupPath, "SMS Search.json");
                ConfigManager config = new ConfigManager(configPath);
                var rawConfig = config.GetRawConfig();

                foreach (var section in rawConfig)
                {
                    writer.WriteLine($"[{section.Key}]");
                    foreach (var kvp in section.Value)
                    {
                        string val = kvp.Value;
                        // Obfuscate potential passwords
                        if (kvp.Key.ToUpper().Contains("PASSWORD") || kvp.Key.ToUpper().Contains("PWD"))
                        {
                            val = "********";
                        }
                        writer.WriteLine($"{kvp.Key}={val}");
                    }
                }
                writer.WriteLine(new string('=', 50));
            }
            catch (Exception ex)
            {
                writer.WriteLine("Failed to write header: " + ex.Message);
            }
        }

        public void CleanupLogs()
        {
            if (!_isConfigLoaded) ReloadConfig();
            CleanupLogs(_retentionDays);
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
