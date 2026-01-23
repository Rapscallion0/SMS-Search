using DbConn;
// using Ini;
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
namespace SMS_Search
{
	public partial class frmConfig : Form
	{
        // ConfigFilePath is likely already defined in Designer or another part of the partial class
        // Changing access to utilize the existing one or remove if it duplicates logic.
        // Assuming the error CS0102 means it's duplicated.
        // Let's use the full path directly or rely on the other definition if accessible.
        // Since I cannot see the other part, and typically ConfigFilePath is static on frmMain, let's reference frmMain.ConfigFilePath if needed or just define a local constant.

        // However, looking at the code I injected, I added `private static string ConfigFilePath`.
        // If the original code had it, I should use that.
        // But `frmConfig.Designer.cs` usually doesn't have fields like that.
        // It's possible `frmConfig.cs` (this file) had it before I edited? No, I see my edit added it.
        // Ah, the error says "The type 'frmConfig' already contains a definition".

        // Wait, did I declare it twice in my previous edit? No.
        // Is it in the Designer? `frmConfig` usually just has controls.

        // Let's try removing my definition and use a hardcoded string or reference frmMain.ConfigFilePath.

        private ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMS Search.json"));

		public frmConfig()
		{
            InitializeComponent();
			base.StartPosition = FormStartPosition.Manual;
			base.Top = (Screen.PrimaryScreen.WorkingArea.Height - base.Height) / 2;
			base.Left = (Screen.PrimaryScreen.WorkingArea.Width - base.Width) / 2;
            lblConfigFilePath.Text = Path.Combine(Application.StartupPath, "SMS Search.json");
            toolTip1.SetToolTip(lblConfigFilePath, Path.Combine(Application.StartupPath, "SMS Search.json"));
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
				foreach (DataRow dataRow in dataSources.Rows)
				{
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
			Process.Start(Path.Combine(Application.StartupPath, "SMS Search.json"));
		}
		private void loadConfig()
		{
			if (config.GetValue("CONNECTION", "SCANNETWORK") == "1")
			{
                chkScanNetwork.Checked = true;
			}
			else
			{
                chkScanNetwork.Checked = false;
			}
			if (config.GetValue("CONNECTION", "SERVER") == "")
			{
                cmbDbDatabase.Enabled = false;
			}
			else
			{
                cmbDbServer.Text = config.GetValue("CONNECTION", "SERVER");
                cmbDbDatabase.Enabled = true;
			}
            cmbDbDatabase.Text = config.GetValue("CONNECTION", "DATABASE");
			if (config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || config.GetValue("CONNECTION", "WINDOWSAUTH") == "")
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
            txtDbUser.Text = config.GetValue("CONNECTION", "SQLUSER");
            txtDbPassword.Text = config.GetValue("CONNECTION", "SQLPASSWORD");
			if (config.GetValue("GENERAL", "DEBUG_LOG") == "1")
			{
                chkLogging.Checked = true;
			}
			else
			{
                chkLogging.Checked = false;
			}
			if (config.GetValue("GENERAL", "MULTI_INSTANCE") == "1")
			{
                chkMultiInstance.Checked = true;
			}
			else
			{
                chkMultiInstance.Checked = false;
			}
			if (config.GetValue("GENERAL", "ALWAYSONTOP") == "1")
			{
                chkAlwaysOnTop.Checked = true;
			}
			else
			{
                chkAlwaysOnTop.Checked = false;
			}
			if (config.GetValue("GENERAL", "SHOWINTRAY") == "1")
			{
                chkShowInTray.Checked = true;
			}
			else
			{
                chkShowInTray.Checked = false;
			}
			if (config.GetValue("GENERAL", "SEARCHANY") == "1")
			{
                chkSearchAny.Checked = true;
			}
			else
			{
                chkSearchAny.Checked = false;
			}
			if (config.GetValue("GENERAL", "DESCRIPTIONCOLUMNS") == "1")
			{
                chkDescriptionColumns.Checked = true;
			}
			else
			{
                chkDescriptionColumns.Checked = false;
			}
			if (config.GetValue("GENERAL", "RESIZECOLUMNS") == "1")
			{
                chkResizeColumns.Checked = true;
			}
			else
			{
                chkResizeColumns.Checked = false;
			}
			string a;
			if ((a = config.GetValue("GENERAL", "START_TAB")) != null)
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
			if ((a2 = config.GetValue("GENERAL", "TABLE_LOOKUP")) != null)
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
			if (config.GetValue("GENERAL", "COPYCLEANSQL") == "1")
			{
                chkCopyCleanSql.Checked = true;
			}
			else
			{
                chkCopyCleanSql.Checked = false;
			}
			if (config.GetValue("GENERAL", "CHECKUPDATE") == "1")
			{
                chkCheckUpdate.Checked = true;
			}
			else
			{
                chkCheckUpdate.Checked = false;
			}
			if (config.GetValue("QUERY", "FUNCTION") == "")
			{
                txtQryFct.Text = "F1063, F1064, F1051, F1050, F1081";
			}
			else
			{
                txtQryFct.Text = config.GetValue("QUERY", "FUNCTION");
			}
			if (config.GetValue("QUERY", "TOTALIZER") == "")
			{
                txtQryTlz.Text = "F1034, F1039, F1128, F1129, F1179, F1253, F1710, F1131, F1048, F1709";
			}
			else
			{
                txtQryTlz.Text = config.GetValue("QUERY", "TOTALIZER");
			}
			if (config.GetValue("UNARCHIVE", "SHOWTARGET") == "1")
			{
                chkUnarchiveTarget.Checked = true;
			}
            else
            {
                chkUnarchiveTarget.Checked = false;
            }

            LoadCleanSqlGrid(false);
		}

        private void btnResetCleanSql_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the Clean SQL rules to defaults?", "Reset Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoadCleanSqlGrid(true);
            }
        }

        private void LoadCleanSqlGrid(bool useDefaults)
        {
            dgvCleanSqlRules.Rows.Clear();
            List<SqlCleaningRule> rules = new List<SqlCleaningRule>();

            if (!useDefaults)
            {
                string countStr = config.GetValue("CLEAN_SQL", "Count");
                int count;
                if (!string.IsNullOrEmpty(countStr) && int.TryParse(countStr, out count) && count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string pattern = config.GetValue("CLEAN_SQL", "Rule_" + i + "_Regex");
                        string replacement = config.GetValue("CLEAN_SQL", "Rule_" + i + "_Replace");
                         if (!string.IsNullOrEmpty(pattern))
                        {
                            rules.Add(new SqlCleaningRule { Pattern = pattern, Replacement = replacement });
                        }
                    }
                }
            }

            if (rules.Count == 0)
            {
                rules = SqlCleaner.DefaultRules;
            }

            foreach (var rule in rules)
            {
                dgvCleanSqlRules.Rows.Add(EscapeForDisplay(rule.Pattern), EscapeForDisplay(rule.Replacement));
            }
        }

        private string EscapeForDisplay(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Replace("\\", "\\\\")
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n")
                        .Replace("\t", "\\t");
        }

        private string UnescapeFromDisplay(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\\' && i + 1 < input.Length)
                {
                    char next = input[i + 1];
                    switch (next)
                    {
                        case 'r': sb.Append('\r'); i++; break;
                        case 'n': sb.Append('\n'); i++; break;
                        case 't': sb.Append('\t'); i++; break;
                        case '\\': sb.Append('\\'); i++; break;
                        default: sb.Append('\\'); break;
                    }
                }
                else
                {
                    sb.Append(input[i]);
                }
            }
            return sb.ToString();
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
                config.SetValue("CONNECTION", "SCANNETWORK", "1");
			}
			else
			{
                config.SetValue("CONNECTION", "SCANNETWORK", "0");
			}
            config.SetValue("CONNECTION", "SERVER", cmbDbServer.Text);
            config.SetValue("CONNECTION", "DATABASE", cmbDbDatabase.Text);
			if (chkWindowsAuth.Checked)
			{
                config.SetValue("CONNECTION", "WINDOWSAUTH", "1");
                config.SetValue("CONNECTION", "SQLUSER", txtDbUser.Text);
                config.SetValue("CONNECTION", "SQLPASSWORD", txtDbPassword.Text);
			}
			else
			{
                config.SetValue("CONNECTION", "WINDOWSAUTH", "0");
                config.SetValue("CONNECTION", "SQLUSER", "");
                config.SetValue("CONNECTION", "SQLPASSWORD", "");
			}
			if (chkLogging.Checked)
			{
                config.SetValue("GENERAL", "DEBUG_LOG", "1");
			}
			else
			{
                config.SetValue("GENERAL", "DEBUG_LOG", "0");
			}
			if (chkMultiInstance.Checked)
			{
                config.SetValue("GENERAL", "MULTI_INSTANCE", "1");
			}
			else
			{
                config.SetValue("GENERAL", "MULTI_INSTANCE", "0");
			}
			if (chkAlwaysOnTop.Checked)
			{
                config.SetValue("GENERAL", "ALWAYSONTOP", "1");
			}
			else
			{
                config.SetValue("GENERAL", "ALWAYSONTOP", "0");
			}
			if (chkShowInTray.Checked)
			{
                config.SetValue("GENERAL", "SHOWINTRAY", "1");
			}
			else
			{
                config.SetValue("GENERAL", "SHOWINTRAY", "0");
			}
			if (chkSearchAny.Checked)
			{
                config.SetValue("GENERAL", "SEARCHANY", "1");
			}
			else
			{
                config.SetValue("GENERAL", "SEARCHANY", "0");
			}
			if (chkDescriptionColumns.Checked)
			{
                config.SetValue("GENERAL", "DESCRIPTIONCOLUMNS", "1");
			}
			else
			{
                config.SetValue("GENERAL", "DESCRIPTIONCOLUMNS", "0");
			}
			if (chkResizeColumns.Checked)
			{
                config.SetValue("GENERAL", "RESIZECOLUMNS", "1");
			}
			else
			{
                config.SetValue("GENERAL", "RESIZECOLUMNS", "0");
			}
			string text;
			if ((text = cmbStartTab.Text) != null)
			{
				if (text == "Function")
				{
                    config.SetValue("GENERAL", "START_TAB", "FCT_TAB");
					goto IL_3B5;
				}
				if (text == "Totalizer")
				{
                    config.SetValue("GENERAL", "START_TAB", "TLZ_TAB");
					goto IL_3B5;
				}
				if (text == "Fields")
				{
                    config.SetValue("GENERAL", "START_TAB", "FIELDS");
					goto IL_3B5;
				}
			}
            config.SetValue("GENERAL", "START_TAB", "FCT_TAB");
			IL_3B5:
			string text2;
			if ((text2 = cmbTableLookup.Text) != null)
			{
				if (text2 == "Show Fields")
				{
                    config.SetValue("GENERAL", "TABLE_LOOKUP", "FIELDS");
					goto IL_432;
				}
				if (text2 == "Show Records")
				{
                    config.SetValue("GENERAL", "TABLE_LOOKUP", "RECORDS");
					goto IL_432;
				}
			}
            config.SetValue("GENERAL", "TABLE_LOOKUP", "FIELDS");
			IL_432:
			if (chkCopyCleanSql.Checked)
			{
                config.SetValue("GENERAL", "COPYCLEANSQL", "1");
			}
			else
			{
                config.SetValue("GENERAL", "COPYCLEANSQL", "0");
			}
			if (chkCheckUpdate.Checked)
			{
                config.SetValue("GENERAL", "CHECKUPDATE", "1");
			}
			else
			{
                config.SetValue("GENERAL", "CHECKUPDATE", "0");
			}
            config.SetValue("QUERY", "FUNCTION", txtQryFct.Text);
            config.SetValue("QUERY", "TOTALIZER", txtQryTlz.Text);
			if (chkUnarchiveTarget.Checked)
			{
                config.SetValue("UNARCHIVE", "SHOWTARGET", "1");
			} else {
                config.SetValue("UNARCHIVE", "SHOWTARGET", "0");
            }

            // Save Clean SQL Rules
            config.ClearSection("CLEAN_SQL");
            int ruleCount = 0;
            foreach (DataGridViewRow row in dgvCleanSqlRules.Rows)
            {
                if (row.IsNewRow) continue;
                string pattern = UnescapeFromDisplay(row.Cells[0].Value?.ToString());
                string replacement = UnescapeFromDisplay(row.Cells[1].Value?.ToString() ?? "");

                if (!string.IsNullOrEmpty(pattern))
                {
                     config.SetValue("CLEAN_SQL", "Rule_" + ruleCount + "_Regex", pattern);
                     config.SetValue("CLEAN_SQL", "Rule_" + ruleCount + "_Replace", replacement);
                     ruleCount++;
                }
            }
            config.SetValue("CLEAN_SQL", "Count", ruleCount.ToString());

            config.Save();
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
		private async void btnChkUpdate_Click(object sender, EventArgs e)
		{
            Cursor = Cursors.WaitCursor;
			UpdateInfo updateInfo = await Versions.CheckForUpdatesAsync();

			if (updateInfo.IsNewer)
			{
				string text = "There is an update available for download.\n\nCurrent Version:\t" +
					Application.ProductVersion +
					"\nNew Version:\t" +
					updateInfo.Version +
                    "\n\nWould you like to update now?";

                if (MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await Versions.PerformUpdate(updateInfo);
                }
			}
			else
			{
				string text = "You are running the latest version.\n\nCurrent Version:\t" + Application.ProductVersion;
                MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
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

        private void btnTestToast_Click(object sender, EventArgs e)
        {
            Utils.showToast(0, "This is a test toast notification", "Test Toast");
        }
    }
}
