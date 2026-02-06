using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SMS_Search
{
    public class VirtualGridContext
    {
        private readonly DataRepository _repo;

        // Connection info
        private string _server;
        private string _database;
        private string _user;
        private string _pass;

        // Query info
        private string _baseSql;
        private object _parameters;

        // State
        public int TotalCount { get; private set; }
        public int UnfilteredCount { get; private set; }
        private Dictionary<int, DataRow> _cache;
        private HashSet<int> _pagesBeingFetched;
        private const int PageSize = 100;
        private int _version = 0;

        // Sorting & Filtering
        public string SortColumn { get; private set; }
        public string SortDirection { get; private set; } = "ASC";
        public string FilterText { get; private set; }
        private string _rawFilterText;
        private List<string> _filterColumns;

        private Dictionary<string, string> _columnSqlTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<string> SafeStringTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "char", "nchar", "varchar", "nvarchar", "text", "ntext", "sysname"
        };

        public event EventHandler DataReady;
        public event EventHandler<string> LoadError;

        public VirtualGridContext(DataRepository repo)
        {
            _repo = repo;
            _cache = new Dictionary<int, DataRow>();
            _pagesBeingFetched = new HashSet<int>();
        }

        public void SetConnection(string server, string database, string user, string pass)
        {
            _server = server;
            _database = database;
            _user = user;
            _pass = pass;
        }

        public async Task LoadAsync(string sql, object parameters, string initialSortColumn = null)
        {
            _baseSql = sql;
            _parameters = parameters;
            SortColumn = initialSortColumn;
            SortDirection = "ASC";
            FilterText = null;
            UnfilteredCount = 0; // Reset before reload

            await ReloadAsync();

            // On initial load, TotalCount is the UnfilteredCount
            if (string.IsNullOrEmpty(FilterText))
            {
                UnfilteredCount = TotalCount;
            }
        }

        public async Task ApplyFilterAsync(string filterText, IEnumerable<string> columns)
        {
            _rawFilterText = filterText;
            _filterColumns = new List<string>(columns);

            // Convert simple text filter to SQL WHERE
            // "val" -> "Col1 LIKE '%val%' OR Col2 LIKE '%val%'"
            if (string.IsNullOrWhiteSpace(filterText))
            {
                FilterText = null;
            }
            else
            {
                var clauses = new List<string>();
                string safeFilter = filterText.Replace("'", "''");
                foreach (var col in columns)
                {
                    if (_columnSqlTypes.TryGetValue(col, out string type) && SafeStringTypes.Contains(type))
                    {
                        clauses.Add($"[{col}] LIKE '%{safeFilter}%'");
                    }
                    else
                    {
                        clauses.Add($"CAST([{col}] AS NVARCHAR(MAX)) LIKE '%{safeFilter}%'");
                    }
                }
                FilterText = string.Join(" OR ", clauses);
            }

            await ReloadAsync();
        }

        public async Task<long> GetTotalMatchCountAsync()
        {
            if (string.IsNullOrWhiteSpace(_rawFilterText) || _filterColumns == null || _filterColumns.Count == 0)
                return 0;

            var colTypes = new Dictionary<string, string>();
            foreach(var col in _filterColumns)
            {
                if(_columnSqlTypes.TryGetValue(col, out string type)) colTypes[col] = type;
                else colTypes[col] = null;
            }

            return await _repo.GetTotalMatchCountAsync(_server, _database, _user, _pass, _baseSql, _parameters, FilterText, _rawFilterText, colTypes);
        }

        public async Task<long> GetPrecedingMatchCountAsync(int limitRowIndex)
        {
            if (string.IsNullOrWhiteSpace(_rawFilterText) || _filterColumns == null || _filterColumns.Count == 0 || limitRowIndex <= 0)
                return 0;

            var colTypes = new Dictionary<string, string>();
            foreach (var col in _filterColumns)
            {
                if (_columnSqlTypes.TryGetValue(col, out string type)) colTypes[col] = type;
                else colTypes[col] = null;
            }

            return await _repo.GetPrecedingMatchCountAsync(_server, _database, _user, _pass, _baseSql, _parameters, FilterText, _rawFilterText, colTypes, limitRowIndex, SortColumn, SortDirection);
        }

        public async Task WaitForRowAsync(int rowIndex)
        {
            if (_cache.ContainsKey(rowIndex)) return;

            // Trigger fetch by accessing (ignoring return)
            GetValue(rowIndex, 0);

            // Poll for data arrival
            int timeout = 5000; // 5 seconds max wait
            int interval = 50;
            while (timeout > 0)
            {
                if (_cache.ContainsKey(rowIndex)) return;
                await Task.Delay(interval);
                timeout -= interval;
            }
        }

        public async Task EnsureRangeLoadedAsync(int startIndex, int count)
        {
            if (count <= 0) return;
            var tasks = new List<Task>();
            int endRow = startIndex + count;

            // Trigger fetch for all pages in range first
            for (int i = startIndex; i < endRow; i += PageSize)
            {
                GetValue(i, 0);
            }

            // Then wait for all of them
            for (int i = startIndex; i < endRow; i += PageSize)
            {
                tasks.Add(WaitForRowAsync(i));
            }

            await Task.WhenAll(tasks);
        }

        public async Task ApplySortAsync(string column)
        {
            if (SortColumn == column)
            {
                SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                SortColumn = column;
                SortDirection = "ASC";
            }

            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            _version++;
            int currentVersion = _version;

            try
            {
                _cache.Clear();
                _pagesBeingFetched.Clear();

                // If filter/sort makes query invalid, this throws
                TotalCount = await _repo.GetQueryCountAsync(_server, _database, _user, _pass, _baseSql, _parameters, FilterText);

                if (_version == currentVersion)
                {
                    DataReady?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                LoadError?.Invoke(this, ex.Message);
            }
        }

        public object GetValue(int rowIndex, int colIndex)
        {
            if (_cache.TryGetValue(rowIndex, out DataRow row))
            {
                if (colIndex >= 0 && colIndex < row.Table.Columns.Count)
                    return row[colIndex];
                return "";
            }

            // Not in cache, trigger fetch
            RequestPage(rowIndex);
            return null; // Return null so grid displays empty/default. "..." might break typed columns if we used them.
        }

        private async void RequestPage(int rowIndex)
        {
            int pageIndex = rowIndex / PageSize;

            if (_pagesBeingFetched.Contains(pageIndex)) return;

            int currentVersion = _version;
            _pagesBeingFetched.Add(pageIndex);

            try
            {
                int offset = pageIndex * PageSize;

                // Capture state for this request
                string sortCol = SortColumn;
                string sortDir = SortDirection;
                string filter = FilterText;

                var dt = await _repo.GetQueryPageAsync(_server, _database, _user, _pass, _baseSql, _parameters, offset, PageSize, sortCol, sortDir, filter);

                // Check version before updating cache
                if (_version != currentVersion) return;

                // Add to cache
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int absIndex = offset + i;
                    // We overwrite if exists (unlikely given check)
                    _cache[absIndex] = dt.Rows[i];
                }

                // Notify UI to repaint
                DataReady?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Page fetch error: " + ex.Message);
            }
            finally
            {
                if (_version == currentVersion)
                {
                    _pagesBeingFetched.Remove(pageIndex);
                }
            }
        }

        public async Task<DataTable> GetSchemaAsync(string sql, object parameters)
        {
             var dt = await _repo.GetQuerySchemaAsync(_server, _database, _user, _pass, sql, parameters);
             _columnSqlTypes.Clear();
             foreach(DataColumn col in dt.Columns)
             {
                 if(col.ExtendedProperties.ContainsKey("SqlType"))
                 {
                     _columnSqlTypes[col.ColumnName] = col.ExtendedProperties["SqlType"] as string;
                 }
             }
             return dt;
        }

        public async Task ExportToCsvAsync(string filename, Dictionary<string, string> headerMap = null, bool includeHeaders = true)
        {
             string finalSql = _baseSql;
             if (!string.IsNullOrWhiteSpace(FilterText))
             {
                 finalSql = $"SELECT * FROM ({_baseSql}) AS _FilterQ WHERE {FilterText}";
             }

             // Ensure sort is applied to export?
             // Ideally yes, user expects export to match grid order.
             if (!string.IsNullOrEmpty(SortColumn))
             {
                string safeCol = SortColumn.Replace("[", "").Replace("]", "");
                // If the query is wrapped for filtering, we can just append ORDER BY
                // If it wasn't wrapped, we wrap it now for sorting
                if (string.IsNullOrWhiteSpace(FilterText))
                {
                    finalSql = $"SELECT * FROM ({finalSql}) AS _SortQ ORDER BY [{safeCol}] {SortDirection}";
                }
                else
                {
                    // Already wrapped as _FilterQ, just append ORDER BY
                    finalSql += $" ORDER BY [{safeCol}] {SortDirection}";
                }
             }

             using (var reader = await _repo.GetQueryDataReaderAsync(_server, _database, _user, _pass, finalSql, _parameters))
             {
                 using (var writer = new System.IO.StreamWriter(filename))
                 {
                     // Write headers
                     if (includeHeaders)
                     {
                         for (int i = 0; i < reader.FieldCount; i++)
                         {
                             if (i > 0) writer.Write(",");
                             string colName = reader.GetName(i);
                             string header = (headerMap != null && headerMap.ContainsKey(colName)) ? headerMap[colName] : colName;
                             writer.Write("\"" + header.Replace("\"", "\"\"") + "\"");
                         }
                         writer.WriteLine();
                     }

                     while (await reader.ReadAsync())
                     {
                         for (int i = 0; i < reader.FieldCount; i++)
                         {
                             if (i > 0) writer.Write(",");
                             var val = reader.GetValue(i);
                             string sVal = val == DBNull.Value ? "" : val.ToString();
                             writer.Write("\"" + sVal.Replace("\"", "\"\"") + "\"");
                         }
                         writer.WriteLine();
                     }
                 }
             }
        }
    }
}
