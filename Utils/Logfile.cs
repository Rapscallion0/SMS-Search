using SMS_Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Events;
using Serilog.Formatting;
using System.Text.Json;

namespace SMS_Search.Utils
{
    public enum LogLevel
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }

    public class LogState
    {
        public DateTime LastFullLogDate { get; set; } = DateTime.MinValue;
        public Dictionary<string, Dictionary<string, string>> LastConfig { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    }

	public class Logfile
	{
        private static ILogger _logger;
        private static readonly object _syncRoot = new object();
        private string _source = "App";
        private readonly string _stateFilePath;

        // Keep these for internal state tracking if needed, but primarily driven by Serilog config
        private static bool _debugLogEnabled = false;
        private static LogLevel _configuredLogLevel = LogLevel.Info;

        public Logfile(string source = "App")
        {
            _source = source;
            _stateFilePath = Path.Combine(Application.StartupPath, "SMSSearch.state");

            // Migrate legacy state file
            string legacyStatePath = Path.Combine(Application.StartupPath, "SMS Search.state");
            if (File.Exists(legacyStatePath) && !File.Exists(_stateFilePath))
            {
                try { File.Move(legacyStatePath, _stateFilePath); } catch { }
            }

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
                    string configPath = Path.Combine(Application.StartupPath, "SMSSearch_settings.json");
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

                    string logPath = Path.Combine(Application.StartupPath, "SMSSearch_log..json");

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

                    // Process configuration logging (Full vs Delta)
                    ProcessConfigLogging(config);
                }
                catch (Exception ex)
                {
                    // Fallback if config fails
                    _logger = new LoggerConfiguration()
                        .WriteTo.File(Path.Combine(Application.StartupPath, "SMSSearch_Fallback_log..log"), rollingInterval: RollingInterval.Day)
                        .CreateLogger();
                    _logger.Error(ex, "Failed to load configuration");
                }
            }
        }

        private void ProcessConfigLogging(ConfigManager config)
        {
            try
            {
                // 1. Get current sanitized config
                var currentRaw = config.GetRawConfig();
                var currentSafe = SanitizeConfig(currentRaw);

                // 2. Load previous state
                LogState state = LoadLogState();

                // 3. Check if we need full log (Different Day)
                if (state.LastFullLogDate.Date != DateTime.Now.Date)
                {
                    // Full Log
                    var contextLogger = _logger.ForContext("Source", "System");
                    contextLogger.Information("Settings Loaded. Version: {Version}. Machine: {Machine}. User: {User}. Config: {@Config}",
                        Application.ProductVersion,
                        Environment.MachineName,
                        Environment.UserName,
                        currentSafe);

                    // Update State
                    state.LastFullLogDate = DateTime.Now;
                    state.LastConfig = currentSafe;
                    SaveLogState(state);
                }
                else
                {
                    // Same Day -> Check for Deltas
                    // Compare stored LastConfig with CurrentSafe
                    var changes = GetConfigChanges(state.LastConfig, currentSafe);

                    if (changes.Count > 0)
                    {
                        var contextLogger = _logger.ForContext("Source", "System");
                        foreach (var change in changes)
                        {
                            contextLogger.Information(change);
                        }

                        // Update State with new config
                        state.LastConfig = currentSafe;
                        SaveLogState(state);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing config logging state");
            }
        }

        private List<string> GetConfigChanges(
            Dictionary<string, Dictionary<string, string>> oldConfig,
            Dictionary<string, Dictionary<string, string>> newConfig)
        {
            var changes = new List<string>();

            // Check for added or modified keys
            foreach (var sectionKey in newConfig.Keys)
            {
                if (!oldConfig.ContainsKey(sectionKey))
                {
                    foreach (var key in newConfig[sectionKey].Keys)
                    {
                        changes.Add($"Config changed: [{sectionKey}] {key}: (null) -> {newConfig[sectionKey][key]}");
                    }
                }
                else
                {
                    var oldSection = oldConfig[sectionKey];
                    var newSection = newConfig[sectionKey];

                    foreach (var key in newSection.Keys)
                    {
                        if (!oldSection.ContainsKey(key))
                        {
                            changes.Add($"Config changed: [{sectionKey}] {key}: (null) -> {newSection[key]}");
                        }
                        else if (oldSection[key] != newSection[key])
                        {
                            changes.Add($"Config changed: [{sectionKey}] {key}: {oldSection[key]} -> {newSection[key]}");
                        }
                    }
                }
            }

            // Check for removed keys (optional, but good for completeness)
            foreach (var sectionKey in oldConfig.Keys)
            {
                if (!newConfig.ContainsKey(sectionKey))
                {
                    foreach (var key in oldConfig[sectionKey].Keys)
                    {
                        changes.Add($"Config changed: [{sectionKey}] {key}: {oldConfig[sectionKey][key]} -> (null)");
                    }
                }
                else
                {
                    var oldSection = oldConfig[sectionKey];
                    var newSection = newConfig[sectionKey];

                    foreach (var key in oldSection.Keys)
                    {
                        if (!newSection.ContainsKey(key))
                        {
                            changes.Add($"Config changed: [{sectionKey}] {key}: {oldSection[key]} -> (null)");
                        }
                    }
                }
            }

            return changes;
        }

        private Dictionary<string, Dictionary<string, string>> SanitizeConfig(Dictionary<string, Dictionary<string, string>> rawConfig)
        {
            var safeConfig = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var section in rawConfig)
            {
                var safeSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
            return safeConfig;
        }

        private LogState LoadLogState()
        {
            if (File.Exists(_stateFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_stateFilePath);
                    var state = JsonSerializer.Deserialize<LogState>(json);
                    if (state != null) return state;
                }
                catch { }
            }
            return new LogState();
        }

        private void SaveLogState(LogState state)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(state, options);
                File.WriteAllText(_stateFilePath, json);
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
