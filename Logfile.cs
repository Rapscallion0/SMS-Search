using SMS_Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace Log
{
    public enum LogLevel
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }

	public class Logfile
	{
        private static ILogger _logger;
        private static readonly object _syncRoot = new object();
        private string _source = "App";

        // Keep these for internal state tracking if needed, but primarily driven by Serilog config
        private static bool _debugLogEnabled = false;
        private static LogLevel _configuredLogLevel = LogLevel.Info;

        public Logfile(string source = "App")
        {
            _source = source;
            EnsureConfigured();
        }

        private void EnsureConfigured()
        {
            if (_logger == null)
            {
                lock (_syncRoot)
                {
                    if (_logger == null)
                    {
                        ReloadConfig();
                    }
                }
            }
        }

        public void ReloadConfig()
        {
            lock (_syncRoot)
            {
                try
                {
                    string configPath = Path.Combine(Application.StartupPath, "SMS Search.json");
                    ConfigManager config = new ConfigManager(configPath);

                    _debugLogEnabled = config.GetValue("GENERAL", "DEBUG_LOG") == "1";
                    string configLevelStr = config.GetValue("GENERAL", "LOG_LEVEL");
                    string retentionStr = config.GetValue("GENERAL", "LOG_RETENTION");

                    int retentionDays = 14;
                    if (!int.TryParse(retentionStr, out retentionDays))
                    {
                        retentionDays = 14;
                    }

                    _configuredLogLevel = LogLevel.Info;
                    if (!string.IsNullOrEmpty(configLevelStr))
                    {
                        try
                        {
                            _configuredLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), configLevelStr, true);
                        }
                        catch { }
                    }

                    // Map internal LogLevel to Serilog LogEventLevel
                    LogEventLevel minimumLevel = LogEventLevel.Information;

                    // Logic:
                    // If DEBUG_LOG is OFF, maybe we only log Errors?
                    // The original logic was:
                    // "Always log Errors, or if Debug Log is enabled and level is within range"

                    // New Logic to match:
                    // If DEBUG_LOG is 0, we restrict to Error/Fatal?
                    // Or does LogLevel config take precedence?
                    // "Error events are always logged, while other levels require DEBUG_LOG to be enabled and the event level to be within the configured LOG_LEVEL."

                    if (!_debugLogEnabled)
                    {
                        minimumLevel = LogEventLevel.Error;
                    }
                    else
                    {
                        switch (_configuredLogLevel)
                        {
                            case LogLevel.Critical: minimumLevel = LogEventLevel.Fatal; break;
                            case LogLevel.Error: minimumLevel = LogEventLevel.Error; break;
                            case LogLevel.Warning: minimumLevel = LogEventLevel.Warning; break;
                            case LogLevel.Info: minimumLevel = LogEventLevel.Information; break;
                            case LogLevel.Debug: minimumLevel = LogEventLevel.Debug; break;
                            default: minimumLevel = LogEventLevel.Information; break;
                        }
                    }

                    string logPath = Path.Combine(Application.StartupPath, "SMS_Search_.log.json");

                    _logger = new LoggerConfiguration()
                        .MinimumLevel.Is(minimumLevel)
                        .Enrich.FromLogContext()
                        .WriteTo.File(
                            new CompactLocalTimeFormatter(),
                            logPath,
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: retentionDays,
                            shared: true)
                        .CreateLogger();

                    // Log the configuration snapshot as an event
                    LogConfigSnapshot(config);
                }
                catch (Exception ex)
                {
                    // Fallback if config fails
                    _logger = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(Application.StartupPath, "SMS_Search_Fallback_.log"), rollingInterval: RollingInterval.Day)
                        .CreateLogger();
                    _logger.Error(ex, "Failed to load configuration");
                }
            }
        }

        private void LogConfigSnapshot(ConfigManager config)
        {
            try
            {
                var rawConfig = config.GetRawConfig();
                var safeConfig = new Dictionary<string, Dictionary<string, string>>();

                foreach (var section in rawConfig)
                {
                    var safeSection = new Dictionary<string, string>();
                    foreach (var kvp in section.Value)
                    {
                        string val = kvp.Value;
                        if (kvp.Key.ToUpper().Contains("PASSWORD") || kvp.Key.ToUpper().Contains("PWD"))
                        {
                            val = "********";
                        }
                        safeSection[kvp.Key] = val;
                    }
                    safeConfig[section.Key] = safeSection;
                }

                var contextLogger = _logger.ForContext("Source", "System");
                contextLogger.Information("Configuration Loaded. Version: {Version}. Machine: {Machine}. User: {User}. Config: {@Config}",
                    Application.ProductVersion,
                    Environment.MachineName,
                    Environment.UserName,
                    safeConfig);
            }
            catch { }
        }

		public void Logger(LogLevel level, string message)
		{
            EnsureConfigured();
            var contextLogger = _logger.ForContext("Source", _source);

            switch (level)
            {
                case LogLevel.Critical:
                    contextLogger.Fatal(message);
                    break;
                case LogLevel.Error:
                    contextLogger.Error(message);
                    break;
                case LogLevel.Warning:
                    contextLogger.Warning(message);
                    break;
                case LogLevel.Info:
                    contextLogger.Information(message);
                    break;
                case LogLevel.Debug:
                    contextLogger.Debug(message);
                    break;
                default:
                    contextLogger.Information(message);
                    break;
            }
		}

        public void LogSql(string query, long elapsedMs, int rowCount, bool success, string errorMessage = null)
        {
            EnsureConfigured();
            var contextLogger = _logger.ForContext("Source", _source);

            if (!success)
            {
                contextLogger.Error("SQL Execution Failed. Elapsed: {ElapsedMs}ms. Error: {ErrorMessage}. Query: {Query}",
                    elapsedMs, errorMessage, query);
            }
            else
            {
                // Requirement: Debug for deep-dive SQL execution details
                contextLogger.Debug("SQL Execution Success. Elapsed: {ElapsedMs}ms. Rows: {RowCount}. Query: {Query}",
                    elapsedMs, rowCount, query);
            }
        }

        public void CleanupLogs(int retentionDays)
        {
            // Serilog handles retention automatically based on 'retainedFileCountLimit'.
            // However, if we want to force a reload of config to ensure retention is updated:
            ReloadConfig();
        }
	}

    public class CompactLocalTimeFormatter : ITextFormatter
    {
        private readonly JsonValueFormatter _valueFormatter;

        public CompactLocalTimeFormatter(JsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new JsonValueFormatter(typeTagName: null);
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            output.Write("{");

            // @t (Local Time)
            output.Write("\"@t\":\"");
            output.Write(logEvent.Timestamp.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            output.Write("\",");

            // @src (Source)
            if (logEvent.Properties.TryGetValue("Source", out var sourceValue))
            {
                output.Write("\"@src\":");
                _valueFormatter.Format(sourceValue, output);
                output.Write(",");
            }

            // @l (Level)
            output.Write("\"@l\":\"");
            output.Write(GetLevelName(logEvent.Level));
            output.Write("\",");

            // @m (Message)
            output.Write("\"@m\":");
            var message = logEvent.RenderMessage();
            _valueFormatter.Format(new ScalarValue(message), output);

            // @x (Exception)
            if (logEvent.Exception != null)
            {
                output.Write(",\"@x\":");
                _valueFormatter.Format(new ScalarValue(logEvent.Exception.ToString()), output);
            }

            // Other properties
            foreach (var property in logEvent.Properties)
            {
                if (property.Key == "Source") continue;

                output.Write(",");
                JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
                output.Write(":");
                _valueFormatter.Format(property.Value, output);
            }

            output.Write("}");
            output.WriteLine();
        }

        private string GetLevelName(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose: return "V";
                case LogEventLevel.Debug: return "D";
                case LogEventLevel.Information: return "I";
                case LogEventLevel.Warning: return "W";
                case LogEventLevel.Error: return "E";
                case LogEventLevel.Fatal: return "F";
                default: return "I";
            }
        }
    }
}
