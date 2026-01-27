using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SMS_Search;
using Log;
using DbConn;

namespace SMS_Search.Settings
{
    public partial class frmConfig : Form
    {
        private ConfigManager config = new ConfigManager(Path.Combine(Application.StartupPath, "SMSSearch_settings.json"));
        private Logfile log = new Logfile();

        private ApplicationSettings applicationSettings;
        private DisplaySettings displaySettings;
        private LoggingSettings loggingSettings;
        private SearchBehaviorSettings searchBehaviorSettings;
        private CleanSqlSettings cleanSqlSettings;
        private DatabaseSettings databaseSettings;
        private LauncherSettings launcherSettings;

        public bool ForceDatabaseSetup { get; set; } = false;

        private Timer _timerSaved;

        public frmConfig()
        {
            InitializeComponent();

            toolTip1.SetToolTip(btnOpenConfig, Path.Combine(Application.StartupPath, "SMSSearch_settings.json"));

            _timerSaved = new Timer();
            _timerSaved.Interval = 2000;
            _timerSaved.Tick += (s, e) => { lblSavedStatus.Visible = false; _timerSaved.Stop(); };

            InitializeUserControls();
        }

        private void InitializeUserControls()
        {
            // Pass FlashSaved action to controls if constructors support it, or they can call parent method
            applicationSettings = new ApplicationSettings(config);
            displaySettings = new DisplaySettings(config);
            loggingSettings = new LoggingSettings(config);
            searchBehaviorSettings = new SearchBehaviorSettings(config);
            cleanSqlSettings = new CleanSqlSettings(config);
            databaseSettings = new DatabaseSettings(config);
            launcherSettings = new LauncherSettings(config);

            AddControl(applicationSettings);
            AddControl(displaySettings);
            AddControl(loggingSettings);
            AddControl(searchBehaviorSettings);
            AddControl(cleanSqlSettings);
            AddControl(databaseSettings);
            AddControl(launcherSettings);
        }

        public void FlashSaved()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(FlashSaved));
                return;
            }
            lblSavedStatus.Visible = true;
            _timerSaved.Stop();
            _timerSaved.Start();
        }

        private void AddControl(UserControl ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            ctrl.Visible = false;
            splitConfig.Panel2.Controls.Add(ctrl);
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            try { this.Icon = new Icon(Path.Combine(Application.StartupPath, "SMS Search.ico")); } catch { }
            log.Logger(LogLevel.Info, "Settings window opened");

            // Icons - Create a new ImageList to avoid state corruption or fallback issues
            var icons = new ImageList();
            icons.ColorDepth = ColorDepth.Depth32Bit;
            icons.ImageSize = new Size(16, 16);
            icons.TransparentColor = Color.Transparent;

            icons.Images.Add("General", IconLoader.GetIcon("General"));
            icons.Images.Add("Application", IconLoader.GetIcon("Application"));
            icons.Images.Add("Display", IconLoader.GetIcon("Display"));
            icons.Images.Add("Database", IconLoader.GetIcon("Database"));
            icons.Images.Add("Search", IconLoader.GetIcon("Search"));
            icons.Images.Add("Behavior", IconLoader.GetIcon("Behavior"));
            icons.Images.Add("CleanSql", IconLoader.GetIcon("CleanSql"));
            icons.Images.Add("Launcher", IconLoader.GetIcon("Launcher"));
            icons.Images.Add("Logging", IconLoader.GetIcon("Logging"));

            tvSettings.ImageList = icons;

            // Force refresh of node keys to ensure binding
            void RefreshNodeKeys(TreeNodeCollection nodes)
            {
                foreach (TreeNode node in nodes)
                {
                    if (!string.IsNullOrEmpty(node.ImageKey)) node.ImageKey = node.ImageKey;
                    if (!string.IsNullOrEmpty(node.SelectedImageKey)) node.SelectedImageKey = node.SelectedImageKey;
                    if (node.Nodes.Count > 0) RefreshNodeKeys(node.Nodes);
                }
            }
            RefreshNodeKeys(tvSettings.Nodes);

            // Expand all nodes
            tvSettings.ExpandAll();

            // Select default
            if (tvSettings.Nodes.Count > 0)
            {
                if (ForceDatabaseSetup)
                {
                    TreeNode[] nodes = tvSettings.Nodes.Find("Database", true);
                    if (nodes.Length > 0) tvSettings.SelectedNode = nodes[0];
                }
                else
                {
                    // Select Application by default (Node "Application" is child of "General")
                    TreeNode[] nodes = tvSettings.Nodes.Find("Application", true);
                    if (nodes.Length > 0) tvSettings.SelectedNode = nodes[0];
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (ForceDatabaseSetup && this.DialogResult != DialogResult.OK)
            {
                string server = config.GetValue("CONNECTION", "SERVER");
                string database = config.GetValue("CONNECTION", "DATABASE");
                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

                dbConnector db = new dbConnector();
                if (db.TestDbConn(server, database, false, user, pass))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    if (MessageBox.Show("A valid database connection is required to use this application.\n\nAre you sure you want to exit?", "Database Connection Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            base.OnFormClosing(e);
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            // Hide all
            if (applicationSettings != null) applicationSettings.Visible = false;
            if (displaySettings != null) displaySettings.Visible = false;
            if (loggingSettings != null) loggingSettings.Visible = false;
            if (searchBehaviorSettings != null) searchBehaviorSettings.Visible = false;
            if (cleanSqlSettings != null) cleanSqlSettings.Visible = false;
            if (databaseSettings != null) databaseSettings.Visible = false;
            if (launcherSettings != null) launcherSettings.Visible = false;

            // Mapping Node Name to Control
            // General -> Application (Default behavior if parent selected)
            // Search -> Behavior (Default behavior if parent selected)

            switch (e.Node.Name)
            {
                case "General":
                case "Application":
                    if (applicationSettings != null) applicationSettings.Visible = true;
                    break;
                case "Display":
                    if (displaySettings != null) displaySettings.Visible = true;
                    break;
                case "Logging":
                    if (loggingSettings != null) loggingSettings.Visible = true;
                    break;
                case "Search":
                case "Behavior":
                    if (searchBehaviorSettings != null) searchBehaviorSettings.Visible = true;
                    break;
                case "CleanSql":
                    if (cleanSqlSettings != null) cleanSqlSettings.Visible = true;
                    break;
                case "Database":
                    if (databaseSettings != null) databaseSettings.Visible = true;
                    break;
                case "Launcher":
                    if (launcherSettings != null) launcherSettings.Visible = true;
                    break;
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
             if (MessageBox.Show("This will reset all settings to their default values (including Database connection).\nAre you sure?",
                 "Revert to Default", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
             {
                 string path = Path.Combine(Application.StartupPath, "SMSSearch_settings.json");
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

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(Application.StartupPath, "SMSSearch_settings.json"));
        }
    }
}
