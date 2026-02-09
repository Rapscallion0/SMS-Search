using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Search.Data
{
    /// <summary>
    /// Interface defining the data access contract for the application.
    /// Supports asynchronous query execution, pagination, schema retrieval, and metadata operations.
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Gets the total count of records for a query, optionally filtered.
        /// </summary>
        Task<int> GetQueryCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific page of data for the grid.
        /// </summary>
        Task<DataTable> GetQueryPageAsync(string server, string database, string user, string pass, string sql, object parameters, int offset, int limit, string sortCol, string sortDir, string filter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the schema (column names and types) for the query without fetching data.
        /// </summary>
        Task<DataTable> GetQuerySchemaAsync(string server, string database, string user, string pass, string sql, object parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates the total number of matches for a specific search text across specified columns.
        /// </summary>
        Task<long> GetTotalMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, Dictionary<string, string> columnTypes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates the number of matches occurring before a specific row index (for "Match X of Y" display).
        /// </summary>
        Task<long> GetPrecedingMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, Dictionary<string, string> columnTypes, int limitRowIndex, string sortCol, string sortDir, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds the row index of the next/previous match relative to a start index.
        /// </summary>
        Task<int> GetMatchRowIndexAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string searchText, Dictionary<string, string> columnTypes, int startRowIndex, string sortCol, string sortDir, bool forward, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a DbDataReader for streaming results (used for Exports).
        /// </summary>
        Task<DbDataReader> GetQueryDataReaderAsync(string server, string database, string user, string pass, string sql, object parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of tables from the database.
        /// </summary>
        Task<IEnumerable<string>> GetTablesAsync(string server, string database, string user, string pass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of databases from the server.
        /// </summary>
        Task<IEnumerable<string>> GetDatabasesAsync(string server, string user, string pass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves friendly descriptions for columns from the RB_FIELDS table.
        /// </summary>
        Task<List<string>> GetColumnDescriptionsAsync(string server, string database, string user, string pass, IEnumerable<string> columnNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tests the database connection.
        /// </summary>
        Task<bool> TestConnectionAsync(string server, string database, string user, string pass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a raw query and returns the full DataTable (Legacy/Non-virtual mode).
        /// </summary>
        Task<DataTable> ExecuteQueryAsync(string server, string database, string user, string pass, string sql, object parameters = null, CancellationToken cancellationToken = default);
    }
}
