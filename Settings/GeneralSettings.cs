using System;
using System.Windows.Forms;
using SMS_Search;

namespace SMS_Search.Settings
{
    public partial class GeneralSettings : UserControl
    {
        private ConfigManager _config;
        private bool _isLoaded = false;

        public GeneralSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public GeneralSettings()
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

            chkAlwaysOnTop.Checked = _config.GetValue("GENERAL", "ALWAYSONTOP") == "1";
            chkDescriptionColumns.Checked = _config.GetValue("GENERAL", "DESCRIPTIONCOLUMNS") == "1";
            chkResizeColumns.Checked = _config.GetValue("GENERAL", "RESIZECOLUMNS") == "1";
            chkShowInTray.Checked = _config.GetValue("GENERAL", "SHOWINTRAY") == "1";
            chkCopyCleanSql.Checked = _config.GetValue("GENERAL", "COPYCLEANSQL") == "1";
            chkSearchAny.Checked = _config.GetValue("GENERAL", "SEARCHANY") == "1";
            chkMultiInstance.Checked = _config.GetValue("GENERAL", "MULTI_INSTANCE") == "1";
            chkUnarchiveTarget.Checked = _config.GetValue("UNARCHIVE", "SHOWTARGET") == "1";

            string startTab = _config.GetValue("GENERAL", "START_TAB");
            if (startTab == "TLZ_TAB") cmbStartTab.Text = "Totalizer";
            else if (startTab == "FIELDS") cmbStartTab.Text = "Fields";
            else cmbStartTab.Text = "Function";

            string tableLookup = _config.GetValue("GENERAL", "TABLE_LOOKUP");
            if (tableLookup == "RECORDS") cmbTableLookup.Text = "Show Records";
            else cmbTableLookup.Text = "Show Fields";

            string startupLoc = _config.GetValue("GENERAL", "STARTUP_LOCATION");
            if (startupLoc == "PRIMARY") cmbStartupLocation.Text = "Primary display";
            else if (startupLoc == "ACTIVE") cmbStartupLocation.Text = "Active display";
            else if (startupLoc == "CURSOR") cmbStartupLocation.Text = "Cursor location";
            else cmbStartupLocation.Text = "Last location"; // Default

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            chkAlwaysOnTop.CheckedChanged += (s, e) => SaveSetting("GENERAL", "ALWAYSONTOP", chkAlwaysOnTop.Checked ? "1" : "0");
            chkDescriptionColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "DESCRIPTIONCOLUMNS", chkDescriptionColumns.Checked ? "1" : "0");
            chkResizeColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "RESIZECOLUMNS", chkResizeColumns.Checked ? "1" : "0");
            chkShowInTray.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SHOWINTRAY", chkShowInTray.Checked ? "1" : "0");
            chkCopyCleanSql.CheckedChanged += (s, e) => SaveSetting("GENERAL", "COPYCLEANSQL", chkCopyCleanSql.Checked ? "1" : "0");
            chkSearchAny.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SEARCHANY", chkSearchAny.Checked ? "1" : "0");
            chkMultiInstance.CheckedChanged += (s, e) => SaveSetting("GENERAL", "MULTI_INSTANCE", chkMultiInstance.Checked ? "1" : "0");
            chkUnarchiveTarget.CheckedChanged += (s, e) => SaveSetting("UNARCHIVE", "SHOWTARGET", chkUnarchiveTarget.Checked ? "1" : "0");

            cmbStartTab.SelectedIndexChanged += (s, e) =>
            {
                string val = "FCT_TAB";
                if (cmbStartTab.Text == "Totalizer") val = "TLZ_TAB";
                else if (cmbStartTab.Text == "Fields") val = "FIELDS";
                SaveSetting("GENERAL", "START_TAB", val);
            };

            cmbTableLookup.SelectedIndexChanged += (s, e) =>
            {
                string val = "FIELDS";
                if (cmbTableLookup.Text == "Show Records") val = "RECORDS";
                SaveSetting("GENERAL", "TABLE_LOOKUP", val);
            };

            cmbStartupLocation.SelectedIndexChanged += (s, e) =>
            {
                string val = "LAST";
                if (cmbStartupLocation.Text == "Primary display") val = "PRIMARY";
                else if (cmbStartupLocation.Text == "Active display") val = "ACTIVE";
                else if (cmbStartupLocation.Text == "Cursor location") val = "CURSOR";
                SaveSetting("GENERAL", "STARTUP_LOCATION", val);
            };
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();
        }
    }
}
