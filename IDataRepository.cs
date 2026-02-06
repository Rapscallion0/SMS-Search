using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Search
{
    public interface IDataRepository
    {
        Task<int> GetQueryCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filter = null);
        Task<DataTable> GetQueryPageAsync(string server, string database, string user, string pass, string sql, object parameters, int offset, int limit, string sortCol, string sortDir, string filter = null);
        Task<DataTable> GetQuerySchemaAsync(string server, string database, string user, string pass, string sql, object parameters);
        Task<long> GetTotalMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, Dictionary<string, string> columnTypes);
        Task<long> GetPrecedingMatchCountAsync(string server, string database, string user, string pass, string sql, object parameters, string filterClause, string filterText, Dictionary<string, string> columnTypes, int limitRowIndex, string sortCol, string sortDir);
        Task<DbDataReader> GetQueryDataReaderAsync(string server, string database, string user, string pass, string sql, object parameters);
    }
}
