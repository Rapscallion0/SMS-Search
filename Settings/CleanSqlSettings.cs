using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SMS_Search;
using Log;

namespace SMS_Search.Settings
{
    public partial class CleanSqlSettings : UserControl
    {
        private ConfigManager _config;
        private Logfile _log = new Logfile();
        private bool _isLoaded = false;

        public CleanSqlSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings(false);
            WireUpEvents();
        }

        public CleanSqlSettings()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            LoadSettings(false);
        }

        private void LoadSettings(bool useDefaults)
        {
            if (_config == null && !useDefaults) return;
            _isLoaded = false;

            chkCopyCleanSql.Checked = _config.GetValue("GENERAL", "COPYCLEANSQL") == "1";

            dgvCleanSqlRules.Rows.Clear();
            List<SqlCleaningRule> rules = new List<SqlCleaningRule>();

            if (!useDefaults && _config != null)
            {
                string countStr = _config.GetValue("CLEAN_SQL", "Count");
                int count;
                if (!string.IsNullOrEmpty(countStr) && int.TryParse(countStr, out count) && count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string pattern = _config.GetValue("CLEAN_SQL", "Rule_" + i + "_Regex");
                        string replacement = _config.GetValue("CLEAN_SQL", "Rule_" + i + "_Replace");
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

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
             btnResetCleanSql.Click += btnResetCleanSql_Click;
             dgvCleanSqlRules.CellValueChanged += (s, e) => SaveRules();
             dgvCleanSqlRules.RowsAdded += (s, e) => SaveRules();
             dgvCleanSqlRules.RowsRemoved += (s, e) => SaveRules();

             chkCopyCleanSql.CheckedChanged += (s, e) => {
                 if (!_isLoaded || _config == null) return;
                 _config.SetValue("GENERAL", "COPYCLEANSQL", chkCopyCleanSql.Checked ? "1" : "0");
                 _config.Save();
                 (this.ParentForm as frmConfig)?.FlashSaved();
             };
        }

        private void SaveRules()
        {
            if (!_isLoaded || _config == null) return;

            _config.ClearSection("CLEAN_SQL");
            int ruleCount = 0;
            foreach (DataGridViewRow row in dgvCleanSqlRules.Rows)
            {
                if (row.IsNewRow) continue;
                string pattern = UnescapeFromDisplay(row.Cells[0].Value?.ToString());
                string replacement = UnescapeFromDisplay(row.Cells[1].Value?.ToString() ?? "");

                if (!string.IsNullOrEmpty(pattern))
                {
                     _config.SetValue("CLEAN_SQL", "Rule_" + ruleCount + "_Regex", pattern);
                     _config.SetValue("CLEAN_SQL", "Rule_" + ruleCount + "_Replace", replacement);
                     ruleCount++;
                }
            }
            _config.SetValue("CLEAN_SQL", "Count", ruleCount.ToString());
            _config.Save();
            (this.ParentForm as frmConfig)?.FlashSaved();
        }

        private void btnResetCleanSql_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the Clean SQL rules to defaults?", "Reset Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _log.Logger(LogLevel.Info, "Resetting Clean SQL rules to default");
                LoadSettings(true);
                SaveRules();
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
    }
}
