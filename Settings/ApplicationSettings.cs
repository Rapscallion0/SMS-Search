using System;
using System.Windows.Forms;
using SMS_Search;

namespace SMS_Search.Settings
{
    public partial class ApplicationSettings : UserControl
    {
        private ConfigManager _config;
        private UpdateChecker _versions = new UpdateChecker();
        private bool _isLoaded = false;

        public ApplicationSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public ApplicationSettings()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (_config == null) return;
            _isLoaded = false;

            // Environment
            chkAlwaysOnTop.Checked = _config.GetValue("GENERAL", "ALWAYSONTOP") == "1";
            chkShowInTray.Checked = _config.GetValue("GENERAL", "SHOWINTRAY") == "1";
            chkMultiInstance.Checked = _config.GetValue("GENERAL", "MULTI_INSTANCE") == "1";
            chkUnarchiveTarget.Checked = _config.GetValue("UNARCHIVE", "SHOWTARGET") == "1";

            string startTab = _config.GetValue("GENERAL", "START_TAB");
            if (startTab == "TLZ_TAB") cmbStartTab.Text = "Totalizer";
            else if (startTab == "FIELDS") cmbStartTab.Text = "Fields";
            else cmbStartTab.Text = "Function";

            string startupLoc = _config.GetValue("GENERAL", "STARTUP_LOCATION");
            if (startupLoc == "PRIMARY") cmbStartupLocation.Text = "Primary display";
            else if (startupLoc == "ACTIVE") cmbStartupLocation.Text = "Active display";
            else if (startupLoc == "CURSOR") cmbStartupLocation.Text = "Cursor location";
            else cmbStartupLocation.Text = "Last location"; // Default

            // Update
            chkCheckUpdate.Checked = _config.GetValue("GENERAL", "CHECKUPDATE") == "1";

            // Misc
            chkCopyCleanSql.Checked = _config.GetValue("GENERAL", "COPYCLEANSQL") == "1";

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            // Environment
            chkAlwaysOnTop.CheckedChanged += (s, e) => SaveSetting("GENERAL", "ALWAYSONTOP", chkAlwaysOnTop.Checked ? "1" : "0");
            chkShowInTray.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SHOWINTRAY", chkShowInTray.Checked ? "1" : "0");
            chkMultiInstance.CheckedChanged += (s, e) => SaveSetting("GENERAL", "MULTI_INSTANCE", chkMultiInstance.Checked ? "1" : "0");
            chkUnarchiveTarget.CheckedChanged += (s, e) => SaveSetting("UNARCHIVE", "SHOWTARGET", chkUnarchiveTarget.Checked ? "1" : "0");

            cmbStartTab.SelectedIndexChanged += (s, e) =>
            {
                string val = "FCT_TAB";
                if (cmbStartTab.Text == "Totalizer") val = "TLZ_TAB";
                else if (cmbStartTab.Text == "Fields") val = "FIELDS";
                SaveSetting("GENERAL", "START_TAB", val);
            };

            cmbStartupLocation.SelectedIndexChanged += (s, e) =>
            {
                string val = "LAST";
                if (cmbStartupLocation.Text == "Primary display") val = "PRIMARY";
                else if (cmbStartupLocation.Text == "Active display") val = "ACTIVE";
                else if (cmbStartupLocation.Text == "Cursor location") val = "CURSOR";
                SaveSetting("GENERAL", "STARTUP_LOCATION", val);
            };

            // Update
            chkCheckUpdate.CheckedChanged += (s, e) => SaveSetting("GENERAL", "CHECKUPDATE", chkCheckUpdate.Checked ? "1" : "0");
            btnChkUpdate.Click += btnChkUpdate_Click;

            // Misc
            chkCopyCleanSql.CheckedChanged += (s, e) => SaveSetting("GENERAL", "COPYCLEANSQL", chkCopyCleanSql.Checked ? "1" : "0");
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();
        }

        private async void btnChkUpdate_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            UpdateInfo updateInfo = await _versions.CheckForUpdatesAsync();

            if (updateInfo.IsNewer)
            {
                string text = "There is an update available for download.\n\nCurrent Version:\t" +
                    Application.ProductVersion +
                    "\nNew Version:\t" +
                    updateInfo.Version +
                    "\n\nWould you like to update now?";

                if (MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await _versions.PerformUpdate(updateInfo);
                }
            }
            else
            {
                string text = "You are running the latest version.\n\nCurrent Version:\t" + Application.ProductVersion;
                MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            Cursor = Cursors.Default;
        }
    }
}
