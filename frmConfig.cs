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
using System.Runtime.InteropServices;

namespace SMS_Search
{
	public partial class frmConfig : Form
	{
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMS Search.json"));
        private const string LauncherExe = "SMS Search Launcher.exe";
        private string _lastValidHotkey = "";
        private string _currentValidHotkey = "";
        private bool _isCurrentHotkeyValid = false;

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
            txtHotkey.KeyUp += txtHotkey_KeyUp;
            txtHotkey.Leave += txtHotkey_Leave;
            loadConfig();
            UpdateLauncherStatusUI();
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

            // Load Hotkey
            string savedHotkey = config.GetValue("LAUNCHER", "HOTKEY");
            if (string.IsNullOrEmpty(savedHotkey))
            {
                // Default
                Keys defaultKey = HotkeyUtils.GetDefaultHotkey();
                _lastValidHotkey = HotkeyUtils.ToString(defaultKey);
            }
            else
            {
                // Parse potentially legacy format and standardize
                Keys k = HotkeyUtils.Parse(savedHotkey);
                if (k == Keys.None)
                {
                     Keys defaultKey = HotkeyUtils.GetDefaultHotkey();
                     _lastValidHotkey = HotkeyUtils.ToString(defaultKey);
                }
                else
                {
                     _lastValidHotkey = HotkeyUtils.ToString(k);
                }
            }
            txtHotkey.Text = _lastValidHotkey;
            _currentValidHotkey = _lastValidHotkey;
            _isCurrentHotkeyValid = true;

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

            // Save Hotkey (if valid)
            if (_isCurrentHotkeyValid)
            {
                 config.SetValue("LAUNCHER", "HOTKEY", txtHotkey.Text);
            }
            // else ignore or save last valid? The UI shouldn't allow invalid state on OK.

            config.Save();
            ReloadLauncherIfRunning();
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

        private void txtHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true; // Prevent default typing

            // If user deletes, we might want to reset to empty or default?
            // User requirement: "Do not allow an empty shortcut".
            // So Delete/Back resets to default? Or does nothing?
            // Let's make it reset to Default if they try to clear it.
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                 // Resetting to default behavior or just ignoring.
                 // Let's reset to default.
                 Keys def = HotkeyUtils.GetDefaultHotkey();
                 txtHotkey.Text = HotkeyUtils.ToString(def);
                 _currentValidHotkey = txtHotkey.Text;
                 _isCurrentHotkeyValid = true;
                 return;
            }

            // Build visual shortcut
            string hotkeyStr = HotkeyUtils.ToString(e.KeyData);
            txtHotkey.Text = hotkeyStr;
            txtHotkey.SelectionStart = txtHotkey.Text.Length;

            // Validate
            if (HotkeyUtils.IsValid(e.KeyData))
            {
                if (HotkeyUtils.IsStandard(e.KeyData))
                {
                    // Invalid (Standard)
                    txtHotkey.BackColor = Color.MistyRose;
                    _isCurrentHotkeyValid = false;
                    toolTip1.SetToolTip(txtHotkey, "This shortcut is a standard system shortcut and cannot be used.");
                }
                else
                {
                    // Check availability
                    if (CheckHotkeyAvailability(e.KeyData))
                    {
                        txtHotkey.BackColor = SystemColors.Window;
                        _currentValidHotkey = hotkeyStr;
                        _isCurrentHotkeyValid = true;
                        toolTip1.SetToolTip(txtHotkey, "");
                    }
                    else
                    {
                        txtHotkey.BackColor = Color.MistyRose;
                        _isCurrentHotkeyValid = false;
                         toolTip1.SetToolTip(txtHotkey, "This shortcut is already in use by another application.");
                    }
                }
            }
            else
            {
                // Incomplete
                txtHotkey.BackColor = SystemColors.Window;
                _isCurrentHotkeyValid = false;
            }
        }

        private void txtHotkey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.None || (e.KeyData & Keys.Modifiers) == Keys.None)
            {
                // All keys released. If current text is not valid, revert.
                if (!_isCurrentHotkeyValid)
                {
                    txtHotkey.Text = _lastValidHotkey;
                    _currentValidHotkey = _lastValidHotkey;
                    _isCurrentHotkeyValid = true;
                    txtHotkey.BackColor = SystemColors.Window;
                }
                else
                {
                    // It was valid, store it as the new last valid
                    _lastValidHotkey = _currentValidHotkey;
                }
            }
        }

        private void txtHotkey_Leave(object sender, EventArgs e)
        {
            // Ensure we leave with a valid hotkey
             if (!_isCurrentHotkeyValid)
            {
                txtHotkey.Text = _lastValidHotkey;
                _currentValidHotkey = _lastValidHotkey;
                _isCurrentHotkeyValid = true;
                txtHotkey.BackColor = SystemColors.Window;
            }
        }

        private bool CheckHotkeyAvailability(Keys keyData)
        {
            // If the hotkey is the same as currently registered/running one, it IS available (to us).
            // But we don't know the running one easily here without querying config, but we are editing config.
            // If the user types the SAME hotkey as before, it is valid.
            // If the launcher is running, it holds the hotkey.
            // So if Launcher is running AND key matches saved config, return true.

            string saved = config.GetValue("LAUNCHER", "HOTKEY");
            if (!string.IsNullOrEmpty(saved))
            {
                Keys savedKey = HotkeyUtils.Parse(saved);
                if (savedKey == keyData) return true;
            }

            // Try to register
            // We need a unique ID. 0x9000
            int id = 0x9000;
            uint modifiers = 0;
            if ((keyData & Keys.Control) == Keys.Control) modifiers |= 0x0002;
            if ((keyData & Keys.Alt) == Keys.Alt) modifiers |= 0x0001;
            if ((keyData & Keys.Shift) == Keys.Shift) modifiers |= 0x0004;
            // Win key? Not supported by Keys usually, but HotKeyManager supports it.

            uint vk = (uint)(keyData & ~Keys.Modifiers);

            bool success = RegisterHotKey(this.Handle, id, modifiers, vk);
            if (success)
            {
                UnregisterHotKey(this.Handle, id);
                return true;
            }
            else
            {
                // Failed to register -> In use
                return false;
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lblLauncherStatus.Text = "Status: Registering...";
                DrawStatusLight(Color.Orange);
                btnRegister.Enabled = false;
                btnUnregister.Enabled = false;

                await Task.Run(() =>
                {
                    KillLauncher();
                    ExtractLauncher();
                    CreateStartupShortcut();
                    StartLauncher();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering launcher: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateLauncherStatusUI();
                Cursor = Cursors.Default;
            }
        }

        private async void btnUnregister_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lblLauncherStatus.Text = "Status: Unregistering...";
                DrawStatusLight(Color.Orange);
                btnRegister.Enabled = false;
                btnUnregister.Enabled = false;

                await Task.Run(() =>
                {
                    RemoveStartupShortcut();
                    KillLauncher();
                    DeleteLauncher();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering launcher: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateLauncherStatusUI();
                Cursor = Cursors.Default;
            }
        }

        private void ExtractLauncher()
        {
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                string resourceName = "SMS_Search.Resources.SMS Search Launcher.exe";

                // Diagnostic: Check if resource exists or find partial match
                var resources = assembly.GetManifestResourceNames();
                var foundResource = Array.Find(resources, r => r.EndsWith("SMS Search Launcher.exe", StringComparison.OrdinalIgnoreCase));

                if (foundResource != null)
                {
                    resourceName = foundResource;
                }
                else
                {
                    string availableResources = string.Join(Environment.NewLine, resources);
                    throw new Exception("Embedded launcher resource not found.\nAvailable resources:\n" + availableResources);
                }

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        throw new Exception("Embedded launcher resource stream is null for '" + resourceName + "'.");
                    }
                    using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to extract launcher: " + ex.Message);
            }
        }

        private void DeleteLauncher()
        {
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            if (File.Exists(targetPath))
            {
                int retries = 5;
                while (retries >= 0)
                {
                    try
                    {
                        File.Delete(targetPath);
                        return;
                    }
                    catch (Exception ex)
                    {
                        bool isRetryable = ex is IOException || ex is UnauthorizedAccessException;
                        if (retries == 0 || !isRetryable)
                        {
                            throw new Exception("Failed to delete launcher executable: " + ex.Message);
                        }
                        retries--;
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
        }

        private void CreateStartupShortcut()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);

            if (!File.Exists(targetPath))
            {
                throw new FileNotFoundException("Launcher executable not found at: " + targetPath);
            }

            // Create shortcut using PowerShell to avoid COM references
            string script = String.Format("$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('{0}'); $Shortcut.TargetPath = '{1}'; $Shortcut.WorkingDirectory = '{2}'; $Shortcut.Save()",
                shortcutPath, targetPath, Application.StartupPath);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell.exe";
            psi.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + script + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            using (Process p = Process.Start(psi))
            {
                p.WaitForExit();
            }
        }

        private void RemoveStartupShortcut()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");

            if (File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
            }
        }

        private void StartLauncher()
        {
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            if (File.Exists(targetPath))
            {
                // Check if already running
                if (!IsLauncherRunning())
                {
                    Process.Start(targetPath);
                }
            }
        }

        private void KillLauncher()
        {
            foreach (var name in new[] { "Launcher", "SMS Search Launcher" })
            {
                Process[] processes = Process.GetProcessesByName(name);
                foreach (Process p in processes)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(5000);
                    }
                    catch { }
                }
            }
        }

        private bool IsLauncherRunning()
        {
            return Process.GetProcessesByName("Launcher").Length > 0 || Process.GetProcessesByName("SMS Search Launcher").Length > 0;
        }

        private void ReloadLauncherIfRunning()
        {
            if (IsLauncherRunning())
            {
                KillLauncher();
                StartLauncher();
            }
        }

        private void DrawStatusLight(bool isGreen)
        {
            DrawStatusLight(isGreen ? Color.Green : Color.Red);
        }

        private void DrawStatusLight(Color c)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(c))
                {
                    g.FillEllipse(b, 1, 1, 14, 14);
                }
            }
            if (pbLauncherStatus.Image != null) pbLauncherStatus.Image.Dispose();
            pbLauncherStatus.Image = bmp;
        }

        private void UpdateLauncherStatusUI()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);

            bool isRegistered = File.Exists(shortcutPath) && File.Exists(targetPath);

            DrawStatusLight(isRegistered);

            if (isRegistered)
            {
                 btnRegister.Enabled = false;
                 btnUnregister.Enabled = true;

                 if (IsLauncherRunning())
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Running";
                 }
                 else
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Stopped";
                 }
            }
            else
            {
                 lblLauncherStatus.Text = "Status: Not Registered";
                 btnRegister.Enabled = true;
                 btnUnregister.Enabled = false;
            }
        }
    }
}
