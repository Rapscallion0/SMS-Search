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

                 // Map back to list in order? Or return dictionary?
                 // Original logic returned a list matching the input columns index.

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
    }
}
