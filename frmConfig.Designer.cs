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
        // private static string ConfigFilePath = ".\\SMS Search.ini";
        // private IniFile ini = new IniFile(frmConfig.ConfigFilePath);
        private UpdateChecker Versions = new UpdateChecker();
        private dbConnector dbConn = new dbConnector();
        private ArrayList ServerNames = new ArrayList();
        private ArrayList DbNames = new ArrayList();
        //private IContainer components;
        private Button btnResetConfig;
        private Button btnApplyConfig;
        private Label label4;
        private Label label3;
        private Label lblConfigFilePath;
        private ToolTip toolTip1;
        private Button btnCancel;
        private Button btnOk;
        private ComboBox cmbDbDatabase;
        private Button btnTestConn;
        private ComboBox cmbDbServer;
        private CheckBox chkScanNetwork;
        private CheckBox chkWindowsAuth;
        private TextBox txtDbUser;
        private TextBox txtDbPassword;
        private Label lblDbUser;
        private Label lblDbPassword;
        private TabPage tabGeneral;
        private CheckBox chkUnarchiveTarget;
        private CheckBox chkCopyCleanSql;
        private CheckBox chkResizeColumns;
        private ComboBox cmbStartTab;
        private Label label7;
        private Label label6;
        private CheckBox chkSearchAny;
        private CheckBox chkShowInTray;
        private CheckBox chkAlwaysOnTop;
        private TabControl tabCtlConfig;
        private ComboBox cmbTableLookup;
        private Label lblTableLookup;
        private TabPage tabAdvanced;
        private TextBox txtQryTlz;
        private Label label5;
        private Label label2;
        private TextBox txtQryFct;
        private CheckBox chkMultiInstance;
        private CheckBox chkLogging;
        private ComboBox cmbLogLevel;
        private Label lblLogLevel;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private GroupBox groupBox1;
        private CheckBox chkDescriptionColumns;
        private CheckBox chkCheckUpdate;
        private Button btnChkUpdate;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnResetConfig = new System.Windows.Forms.Button();
            this.btnApplyConfig = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblConfigFilePath = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cmbDbDatabase = new System.Windows.Forms.ComboBox();
            this.btnTestConn = new System.Windows.Forms.Button();
            this.cmbDbServer = new System.Windows.Forms.ComboBox();
            this.chkScanNetwork = new System.Windows.Forms.CheckBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.lblDbUser = new System.Windows.Forms.Label();
            this.lblDbPassword = new System.Windows.Forms.Label();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkDescriptionColumns = new System.Windows.Forms.CheckBox();
            this.chkMultiInstance = new System.Windows.Forms.CheckBox();
            this.cmbTableLookup = new System.Windows.Forms.ComboBox();
            this.lblTableLookup = new System.Windows.Forms.Label();
            this.chkUnarchiveTarget = new System.Windows.Forms.CheckBox();
            this.chkCopyCleanSql = new System.Windows.Forms.CheckBox();
            this.chkResizeColumns = new System.Windows.Forms.CheckBox();
            this.cmbStartTab = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSearchAny = new System.Windows.Forms.CheckBox();
            this.chkShowInTray = new System.Windows.Forms.CheckBox();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.tabCtlConfig = new System.Windows.Forms.TabControl();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.btnChkUpdate = new System.Windows.Forms.Button();
            this.chkCheckUpdate = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtQryFct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQryTlz = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.cmbStartupLocation = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabCleanSql = new System.Windows.Forms.TabPage();
            this.dgvCleanSqlRules = new System.Windows.Forms.DataGridView();
            this.colRegex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnResetCleanSql = new System.Windows.Forms.Button();
            this.btnTestToast = new System.Windows.Forms.Button();
            this.tabGeneral.SuspendLayout();
            this.tabCtlConfig.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.tabCleanSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).BeginInit();
            this.tabLauncher = new System.Windows.Forms.TabPage();
            this.lblLauncherInfo = new System.Windows.Forms.Label();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUnregister = new System.Windows.Forms.Button();
            this.lblLauncherStatus = new System.Windows.Forms.Label();
            this.pbLauncherStatus = new System.Windows.Forms.PictureBox();
            this.tabLauncher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // btnResetConfig
            // 
            this.btnResetConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetConfig.Location = new System.Drawing.Point(321, 404);
            this.btnResetConfig.Name = "btnResetConfig";
            this.btnResetConfig.Size = new System.Drawing.Size(75, 23);
            this.btnResetConfig.TabIndex = 5;
            this.btnResetConfig.Text = "Reload";
            this.toolTip1.SetToolTip(this.btnResetConfig, "Reload settings from file");
            this.btnResetConfig.UseVisualStyleBackColor = true;
            this.btnResetConfig.Click += new System.EventHandler(this.btnReloadConfig_Click);
            // 
            // btnApplyConfig
            // 
            this.btnApplyConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyConfig.Location = new System.Drawing.Point(402, 404);
            this.btnApplyConfig.Name = "btnApplyConfig";
            this.btnApplyConfig.Size = new System.Drawing.Size(75, 23);
            this.btnApplyConfig.TabIndex = 6;
            this.btnApplyConfig.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApplyConfig, "Save all settings changes to file");
            this.btnApplyConfig.UseVisualStyleBackColor = true;
            this.btnApplyConfig.Click += new System.EventHandler(this.btnApplyConfig_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "DB Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "DB Server:";
            // 
            // lblConfigFilePath
            // 
            this.lblConfigFilePath.AutoEllipsis = true;
            this.lblConfigFilePath.AutoSize = true;
            this.lblConfigFilePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConfigFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblConfigFilePath.Location = new System.Drawing.Point(12, 9);
            this.lblConfigFilePath.MaximumSize = new System.Drawing.Size(530, 13);
            this.lblConfigFilePath.Name = "lblConfigFilePath";
            this.lblConfigFilePath.Size = new System.Drawing.Size(85, 13);
            this.lblConfigFilePath.TabIndex = 0;
            this.lblConfigFilePath.Text = "lblConfigFilePath";
            this.lblConfigFilePath.Click += new System.EventHandler(this.lblConfigFilePath_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(483, 404);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Close";
            this.toolTip1.SetToolTip(this.btnCancel, "Close settings without saving changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(240, 404);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.toolTip1.SetToolTip(this.btnOk, "Apply changes and close settings");
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cmbDbDatabase
            // 
            this.cmbDbDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDbDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDbDatabase.FormattingEnabled = true;
            this.cmbDbDatabase.Location = new System.Drawing.Point(73, 59);
            this.cmbDbDatabase.Name = "cmbDbDatabase";
            this.cmbDbDatabase.Size = new System.Drawing.Size(196, 21);
            this.cmbDbDatabase.Sorted = true;
            this.cmbDbDatabase.TabIndex = 1;
            // 
            // btnTestConn
            // 
            this.btnTestConn.Location = new System.Drawing.Point(443, 30);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(115, 23);
            this.btnTestConn.TabIndex = 2;
            this.btnTestConn.Text = "Test DB connection";
            this.btnTestConn.UseVisualStyleBackColor = true;
            this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);
            // 
            // cmbDbServer
            // 
            this.cmbDbServer.FormattingEnabled = true;
            this.cmbDbServer.Location = new System.Drawing.Point(73, 32);
            this.cmbDbServer.Name = "cmbDbServer";
            this.cmbDbServer.Size = new System.Drawing.Size(196, 21);
            this.cmbDbServer.TabIndex = 0;
            this.cmbDbServer.SelectedIndexChanged += new System.EventHandler(this.cmbDbServer_TextChanged);
            this.cmbDbServer.TextChanged += new System.EventHandler(this.cmbDbServer_TextChanged);
            this.cmbDbServer.Leave += new System.EventHandler(this.cmbDbServer_TextChanged);
            // 
            // chkScanNetwork
            // 
            this.chkScanNetwork.AutoSize = true;
            this.chkScanNetwork.Enabled = false;
            this.chkScanNetwork.Location = new System.Drawing.Point(275, 34);
            this.chkScanNetwork.Name = "chkScanNetwork";
            this.chkScanNetwork.Size = new System.Drawing.Size(131, 17);
            this.chkScanNetwork.TabIndex = 18;
            this.chkScanNetwork.Text = "Scan network (slower)";
            this.chkScanNetwork.UseVisualStyleBackColor = true;
            this.chkScanNetwork.Visible = false;
            // 
            // chkWindowsAuth
            // 
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Checked = true;
            this.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuth.Enabled = false;
            this.chkWindowsAuth.Location = new System.Drawing.Point(15, 86);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(141, 17);
            this.chkWindowsAuth.TabIndex = 19;
            this.chkWindowsAuth.Text = "Windows Authentication";
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            this.chkWindowsAuth.Visible = false;
            this.chkWindowsAuth.CheckedChanged += new System.EventHandler(this.chkWindowsAuth_CheckedChanged);
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(95, 107);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(150, 20);
            this.txtDbUser.TabIndex = 20;
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(317, 107);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(150, 20);
            this.txtDbPassword.TabIndex = 21;
            // 
            // lblDbUser
            // 
            this.lblDbUser.AutoSize = true;
            this.lblDbUser.Location = new System.Drawing.Point(32, 110);
            this.lblDbUser.Name = "lblDbUser";
            this.lblDbUser.Size = new System.Drawing.Size(61, 13);
            this.lblDbUser.TabIndex = 22;
            this.lblDbUser.Text = "User name:";
            // 
            // lblDbPassword
            // 
            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(259, 110);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(56, 13);
            this.lblDbPassword.TabIndex = 23;
            this.lblDbPassword.Text = "Password:";
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabGeneral.Controls.Add(this.cmbStartupLocation);
            this.tabGeneral.Controls.Add(this.label8);
            this.tabGeneral.Controls.Add(this.chkDescriptionColumns);
            this.tabGeneral.Controls.Add(this.chkMultiInstance);
            this.tabGeneral.Controls.Add(this.cmbTableLookup);
            this.tabGeneral.Controls.Add(this.lblTableLookup);
            this.tabGeneral.Controls.Add(this.chkUnarchiveTarget);
            this.tabGeneral.Controls.Add(this.chkCopyCleanSql);
            this.tabGeneral.Controls.Add(this.chkResizeColumns);
            this.tabGeneral.Controls.Add(this.cmbStartTab);
            this.tabGeneral.Controls.Add(this.label7);
            this.tabGeneral.Controls.Add(this.label6);
            this.tabGeneral.Controls.Add(this.chkSearchAny);
            this.tabGeneral.Controls.Add(this.chkShowInTray);
            this.tabGeneral.Controls.Add(this.chkAlwaysOnTop);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(539, 227);
            this.tabGeneral.TabIndex = 3;
            this.tabGeneral.Text = "General";
            // 
            // chkDescriptionColumns
            // 
            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(295, 42);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(143, 17);
            this.chkDescriptionColumns.TabIndex = 45;
            this.chkDescriptionColumns.Text = "Show header description";
            this.chkDescriptionColumns.UseVisualStyleBackColor = true;
            // 
            // chkMultiInstance
            // 
            this.chkMultiInstance.AutoSize = true;
            this.chkMultiInstance.Location = new System.Drawing.Point(295, 138);
            this.chkMultiInstance.Name = "chkMultiInstance";
            this.chkMultiInstance.Size = new System.Drawing.Size(220, 17);
            this.chkMultiInstance.TabIndex = 44;
            this.chkMultiInstance.Text = "Enable multiple instances of SMS Search";
            this.chkMultiInstance.UseVisualStyleBackColor = true;
            // 
            // cmbTableLookup
            // 
            this.cmbTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableLookup.FormattingEnabled = true;
            this.cmbTableLookup.Items.AddRange(new object[] {
            "Show Fields",
            "Show Records"});
            this.cmbTableLookup.Location = new System.Drawing.Point(92, 192);
            this.cmbTableLookup.Name = "cmbTableLookup";
            this.cmbTableLookup.Size = new System.Drawing.Size(102, 21);
            this.cmbTableLookup.TabIndex = 43;
            // 
            // lblTableLookup
            // 
            this.lblTableLookup.AutoSize = true;
            this.lblTableLookup.Location = new System.Drawing.Point(17, 195);
            this.lblTableLookup.Name = "lblTableLookup";
            this.lblTableLookup.Size = new System.Drawing.Size(69, 13);
            this.lblTableLookup.TabIndex = 42;
            this.lblTableLookup.Text = "Table lookup";
            // 
            // chkUnarchiveTarget
            // 
            this.chkUnarchiveTarget.AutoSize = true;
            this.chkUnarchiveTarget.Location = new System.Drawing.Point(295, 174);
            this.chkUnarchiveTarget.Name = "chkUnarchiveTarget";
            this.chkUnarchiveTarget.Size = new System.Drawing.Size(191, 17);
            this.chkUnarchiveTarget.TabIndex = 41;
            this.chkUnarchiveTarget.Text = "Show unarchiving target on startup";
            this.chkUnarchiveTarget.UseVisualStyleBackColor = true;
            // 
            // chkCopyCleanSql
            // 
            this.chkCopyCleanSql.AutoSize = true;
            this.chkCopyCleanSql.Location = new System.Drawing.Point(295, 106);
            this.chkCopyCleanSql.Name = "chkCopyCleanSql";
            this.chkCopyCleanSql.Size = new System.Drawing.Size(202, 17);
            this.chkCopyCleanSql.TabIndex = 39;
            this.chkCopyCleanSql.Text = "Copy cleaned SQL query to clipboard";
            this.chkCopyCleanSql.UseVisualStyleBackColor = true;
            // 
            // chkResizeColumns
            // 
            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.Location = new System.Drawing.Point(295, 74);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(201, 17);
            this.chkResizeColumns.TabIndex = 38;
            this.chkResizeColumns.Text = "Resize columns on description toggle";
            this.chkResizeColumns.UseVisualStyleBackColor = true;
            // 
            // cmbStartTab
            // 
            this.cmbStartTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTab.FormattingEnabled = true;
            this.cmbStartTab.Items.AddRange(new object[] {
            "Function",
            "Totalizer",
            "Fields"});
            this.cmbStartTab.Location = new System.Drawing.Point(92, 164);
            this.cmbStartTab.Name = "cmbStartTab";
            this.cmbStartTab.Size = new System.Drawing.Size(85, 21);
            this.cmbStartTab.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Startup tab";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Set SMS Search defaults";
            // 
            // chkSearchAny
            // 
            this.chkSearchAny.AutoSize = true;
            this.chkSearchAny.Location = new System.Drawing.Point(20, 138);
            this.chkSearchAny.Name = "chkSearchAny";
            this.chkSearchAny.Size = new System.Drawing.Size(174, 17);
            this.chkSearchAny.TabIndex = 3;
            this.chkSearchAny.Text = "Search anywhere in description";
            this.chkSearchAny.UseVisualStyleBackColor = true;
            // 
            // chkShowInTray
            // 
            this.chkShowInTray.AutoSize = true;
            this.chkShowInTray.Location = new System.Drawing.Point(20, 106);
            this.chkShowInTray.Name = "chkShowInTray";
            this.chkShowInTray.Size = new System.Drawing.Size(240, 17);
            this.chkShowInTray.TabIndex = 2;
            this.chkShowInTray.Text = "Show in system tray (will not show in task bar)";
            this.chkShowInTray.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(21, 42);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.chkAlwaysOnTop.TabIndex = 1;
            this.chkAlwaysOnTop.Text = "Always on top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // tabCtlConfig
            // 
            this.tabCtlConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtlConfig.Controls.Add(this.tabGeneral);
            this.tabCtlConfig.Controls.Add(this.tabAdvanced);
            this.tabCtlConfig.Controls.Add(this.tabCleanSql);
            this.tabCtlConfig.Controls.Add(this.tabLauncher);
            this.tabCtlConfig.Location = new System.Drawing.Point(11, 145);
            this.tabCtlConfig.Name = "tabCtlConfig";
            this.tabCtlConfig.SelectedIndex = 0;
            this.tabCtlConfig.Size = new System.Drawing.Size(547, 253);
            this.tabCtlConfig.TabIndex = 3;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.BackColor = System.Drawing.SystemColors.Control;
            this.tabAdvanced.Controls.Add(this.btnChkUpdate);
            this.tabAdvanced.Controls.Add(this.btnTestToast);
            this.tabAdvanced.Controls.Add(this.chkCheckUpdate);
            this.tabAdvanced.Controls.Add(this.groupBox1);
            this.tabAdvanced.Controls.Add(this.chkLogging);
            this.tabAdvanced.Controls.Add(this.lblLogLevel);
            this.tabAdvanced.Controls.Add(this.cmbLogLevel);
            this.tabAdvanced.Controls.Add(this.lblRetention);
            this.tabAdvanced.Controls.Add(this.numRetention);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced.Size = new System.Drawing.Size(539, 205);
            this.tabAdvanced.TabIndex = 4;
            this.tabAdvanced.Text = "Advanced";
            // 
            // tabCleanSql
            //
            this.tabCleanSql.Controls.Add(this.dgvCleanSqlRules);
            this.tabCleanSql.Controls.Add(this.btnResetCleanSql);
            this.tabCleanSql.Location = new System.Drawing.Point(4, 22);
            this.tabCleanSql.Name = "tabCleanSql";
            this.tabCleanSql.Padding = new System.Windows.Forms.Padding(3);
            this.tabCleanSql.Size = new System.Drawing.Size(539, 227);
            this.tabCleanSql.TabIndex = 5;
            this.tabCleanSql.Text = "Clean SQL";
            this.tabCleanSql.UseVisualStyleBackColor = true;
            //
            // dgvCleanSqlRules
            //
            this.dgvCleanSqlRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCleanSqlRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCleanSqlRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRegex,
            this.colReplace});
            this.dgvCleanSqlRules.Location = new System.Drawing.Point(6, 6);
            this.dgvCleanSqlRules.Name = "dgvCleanSqlRules";
            this.dgvCleanSqlRules.Size = new System.Drawing.Size(527, 185);
            this.dgvCleanSqlRules.TabIndex = 0;
            //
            // colRegex
            //
            this.colRegex.HeaderText = "Regex Pattern";
            this.colRegex.Name = "colRegex";
            this.colRegex.Width = 250;
            //
            // colReplace
            //
            this.colReplace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colReplace.HeaderText = "Replacement";
            this.colReplace.Name = "colReplace";
            //
            // btnResetCleanSql
            //
            this.btnResetCleanSql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetCleanSql.Location = new System.Drawing.Point(448, 198);
            this.btnResetCleanSql.Name = "btnResetCleanSql";
            this.btnResetCleanSql.Size = new System.Drawing.Size(85, 23);
            this.btnResetCleanSql.TabIndex = 1;
            this.btnResetCleanSql.Text = "Reset Defaults";
            this.btnResetCleanSql.UseVisualStyleBackColor = true;
            this.btnResetCleanSql.Click += new System.EventHandler(this.btnResetCleanSql_Click);
            //
            // btnChkUpdate
            // 
            this.btnChkUpdate.Location = new System.Drawing.Point(452, 92);
            this.btnChkUpdate.Name = "btnChkUpdate";
            this.btnChkUpdate.Size = new System.Drawing.Size(75, 41);
            this.btnChkUpdate.TabIndex = 10;
            this.btnChkUpdate.Text = "Check for update now";
            this.btnChkUpdate.UseVisualStyleBackColor = true;
            this.btnChkUpdate.Click += new System.EventHandler(this.btnChkUpdate_Click);
            // 
            // btnTestToast
            //
            this.btnTestToast.Location = new System.Drawing.Point(452, 140);
            this.btnTestToast.Name = "btnTestToast";
            this.btnTestToast.Size = new System.Drawing.Size(75, 23);
            this.btnTestToast.TabIndex = 11;
            this.btnTestToast.Text = "Test Toast";
            this.btnTestToast.UseVisualStyleBackColor = true;
            this.btnTestToast.Click += new System.EventHandler(this.btnTestToast_Click);
            //
            // tabLauncher
            //
            this.tabLauncher.BackColor = System.Drawing.SystemColors.Control;
            this.tabLauncher.Controls.Add(this.pbLauncherStatus);
            this.tabLauncher.Controls.Add(this.lblLauncherStatus);
            this.tabLauncher.Controls.Add(this.btnUnregister);
            this.tabLauncher.Controls.Add(this.btnRegister);
            this.tabLauncher.Controls.Add(this.txtHotkey);
            this.tabLauncher.Controls.Add(this.lblHotkey);
            this.tabLauncher.Controls.Add(this.lblLauncherInfo);
            this.tabLauncher.Location = new System.Drawing.Point(4, 22);
            this.tabLauncher.Name = "tabLauncher";
            this.tabLauncher.Padding = new System.Windows.Forms.Padding(3);
            this.tabLauncher.Size = new System.Drawing.Size(539, 227);
            this.tabLauncher.TabIndex = 6;
            this.tabLauncher.Text = "Launcher";
            //
            // lblLauncherInfo
            //
            this.lblLauncherInfo.AutoSize = true;
            this.lblLauncherInfo.Location = new System.Drawing.Point(16, 16);
            this.lblLauncherInfo.Name = "lblLauncherInfo";
            this.lblLauncherInfo.Size = new System.Drawing.Size(400, 13);
            this.lblLauncherInfo.TabIndex = 0;
            this.lblLauncherInfo.Text = "Update the hidden launcher settings to switch to SMS Search via global hotkey.";
            //
            // lblHotkey
            //
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(16, 50);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(44, 13);
            this.lblHotkey.TabIndex = 1;
            this.lblHotkey.Text = "Hotkey:";
            //
            // txtHotkey
            //
            this.txtHotkey.Location = new System.Drawing.Point(66, 47);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.Size = new System.Drawing.Size(200, 20);
            this.txtHotkey.TabIndex = 2;
            this.txtHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHotkey_KeyDown);
            //
            // btnRegister
            //
            this.btnRegister.Location = new System.Drawing.Point(19, 90);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 23);
            this.btnRegister.TabIndex = 3;
            this.btnRegister.Text = "Register Service";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            //
            // btnUnregister
            //
            this.btnUnregister.Location = new System.Drawing.Point(150, 90);
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(120, 23);
            this.btnUnregister.TabIndex = 4;
            this.btnUnregister.Text = "Unregister Service";
            this.btnUnregister.UseVisualStyleBackColor = true;
            this.btnUnregister.Click += new System.EventHandler(this.btnUnregister_Click);
            //
            // lblLauncherStatus
            //
            this.lblLauncherStatus.AutoSize = true;
            this.lblLauncherStatus.Location = new System.Drawing.Point(40, 132);
            this.lblLauncherStatus.Name = "lblLauncherStatus";
            this.lblLauncherStatus.Size = new System.Drawing.Size(0, 13);
            this.lblLauncherStatus.TabIndex = 5;
            //
            // pbLauncherStatus
            //
            this.pbLauncherStatus.Location = new System.Drawing.Point(19, 130);
            this.pbLauncherStatus.Name = "pbLauncherStatus";
            this.pbLauncherStatus.Size = new System.Drawing.Size(16, 16);
            this.pbLauncherStatus.TabIndex = 6;
            this.pbLauncherStatus.TabStop = false;
            //
            // chkCheckUpdate
            // 
            this.chkCheckUpdate.AutoSize = true;
            this.chkCheckUpdate.Location = new System.Drawing.Point(16, 92);
            this.chkCheckUpdate.Name = "chkCheckUpdate";
            this.chkCheckUpdate.Size = new System.Drawing.Size(222, 17);
            this.chkCheckUpdate.TabIndex = 7;
            this.chkCheckUpdate.Text = "Automatically check for update on startup";
            this.chkCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtQryFct);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtQryTlz);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(530, 80);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select the fields to query when using filters.";
            // 
            // txtQryFct
            // 
            this.txtQryFct.Location = new System.Drawing.Point(60, 19);
            this.txtQryFct.Name = "txtQryFct";
            this.txtQryFct.Size = new System.Drawing.Size(464, 20);
            this.txtQryFct.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Function:";
            // 
            // txtQryTlz
            // 
            this.txtQryTlz.Location = new System.Drawing.Point(59, 52);
            this.txtQryTlz.Name = "txtQryTlz";
            this.txtQryTlz.Size = new System.Drawing.Size(465, 20);
            this.txtQryTlz.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Totalizer:";
            // 
            // chkLogging
            // 
            this.chkLogging.AutoSize = true;
            this.chkLogging.Location = new System.Drawing.Point(16, 172);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(96, 17);
            this.chkLogging.TabIndex = 5;
            this.chkLogging.Text = "Enable logging";
            this.chkLogging.UseVisualStyleBackColor = true;
            // 
            // cmbLogLevel
            //
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Items.AddRange(new object[] {
            "Critical",
            "Error",
            "Warning",
            "Info",
            "Debug"});
            this.cmbLogLevel.Location = new System.Drawing.Point(190, 170);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(80, 21);
            this.cmbLogLevel.TabIndex = 12;
            //
            // lblLogLevel
            //
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(130, 173);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(53, 13);
            this.lblLogLevel.TabIndex = 13;
            this.lblLogLevel.Text = "Log Level:";
            //
            // numRetention
            //
            this.numRetention.Location = new System.Drawing.Point(400, 171);
            this.numRetention.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numRetention.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRetention.Name = "numRetention";
            this.numRetention.Size = new System.Drawing.Size(50, 20);
            this.numRetention.TabIndex = 14;
            this.numRetention.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            //
            // lblRetention
            //
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(290, 173);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(100, 13);
            this.lblRetention.TabIndex = 15;
            this.lblRetention.Text = "Retention (Days):";
            //
            // cmbStartupLocation
            // 
            this.cmbStartupLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartupLocation.FormattingEnabled = true;
            this.cmbStartupLocation.Items.AddRange(new object[] {
            "Primary display",
            "Active display",
            "Cursor location",
            "Last location"});
            this.cmbStartupLocation.Location = new System.Drawing.Point(104, 72);
            this.cmbStartupLocation.Name = "cmbStartupLocation";
            this.cmbStartupLocation.Size = new System.Drawing.Size(117, 21);
            this.cmbStartupLocation.TabIndex = 47;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "Startup location";
            // 
            // frmConfig
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(564, 431);
            this.Controls.Add(this.chkWindowsAuth);
            this.Controls.Add(this.chkScanNetwork);
            this.Controls.Add(this.cmbDbServer);
            this.Controls.Add(this.btnTestConn);
            this.Controls.Add(this.cmbDbDatabase);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabCtlConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnResetConfig);
            this.Controls.Add(this.btnApplyConfig);
            this.Controls.Add(this.lblConfigFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblDbPassword);
            this.Controls.Add(this.lblDbUser);
            this.Controls.Add(this.txtDbPassword);
            this.Controls.Add(this.txtDbUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(580, 470);
            this.Name = "frmConfig";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SMS Search Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.Shown += new System.EventHandler(this.frmConfig_Shown);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabCtlConfig.ResumeLayout(false);
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.tabCleanSql.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).EndInit();
            this.tabLauncher.ResumeLayout(false);
            this.tabLauncher.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ComboBox cmbStartupLocation;
        private Label label8;
        private TabPage tabCleanSql;
        private DataGridView dgvCleanSqlRules;
        private DataGridViewTextBoxColumn colRegex;
        private DataGridViewTextBoxColumn colReplace;
        private Button btnResetCleanSql;
        private Button btnTestToast;
        private TabPage tabLauncher;
        private Label lblLauncherInfo;
        private Label lblHotkey;
        private TextBox txtHotkey;
        private Button btnRegister;
        private Button btnUnregister;
        private Label lblLauncherStatus;
        private PictureBox pbLauncherStatus;
    }
}
