using System;
using System.Windows.Forms;
using SMS_Search.Utils;

namespace SMS_Search.Settings
{
    public partial class DisplaySettings : UserControl
    {
        private ConfigManager _config;
        private bool _isLoaded = false;

        public DisplaySettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public DisplaySettings()
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

            chkDescriptionColumns.Checked = _config.GetValue("GENERAL", "DESCRIPTIONCOLUMNS") == "1";
            chkResizeColumns.Checked = _config.GetValue("GENERAL", "RESIZECOLUMNS") == "1";
            chkShowRowNumbers.Checked = _config.GetValue("GENERAL", "SHOW_ROW_NUMBERS") == "1";

            string resizeLimit = _config.GetValue("GENERAL", "AUTO_RESIZE_LIMIT");
            if (string.IsNullOrEmpty(resizeLimit)) resizeLimit = "5000";
            txtAutoResizeLimit.Text = resizeLimit;

            InitializeComboBox();
            string delimiter = _config.GetValue("GENERAL", "COPY_DELIMITER");
            if (string.IsNullOrEmpty(delimiter)) delimiter = "TAB";

            if (cmbCopyDelimiter.Items.Contains(delimiter))
                cmbCopyDelimiter.SelectedItem = delimiter;
            else
                cmbCopyDelimiter.SelectedIndex = 0;

            txtCustomDelimiter.Text = _config.GetValue("GENERAL", "COPY_DELIMITER_CUSTOM");

            UpdateDelimiterUI();

            _isLoaded = true;
        }

        private void InitializeComboBox()
        {
            cmbCopyDelimiter.Items.Clear();
            cmbCopyDelimiter.Items.Add("TAB");
            cmbCopyDelimiter.Items.Add("Comma (,)");
            cmbCopyDelimiter.Items.Add("Pipe (|)");
            cmbCopyDelimiter.Items.Add("Semicolon (;)");
            cmbCopyDelimiter.Items.Add("Custom...");
        }

        private void UpdateDelimiterUI()
        {
            bool isCustom = cmbCopyDelimiter.Text == "Custom...";
            txtCustomDelimiter.Visible = isCustom;
            lblCopyWarning.Visible = cmbCopyDelimiter.Text != "TAB";
        }

        private void WireUpEvents()
        {
            chkDescriptionColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "DESCRIPTIONCOLUMNS", chkDescriptionColumns.Checked ? "1" : "0");
            chkResizeColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "RESIZECOLUMNS", chkResizeColumns.Checked ? "1" : "0");
            chkShowRowNumbers.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SHOW_ROW_NUMBERS", chkShowRowNumbers.Checked ? "1" : "0");

            txtAutoResizeLimit.Leave += (s, e) =>
            {
                if (int.TryParse(txtAutoResizeLimit.Text, out int val) && val >= 0)
                {
                    SaveSetting("GENERAL", "AUTO_RESIZE_LIMIT", val.ToString());
                }
                else
                {
                    // Revert to last saved value
                    string saved = _config.GetValue("GENERAL", "AUTO_RESIZE_LIMIT");
                    if (string.IsNullOrEmpty(saved)) saved = "5000";
                    txtAutoResizeLimit.Text = saved;
                }
            };

            cmbCopyDelimiter.SelectedIndexChanged += (s, e) =>
            {
                UpdateDelimiterUI();
                SaveSetting("GENERAL", "COPY_DELIMITER", cmbCopyDelimiter.Text);
            };

            txtCustomDelimiter.TextChanged += (s, e) => SaveSetting("GENERAL", "COPY_DELIMITER_CUSTOM", txtCustomDelimiter.Text);
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();

            (this.ParentForm as frmConfig)?.FlashSaved();
        }
    }
}
