using System;
using System.Drawing;
using System.Windows.Forms;
using SMS_Search;

namespace SMS_Search.Settings
{
    public partial class SearchBehaviorSettings : UserControl
    {
        private ConfigManager _config;
        private bool _isLoaded = false;

        public SearchBehaviorSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            InitializeCustomControls();
            LoadSettings();
            WireUpEvents();
        }

        private void InitializeCustomControls()
        {
            picHighlightWarning.Image = SystemIcons.Warning.ToBitmap();
            toolTip1.SetToolTip(picHighlightWarning, "Enabling this feature may impact performance on large datasets.");
        }

        public SearchBehaviorSettings()
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

            chkSearchAny.Checked = _config.GetValue("GENERAL", "SEARCHANY") == "1";

            string lookup = _config.GetValue("GENERAL", "TABLE_LOOKUP");
            if (lookup == "RECORDS") cmbTableLookup.Text = "Show Records";
            else cmbTableLookup.Text = "Show Fields";

            chkHighlightMatches.Checked = _config.GetValue("GENERAL", "HIGHLIGHT_MATCHES") == "1";

            string highlightColor = _config.GetValue("GENERAL", "MATCH_HIGHLIGHT_COLOR");
            if (!string.IsNullOrEmpty(highlightColor) && int.TryParse(highlightColor, out int argb))
            {
                btnHighlightColor.BackColor = Color.FromArgb(argb);
            }
            else
            {
                btnHighlightColor.BackColor = Color.Yellow; // Default
            }

            // Advanced Query Fields
            string fct = _config.GetValue("QUERY", "FUNCTION");
            if (string.IsNullOrEmpty(fct)) txtQryFct.Text = "F1063, F1064, F1051, F1050, F1081";
            else txtQryFct.Text = fct;

            string tlz = _config.GetValue("QUERY", "TOTALIZER");
            if (string.IsNullOrEmpty(tlz)) txtQryTlz.Text = "F1034, F1039, F1128, F1129, F1179, F1253, F1710, F1131, F1048, F1709";
            else txtQryTlz.Text = tlz;

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            chkSearchAny.CheckedChanged += (s, e) => SaveSetting("GENERAL", "SEARCHANY", chkSearchAny.Checked ? "1" : "0");

            cmbTableLookup.SelectedIndexChanged += (s, e) =>
            {
                string val = "FIELDS";
                if (cmbTableLookup.Text == "Show Records") val = "RECORDS";
                SaveSetting("GENERAL", "TABLE_LOOKUP", val);
            };

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

            txtQryFct.TextChanged += (s, e) => SaveSetting("QUERY", "FUNCTION", txtQryFct.Text);
            txtQryTlz.TextChanged += (s, e) => SaveSetting("QUERY", "TOTALIZER", txtQryTlz.Text);
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
