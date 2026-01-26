using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace SMS_Search
{
    public class DataRepository
    {
        public string GetConnectionString(string server, string database, string user, string pass)
        {
            if (string.IsNullOrEmpty(user))
            {
                return $"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={database};Data Source={server}";
            }
            else
            {
                return $"Data Source={server};Initial Catalog={database};User ID={user};Password={pass};Persist Security Info=False;";
            }
        }

        public async Task<bool> TestConnectionAsync(string server, string database, string user, string pass)
        {
            try
            {
                using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
                {
                    await conn.OpenAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool TestConnection(string server, string database, string user, string pass)
        {
            try
            {
                using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string server, string database, string user, string pass, string sql, object parameters = null)
        {
            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                using (var reader = await conn.ExecuteReaderAsync(sql, parameters))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public async Task<int> GetQueryCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filter = null)
        {
            string finalSql = ApplyFilter(sql, filter);
            // Inject TOP 100 PERCENT to handle potential inner ORDER BYs if the user provided one in Custom SQL
            // But this is tricky to do robustly with string manipulation.
            // We will rely on the caller (QueryBuilder) to provide clean SQL, or fallback if it fails.
            string countSql = $"SELECT COUNT(*) FROM ({finalSql}) AS _CountQ";

            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                return await conn.ExecuteScalarAsync<int>(countSql, parameters);
            }
        }

        public async Task<DataTable> GetQueryPageAsync(string server, string database, string user, string pass, string sql, object parameters, int offset, int limit, string sortCol, string sortDir, string filter = null)
        {
            string finalSql = ApplyFilter(sql, filter);

            // Ensure we have an ORDER BY for OFFSET
            string orderBy = "(SELECT NULL)";
            if (!string.IsNullOrEmpty(sortCol))
            {
                // Sanitize sortCol (remove brackets to be safe, then re-add)
                string safeCol = sortCol.Replace("[", "").Replace("]", "");
                orderBy = $"[{safeCol}] {sortDir}";
            }

            string pageSql = $@"
                SELECT * FROM ({finalSql}) AS _PageQ
                ORDER BY {orderBy}
                OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";

            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                using (var reader = await conn.ExecuteReaderAsync(pageSql, parameters))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public async Task<DataTable> GetQuerySchemaAsync(string server, string database, string user, string pass, string sql, object parameters)
        {
            // Get schema (0 rows)
            string schemaSql = $"SELECT TOP 0 * FROM ({sql}) AS _SchemaQ";

            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                using (var reader = await conn.ExecuteReaderAsync(schemaSql, parameters))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public async Task<SqlDataReader> GetQueryDataReaderAsync(string server, string database, string user, string pass, string sql, object parameters)
        {
            var conn = new SqlConnection(GetConnectionString(server, database, user, pass));
            await conn.OpenAsync();

            using (var cmd = new SqlCommand(sql, conn))
            {
                // We must manually map Dapper parameters because Dapper's ExecuteReaderAsync
                // does not support CommandBehavior.CloseConnection, which is critical here.
                if (parameters is DynamicParameters dp)
                {
                    foreach (var name in dp.ParameterNames)
                    {
                        var val = dp.Get<object>(name);
                        cmd.Parameters.AddWithValue(name, val ?? DBNull.Value);
                    }
                }

                // Caller is responsible for disposing reader which closes connection
                return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        private string ApplyFilter(string sql, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return sql;
            // The filter must be a valid WHERE clause snippet
            return $"SELECT * FROM ({sql}) AS _FilterQ WHERE {filter}";
        }

        public async Task<IEnumerable<string>> GetDatabasesAsync(string server, string user, string pass)
        {
            // Connect to master
            using (var conn = new SqlConnection(GetConnectionString(server, "master", user, pass)))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<string>("SELECT name FROM sys.databases ORDER BY name");
            }
        }

        public async Task<IEnumerable<string>> GetTablesAsync(string server, string database, string user, string pass)
        {
            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<string>("SELECT NAME FROM sys.tables ORDER BY NAME");
            }
        }

        public async Task<List<string>> GetColumnDescriptionsAsync(string server, string database, string user, string pass, IEnumerable<string> columnNames)
        {
             using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
             {
                 await conn.OpenAsync();
                 // Batch query
                 string sql = "SELECT F1453 as Name, F1454 as Description FROM RB_FIELDS WHERE F1453 IN @Names";
                 var result = await conn.QueryAsync(sql, new { Names = columnNames });

                 var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                 foreach(var r in result)
                 {
                     dict[r.Name.ToString()] = r.Description.ToString();
                 }

                 var list = new List<string>();
                 foreach(var name in columnNames)
                 {
                     if (dict.TryGetValue(name, out string desc))
                         list.Add(desc);
                     else
                         list.Add("");
                 }
                 return list;
             }
        }

        public async Task<long> GetTotalMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, IEnumerable<string> columns)
        {
            if (string.IsNullOrWhiteSpace(filterText)) return 0;
            string safeFilter = filterText.Replace("'", "''");

            List<string> sumParts = new List<string>();
            foreach (var col in columns)
            {
                sumParts.Add($"(CASE WHEN CAST([{col}] AS NVARCHAR(MAX)) LIKE '%{safeFilter}%' THEN 1 ELSE 0 END)");
            }
            string sumExpression = string.Join(" + ", sumParts);

            string finalSql = ApplyFilter(sql, filterClause);
            string countSql = $"SELECT SUM((0 + {sumExpression})) FROM ({finalSql}) AS _CountQ";

            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                var result = await conn.ExecuteScalarAsync<object>(countSql, parameters);
                if (result == null || result == DBNull.Value) return 0;
                return Convert.ToInt64(result);
            }
        }

        public async Task<long> GetPrecedingMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, IEnumerable<string> columns, int limitRowIndex, string sortCol, string sortDir)
        {
            if (limitRowIndex <= 0) return 0;
            if (string.IsNullOrWhiteSpace(filterText)) return 0;
            string safeFilter = filterText.Replace("'", "''");

            List<string> sumParts = new List<string>();
            foreach (var col in columns)
            {
                sumParts.Add($"(CASE WHEN CAST([{col}] AS NVARCHAR(MAX)) LIKE '%{safeFilter}%' THEN 1 ELSE 0 END)");
            }
            string sumExpression = string.Join(" + ", sumParts);

            string finalSql = ApplyFilter(sql, filterClause);

            string orderBy = "(SELECT NULL)";
            if (!string.IsNullOrEmpty(sortCol))
            {
                string safeCol = sortCol.Replace("[", "").Replace("]", "");
                orderBy = $"[{safeCol}] {sortDir}";
            }

            // Using OFFSET/FETCH in subquery to limit rows
            string countSql = $@"
                SELECT SUM((0 + {sumExpression}))
                FROM (
                    SELECT * FROM ({finalSql}) AS _Base
                    ORDER BY {orderBy}
                    OFFSET 0 ROWS FETCH NEXT {limitRowIndex} ROWS ONLY
                ) AS _Preceding";

            using (var conn = new SqlConnection(GetConnectionString(server, database, user, pass)))
            {
                await conn.OpenAsync();
                var result = await conn.ExecuteScalarAsync<object>(countSql, parameters);
                if (result == null || result == DBNull.Value) return 0;
                return Convert.ToInt64(result);
            }
        }
    }
}
