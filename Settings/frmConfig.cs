using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SMS_Search;
using Log;

namespace SMS_Search.Settings
{
    public partial class frmConfig : Form
    {
        private ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMS Search_settings.json"));
        private Logfile log = new Logfile();

        private GeneralSettings generalSettings;
        private DatabaseSettings databaseSettings;
        private AdvancedSettings advancedSettings;
        private UpdateSettings updateSettings;
        private LoggingSettings loggingSettings;
        private CleanSqlSettings cleanSqlSettings;
        private LauncherSettings launcherSettings;

        public frmConfig()
        {
            InitializeComponent();
            base.StartPosition = FormStartPosition.Manual;
            base.Top = (Screen.PrimaryScreen.WorkingArea.Height - base.Height) / 2;
            base.Left = (Screen.PrimaryScreen.WorkingArea.Width - base.Width) / 2;
            lblConfigFilePath.Text = Path.Combine(Application.StartupPath, "SMS Search_settings.json");
            toolTip1.SetToolTip(lblConfigFilePath, Path.Combine(Application.StartupPath, "SMS Search_settings.json"));

            InitializeUserControls();
        }

        private void InitializeUserControls()
        {
            generalSettings = new GeneralSettings(config);
            databaseSettings = new DatabaseSettings(config);
            advancedSettings = new AdvancedSettings(config);
            updateSettings = new UpdateSettings(config);
            loggingSettings = new LoggingSettings(config);
            cleanSqlSettings = new CleanSqlSettings(config);
            launcherSettings = new LauncherSettings(config);

            // Add to Panel2
            AddControl(generalSettings);
            AddControl(databaseSettings);
            AddControl(advancedSettings);
            AddControl(updateSettings);
            AddControl(loggingSettings);
            AddControl(cleanSqlSettings);
            AddControl(launcherSettings);
        }

        private void AddControl(UserControl ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            ctrl.Visible = false;
            splitConfig.Panel2.Controls.Add(ctrl);
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            log.Logger(LogLevel.Info, "Settings window opened");

            // Icons
            imgListIcons.Images.Clear();
            imgListIcons.Images.Add("General", SystemIcons.Application);
            imgListIcons.Images.Add("Database", SystemIcons.Shield);
            imgListIcons.Images.Add("Advanced", SystemIcons.Warning);
            imgListIcons.Images.Add("Update", SystemIcons.Question);
            imgListIcons.Images.Add("Logging", SystemIcons.Information);
            imgListIcons.Images.Add("CleanSql", SystemIcons.Asterisk);
            imgListIcons.Images.Add("Launcher", SystemIcons.WinLogo);

            // Select default
            if (tvSettings.Nodes.Count > 0)
                tvSettings.SelectedNode = tvSettings.Nodes["General"];
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            // Hide all
            if (generalSettings != null) generalSettings.Visible = false;
            if (databaseSettings != null) databaseSettings.Visible = false;
            if (advancedSettings != null) advancedSettings.Visible = false;
            if (updateSettings != null) updateSettings.Visible = false;
            if (loggingSettings != null) loggingSettings.Visible = false;
            if (cleanSqlSettings != null) cleanSqlSettings.Visible = false;
            if (launcherSettings != null) launcherSettings.Visible = false;

            switch (e.Node.Name)
            {
                case "General": if (generalSettings != null) generalSettings.Visible = true; break;
                case "Database": if (databaseSettings != null) databaseSettings.Visible = true; break;
                case "Advanced": if (advancedSettings != null) advancedSettings.Visible = true; break;
                case "Update": if (updateSettings != null) updateSettings.Visible = true; break;
                case "Logging": if (loggingSettings != null) loggingSettings.Visible = true; break;
                case "CleanSql": if (cleanSqlSettings != null) cleanSqlSettings.Visible = true; break;
                case "Launcher": if (launcherSettings != null) launcherSettings.Visible = true; break;
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
             if (MessageBox.Show("This will reset all settings to their default values (including Database connection).\nAre you sure?",
                 "Revert to Default", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
             {
                 string path = Path.Combine(Application.StartupPath, "SMS Search_settings.json");
                 try
                 {
                     if (File.Exists(path)) File.Delete(path);
                 } catch {}

                 config = new ConfigManager(path);

                 splitConfig.Panel2.Controls.Clear();
                 InitializeUserControls();

                 if (tvSettings.SelectedNode != null)
                    tvSettings_AfterSelect(this, new TreeViewEventArgs(tvSettings.SelectedNode));

                 log.Logger(LogLevel.Info, "Settings reverted to default");
                 Utils.showToast(0, "Settings reverted to default", "Settings", Screen.FromControl(this));
             }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblConfigFilePath_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(Application.StartupPath, "SMS Search_settings.json"));
        }
    }
}
