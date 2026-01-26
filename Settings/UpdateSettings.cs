using System;
using System.Windows.Forms;
using SMS_Search;

namespace SMS_Search.Settings
{
    public partial class UpdateSettings : UserControl
    {
        private ConfigManager _config;
        private UpdateChecker _versions = new UpdateChecker();
        private bool _isLoaded = false;

        public UpdateSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public UpdateSettings()
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

            chkCheckUpdate.Checked = _config.GetValue("GENERAL", "CHECKUPDATE") == "1";

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            chkCheckUpdate.CheckedChanged += (s, e) => SaveSetting("GENERAL", "CHECKUPDATE", chkCheckUpdate.Checked ? "1" : "0");
            btnChkUpdate.Click += btnChkUpdate_Click;
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
