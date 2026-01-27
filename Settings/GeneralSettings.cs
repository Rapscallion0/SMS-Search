using System;
using System.Drawing;
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
            InitializeCustomControls();
            LoadSettings();
            WireUpEvents();
        }

        public GeneralSettings()
        {
            InitializeComponent();
        }

        private void InitializeCustomControls()
        {
            picHighlightWarning.Image = SystemIcons.Warning.ToBitmap();
            toolTip1.SetToolTip(picHighlightWarning, "Turning this feature on can slow down displaying query result as it is a heavy process.");
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
            chkCopyCleanSql.Checked = _config.GetValue("GENERAL", "COPYCLEANSQL") == "1";
            chkSearchAny.Checked = _config.GetValue("GENERAL", "SEARCHANY") == "1";

            string tableLookup = _config.GetValue("GENERAL", "TABLE_LOOKUP");
            if (tableLookup == "RECORDS") cmbTableLookup.Text = "Show Records";
            else cmbTableLookup.Text = "Show Fields";

            string autoResizeLimit = _config.GetValue("GENERAL", "AUTO_RESIZE_LIMIT");
            if (string.IsNullOrEmpty(autoResizeLimit)) autoResizeLimit = "5000";
            txtAutoResizeLimit.Text = autoResizeLimit;

            // Highlight Settings
            chkHighlightMatches.Checked = _config.GetValue("GENERAL", "HIGHLIGHT_MATCHES") == "1";

            string colorVal = _config.GetValue("GENERAL", "MATCH_HIGHLIGHT_COLOR");
            Color highlightColor = Color.Yellow;
            if (!string.IsNullOrEmpty(colorVal) && int.TryParse(colorVal, out int argb))
            {
                highlightColor = Color.FromArgb(argb);
            }
            btnHighlightColor.BackColor = highlightColor;

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            chkDescriptionColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "DESCRIPTIONCOLUMNS", chkDescriptionColumns.Checked ? "1" : "0");
            chkResizeColumns.CheckedChanged += (s, e) => SaveSetting("GENERAL", "RESIZECOLUMNS", chkResizeColumns.Checked ? "1" : "0");
            chkCopyCleanSql.CheckedChanged += (s, e) => SaveSetting("GENERAL", "COPYCLEANSQL", chkCopyCleanSql.Checked ? "1" : "0");
            chkSearchAny.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SEARCHANY", chkSearchAny.Checked ? "1" : "0");

            cmbTableLookup.SelectedIndexChanged += (s, e) =>
            {
                string val = "FIELDS";
                if (cmbTableLookup.Text == "Show Records") val = "RECORDS";
                SaveSetting("GENERAL", "TABLE_LOOKUP", val);
            };

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

            // Highlight Events
            chkHighlightMatches.CheckedChanged += (s, e) => SaveSetting("GENERAL", "HIGHLIGHT_MATCHES", chkHighlightMatches.Checked ? "1" : "0");

            btnHighlightColor.Click += (s, e) =>
            {
                colorDialog1.Color = btnHighlightColor.BackColor;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnHighlightColor.BackColor = colorDialog1.Color;
                    SaveSetting("GENERAL", "MATCH_HIGHLIGHT_COLOR", colorDialog1.Color.ToArgb().ToString());
                }
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
