using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMS_Search.Data;
using Microsoft.Win32;
using SMS_Search.Utils;

namespace SMS_Search.Settings
{
    public partial class DatabaseSettings : UserControl
    {
        private ConfigManager _config;
        private DataRepository _repo = new DataRepository();
        private dbConnector _dbConn = new dbConnector();
        private List<string> _serverNames = new List<string>();
        private List<string> _dbNames = new List<string>();
        private bool _isLoaded = false;
        private bool _hasDbError = false;
        private string _lastLoadedServer;
        private bool _isLoadingDb;

        public DatabaseSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            // Ensure Windows Auth is visible
            chkWindowsAuth.Visible = true;

            LoadSettings();
            WireUpEvents();
        }

        public DatabaseSettings()
        {
            InitializeComponent();
            chkWindowsAuth.Visible = true;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                await PopulateDbServersAsync();
            }
        }

        public void Reload()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (_config == null) return;
            _isLoaded = false;

            chkScanNetwork.Checked = _config.GetValue("CONNECTION", "SCANNETWORK") == "1";

            string server = _config.GetValue("CONNECTION", "SERVER");
            if (string.IsNullOrEmpty(server))
            {
                 cmbDbDatabase.Enabled = false;
            }
            else
            {
                 cmbDbServer.Text = server;
                 cmbDbDatabase.Enabled = true;
            }

            cmbDbDatabase.Text = _config.GetValue("CONNECTION", "DATABASE");

            string winAuth = _config.GetValue("CONNECTION", "WINDOWSAUTH");
            if (winAuth == "1" || winAuth == "")
            {
                chkWindowsAuth.Checked = true;
                ToggleAuthFields(false);
            }
            else
            {
                chkWindowsAuth.Checked = false;
                ToggleAuthFields(true);
            }

            txtDbUser.Text = _config.GetValue("CONNECTION", "SQLUSER");
            txtDbPassword.Text = GeneralUtils.Decrypt(_config.GetValue("CONNECTION", "SQLPASSWORD"));

            _isLoaded = true;
        }

        private void ToggleAuthFields(bool visible)
        {
            txtDbUser.Visible = visible;
            txtDbPassword.Visible = visible;
            lblDbUser.Visible = visible;
            lblDbPassword.Visible = visible;
        }

        private void WireUpEvents()
        {
            chkScanNetwork.CheckedChanged += (s, e) => SaveSetting("CONNECTION", "SCANNETWORK", chkScanNetwork.Checked ? "1" : "0");

            cmbDbServer.TextChanged += async (s, e) =>
            {
                 SaveSetting("CONNECTION", "SERVER", cmbDbServer.Text);
                 await ValidateServer();
            };

            cmbDbDatabase.TextChanged += (s, e) =>
            {
                SaveSetting("CONNECTION", "DATABASE", cmbDbDatabase.Text);
                if (_hasDbError)
                {
                    _hasDbError = false;
                    this.Invalidate();
                }
            };

            chkWindowsAuth.CheckedChanged += (s, e) =>
            {
                ToggleAuthFields(!chkWindowsAuth.Checked);
                SaveAuthSettings();
            };

            txtDbUser.TextChanged += (s, e) => SaveAuthSettings();
            txtDbPassword.TextChanged += (s, e) => SaveAuthSettings();

            btnTestConn.Click += btnTestConn_Click;

            cmbDbDatabase.DropDown += (s, e) => _ = GetDbNames();
            cmbDbServer.Leave += (s, e) => _ = GetDbNames();
            this.Paint += DatabaseSettings_Paint;
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();
            (this.ParentForm as frmConfig)?.FlashSaved();
        }

        private void SaveAuthSettings()
        {
            _lastLoadedServer = null; // Invalidate cache so we reload DBs with new credentials
            if (!_isLoaded || _config == null) return;

            if (chkWindowsAuth.Checked)
            {
                _config.SetValue("CONNECTION", "WINDOWSAUTH", "1");
                _config.SetValue("CONNECTION", "SQLUSER", "");
                _config.SetValue("CONNECTION", "SQLPASSWORD", "");
            }
            else
            {
                _config.SetValue("CONNECTION", "WINDOWSAUTH", "0");
                _config.SetValue("CONNECTION", "SQLUSER", txtDbUser.Text);
                _config.SetValue("CONNECTION", "SQLPASSWORD", GeneralUtils.Encrypt(txtDbPassword.Text));
            }
            _config.Save();
            (this.ParentForm as frmConfig)?.FlashSaved();
        }

        private async Task PopulateDbServersAsync()
        {
            bool scanNetwork = chkScanNetwork.Checked;
            List<string> servers = await Task.Run(() => GetDbServersList(scanNetwork));

            if (this.IsDisposed || cmbDbServer.IsDisposed) return;

            string currentServer = cmbDbServer.Text;

            _serverNames.Clear();
            _serverNames.AddRange(servers);
            _serverNames.Sort();

            cmbDbServer.DataSource = null;
            cmbDbServer.DataSource = _serverNames;

            if (!string.IsNullOrEmpty(currentServer))
            {
                cmbDbServer.Text = currentServer;
            }

            await ValidateServer();
        }

        private List<string> GetDbServersList(bool scanNetwork)
        {
            List<string> list = new List<string>();
            try
            {
                if (!scanNetwork)
                {
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    using (RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"))
                    {
                        if (key != null)
                        {
                            foreach(string sqlInstance in key.GetValueNames())
                            {
                                if (sqlInstance == "MSSQLSERVER")
                                    list.Add(Environment.MachineName);
                                else
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
                            list.Add(dataRow["ServerName"] + "\\" + dataRow["InstanceName"]);
                        else
                            list.Add(dataRow["ServerName"].ToString());
                    }
                }
            }
            catch {}
            return list;
        }

        private async Task ValidateServer()
        {
            bool found = false;
            foreach (string s in _serverNames)
            {
                if (cmbDbServer.Text == s)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                cmbDbDatabase.Enabled = true;
                // Pre-fetch DB names if valid server
                await GetDbNames();
            }
            else
            {
                if (!found)
                {
                     // cmbDbDatabase.Enabled = false;
                     // cmbDbDatabase.Text = "Select a valid Server";
                }
                else
                {
                     cmbDbDatabase.Enabled = true;
                }
            }
        }

        private async Task GetDbNames()
        {
            if (_isLoadingDb) return;
            // Prevent reloading if the server hasn't changed and we have items
            if (!string.IsNullOrEmpty(_lastLoadedServer) &&
                _lastLoadedServer == cmbDbServer.Text &&
                cmbDbDatabase.Items.Count > 0 &&
                cmbDbDatabase.Items[0].ToString() != "Loading...")
            {
                return;
            }

            _isLoadingDb = true;

             // Logic to populate DB dropdown
             string currentDb = cmbDbDatabase.Text;
             cmbDbDatabase.Items.Clear();
             cmbDbDatabase.Items.Add("Loading...");

             string user = chkWindowsAuth.Checked ? null : txtDbUser.Text;
             string pass = chkWindowsAuth.Checked ? null : txtDbPassword.Text;

             try
             {
                 Cursor.Current = Cursors.WaitCursor;
                 var dbs = await _repo.GetDatabasesAsync(cmbDbServer.Text, user, pass);

                 cmbDbDatabase.Items.Clear();
                 bool foundCurrent = false;
                 foreach(var db in dbs)
                 {
                     cmbDbDatabase.Items.Add(db);
                     if (!string.IsNullOrEmpty(currentDb) && string.Equals(db, currentDb, StringComparison.OrdinalIgnoreCase))
                        foundCurrent = true;
                 }

                 if (foundCurrent) cmbDbDatabase.Text = currentDb;
                 else if (!string.IsNullOrEmpty(currentDb) && currentDb != "Loading..." && currentDb != "Select a valid Server")
                    cmbDbDatabase.Text = currentDb;
                 else
                    cmbDbDatabase.Text = "Select Database";

                _lastLoadedServer = cmbDbServer.Text;
             }
             catch (Exception ex)
             {
                 cmbDbDatabase.Items.Clear();
                 // Suppress error here? Original showed message box.
                 MessageBox.Show("Failed to connect to data source.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
             }
             finally
             {
                 Cursor.Current = Cursors.Default;
                 _isLoadingDb = false;
             }
        }

        private async void btnTestConn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbDbDatabase.Text) ||
                cmbDbDatabase.Text == "Select Database" ||
                cmbDbDatabase.Text == "Loading..." ||
                cmbDbDatabase.Text == "Select a valid Server")
            {
                MessageBox.Show("Please select a database first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblConnStatus.Text = "Connecting...";
            lblConnStatus.ForeColor = Color.Black;
            btnTestConn.Enabled = false;

            try
            {
                bool success = await _dbConn.TestDbConnAsync(
                    cmbDbServer.Text,
                    cmbDbDatabase.Text,
                    true,
                    chkWindowsAuth.Checked ? null : txtDbUser.Text,
                    chkWindowsAuth.Checked ? null : txtDbPassword.Text);

                if (success)
                {
                    lblConnStatus.Text = "Connection Successful";
                    lblConnStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblConnStatus.Text = "Connection Failed";
                    lblConnStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {
                lblConnStatus.Text = "Connection Failed";
                lblConnStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnTestConn.Enabled = true;
            }
        }

        private void DatabaseSettings_Paint(object sender, PaintEventArgs e)
        {
             if (_hasDbError)
             {
                 // Draw red border around cmbDbDatabase
                 Rectangle r = cmbDbDatabase.Bounds;
                 r.Inflate(2, 2);
                 ControlPaint.DrawBorder(e.Graphics, r, Color.Red, ButtonBorderStyle.Solid);
             }
        }
    }
}
