using DbConn;
// using Ini;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMS_Search
{
    partial class frmConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private const string MenuName = "*\\shell\\UnArchiver";
        private const string Command = "*\\shell\\UnArchiver\\command";
        private UpdateChecker Versions = new UpdateChecker();
        private dbConnector dbConn = new dbConnector();
        private ArrayList ServerNames = new ArrayList();
        private ArrayList DbNames = new ArrayList();

        // Controls
        private SplitContainer splitConfig;
        private TreeView tvSettings;
        private ImageList imgListIcons;

        // Page Panels
        private Panel pnlGeneral;
        private Panel pnlDatabase;
        private Panel pnlAdvanced;
        private Panel pnlUpdate;
        private Panel pnlLogging;
        private Panel pnlCleanSql;
        private Panel pnlLauncher;

        // Buttons
        private Button btnResetConfig;
        private Button btnApplyConfig;
        private Button btnCancel;
        private Button btnOk;
        private ToolTip toolTip1;
        private Label lblConfigFilePath;

        // Database Page Controls
        private Label label4;
        private Label label3;
        private ComboBox cmbDbDatabase;
        private Button btnTestConn;
        private ComboBox cmbDbServer;
        private CheckBox chkScanNetwork;
        private CheckBox chkWindowsAuth;
        private TextBox txtDbUser;
        private TextBox txtDbPassword;
        private Label lblDbUser;
        private Label lblDbPassword;

        // General Page Controls
        private CheckBox chkUnarchiveTarget;
        private CheckBox chkCopyCleanSql;
        private CheckBox chkResizeColumns;
        private ComboBox cmbStartTab;
        private Label label7;
        private Label label6;
        private CheckBox chkSearchAny;
        private CheckBox chkShowInTray;
        private CheckBox chkAlwaysOnTop;
        private ComboBox cmbTableLookup;
        private Label lblTableLookup;
        private CheckBox chkDescriptionColumns;
        private CheckBox chkMultiInstance;
        private ComboBox cmbStartupLocation;
        private Label label8;

        // Advanced Page Controls
        private GroupBox groupBox1;
        private TextBox txtQryTlz;
        private Label label5;
        private Label label2;
        private TextBox txtQryFct;
        private Button btnTestToast;

        // Update Page Controls
        private CheckBox chkCheckUpdate;
        private Button btnChkUpdate;

        // Logging Page Controls
        private CheckBox chkLogging;
        private ComboBox cmbLogLevel;
        private Label lblLogLevel;
        private NumericUpDown numRetention;
        private Label lblRetention;

        // Clean SQL Page Controls
        private DataGridView dgvCleanSqlRules;
        private DataGridViewTextBoxColumn colRegex;
        private DataGridViewTextBoxColumn colReplace;
        private Button btnResetCleanSql;

        // Launcher Page Controls
        private Label lblLauncherInfo;
        private Label lblHotkey;
        private TextBox txtHotkey;
        private Button btnRegister;
        private Button btnUnregister;
        private Label lblLauncherStatus;
        private PictureBox pbLauncherStatus;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitConfig = new System.Windows.Forms.SplitContainer();
            this.tvSettings = new System.Windows.Forms.TreeView();
            this.imgListIcons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGeneral = new System.Windows.Forms.Panel();
            this.pnlDatabase = new System.Windows.Forms.Panel();
            this.pnlAdvanced = new System.Windows.Forms.Panel();
            this.pnlUpdate = new System.Windows.Forms.Panel();
            this.pnlLogging = new System.Windows.Forms.Panel();
            this.pnlCleanSql = new System.Windows.Forms.Panel();
            this.pnlLauncher = new System.Windows.Forms.Panel();

            // Initialize Buttons
            this.btnResetConfig = new System.Windows.Forms.Button();
            this.btnApplyConfig = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblConfigFilePath = new System.Windows.Forms.Label();

            // Initialize Controls
            // Database
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDbDatabase = new System.Windows.Forms.ComboBox();
            this.btnTestConn = new System.Windows.Forms.Button();
            this.cmbDbServer = new System.Windows.Forms.ComboBox();
            this.chkScanNetwork = new System.Windows.Forms.CheckBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.lblDbUser = new System.Windows.Forms.Label();
            this.lblDbPassword = new System.Windows.Forms.Label();

            // General
            this.label6 = new System.Windows.Forms.Label();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.chkDescriptionColumns = new System.Windows.Forms.CheckBox();
            this.chkResizeColumns = new System.Windows.Forms.CheckBox();
            this.chkShowInTray = new System.Windows.Forms.CheckBox();
            this.chkCopyCleanSql = new System.Windows.Forms.CheckBox();
            this.chkSearchAny = new System.Windows.Forms.CheckBox();
            this.chkMultiInstance = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStartTab = new System.Windows.Forms.ComboBox();
            this.chkUnarchiveTarget = new System.Windows.Forms.CheckBox();
            this.lblTableLookup = new System.Windows.Forms.Label();
            this.cmbTableLookup = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbStartupLocation = new System.Windows.Forms.ComboBox();

            // Advanced
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtQryFct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQryTlz = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTestToast = new System.Windows.Forms.Button();

            // Update
            this.chkCheckUpdate = new System.Windows.Forms.CheckBox();
            this.btnChkUpdate = new System.Windows.Forms.Button();

            // Logging
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();

            // Clean SQL
            this.dgvCleanSqlRules = new System.Windows.Forms.DataGridView();
            this.colRegex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnResetCleanSql = new System.Windows.Forms.Button();

            // Launcher
            this.lblLauncherInfo = new System.Windows.Forms.Label();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUnregister = new System.Windows.Forms.Button();
            this.lblLauncherStatus = new System.Windows.Forms.Label();
            this.pbLauncherStatus = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).BeginInit();
            this.splitConfig.Panel1.SuspendLayout();
            this.splitConfig.Panel2.SuspendLayout();
            this.splitConfig.SuspendLayout();
            this.pnlGeneral.SuspendLayout();
            this.pnlDatabase.SuspendLayout();
            this.pnlAdvanced.SuspendLayout();
            this.pnlUpdate.SuspendLayout();
            this.pnlLogging.SuspendLayout();
            this.pnlCleanSql.SuspendLayout();
            this.pnlLauncher.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).BeginInit();
            this.SuspendLayout();

            // 
            // splitConfig
            // 
            this.splitConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitConfig.Location = new System.Drawing.Point(0, 0);
            this.splitConfig.Name = "splitConfig";
            // 
            // splitConfig.Panel1
            // 
            this.splitConfig.Panel1.Controls.Add(this.tvSettings);
            // 
            // splitConfig.Panel2
            // 
            this.splitConfig.Panel2.Controls.Add(this.pnlDatabase);
            this.splitConfig.Panel2.Controls.Add(this.pnlGeneral);
            this.splitConfig.Panel2.Controls.Add(this.pnlAdvanced);
            this.splitConfig.Panel2.Controls.Add(this.pnlUpdate);
            this.splitConfig.Panel2.Controls.Add(this.pnlLogging);
            this.splitConfig.Panel2.Controls.Add(this.pnlCleanSql);
            this.splitConfig.Panel2.Controls.Add(this.pnlLauncher);
            this.splitConfig.Size = new System.Drawing.Size(584, 400); // Reserve space for buttons
            this.splitConfig.SplitterDistance = 150;
            this.splitConfig.TabIndex = 0;

            //
            // tvSettings
            // 
            this.tvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSettings.ImageIndex = 0;
            this.tvSettings.ImageList = this.imgListIcons;
            this.tvSettings.Location = new System.Drawing.Point(0, 0);
            this.tvSettings.Name = "tvSettings";
            this.tvSettings.SelectedImageIndex = 0;
            this.tvSettings.Size = new System.Drawing.Size(150, 400);
            this.tvSettings.TabIndex = 0;
            this.tvSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSettings_AfterSelect);

            //
            // imgListIcons
            // 
            this.imgListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgListIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imgListIcons.TransparentColor = System.Drawing.Color.Transparent;

            // =========================================================================
            // DATABASE PANEL
            // =========================================================================
            this.pnlDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDatabase.Controls.Add(this.label3);
            this.pnlDatabase.Controls.Add(this.cmbDbServer);
            this.pnlDatabase.Controls.Add(this.chkScanNetwork);
            this.pnlDatabase.Controls.Add(this.btnTestConn);
            this.pnlDatabase.Controls.Add(this.label4);
            this.pnlDatabase.Controls.Add(this.cmbDbDatabase);
            this.pnlDatabase.Controls.Add(this.chkWindowsAuth);
            this.pnlDatabase.Controls.Add(this.lblDbUser);
            this.pnlDatabase.Controls.Add(this.txtDbUser);
            this.pnlDatabase.Controls.Add(this.lblDbPassword);
            this.pnlDatabase.Controls.Add(this.txtDbPassword);
            this.pnlDatabase.Location = new System.Drawing.Point(0, 0);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(430, 400);
            this.pnlDatabase.TabIndex = 1;
            this.pnlDatabase.Visible = false;

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.Text = "DB Server:";

            this.cmbDbServer.FormattingEnabled = true;
            this.cmbDbServer.Location = new System.Drawing.Point(73, 12);
            this.cmbDbServer.Name = "cmbDbServer";
            this.cmbDbServer.Size = new System.Drawing.Size(220, 21);
            this.cmbDbServer.SelectedIndexChanged += new System.EventHandler(this.cmbDbServer_TextChanged);
            this.cmbDbServer.TextChanged += new System.EventHandler(this.cmbDbServer_TextChanged);
            this.cmbDbServer.Leave += new System.EventHandler(this.cmbDbServer_TextChanged);

            this.chkScanNetwork.AutoSize = true;
            this.chkScanNetwork.Enabled = false;
            this.chkScanNetwork.Location = new System.Drawing.Point(300, 14);
            this.chkScanNetwork.Name = "chkScanNetwork";
            this.chkScanNetwork.Size = new System.Drawing.Size(131, 17);
            this.chkScanNetwork.Text = "Scan network (slower)";
            this.chkScanNetwork.Visible = false;

            this.btnTestConn.Location = new System.Drawing.Point(73, 150);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(115, 23);
            this.btnTestConn.Text = "Test DB connection";
            this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.Text = "DB Name:";

            this.cmbDbDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDbDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDbDatabase.FormattingEnabled = true;
            this.cmbDbDatabase.Location = new System.Drawing.Point(73, 42);
            this.cmbDbDatabase.Name = "cmbDbDatabase";
            this.cmbDbDatabase.Size = new System.Drawing.Size(220, 21);
            this.cmbDbDatabase.Sorted = true;

            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Checked = true;
            this.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuth.Enabled = false;
            this.chkWindowsAuth.Location = new System.Drawing.Point(15, 80);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(141, 17);
            this.chkWindowsAuth.Text = "Windows Authentication";
            this.chkWindowsAuth.Visible = false;
            this.chkWindowsAuth.CheckedChanged += new System.EventHandler(this.chkWindowsAuth_CheckedChanged);

            this.lblDbUser.AutoSize = true;
            this.lblDbUser.Location = new System.Drawing.Point(12, 103);
            this.lblDbUser.Name = "lblDbUser";
            this.lblDbUser.Size = new System.Drawing.Size(61, 13);
            this.lblDbUser.Text = "User name:";

            this.txtDbUser.Location = new System.Drawing.Point(79, 100);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(150, 20);

            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(12, 129);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(56, 13);
            this.lblDbPassword.Text = "Password:";

            this.txtDbPassword.Location = new System.Drawing.Point(79, 126);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(150, 20);

            // =========================================================================
            // GENERAL PANEL
            // =========================================================================
            this.pnlGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGeneral.Controls.Add(this.label6);
            this.pnlGeneral.Controls.Add(this.chkAlwaysOnTop);
            this.pnlGeneral.Controls.Add(this.chkDescriptionColumns);
            this.pnlGeneral.Controls.Add(this.chkResizeColumns);
            this.pnlGeneral.Controls.Add(this.chkShowInTray);
            this.pnlGeneral.Controls.Add(this.chkCopyCleanSql);
            this.pnlGeneral.Controls.Add(this.chkSearchAny);
            this.pnlGeneral.Controls.Add(this.chkMultiInstance);
            this.pnlGeneral.Controls.Add(this.label7);
            this.pnlGeneral.Controls.Add(this.cmbStartTab);
            this.pnlGeneral.Controls.Add(this.chkUnarchiveTarget);
            this.pnlGeneral.Controls.Add(this.lblTableLookup);
            this.pnlGeneral.Controls.Add(this.cmbTableLookup);
            this.pnlGeneral.Controls.Add(this.label8);
            this.pnlGeneral.Controls.Add(this.cmbStartupLocation);
            this.pnlGeneral.Location = new System.Drawing.Point(0, 0);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.Size = new System.Drawing.Size(430, 400);
            this.pnlGeneral.TabIndex = 2;
            this.pnlGeneral.Visible = false;

            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.Text = "Set SMS Search defaults";

            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(18, 45);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.chkAlwaysOnTop.Text = "Always on top";

            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(220, 45);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(143, 17);
            this.chkDescriptionColumns.Text = "Show header description";

            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.Location = new System.Drawing.Point(220, 75);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(201, 17);
            this.chkResizeColumns.Text = "Resize columns on description toggle";

            this.chkShowInTray.AutoSize = true;
            this.chkShowInTray.Location = new System.Drawing.Point(18, 105);
            this.chkShowInTray.Name = "chkShowInTray";
            this.chkShowInTray.Size = new System.Drawing.Size(240, 17);
            this.chkShowInTray.Text = "Show in system tray (will not show in task bar)";

            this.chkCopyCleanSql.AutoSize = true;
            this.chkCopyCleanSql.Location = new System.Drawing.Point(220, 105);
            this.chkCopyCleanSql.Name = "chkCopyCleanSql";
            this.chkCopyCleanSql.Size = new System.Drawing.Size(202, 17);
            this.chkCopyCleanSql.Text = "Copy cleaned SQL query to clipboard";

            this.chkSearchAny.AutoSize = true;
            this.chkSearchAny.Location = new System.Drawing.Point(18, 135);
            this.chkSearchAny.Name = "chkSearchAny";
            this.chkSearchAny.Size = new System.Drawing.Size(174, 17);
            this.chkSearchAny.Text = "Search anywhere in description";

            this.chkMultiInstance.AutoSize = true;
            this.chkMultiInstance.Location = new System.Drawing.Point(220, 135);
            this.chkMultiInstance.Name = "chkMultiInstance";
            this.chkMultiInstance.Size = new System.Drawing.Size(220, 17);
            this.chkMultiInstance.Text = "Enable multiple instances of SMS Search";

            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.Text = "Startup tab";

            this.cmbStartTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTab.FormattingEnabled = true;
            this.cmbStartTab.Items.AddRange(new object[] { "Function", "Totalizer", "Fields" });
            this.cmbStartTab.Location = new System.Drawing.Point(90, 165);
            this.cmbStartTab.Name = "cmbStartTab";
            this.cmbStartTab.Size = new System.Drawing.Size(85, 21);

            this.chkUnarchiveTarget.AutoSize = true;
            this.chkUnarchiveTarget.Location = new System.Drawing.Point(220, 165);
            this.chkUnarchiveTarget.Name = "chkUnarchiveTarget";
            this.chkUnarchiveTarget.Size = new System.Drawing.Size(191, 17);
            this.chkUnarchiveTarget.Text = "Show unarchiving target on startup";

            this.lblTableLookup.AutoSize = true;
            this.lblTableLookup.Location = new System.Drawing.Point(15, 200);
            this.lblTableLookup.Name = "lblTableLookup";
            this.lblTableLookup.Size = new System.Drawing.Size(69, 13);
            this.lblTableLookup.Text = "Table lookup";

            this.cmbTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableLookup.FormattingEnabled = true;
            this.cmbTableLookup.Items.AddRange(new object[] { "Show Fields", "Show Records" });
            this.cmbTableLookup.Location = new System.Drawing.Point(90, 197);
            this.cmbTableLookup.Name = "cmbTableLookup";
            this.cmbTableLookup.Size = new System.Drawing.Size(102, 21);

            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.Text = "Startup location";

            this.cmbStartupLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartupLocation.FormattingEnabled = true;
            this.cmbStartupLocation.Items.AddRange(new object[] { "Primary display", "Active display", "Cursor location", "Last location" });
            this.cmbStartupLocation.Location = new System.Drawing.Point(102, 72);
            this.cmbStartupLocation.Name = "cmbStartupLocation";
            this.cmbStartupLocation.Size = new System.Drawing.Size(100, 21);

            // =========================================================================
            // ADVANCED PANEL
            // =========================================================================
            this.pnlAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAdvanced.Controls.Add(this.groupBox1);
            this.pnlAdvanced.Controls.Add(this.btnTestToast);
            this.pnlAdvanced.Location = new System.Drawing.Point(0, 0);
            this.pnlAdvanced.Name = "pnlAdvanced";
            this.pnlAdvanced.Size = new System.Drawing.Size(430, 400);
            this.pnlAdvanced.TabIndex = 3;
            this.pnlAdvanced.Visible = false;

            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtQryFct);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtQryTlz);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 80);
            this.groupBox1.Text = "Select the fields to query when using filters.";

            this.txtQryFct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQryFct.Location = new System.Drawing.Point(60, 19);
            this.txtQryFct.Name = "txtQryFct";
            this.txtQryFct.Size = new System.Drawing.Size(340, 20);

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.Text = "Function:";

            this.txtQryTlz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQryTlz.Location = new System.Drawing.Point(60, 52);
            this.txtQryTlz.Name = "txtQryTlz";
            this.txtQryTlz.Size = new System.Drawing.Size(340, 20);

            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.Text = "Totalizer:";

            this.btnTestToast.Location = new System.Drawing.Point(10, 100);
            this.btnTestToast.Name = "btnTestToast";
            this.btnTestToast.Size = new System.Drawing.Size(75, 23);
            this.btnTestToast.Text = "Test Toast";
            this.btnTestToast.Click += new System.EventHandler(this.btnTestToast_Click);

            // =========================================================================
            // UPDATE PANEL
            // =========================================================================
            this.pnlUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUpdate.Controls.Add(this.chkCheckUpdate);
            this.pnlUpdate.Controls.Add(this.btnChkUpdate);
            this.pnlUpdate.Location = new System.Drawing.Point(0, 0);
            this.pnlUpdate.Name = "pnlUpdate";
            this.pnlUpdate.Size = new System.Drawing.Size(430, 400);
            this.pnlUpdate.TabIndex = 4;
            this.pnlUpdate.Visible = false;

            this.chkCheckUpdate.AutoSize = true;
            this.chkCheckUpdate.Location = new System.Drawing.Point(20, 20);
            this.chkCheckUpdate.Name = "chkCheckUpdate";
            this.chkCheckUpdate.Size = new System.Drawing.Size(222, 17);
            this.chkCheckUpdate.Text = "Automatically check for update on startup";

            this.btnChkUpdate.Location = new System.Drawing.Point(20, 50);
            this.btnChkUpdate.Name = "btnChkUpdate";
            this.btnChkUpdate.Size = new System.Drawing.Size(140, 30);
            this.btnChkUpdate.Text = "Check for update now";
            this.btnChkUpdate.Click += new System.EventHandler(this.btnChkUpdate_Click);

            // =========================================================================
            // LOGGING PANEL
            // =========================================================================
            this.pnlLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogging.Controls.Add(this.chkLogging);
            this.pnlLogging.Controls.Add(this.lblLogLevel);
            this.pnlLogging.Controls.Add(this.cmbLogLevel);
            this.pnlLogging.Controls.Add(this.lblRetention);
            this.pnlLogging.Controls.Add(this.numRetention);
            this.pnlLogging.Location = new System.Drawing.Point(0, 0);
            this.pnlLogging.Name = "pnlLogging";
            this.pnlLogging.Size = new System.Drawing.Size(430, 400);
            this.pnlLogging.TabIndex = 5;
            this.pnlLogging.Visible = false;

            this.chkLogging.AutoSize = true;
            this.chkLogging.Location = new System.Drawing.Point(20, 20);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(96, 17);
            this.chkLogging.Text = "Enable logging";

            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(20, 53);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(53, 13);
            this.lblLogLevel.Text = "Log Level:";

            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Items.AddRange(new object[] { "Critical", "Error", "Warning", "Info", "Debug" });
            this.cmbLogLevel.Location = new System.Drawing.Point(80, 50);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(80, 21);

            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(180, 53);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(100, 13);
            this.lblRetention.Text = "Retention (Days):";

            this.numRetention.Location = new System.Drawing.Point(290, 51);
            this.numRetention.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.numRetention.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numRetention.Name = "numRetention";
            this.numRetention.Size = new System.Drawing.Size(50, 20);
            this.numRetention.Value = new decimal(new int[] { 14, 0, 0, 0 });

            // =========================================================================
            // CLEAN SQL PANEL
            // =========================================================================
            this.pnlCleanSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCleanSql.Controls.Add(this.dgvCleanSqlRules);
            this.pnlCleanSql.Controls.Add(this.btnResetCleanSql);
            this.pnlCleanSql.Location = new System.Drawing.Point(0, 0);
            this.pnlCleanSql.Name = "pnlCleanSql";
            this.pnlCleanSql.Size = new System.Drawing.Size(430, 400);
            this.pnlCleanSql.TabIndex = 6;
            this.pnlCleanSql.Visible = false;

            this.dgvCleanSqlRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCleanSqlRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCleanSqlRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colRegex, this.colReplace });
            this.dgvCleanSqlRules.Location = new System.Drawing.Point(10, 10);
            this.dgvCleanSqlRules.Name = "dgvCleanSqlRules";
            this.dgvCleanSqlRules.Size = new System.Drawing.Size(410, 350);

            this.colRegex.HeaderText = "Regex Pattern";
            this.colRegex.Name = "colRegex";
            this.colRegex.Width = 200;

            this.colReplace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colReplace.HeaderText = "Replacement";
            this.colReplace.Name = "colReplace";

            this.btnResetCleanSql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetCleanSql.Location = new System.Drawing.Point(335, 370);
            this.btnResetCleanSql.Name = "btnResetCleanSql";
            this.btnResetCleanSql.Size = new System.Drawing.Size(85, 23);
            this.btnResetCleanSql.Text = "Reset Defaults";
            this.btnResetCleanSql.Click += new System.EventHandler(this.btnResetCleanSql_Click);

            // =========================================================================
            // LAUNCHER PANEL
            // =========================================================================
            this.pnlLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLauncher.Controls.Add(this.lblLauncherInfo);
            this.pnlLauncher.Controls.Add(this.lblHotkey);
            this.pnlLauncher.Controls.Add(this.txtHotkey);
            this.pnlLauncher.Controls.Add(this.btnRegister);
            this.pnlLauncher.Controls.Add(this.btnUnregister);
            this.pnlLauncher.Controls.Add(this.lblLauncherStatus);
            this.pnlLauncher.Controls.Add(this.pbLauncherStatus);
            this.pnlLauncher.Location = new System.Drawing.Point(0, 0);
            this.pnlLauncher.Name = "pnlLauncher";
            this.pnlLauncher.Size = new System.Drawing.Size(430, 400);
            this.pnlLauncher.TabIndex = 7;
            this.pnlLauncher.Visible = false;

            this.lblLauncherInfo.AutoSize = true;
            this.lblLauncherInfo.Location = new System.Drawing.Point(16, 16);
            this.lblLauncherInfo.Name = "lblLauncherInfo";
            this.lblLauncherInfo.Size = new System.Drawing.Size(400, 13);
            this.lblLauncherInfo.Text = "Update the hidden launcher settings to switch to SMS Search via global hotkey.";

            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(16, 50);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(44, 13);
            this.lblHotkey.Text = "Hotkey:";

            this.txtHotkey.Location = new System.Drawing.Point(66, 47);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.Size = new System.Drawing.Size(200, 20);
            this.txtHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHotkey_KeyDown);

            this.btnRegister.Location = new System.Drawing.Point(19, 90);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 23);
            this.btnRegister.Text = "Register Service";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            this.btnUnregister.Location = new System.Drawing.Point(150, 90);
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(120, 23);
            this.btnUnregister.Text = "Unregister Service";
            this.btnUnregister.Click += new System.EventHandler(this.btnUnregister_Click);

            this.pbLauncherStatus.Location = new System.Drawing.Point(19, 130);
            this.pbLauncherStatus.Name = "pbLauncherStatus";
            this.pbLauncherStatus.Size = new System.Drawing.Size(16, 16);

            this.lblLauncherStatus.AutoSize = true;
            this.lblLauncherStatus.Location = new System.Drawing.Point(40, 132);
            this.lblLauncherStatus.Name = "lblLauncherStatus";
            this.lblLauncherStatus.Size = new System.Drawing.Size(0, 13);

            // =========================================================================
            // BUTTONS & FORM
            // =========================================================================
            this.btnResetConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetConfig.Location = new System.Drawing.Point(340, 415);
            this.btnResetConfig.Name = "btnResetConfig";
            this.btnResetConfig.Size = new System.Drawing.Size(75, 23);
            this.btnResetConfig.Text = "Reload";
            this.toolTip1.SetToolTip(this.btnResetConfig, "Reload settings from file");
            this.btnResetConfig.Click += new System.EventHandler(this.btnReloadConfig_Click);

            this.btnApplyConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyConfig.Location = new System.Drawing.Point(421, 415);
            this.btnApplyConfig.Name = "btnApplyConfig";
            this.btnApplyConfig.Size = new System.Drawing.Size(75, 23);
            this.btnApplyConfig.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApplyConfig, "Save all settings changes to file");
            this.btnApplyConfig.Click += new System.EventHandler(this.btnApplyConfig_Click);

            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Close";
            this.toolTip1.SetToolTip(this.btnCancel, "Close settings without saving changes");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(259, 415);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.Text = "OK";
            this.toolTip1.SetToolTip(this.btnOk, "Apply changes and close settings");
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

            this.lblConfigFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblConfigFilePath.AutoEllipsis = true;
            this.lblConfigFilePath.AutoSize = true;
            this.lblConfigFilePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConfigFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblConfigFilePath.Location = new System.Drawing.Point(12, 420);
            this.lblConfigFilePath.MaximumSize = new System.Drawing.Size(240, 13);
            this.lblConfigFilePath.Name = "lblConfigFilePath";
            this.lblConfigFilePath.Size = new System.Drawing.Size(85, 13);
            this.lblConfigFilePath.Text = "lblConfigFilePath";
            this.lblConfigFilePath.Click += new System.EventHandler(this.lblConfigFilePath_Click);

            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;

            // Form
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 450);
            this.Controls.Add(this.splitConfig);
            this.Controls.Add(this.btnResetConfig);
            this.Controls.Add(this.btnApplyConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblConfigFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "frmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SMS Search Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.Shown += new System.EventHandler(this.frmConfig_Shown);

            this.splitConfig.Panel1.ResumeLayout(false);
            this.splitConfig.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).EndInit();
            this.splitConfig.ResumeLayout(false);
            this.pnlGeneral.ResumeLayout(false);
            this.pnlGeneral.PerformLayout();
            this.pnlDatabase.ResumeLayout(false);
            this.pnlDatabase.PerformLayout();
            this.pnlAdvanced.ResumeLayout(false);
            this.pnlAdvanced.PerformLayout();
            this.pnlUpdate.ResumeLayout(false);
            this.pnlUpdate.PerformLayout();
            this.pnlLogging.ResumeLayout(false);
            this.pnlLogging.PerformLayout();
            this.pnlCleanSql.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).EndInit();
            this.pnlLauncher.ResumeLayout(false);
            this.pnlLauncher.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
