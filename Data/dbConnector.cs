using SMS_Search.Utils;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Search.Data
{
	public class dbConnector
	{
        private Logfile log = new Logfile();
        private DataRepository _repo = new DataRepository();

		public bool TestDbConn(string DbServer, string DbDatabase, bool DispError, string dbUser = null, string dbPassword = null)
		{
            log.Logger(LogLevel.Info, $"TestDbConn: Testing connection to Server='{DbServer}' Database='{DbDatabase}'");

			// Validate input
			if (string.IsNullOrWhiteSpace(DbServer) || string.IsNullOrWhiteSpace(DbDatabase))
			{
                log.Logger(LogLevel.Warning, "TestDbConn: Blank Server or Database");
				if (DispError)
				{
					MessageBox.Show("Cannot connect: blank Server or Database.", "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				return false;
			}

			string connectionString = _repo.GetConnectionString(DbServer, DbDatabase, dbUser, dbPassword);

			try
			{
				using (var sqlConnection = new SqlConnection(connectionString))
				{
                    Stopwatch sw = Stopwatch.StartNew();
					sqlConnection.Open();
                    sw.Stop();
                    log.Logger(LogLevel.Info, $"TestDbConn: Connection successful ({sw.ElapsedMilliseconds}ms)");
				}
				return true;
			}
			catch (Exception ex)
			{
                log.Logger(LogLevel.Error, "TestDbConn: Connection failed - " + ex.Message);
				if (DispError)
				{
					MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				// Do not terminate application here; return false and let caller decide next steps
				return false;
			}
		}

        public async Task<bool> TestDbConnAsync(string DbServer, string DbDatabase, bool DispError, string dbUser = null, string dbPassword = null)
        {
            log.Logger(LogLevel.Info, $"TestDbConnAsync: Testing connection to Server='{DbServer}' Database='{DbDatabase}'");

            try
            {
                if (string.IsNullOrEmpty(DbServer) || string.IsNullOrEmpty(DbDatabase))
                {
                    log.Logger(LogLevel.Warning, "TestDbConnAsync: Blank Server or Database");
                    if (DispError)
                    {
                         MessageBox.Show("Cannot connect: blank Server or Database.", "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    return false;
                }

                string connectionString = _repo.GetConnectionString(DbServer, DbDatabase, dbUser, dbPassword);

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    await sqlConnection.OpenAsync();
                    sw.Stop();
                    log.Logger(LogLevel.Info, $"TestDbConnAsync: Connection successful ({sw.ElapsedMilliseconds}ms)");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "TestDbConnAsync: Connection failed - " + ex.Message);
                if (DispError)
                {
                     MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                return false;
            }
        }
	}
}
