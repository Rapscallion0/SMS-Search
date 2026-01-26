using System;
using System.Windows.Forms;
using SMS_Search;

namespace SMS_Search.Settings
{
    public partial class AdvancedSettings : UserControl
    {
        private ConfigManager _config;
        private bool _isLoaded = false;

        public AdvancedSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            WireUpEvents();
        }

        public AdvancedSettings()
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
            txtQryFct.TextChanged += (s, e) => SaveSetting("QUERY", "FUNCTION", txtQryFct.Text);
            txtQryTlz.TextChanged += (s, e) => SaveSetting("QUERY", "TOTALIZER", txtQryTlz.Text);

            //btnTestToast.Click += (s, e) => Utils.showToast(0, "This is a test toast notification", "Test Toast");
        }

        private void SaveSetting(string section, string key, string value)
        {
            if (!_isLoaded || _config == null) return;
            _config.SetValue(section, key, value);
            _config.Save();
        }
    }
}
