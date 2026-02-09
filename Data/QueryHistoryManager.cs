using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SMS_Search.Data
{
    /// <summary>
    /// Singleton manager for persisting recent queries to the configuration file.
    /// </summary>
    public class QueryHistoryManager
    {
        private static QueryHistoryManager _instance;
        public static QueryHistoryManager Instance => _instance ?? (_instance = new QueryHistoryManager());

        private Utils.ConfigManager _config;
        private const int MaxHistory = 20;
        private const string SectionName = "QUERY_HISTORY";

        /// <summary>
        /// Initializes the manager with the configuration provider.
        /// </summary>
        public void Initialize(Utils.ConfigManager config)
        {
            _config = config;
        }

        /// <summary>
        /// Adds a query to history, moving it to the top if it already exists.
        /// </summary>
        public void AddQuery(string type, string sql)
        {
            if (string.IsNullOrWhiteSpace(sql) || _config == null) return;

            var history = GetHistory(type);

            // Remove if exists to move to top
            history.RemoveAll(x => x.Equals(sql, StringComparison.OrdinalIgnoreCase));

            // Add to top
            history.Insert(0, sql);

            // Trim
            if (history.Count > MaxHistory)
            {
                history = history.Take(MaxHistory).ToList();
            }

            SaveHistory(type, history);
        }

        /// <summary>
        /// Retrieves the list of history items for a specific search type.
        /// </summary>
        public List<string> GetHistory(string type)
        {
            if (_config == null) return new List<string>();

            string json = _config.GetValue(SectionName, type);
            if (string.IsNullOrEmpty(json)) return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Clears history for a specific search type.
        /// </summary>
        public void ClearHistory(string type)
        {
            if (_config == null) return;
            SaveHistory(type, new List<string>());
        }

        private void SaveHistory(string type, List<string> history)
        {
            if (_config == null) return;
            try
            {
                string json = JsonSerializer.Serialize(history);
                _config.SetValue(SectionName, type, json);
                _config.Save();
            }
            catch { }
        }
    }
}
