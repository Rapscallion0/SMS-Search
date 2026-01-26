using System;
using System.Windows.Forms;
using SMS_Search;
using Log;

namespace SMS_Search.Settings
{
    public partial class LoggingSettings : UserControl
    {
        private ConfigManager _config;
        private bool _isLoaded = false;

        public LoggingSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public LoggingSettings()
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

            chkLogging.Checked = _config.GetValue("GENERAL", "DEBUG_LOG") == "1";

            string logLevel = _config.GetValue("GENERAL", "LOG_LEVEL");
            if (string.IsNullOrEmpty(logLevel)) logLevel = "Info";
            cmbLogLevel.Text = logLevel;

            string retentionStr = _config.GetValue("GENERAL", "LOG_RETENTION");
            int retention = 14;
            if (int.TryParse(retentionStr, out int r)) retention = r;
            numRetention.Value = retention;

            ToggleControls();

            _isLoaded = true;
        }

        private void ToggleControls()
        {
             bool enabled = chkLogging.Checked;
             cmbLogLevel.Enabled = enabled;
             numRetention.Enabled = enabled;
             lblLogLevel.Enabled = enabled;
             lblRetention.Enabled = enabled;
        }

        private void WireUpEvents()
        {
            chkLogging.CheckedChanged += (s, e) =>
            {
                ToggleControls();
                SaveSetting("GENERAL", "DEBUG_LOG", chkLogging.Checked ? "1" : "0");
            };

            cmbLogLevel.SelectedIndexChanged += (s, e) => SaveSetting("GENERAL", "LOG_LEVEL", cmbLogLevel.Text);
            numRetention.ValueChanged += (s, e) => SaveSetting("GENERAL", "LOG_RETENTION", numRetention.Value.ToString());
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();

            // Reload logger configuration
            new Logfile().ReloadConfig();
        }
    }
}
