using DbConn;
using Ini;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Versions;
namespace SMS_Search
{
	public partial class frmConfig : Form
	{

		public frmConfig()
		{
            InitializeComponent();
			base.StartPosition = FormStartPosition.Manual;
			base.Top = (Screen.PrimaryScreen.WorkingArea.Height - base.Height) / 2;
			base.Left = (Screen.PrimaryScreen.WorkingArea.Width - base.Width) / 2;
            lblConfigFilePath.Text = Path.GetFullPath(frmConfig.ConfigFilePath);
            toolTip1.SetToolTip(lblConfigFilePath, Path.GetFullPath(frmConfig.ConfigFilePath));
		}
		private void frmConfig_Load(object sender, EventArgs e)
		{
            loadConfig();
		}
		private async void frmConfig_Shown(object sender, EventArgs e)
		{
            Cursor = Cursors.WaitCursor;

			if (ServerNames.Count < 1)
			{
                await PopulateDbServersAsync();
			}

            if (cmbDbServer.DataSource != ServerNames)
            {
                cmbDbServer.DataSource = ServerNames;
            }

            Cursor = Cursors.Default;
		}

        private async Task PopulateDbServersAsync()
        {
            bool scanNetwork = chkScanNetwork.Checked;
            List<string> servers = await Task.Run(() => GetDbServersList(scanNetwork));

            if (this.IsDisposed || cmbDbServer.IsDisposed) return;

            ServerNames.Clear();
            foreach (var server in servers)
            {
                ServerNames.Add(server);
            }
            ServerNames.Sort();

            cmbDbServer.DataSource = null;
            cmbDbServer.DataSource = ServerNames;
        }

		private List<string> GetDbServersList(bool scanNetwork)
		{
            List<string> list = new List<string>();
			if (!scanNetwork)
			{
                RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view: RegistryView.Registry64);
                RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                if (key != null)
                {
                    foreach(string sqlInstance in key.GetValueNames())
				    {
					    if (sqlInstance == "MSSQLSERVER")
					    {
                            list.Add(Environment.MachineName);
					    }
					    else
					    {
                            list.Add(Environment.MachineName + "\\" + sqlInstance);
					    }
				    }
                }
			}
			else
			{
				SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
				DataTable dataSources = instance.GetDataSources();
				DataRow[] array3 = dataSources.Select("");
				DataRow[] array4 = array3;
				for (int j = 0; j < array4.Length; j++)
				{
					DataRow dataRow = array4[j];
					if (dataRow["InstanceName"] is string)
					{
                        list.Add(dataRow["ServerName"] + "\\" + dataRow["InstanceName"]);
					}
					else
					{
                        list.Add(dataRow["ServerName"].ToString());
					}
				}
			}
            return list;
		}

		private void GetDbNames()
		{
			if (DbNames.Count < 1)
			{
                cmbDbDatabase.Items.Clear();
				string connectionString = "Data Source=" + cmbDbServer.Text + "; Integrated Security=True;";
				try
				{
                    Cursor = Cursors.WaitCursor;
					using (SqlConnection sqlConnection = new SqlConnection(connectionString))
					{
						sqlConnection.Open();
						DataTable schema = sqlConnection.GetSchema("Databases");
						sqlConnection.Close();
						foreach (DataRow dataRow in schema.Rows)
						{
                            cmbDbDatabase.Items.Add(dataRow["database_name"]);
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				finally
				{
                    Cursor = Cursors.Default;
				}
			}
            DbNames.Sort();
		}
		private void lblConfigFilePath_Click(object sender, EventArgs e)
		{
			Process.Start(frmConfig.ConfigFilePath);
		}
		private void loadConfig()
		{
			if (ini.IniReadValue("CONNECTION", "SCANNETWORK") == "1")
			{
                chkScanNetwork.Checked = true;
			}
			else
			{
                chkScanNetwork.Checked = false;
			}
			if (ini.IniReadValue("CONNECTION", "SERVER") == "")
			{
                cmbDbDatabase.Enabled = false;
			}
			else
			{
                cmbDbServer.Text = ini.IniReadValue("CONNECTION", "SERVER");
                cmbDbDatabase.Enabled = true;
			}
            cmbDbDatabase.Text = ini.IniReadValue("CONNECTION", "DATABASE");
			if (ini.IniReadValue("CONNECTION", "WINDOWSAUTH") == "1" || ini.IniReadValue("CONNECTION", "WINDOWSAUTH") == "")
			{
                chkWindowsAuth.Checked = true;
                txtDbUser.Visible = false;
                txtDbPassword.Visible = false;
                lblDbUser.Visible = false;
                lblDbPassword.Visible = false;
				base.Height = MinimumSize.Height;
			}
			else
			{
                chkWindowsAuth.Checked = false;
                txtDbUser.Visible = true;
                txtDbPassword.Visible = true;
                lblDbUser.Visible = true;
                lblDbPassword.Visible = true;
				base.Height = MaximumSize.Height;
			}
            txtDbUser.Text = ini.IniReadValue("CONNECTION", "SQLUSER");
            txtDbPassword.Text = ini.IniReadValue("CONNECTION", "SQLPASSWORD");
			if (ini.IniReadValue("GENERAL", "DEBUG_LOG") == "1")
			{
                chkLogging.Checked = true;
			}
			else
			{
                chkLogging.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "MULTI_INSTANCE") == "1")
			{
                chkMultiInstance.Checked = true;
			}
			else
			{
                chkMultiInstance.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "ALWAYSONTOP") == "1")
			{
                chkAlwaysOnTop.Checked = true;
			}
			else
			{
                chkAlwaysOnTop.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "SHOWINTRAY") == "1")
			{
                chkShowInTray.Checked = true;
			}
			else
			{
                chkShowInTray.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "SEARCHANY") == "1")
			{
                chkSearchAny.Checked = true;
			}
			else
			{
                chkSearchAny.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "DESCRIPTIONCOLUMNS") == "1")
			{
                chkDescriptionColumns.Checked = true;
			}
			else
			{
                chkDescriptionColumns.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "RESIZECOLUMNS") == "1")
			{
                chkResizeColumns.Checked = true;
			}
			else
			{
                chkResizeColumns.Checked = false;
			}
			string a;
			if ((a = ini.IniReadValue("GENERAL", "START_TAB")) != null)
			{
				if (a == "FCT_TAB")
				{
                    cmbStartTab.Text = "Function";
					goto IL_3FF;
				}
				if (a == "TLZ_TAB")
				{
                    cmbStartTab.Text = "Totalizer";
					goto IL_3FF;
				}
				if (a == "FIELDS")
				{
                    cmbStartTab.Text = "Fields";
					goto IL_3FF;
				}
			}
            cmbStartTab.Text = "Function";
			IL_3FF:
			string a2;
			if ((a2 = ini.IniReadValue("GENERAL", "TABLE_LOOKUP")) != null)
			{
				if (a2 == "FIELDS")
				{
                    cmbTableLookup.Text = "Show Fields";
					goto IL_468;
				}
				if (a2 == "RECORDS")
				{
                    cmbTableLookup.Text = "Show Records";
					goto IL_468;
				}
			}
            cmbTableLookup.Text = "Show Fields";
			IL_468:
			if (ini.IniReadValue("GENERAL", "COPYCLEANSQL") == "1")
			{
                chkCopyCleanSql.Checked = true;
			}
			else
			{
                chkCopyCleanSql.Checked = false;
			}
			if (ini.IniReadValue("GENERAL", "CHECKUPDATE") == "1")
			{
                chkCheckUpdate.Checked = true;
			}
			else
			{
                chkCheckUpdate.Checked = false;
			}
			if (ini.IniReadValue("QUERY", "FUNCTION") == "")
			{
                txtQryFct.Text = "F1063, F1064, F1051, F1050, F1081";
			}
			else
			{
                txtQryFct.Text = ini.IniReadValue("QUERY", "FUNCTION");
			}
			if (ini.IniReadValue("QUERY", "TOTALIZER") == "")
			{
                txtQryTlz.Text = "F1034, F1039, F1128, F1129, F1179, F1253, F1710, F1131, F1048, F1709";
			}
			else
			{
                txtQryTlz.Text = ini.IniReadValue("QUERY", "TOTALIZER");
			}
			if (ini.IniReadValue("UNARCHIVE", "SHOWTARGET") == "1")
			{
                chkUnarchiveTarget.Checked = true;
				return;
			}
            chkUnarchiveTarget.Checked = false;
		}
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (dbConn.TestDbConn(cmbDbServer.Text, cmbDbDatabase.Text, true))
			{
                SaveEdits();
				base.Close();
			}
		}
		private void btnApplyConfig_Click(object sender, EventArgs e)
		{
			if (dbConn.TestDbConn(cmbDbServer.Text, cmbDbDatabase.Text, true))
			{
                SaveEdits();
			}
		}
		private void SaveEdits()
		{
			if (chkScanNetwork.Checked)
			{
                ini.IniWriteValue("CONNECTION", "SCANNETWORK", "1");
			}
			else
			{
                ini.IniWriteValue("CONNECTION", "SCANNETWORK", "0");
			}
            ini.IniWriteValue("CONNECTION", "SERVER", cmbDbServer.Text);
            ini.IniWriteValue("CONNECTION", "DATABASE", cmbDbDatabase.Text);
			if (chkWindowsAuth.Checked)
			{
                ini.IniWriteValue("CONNECTION", "WINDOWSAUTH", "1");
                ini.IniWriteValue("CONNECTION", "SQLUSER", txtDbUser.Text);
                ini.IniWriteValue("CONNECTION", "SQLPASSWORD", txtDbPassword.Text);
			}
			else
			{
                ini.IniWriteValue("CONNECTION", "WINDOWSAUTH", "0");
                ini.IniWriteValue("CONNECTION", "SQLUSER", "");
                ini.IniWriteValue("CONNECTION", "SQLPASSWORD", "");
			}
			if (chkLogging.Checked)
			{
                ini.IniWriteValue("GENERAL", "DEBUG_LOG", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "DEBUG_LOG", "0");
			}
			if (chkMultiInstance.Checked)
			{
                ini.IniWriteValue("GENERAL", "MULTI_INSTANCE", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "MULTI_INSTANCE", "0");
			}
			if (chkAlwaysOnTop.Checked)
			{
                ini.IniWriteValue("GENERAL", "ALWAYSONTOP", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "ALWAYSONTOP", "0");
			}
			if (chkShowInTray.Checked)
			{
                ini.IniWriteValue("GENERAL", "SHOWINTRAY", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "SHOWINTRAY", "0");
			}
			if (chkSearchAny.Checked)
			{
                ini.IniWriteValue("GENERAL", "SEARCHANY", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "SEARCHANY", "0");
			}
			if (chkDescriptionColumns.Checked)
			{
                ini.IniWriteValue("GENERAL", "DESCRIPTIONCOLUMNS", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "DESCRIPTIONCOLUMNS", "0");
			}
			if (chkResizeColumns.Checked)
			{
                ini.IniWriteValue("GENERAL", "RESIZECOLUMNS", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "RESIZECOLUMNS", "0");
			}
			string text;
			if ((text = cmbStartTab.Text) != null)
			{
				if (text == "Function")
				{
                    ini.IniWriteValue("GENERAL", "START_TAB", "FCT_TAB");
					goto IL_3B5;
				}
				if (text == "Totalizer")
				{
                    ini.IniWriteValue("GENERAL", "START_TAB", "TLZ_TAB");
					goto IL_3B5;
				}
				if (text == "Fields")
				{
                    ini.IniWriteValue("GENERAL", "START_TAB", "FIELDS");
					goto IL_3B5;
				}
			}
            ini.IniWriteValue("GENERAL", "START_TAB", "FCT_TAB");
			IL_3B5:
			string text2;
			if ((text2 = cmbTableLookup.Text) != null)
			{
				if (text2 == "Show Fields")
				{
                    ini.IniWriteValue("GENERAL", "TABLE_LOOKUP", "FIELDS");
					goto IL_432;
				}
				if (text2 == "Show Records")
				{
                    ini.IniWriteValue("GENERAL", "TABLE_LOOKUP", "RECORDS");
					goto IL_432;
				}
			}
            ini.IniWriteValue("GENERAL", "TABLE_LOOKUP", "FIELDS");
			IL_432:
			if (chkCopyCleanSql.Checked)
			{
                ini.IniWriteValue("GENERAL", "COPYCLEANSQL", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "COPYCLEANSQL", "0");
			}
			if (chkCheckUpdate.Checked)
			{
                ini.IniWriteValue("GENERAL", "CHECKUPDATE", "1");
			}
			else
			{
                ini.IniWriteValue("GENERAL", "CHECKUPDATE", "0");
			}
            ini.IniWriteValue("QUERY", "FUNCTION", txtQryFct.Text);
            ini.IniWriteValue("QUERY", "TOTALIZER", txtQryTlz.Text);
			if (chkUnarchiveTarget.Checked)
			{
                ini.IniWriteValue("UNARCHIVE", "SHOWTARGET", "1");
				return;
			}
            ini.IniWriteValue("UNARCHIVE", "SHOWTARGET", "0");
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (!dbConn.TestDbConn(cmbDbServer.Text, cmbDbDatabase.Text, false))
			{
				Application.Exit();
			}
			base.Close();
		}
		private void btnReloadConfig_Click(object sender, EventArgs e)
		{
            loadConfig();
		}
		private void verifyUnArchInstall()
		{
		}
		private void btnInstallArch_Click(object sender, EventArgs e)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			bool flag = true;
			try
			{
				registryKey = Registry.ClassesRoot.CreateSubKey("*\\shell\\UnArchiver");
				if (registryKey != null)
				{
					registryKey.SetValue("", "UnArchive");
				}
				registryKey2 = Registry.ClassesRoot.CreateSubKey("*\\shell\\UnArchiver\\command");
				if (registryKey2 != null)
				{
					registryKey2.SetValue("", "@" + Application.ExecutablePath + " %1");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Install UnArchiver", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
			if (flag)
			{
				MessageBox.Show("UnArchiver Installed successfully", "Install UnArchiver", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		private void btnRemoveArch_Click(object sender, EventArgs e)
		{
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("*\\shell\\UnArchiver\\command");
				if (registryKey != null)
				{
					registryKey.Close();
					Registry.ClassesRoot.DeleteSubKey("*\\shell\\UnArchiver\\command");
				}
				registryKey = Registry.ClassesRoot.OpenSubKey("*\\shell\\UnArchiver");
				if (registryKey != null)
				{
					registryKey.Close();
					Registry.ClassesRoot.DeleteSubKey("*\\shell\\UnArchiver");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Uninstall UnArchiver", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			if (flag)
			{
				MessageBox.Show("UnArchiver uninstalled successfully", "Uninstall UnArchiver", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		private void btnTestConn_Click(object sender, EventArgs e)
		{
			if (dbConn.TestDbConn(cmbDbServer.Text, cmbDbDatabase.Text, true))
			{
				MessageBox.Show("Database connection passed.", "Test DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		private void validateServer()
		{
			bool flag = false;
			foreach (string b in ServerNames)
			{
				if (cmbDbServer.Text == b)
				{
                    GetDbNames();
                    cmbDbDatabase.Enabled = true;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
                cmbDbDatabase.Enabled = false;
                cmbDbDatabase.Text = "Select a valid Server";
			}
		}
		private void cmbDbServer_TextChanged(object sender, EventArgs e)
		{
            validateServer();
		}
		private void btnChkUpdate_Click(object sender, EventArgs e)
		{
            Cursor = Cursors.WaitCursor;
			string url = "https://sites.google.com/a/rapscallion.org/develop/sms-search/";
			Version newVersion = Versions.GetNewVersion(url, "SMS%20Search%20Version.xml", "SMSSearch");
			Version v = new Version(Application.ProductVersion);
			string text;
			if (newVersion > v)
			{
				text = string.Concat(new object[]
				{
					"There is an update available for download.\n\nCurrent Version:\t",
					Application.ProductVersion,
					"\nNew Version:\t",
					newVersion
				});
			}
			else
			{
				text = "You are running the latest version.\n\nCurrent Version:\t" + Application.ProductVersion;
			}
			MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Cursor = Cursors.Default;
		}
		private void chkWindowsAuth_CheckedChanged(object sender, EventArgs e)
		{
			if (chkWindowsAuth.Checked)
			{
                txtDbUser.Visible = false;
                txtDbPassword.Visible = false;
                lblDbUser.Visible = false;
                lblDbPassword.Visible = false;
				base.Height = MinimumSize.Height;
				return;
			}
            txtDbUser.Visible = true;
            txtDbPassword.Visible = true;
            lblDbUser.Visible = true;
            lblDbPassword.Visible = true;
			base.Height = MaximumSize.Height;
		}
    }
}
