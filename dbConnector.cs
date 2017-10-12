using System;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DbConn
{
	public class dbConnector
	{
		public bool TestDbConn(string DbServer, string DbDatabase, bool DispError)
		{
			bool result = true;
			string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase;
			SqlConnection sqlConnection = new SqlConnection();
			try
			{
				if (DbServer == "" || DbDatabase == "")
				{
					throw new ArgumentException("Cannot connect to Database when a blank 'Server Name' or 'Database Name' is specified.");
				}
				sqlConnection.ConnectionString = connectionString;
				sqlConnection.Open();
			}
			catch (Exception ex)
			{
				if (DispError)
				{
					MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				result = false;
			}
			sqlConnection.Close();
			sqlConnection.Dispose();
			return result;
		}
	}
}
