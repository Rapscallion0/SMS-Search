using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbConn
{
	public class dbConnector
	{
		public bool TestDbConn(string DbServer, string DbDatabase, bool DispError, string dbUser = null, string dbPassword = null)
		{
			// Validate input
			if (string.IsNullOrWhiteSpace(DbServer) || string.IsNullOrWhiteSpace(DbDatabase))
			{
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
					sqlConnection.Open();
				}
				return true;
			}
			catch (Exception ex)
			{
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
                    await sqlConnection.OpenAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
	}
}
