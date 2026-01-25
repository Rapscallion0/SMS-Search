using Log;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbConn
{
	public class dbConnector
	{
        private Logfile log = new Logfile();

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

			string connectionString;
            if (!string.IsNullOrEmpty(dbUser))
            {
                connectionString = "Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase + ";User ID=" + dbUser + ";Password=" + dbPassword + ";Persist Security Info=False;";
            }
            else
            {
                connectionString = "Integrated Security=SSPI;Persist Security Info=False;Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase;
            }

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

        public async Task<bool> TestDbConnAsync(string DbServer, string DbDatabase, string dbUser = null, string dbPassword = null)
        {
            log.Logger(LogLevel.Info, $"TestDbConnAsync: Testing connection to Server='{DbServer}' Database='{DbDatabase}'");

            string connectionString;
            if (!string.IsNullOrEmpty(dbUser))
            {
                connectionString = "Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase + ";User ID=" + dbUser + ";Password=" + dbPassword + ";Persist Security Info=False;";
            }
            else
            {
                connectionString = "Integrated Security=SSPI;Persist Security Info=False;Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (string.IsNullOrEmpty(DbServer) || string.IsNullOrEmpty(DbDatabase))
                    {
                        throw new ArgumentException("Cannot connect to Database when a blank 'Server Name' or 'Database Name' is specified.");
                    }
                    Stopwatch sw = Stopwatch.StartNew();
                    await sqlConnection.OpenAsync();
                    sw.Stop();
                    log.Logger(LogLevel.Info, $"TestDbConnAsync: Connection successful ({sw.ElapsedMilliseconds}ms)");
                    return true;
                }
                catch (Exception ex)
                {
                    log.Logger(LogLevel.Error, "TestDbConnAsync: Connection failed - " + ex.Message);
                    return false;
                }
            }
        }
	}
}
