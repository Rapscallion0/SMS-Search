using SMS_Search.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMS_Search
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /*protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //components.Dispose();
            }
            Dispose(disposing);
        }*/

        private BindingSource bindingSource;
        private Button btnPopGrid;
        private SMS_Search.ExDataGridView dGrd;
        private ToolTip toolTip;
        private GroupBox groupBox1;
        private DateTimePicker dateGregorian;
        private Label label1;
        private TextBox txtJulian;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private RadioButton rdbCustSqlFct;
        private Button btnBuildQryFct;
        private Button btnClearFct;
        private RadioButton rdbNumFct;
        private RadioButton rdbDescFct;
        private TextBox txtDescFct;
        private TextBox txtNumFct;
        private TextBox txtCustSqlFct;
        private RadioButton rdbCustSqlTlz;
        private RadioButton rdbNumTlz;
        private Button btnBuildQryTlz;
        private TextBox txtCustSqlTlz;
        private Button btnClearTlz;
        private TextBox txtNumTlz;
        private TextBox txtDescTlz;
        private RadioButton rdbDescTlz;
        private Button btnSetup;
        private TabControl tabCtl;
        private TabPage tabFct;
        private TabPage tabTlz;
        private TabPage tabFields;
        private Label label5;
        private Label label3;
        private Label label4;
        private RadioButton rdbNumFld;
        private RadioButton rdbDescFld;
        private RadioButton rdbCustSqlFld;
        private TextBox txtDescFld;
        private TextBox txtNumFld;
        private Button btnBuildQryFld;
        private Button btnClearFld;
        private TextBox txtCustSqlFld;
        private Label label6;
        private RadioButton rdbTableFld;
        private ComboBox cmbTableFld;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStripNotify;
        private ToolStripMenuItem exitToolStripMenuItem;
        private CheckBox chkSearchAnyFct;
        private CheckBox chkSearchAnyTlz;
        private CheckBox chkSearchAnyFld;
        private BindingSource bindingSourceTbl;
        private RadioButton rdbShowRecords;
        private RadioButton rdbShowFields;
        private Panel panel1;
        private PictureBox picRefresh;
        private ToolStrip toolStrip;
        private ToolStripComboBox tscmbDbServer;
        private ToolStripLabel tslblRecordCnt;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripComboBox tscmbDbDatabase;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton onTop;
        private Button btnCleanSqlFct;
        private Button btnCleanSqlTlz;
        private Button btnCleanSqlFld;
        private ToolStripLabel tsInfo;
        private ToolStripSeparator toolStripSeparator3;
        private CheckBox btnShowTarget;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel tslblInfo;
        private ToolStripProgressBar tsProgressBar;
        private SplitContainer splitContainer;
        private CheckBox chkLastTransaction;
        private CheckBox chkToggleDesc;
        private Label lblFilter;
        private TextBox txtGridFilter;
        private Button btnClearFilter;
        private Button btnExport;
        private Label lblMatchCount;
        private Button btnPrevMatch;
        private Button btnNextMatch;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnPopGrid = new System.Windows.Forms.Button();
            this.dGrd = new SMS_Search.ExDataGridView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnSetup = new System.Windows.Forms.Button();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.btnBuildQryFct = new System.Windows.Forms.Button();
            this.btnBuildQryTlz = new System.Windows.Forms.Button();
            this.btnCleanSqlFct = new System.Windows.Forms.Button();
            this.btnCleanSqlTlz = new System.Windows.Forms.Button();
            this.btnCleanSqlFld = new System.Windows.Forms.Button();
            this.btnBuildQryFld = new System.Windows.Forms.Button();
            this.btnShowTarget = new System.Windows.Forms.CheckBox();
            this.dateGregorian = new System.Windows.Forms.DateTimePicker();
            this.txtJulian = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbCustSqlFct = new System.Windows.Forms.RadioButton();
            this.rdbNumFct = new System.Windows.Forms.RadioButton();
            this.txtCustSqlFct = new System.Windows.Forms.TextBox();
            this.btnClearFct = new System.Windows.Forms.Button();
            this.txtNumFct = new System.Windows.Forms.TextBox();
            this.txtDescFct = new System.Windows.Forms.TextBox();
            this.rdbDescFct = new System.Windows.Forms.RadioButton();
            this.rdbCustSqlTlz = new System.Windows.Forms.RadioButton();
            this.rdbNumTlz = new System.Windows.Forms.RadioButton();
            this.txtCustSqlTlz = new System.Windows.Forms.TextBox();
            this.btnClearTlz = new System.Windows.Forms.Button();
            this.txtNumTlz = new System.Windows.Forms.TextBox();
            this.txtDescTlz = new System.Windows.Forms.TextBox();
            this.rdbDescTlz = new System.Windows.Forms.RadioButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabCtl = new System.Windows.Forms.TabControl();
            this.tabFct = new System.Windows.Forms.TabPage();
            this.chkSearchAnyFct = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabTlz = new System.Windows.Forms.TabPage();
            this.chkSearchAnyTlz = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabFields = new System.Windows.Forms.TabPage();
            this.chkLastTransaction = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbShowRecords = new System.Windows.Forms.RadioButton();
            this.rdbShowFields = new System.Windows.Forms.RadioButton();
            this.chkSearchAnyFld = new System.Windows.Forms.CheckBox();
            this.cmbTableFld = new System.Windows.Forms.ComboBox();
            this.rdbTableFld = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbNumFld = new System.Windows.Forms.RadioButton();
            this.rdbDescFld = new System.Windows.Forms.RadioButton();
            this.rdbCustSqlFld = new System.Windows.Forms.RadioButton();
            this.txtDescFld = new System.Windows.Forms.TextBox();
            this.txtNumFld = new System.Windows.Forms.TextBox();
            this.btnClearFld = new System.Windows.Forms.Button();
            this.txtCustSqlFld = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tslblRecordCnt = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tscmbDbServer = new System.Windows.Forms.ToolStripComboBox();
            this.tscmbDbDatabase = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ReconnectDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.onTop = new System.Windows.Forms.ToolStripButton();
            this.tsInfo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tslblInfo = new System.Windows.Forms.ToolStripLabel();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceTbl = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnNextMatch = new System.Windows.Forms.Button();
            this.btnPrevMatch = new System.Windows.Forms.Button();
            this.lblMatchCount = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.txtGridFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.chkToggleDesc = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dGrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabCtl.SuspendLayout();
            this.tabFct.SuspendLayout();
            this.tabTlz.SuspendLayout();
            this.tabFields.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStripNotify.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPopGrid
            // 
            this.btnPopGrid.Location = new System.Drawing.Point(3, 4);
            this.btnPopGrid.Name = "btnPopGrid";
            this.btnPopGrid.Size = new System.Drawing.Size(78, 23);
            this.btnPopGrid.TabIndex = 0;
            this.btnPopGrid.Text = "Execute (F5)";
            this.btnPopGrid.UseVisualStyleBackColor = true;
            this.btnPopGrid.Click += new System.EventHandler(this.btnPopGrid_Click);
            // 
            // dGrd
            // 
            this.dGrd.AllowUserToAddRows = false;
            this.dGrd.AllowUserToDeleteRows = false;
            this.dGrd.AllowUserToOrderColumns = true;
            this.dGrd.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dGrd.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dGrd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGrd.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dGrd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGrd.DefaultCellStyle = dataGridViewCellStyle4;
            this.dGrd.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dGrd.Location = new System.Drawing.Point(2, 65);
            this.dGrd.Name = "dGrd";
            this.dGrd.ReadOnly = true;
            this.dGrd.RowHeadersVisible = false;
            this.dGrd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dGrd.Size = new System.Drawing.Size(594, 304);
            this.dGrd.TabIndex = 4;
            this.dGrd.TabStop = false;
            this.dGrd.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGrd_CellDoubleClick);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Image = global::SMS_Search.Properties.Resources.configure;
            this.btnSetup.Location = new System.Drawing.Point(567, 0);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(28, 24);
            this.btnSetup.TabIndex = 3;
            this.btnSetup.TabStop = false;
            this.toolTip.SetToolTip(this.btnSetup, "Settings");
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // picRefresh
            // 
            this.picRefresh.Image = global::SMS_Search.Properties.Resources.refresh;
            this.picRefresh.InitialImage = ((System.Drawing.Image)(resources.GetObject("picRefresh.InitialImage")));
            this.picRefresh.Location = new System.Drawing.Point(255, 58);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(21, 21);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRefresh.TabIndex = 52;
            this.picRefresh.TabStop = false;
            this.toolTip.SetToolTip(this.picRefresh, "Refresh table list");
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            this.picRefresh.MouseEnter += new System.EventHandler(this.picRefresh_MouseEnter);
            this.picRefresh.MouseLeave += new System.EventHandler(this.picRefresh_MouseLeave);
            // 
            // btnBuildQryFct
            // 
            this.btnBuildQryFct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuildQryFct.Location = new System.Drawing.Point(496, 29);
            this.btnBuildQryFct.Name = "btnBuildQryFct";
            this.btnBuildQryFct.Size = new System.Drawing.Size(85, 23);
            this.btnBuildQryFct.TabIndex = 6;
            this.btnBuildQryFct.Text = "&Build Query";
            this.toolTip.SetToolTip(this.btnBuildQryFct, "Build custom query from active filter");
            this.btnBuildQryFct.UseVisualStyleBackColor = true;
            this.btnBuildQryFct.Click += new System.EventHandler(this.btnBuildQryFct_Click);
            // 
            // btnBuildQryTlz
            // 
            this.btnBuildQryTlz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuildQryTlz.Location = new System.Drawing.Point(509, 29);
            this.btnBuildQryTlz.Name = "btnBuildQryTlz";
            this.btnBuildQryTlz.Size = new System.Drawing.Size(72, 23);
            this.btnBuildQryTlz.TabIndex = 6;
            this.btnBuildQryTlz.Text = "&Build Query";
            this.toolTip.SetToolTip(this.btnBuildQryTlz, "Build custom query from active filter");
            this.btnBuildQryTlz.UseVisualStyleBackColor = true;
            this.btnBuildQryTlz.Click += new System.EventHandler(this.btnBuildQryTlz_Click);
            // 
            // btnCleanSqlFct
            // 
            this.btnCleanSqlFct.Location = new System.Drawing.Point(6, 82);
            this.btnCleanSqlFct.Name = "btnCleanSqlFct";
            this.btnCleanSqlFct.Size = new System.Drawing.Size(85, 23);
            this.btnCleanSqlFct.TabIndex = 100;
            this.btnCleanSqlFct.Text = "Clean &Query";
            this.toolTip.SetToolTip(this.btnCleanSqlFct, "Click to remove HTML formatting from query");
            this.btnCleanSqlFct.UseVisualStyleBackColor = true;
            this.btnCleanSqlFct.Click += new System.EventHandler(this.btnCleanSqlFct_Click);
            // 
            // btnCleanSqlTlz
            // 
            this.btnCleanSqlTlz.Location = new System.Drawing.Point(6, 82);
            this.btnCleanSqlTlz.Name = "btnCleanSqlTlz";
            this.btnCleanSqlTlz.Size = new System.Drawing.Size(85, 23);
            this.btnCleanSqlTlz.TabIndex = 101;
            this.btnCleanSqlTlz.Text = "Clean &Query";
            this.toolTip.SetToolTip(this.btnCleanSqlTlz, "Click to remove HTML formatting from query");
            this.btnCleanSqlTlz.UseVisualStyleBackColor = true;
            this.btnCleanSqlTlz.Click += new System.EventHandler(this.btnCleanSqlTlz_Click);
            // 
            // btnCleanSqlFld
            // 
            this.btnCleanSqlFld.Location = new System.Drawing.Point(6, 108);
            this.btnCleanSqlFld.Name = "btnCleanSqlFld";
            this.btnCleanSqlFld.Size = new System.Drawing.Size(85, 23);
            this.btnCleanSqlFld.TabIndex = 102;
            this.btnCleanSqlFld.Text = "Clean &Query";
            this.toolTip.SetToolTip(this.btnCleanSqlFld, "Click to remove HTML formatting from query");
            this.btnCleanSqlFld.UseVisualStyleBackColor = true;
            this.btnCleanSqlFld.Click += new System.EventHandler(this.btnCleanSqlFld_Click);
            // 
            // btnBuildQryFld
            // 
            this.btnBuildQryFld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuildQryFld.Location = new System.Drawing.Point(509, 29);
            this.btnBuildQryFld.Name = "btnBuildQryFld";
            this.btnBuildQryFld.Size = new System.Drawing.Size(72, 23);
            this.btnBuildQryFld.TabIndex = 8;
            this.btnBuildQryFld.Text = "&Build Query";
            this.toolTip.SetToolTip(this.btnBuildQryFld, "Build custom query from active filter");
            this.btnBuildQryFld.UseVisualStyleBackColor = true;
            this.btnBuildQryFld.Click += new System.EventHandler(this.btnBuildQryFld_Click);
            // 
            // btnShowTarget
            // 
            this.btnShowTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowTarget.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnShowTarget.Image = global::SMS_Search.Properties.Resources.Unarchive1;
            this.btnShowTarget.Location = new System.Drawing.Point(535, 0);
            this.btnShowTarget.Name = "btnShowTarget";
            this.btnShowTarget.Size = new System.Drawing.Size(28, 24);
            this.btnShowTarget.TabIndex = 101;
            this.btnShowTarget.TabStop = false;
            this.toolTip.SetToolTip(this.btnShowTarget, "Toggle Unarchive target - <CRTL>+T");
            this.btnShowTarget.UseVisualStyleBackColor = true;
            this.btnShowTarget.Click += new System.EventHandler(this.btnShowTarget_Click);
            // 
            // dateGregorian
            // 
            this.dateGregorian.CustomFormat = "MM/dd/yyyy";
            this.dateGregorian.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateGregorian.Location = new System.Drawing.Point(93, 11);
            this.dateGregorian.Name = "dateGregorian";
            this.dateGregorian.Size = new System.Drawing.Size(96, 23);
            this.dateGregorian.TabIndex = 0;
            this.dateGregorian.ValueChanged += new System.EventHandler(this.dateGregorian_ValueChanged);
            // 
            // txtJulian
            // 
            this.txtJulian.Location = new System.Drawing.Point(196, 11);
            this.txtJulian.Name = "txtJulian";
            this.txtJulian.Size = new System.Drawing.Size(100, 23);
            this.txtJulian.TabIndex = 1;
            this.txtJulian.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJulian_KeyDown);
            this.txtJulian.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Num_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dateGregorian);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtJulian);
            this.groupBox1.Location = new System.Drawing.Point(297, -4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 34);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 28;
            this.label1.Text = "Date convertor:";
            // 
            // rdbCustSqlFct
            // 
            this.rdbCustSqlFct.AutoSize = true;
            this.rdbCustSqlFct.Location = new System.Drawing.Point(6, 59);
            this.rdbCustSqlFct.Name = "rdbCustSqlFct";
            this.rdbCustSqlFct.Size = new System.Drawing.Size(90, 19);
            this.rdbCustSqlFct.TabIndex = 99;
            this.rdbCustSqlFct.Text = "&Custom qry:";
            this.rdbCustSqlFct.UseVisualStyleBackColor = true;
            this.rdbCustSqlFct.CheckedChanged += new System.EventHandler(this.rdbCustSqlFct_CheckedChanged);
            // 
            // rdbNumFct
            // 
            this.rdbNumFct.AutoSize = true;
            this.rdbNumFct.Checked = true;
            this.rdbNumFct.Location = new System.Drawing.Point(6, 6);
            this.rdbNumFct.Name = "rdbNumFct";
            this.rdbNumFct.Size = new System.Drawing.Size(72, 19);
            this.rdbNumFct.TabIndex = 99;
            this.rdbNumFct.TabStop = true;
            this.rdbNumFct.Text = "&Number:";
            this.rdbNumFct.UseVisualStyleBackColor = true;
            this.rdbNumFct.CheckedChanged += new System.EventHandler(this.rdbNumFct_CheckedChanged);
            // 
            // txtCustSqlFct
            // 
            this.txtCustSqlFct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustSqlFct.Location = new System.Drawing.Point(97, 59);
            this.txtCustSqlFct.Multiline = true;
            this.txtCustSqlFct.Name = "txtCustSqlFct";
            this.txtCustSqlFct.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustSqlFct.Size = new System.Drawing.Size(483, 77);
            this.txtCustSqlFct.TabIndex = 4;
            this.txtCustSqlFct.Enter += new System.EventHandler(this.txtSqlFct_Enter);
            // 
            // btnClearFct
            // 
            this.btnClearFct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFct.Location = new System.Drawing.Point(496, 3);
            this.btnClearFct.Name = "btnClearFct";
            this.btnClearFct.Size = new System.Drawing.Size(85, 23);
            this.btnClearFct.TabIndex = 5;
            this.btnClearFct.Text = "Clear &all";
            this.btnClearFct.UseVisualStyleBackColor = true;
            this.btnClearFct.Click += new System.EventHandler(this.btnClearFct_Click);
            // 
            // txtNumFct
            // 
            this.txtNumFct.Location = new System.Drawing.Point(97, 5);
            this.txtNumFct.Name = "txtNumFct";
            this.txtNumFct.Size = new System.Drawing.Size(70, 23);
            this.txtNumFct.TabIndex = 0;
            this.txtNumFct.Enter += new System.EventHandler(this.txtNumFct_Enter);
            this.txtNumFct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Num_KeyPress);
            // 
            // txtDescFct
            // 
            this.txtDescFct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescFct.Location = new System.Drawing.Point(97, 31);
            this.txtDescFct.Name = "txtDescFct";
            this.txtDescFct.Size = new System.Drawing.Size(310, 23);
            this.txtDescFct.TabIndex = 2;
            this.txtDescFct.Enter += new System.EventHandler(this.txtDescFct_Enter);
            // 
            // rdbDescFct
            // 
            this.rdbDescFct.AutoSize = true;
            this.rdbDescFct.Location = new System.Drawing.Point(6, 32);
            this.rdbDescFct.Name = "rdbDescFct";
            this.rdbDescFct.Size = new System.Drawing.Size(88, 19);
            this.rdbDescFct.TabIndex = 99;
            this.rdbDescFct.Text = "&Description:";
            this.rdbDescFct.UseVisualStyleBackColor = true;
            this.rdbDescFct.CheckedChanged += new System.EventHandler(this.rdbDescFct_CheckedChanged);
            // 
            // rdbCustSqlTlz
            // 
            this.rdbCustSqlTlz.AutoSize = true;
            this.rdbCustSqlTlz.Location = new System.Drawing.Point(6, 59);
            this.rdbCustSqlTlz.Name = "rdbCustSqlTlz";
            this.rdbCustSqlTlz.Size = new System.Drawing.Size(90, 19);
            this.rdbCustSqlTlz.TabIndex = 99;
            this.rdbCustSqlTlz.Text = "&Custom qry:";
            this.rdbCustSqlTlz.UseVisualStyleBackColor = true;
            this.rdbCustSqlTlz.CheckedChanged += new System.EventHandler(this.rdbCustSqlTlz_CheckedChanged);
            // 
            // rdbNumTlz
            // 
            this.rdbNumTlz.AutoSize = true;
            this.rdbNumTlz.Checked = true;
            this.rdbNumTlz.Location = new System.Drawing.Point(6, 6);
            this.rdbNumTlz.Name = "rdbNumTlz";
            this.rdbNumTlz.Size = new System.Drawing.Size(72, 19);
            this.rdbNumTlz.TabIndex = 99;
            this.rdbNumTlz.TabStop = true;
            this.rdbNumTlz.Text = "&Number:";
            this.rdbNumTlz.UseVisualStyleBackColor = true;
            this.rdbNumTlz.CheckedChanged += new System.EventHandler(this.rdbNumTlz_CheckedChanged);
            // 
            // txtCustSqlTlz
            // 
            this.txtCustSqlTlz.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustSqlTlz.Location = new System.Drawing.Point(97, 59);
            this.txtCustSqlTlz.Multiline = true;
            this.txtCustSqlTlz.Name = "txtCustSqlTlz";
            this.txtCustSqlTlz.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustSqlTlz.Size = new System.Drawing.Size(483, 77);
            this.txtCustSqlTlz.TabIndex = 4;
            this.txtCustSqlTlz.Enter += new System.EventHandler(this.txtCustSqlTlz_Enter);
            // 
            // btnClearTlz
            // 
            this.btnClearTlz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTlz.Location = new System.Drawing.Point(509, 3);
            this.btnClearTlz.Name = "btnClearTlz";
            this.btnClearTlz.Size = new System.Drawing.Size(72, 23);
            this.btnClearTlz.TabIndex = 5;
            this.btnClearTlz.Text = "Clear &all";
            this.btnClearTlz.UseVisualStyleBackColor = true;
            this.btnClearTlz.Click += new System.EventHandler(this.btnClearTlz_Click);
            // 
            // txtNumTlz
            // 
            this.txtNumTlz.Location = new System.Drawing.Point(97, 5);
            this.txtNumTlz.Name = "txtNumTlz";
            this.txtNumTlz.Size = new System.Drawing.Size(70, 23);
            this.txtNumTlz.TabIndex = 0;
            this.txtNumTlz.Enter += new System.EventHandler(this.txtNumTlz_Enter);
            this.txtNumTlz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Num_KeyPress);
            // 
            // txtDescTlz
            // 
            this.txtDescTlz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescTlz.Location = new System.Drawing.Point(97, 31);
            this.txtDescTlz.Name = "txtDescTlz";
            this.txtDescTlz.Size = new System.Drawing.Size(310, 23);
            this.txtDescTlz.TabIndex = 2;
            this.txtDescTlz.Enter += new System.EventHandler(this.txtDescTlz_Enter);
            // 
            // rdbDescTlz
            // 
            this.rdbDescTlz.AutoSize = true;
            this.rdbDescTlz.Location = new System.Drawing.Point(6, 32);
            this.rdbDescTlz.Name = "rdbDescTlz";
            this.rdbDescTlz.Size = new System.Drawing.Size(88, 19);
            this.rdbDescTlz.TabIndex = 99;
            this.rdbDescTlz.Text = "&Description:";
            this.rdbDescTlz.UseVisualStyleBackColor = true;
            this.rdbDescTlz.CheckedChanged += new System.EventHandler(this.rdbDescTlz_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(583, 149);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(192, 73);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabCtl
            // 
            this.tabCtl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtl.Controls.Add(this.tabFct);
            this.tabCtl.Controls.Add(this.tabTlz);
            this.tabCtl.Controls.Add(this.tabFields);
            this.tabCtl.Location = new System.Drawing.Point(3, 4);
            this.tabCtl.Name = "tabCtl";
            this.tabCtl.SelectedIndex = 0;
            this.tabCtl.Size = new System.Drawing.Size(596, 166);
            this.tabCtl.TabIndex = 0;
            this.tabCtl.SelectedIndexChanged += new System.EventHandler(this.tabCtl_SelectedIndexChanged);
            // 
            // tabFct
            // 
            this.tabFct.BackColor = System.Drawing.SystemColors.Control;
            this.tabFct.Controls.Add(this.btnCleanSqlFct);
            this.tabFct.Controls.Add(this.chkSearchAnyFct);
            this.tabFct.Controls.Add(this.label5);
            this.tabFct.Controls.Add(this.rdbNumFct);
            this.tabFct.Controls.Add(this.rdbDescFct);
            this.tabFct.Controls.Add(this.rdbCustSqlFct);
            this.tabFct.Controls.Add(this.txtDescFct);
            this.tabFct.Controls.Add(this.txtNumFct);
            this.tabFct.Controls.Add(this.btnBuildQryFct);
            this.tabFct.Controls.Add(this.btnClearFct);
            this.tabFct.Controls.Add(this.txtCustSqlFct);
            this.tabFct.Location = new System.Drawing.Point(4, 24);
            this.tabFct.Name = "tabFct";
            this.tabFct.Padding = new System.Windows.Forms.Padding(3);
            this.tabFct.Size = new System.Drawing.Size(588, 138);
            this.tabFct.TabIndex = 0;
            this.tabFct.Text = "Function";
            this.tabFct.ToolTipText = "<CRTL> + 1";
            // 
            // chkSearchAnyFct
            // 
            this.chkSearchAnyFct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSearchAnyFct.AutoSize = true;
            this.chkSearchAnyFct.Location = new System.Drawing.Point(409, 33);
            this.chkSearchAnyFct.Name = "chkSearchAnyFct";
            this.chkSearchAnyFct.Size = new System.Drawing.Size(79, 19);
            this.chkSearchAnyFct.TabIndex = 3;
            this.chkSearchAnyFct.Text = "Anywhere";
            this.chkSearchAnyFct.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkRed;
            this.label5.Location = new System.Drawing.Point(221, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Wildcards supported ( * and ? )";
            // 
            // tabTlz
            // 
            this.tabTlz.BackColor = System.Drawing.SystemColors.Control;
            this.tabTlz.Controls.Add(this.btnCleanSqlTlz);
            this.tabTlz.Controls.Add(this.chkSearchAnyTlz);
            this.tabTlz.Controls.Add(this.label3);
            this.tabTlz.Controls.Add(this.rdbNumTlz);
            this.tabTlz.Controls.Add(this.rdbDescTlz);
            this.tabTlz.Controls.Add(this.rdbCustSqlTlz);
            this.tabTlz.Controls.Add(this.txtDescTlz);
            this.tabTlz.Controls.Add(this.txtNumTlz);
            this.tabTlz.Controls.Add(this.btnBuildQryTlz);
            this.tabTlz.Controls.Add(this.btnClearTlz);
            this.tabTlz.Controls.Add(this.txtCustSqlTlz);
            this.tabTlz.Location = new System.Drawing.Point(4, 24);
            this.tabTlz.Name = "tabTlz";
            this.tabTlz.Padding = new System.Windows.Forms.Padding(3);
            this.tabTlz.Size = new System.Drawing.Size(588, 138);
            this.tabTlz.TabIndex = 1;
            this.tabTlz.Text = "Totalizer";
            // 
            // chkSearchAnyTlz
            // 
            this.chkSearchAnyTlz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSearchAnyTlz.AutoSize = true;
            this.chkSearchAnyTlz.Location = new System.Drawing.Point(409, 33);
            this.chkSearchAnyTlz.Name = "chkSearchAnyTlz";
            this.chkSearchAnyTlz.Size = new System.Drawing.Size(79, 19);
            this.chkSearchAnyTlz.TabIndex = 3;
            this.chkSearchAnyTlz.Text = "Anywhere";
            this.chkSearchAnyTlz.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkRed;
            this.label3.Location = new System.Drawing.Point(221, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Wildcards supported ( * and ? )";
            // 
            // tabFields
            // 
            this.tabFields.BackColor = System.Drawing.SystemColors.Control;
            this.tabFields.Controls.Add(this.chkLastTransaction);
            this.tabFields.Controls.Add(this.btnCleanSqlFld);
            this.tabFields.Controls.Add(this.panel1);
            this.tabFields.Controls.Add(this.chkSearchAnyFld);
            this.tabFields.Controls.Add(this.cmbTableFld);
            this.tabFields.Controls.Add(this.rdbTableFld);
            this.tabFields.Controls.Add(this.label4);
            this.tabFields.Controls.Add(this.rdbNumFld);
            this.tabFields.Controls.Add(this.rdbDescFld);
            this.tabFields.Controls.Add(this.rdbCustSqlFld);
            this.tabFields.Controls.Add(this.txtDescFld);
            this.tabFields.Controls.Add(this.txtNumFld);
            this.tabFields.Controls.Add(this.btnBuildQryFld);
            this.tabFields.Controls.Add(this.btnClearFld);
            this.tabFields.Controls.Add(this.txtCustSqlFld);
            this.tabFields.Controls.Add(this.label6);
            this.tabFields.Controls.Add(this.picRefresh);
            this.tabFields.Location = new System.Drawing.Point(4, 24);
            this.tabFields.Name = "tabFields";
            this.tabFields.Padding = new System.Windows.Forms.Padding(3);
            this.tabFields.Size = new System.Drawing.Size(588, 138);
            this.tabFields.TabIndex = 2;
            this.tabFields.Text = "Fields";
            // 
            // chkLastTransaction
            // 
            this.chkLastTransaction.AutoSize = true;
            this.chkLastTransaction.Location = new System.Drawing.Point(473, 60);
            this.chkLastTransaction.Name = "chkLastTransaction";
            this.chkLastTransaction.Size = new System.Drawing.Size(111, 19);
            this.chkLastTransaction.TabIndex = 103;
            this.chkLastTransaction.Text = "&Last Transaction";
            this.chkLastTransaction.UseVisualStyleBackColor = true;
            this.chkLastTransaction.CheckedChanged += new System.EventHandler(this.chkLastTransaction_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbShowRecords);
            this.panel1.Controls.Add(this.rdbShowFields);
            this.panel1.Location = new System.Drawing.Point(282, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 23);
            this.panel1.TabIndex = 5;
            // 
            // rdbShowRecords
            // 
            this.rdbShowRecords.AutoSize = true;
            this.rdbShowRecords.Location = new System.Drawing.Point(86, 4);
            this.rdbShowRecords.Name = "rdbShowRecords";
            this.rdbShowRecords.Size = new System.Drawing.Size(99, 19);
            this.rdbShowRecords.TabIndex = 1;
            this.rdbShowRecords.Text = "Show &Records";
            this.rdbShowRecords.UseVisualStyleBackColor = true;
            this.rdbShowRecords.CheckedChanged += new System.EventHandler(this.rdbSelectByTable_CheckedChanged);
            // 
            // rdbShowFields
            // 
            this.rdbShowFields.AutoSize = true;
            this.rdbShowFields.Checked = true;
            this.rdbShowFields.Location = new System.Drawing.Point(1, 4);
            this.rdbShowFields.Name = "rdbShowFields";
            this.rdbShowFields.Size = new System.Drawing.Size(87, 19);
            this.rdbShowFields.TabIndex = 0;
            this.rdbShowFields.TabStop = true;
            this.rdbShowFields.Text = "Show &Fields";
            this.rdbShowFields.UseVisualStyleBackColor = true;
            this.rdbShowFields.CheckedChanged += new System.EventHandler(this.rdbSelectByFields_CheckedChanged);
            // 
            // chkSearchAnyFld
            // 
            this.chkSearchAnyFld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSearchAnyFld.AutoSize = true;
            this.chkSearchAnyFld.Location = new System.Drawing.Point(409, 33);
            this.chkSearchAnyFld.Name = "chkSearchAnyFld";
            this.chkSearchAnyFld.Size = new System.Drawing.Size(79, 19);
            this.chkSearchAnyFld.TabIndex = 3;
            this.chkSearchAnyFld.Text = "Anywhere";
            this.chkSearchAnyFld.UseVisualStyleBackColor = true;
            // 
            // cmbTableFld
            // 
            this.cmbTableFld.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbTableFld.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTableFld.FormattingEnabled = true;
            this.cmbTableFld.Location = new System.Drawing.Point(97, 58);
            this.cmbTableFld.Name = "cmbTableFld";
            this.cmbTableFld.Size = new System.Drawing.Size(152, 23);
            this.cmbTableFld.TabIndex = 4;
            this.cmbTableFld.TextChanged += new System.EventHandler(this.cmbTableFld_TextChanged);
            this.cmbTableFld.Enter += new System.EventHandler(this.cmbTableFld_Enter);
            // 
            // rdbTableFld
            // 
            this.rdbTableFld.AutoSize = true;
            this.rdbTableFld.Location = new System.Drawing.Point(6, 59);
            this.rdbTableFld.Name = "rdbTableFld";
            this.rdbTableFld.Size = new System.Drawing.Size(56, 19);
            this.rdbTableFld.TabIndex = 99;
            this.rdbTableFld.Text = "&Table:";
            this.rdbTableFld.UseVisualStyleBackColor = true;
            this.rdbTableFld.CheckedChanged += new System.EventHandler(this.rdbTableFld_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(221, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Wildcards supported ( * and ? )";
            // 
            // rdbNumFld
            // 
            this.rdbNumFld.AutoSize = true;
            this.rdbNumFld.Checked = true;
            this.rdbNumFld.Location = new System.Drawing.Point(6, 6);
            this.rdbNumFld.Name = "rdbNumFld";
            this.rdbNumFld.Size = new System.Drawing.Size(72, 19);
            this.rdbNumFld.TabIndex = 99;
            this.rdbNumFld.TabStop = true;
            this.rdbNumFld.Text = "&Number:";
            this.rdbNumFld.UseVisualStyleBackColor = true;
            this.rdbNumFld.CheckedChanged += new System.EventHandler(this.rdbNumFld_CheckedChanged);
            // 
            // rdbDescFld
            // 
            this.rdbDescFld.AutoSize = true;
            this.rdbDescFld.Location = new System.Drawing.Point(6, 32);
            this.rdbDescFld.Name = "rdbDescFld";
            this.rdbDescFld.Size = new System.Drawing.Size(88, 19);
            this.rdbDescFld.TabIndex = 99;
            this.rdbDescFld.Text = "&Description:";
            this.rdbDescFld.UseVisualStyleBackColor = true;
            this.rdbDescFld.CheckedChanged += new System.EventHandler(this.rdbDescFld_CheckedChanged);
            // 
            // rdbCustSqlFld
            // 
            this.rdbCustSqlFld.AutoSize = true;
            this.rdbCustSqlFld.Location = new System.Drawing.Point(6, 85);
            this.rdbCustSqlFld.Name = "rdbCustSqlFld";
            this.rdbCustSqlFld.Size = new System.Drawing.Size(90, 19);
            this.rdbCustSqlFld.TabIndex = 99;
            this.rdbCustSqlFld.Text = "&Custom qry:";
            this.rdbCustSqlFld.UseVisualStyleBackColor = true;
            this.rdbCustSqlFld.CheckedChanged += new System.EventHandler(this.rdbCustSqlFld_CheckedChanged);
            // 
            // txtDescFld
            // 
            this.txtDescFld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescFld.Location = new System.Drawing.Point(97, 31);
            this.txtDescFld.Name = "txtDescFld";
            this.txtDescFld.Size = new System.Drawing.Size(310, 23);
            this.txtDescFld.TabIndex = 2;
            this.txtDescFld.Enter += new System.EventHandler(this.txtDescFld_Enter);
            // 
            // txtNumFld
            // 
            this.txtNumFld.Location = new System.Drawing.Point(110, 5);
            this.txtNumFld.Name = "txtNumFld";
            this.txtNumFld.Size = new System.Drawing.Size(61, 23);
            this.txtNumFld.TabIndex = 0;
            this.txtNumFld.Enter += new System.EventHandler(this.txtNumFld_Enter);
            this.txtNumFld.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Num_KeyPress);
            // 
            // btnClearFld
            // 
            this.btnClearFld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFld.Location = new System.Drawing.Point(509, 3);
            this.btnClearFld.Name = "btnClearFld";
            this.btnClearFld.Size = new System.Drawing.Size(72, 23);
            this.btnClearFld.TabIndex = 7;
            this.btnClearFld.Text = "Clear &all";
            this.btnClearFld.UseVisualStyleBackColor = true;
            this.btnClearFld.Click += new System.EventHandler(this.btnClearFld_Click);
            // 
            // txtCustSqlFld
            // 
            this.txtCustSqlFld.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustSqlFld.Location = new System.Drawing.Point(97, 84);
            this.txtCustSqlFld.Multiline = true;
            this.txtCustSqlFld.Name = "txtCustSqlFld";
            this.txtCustSqlFld.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustSqlFld.Size = new System.Drawing.Size(484, 52);
            this.txtCustSqlFld.TabIndex = 6;
            this.txtCustSqlFld.Enter += new System.EventHandler(this.txtCustSqlFld_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(96, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 21);
            this.label6.TabIndex = 45;
            this.label6.Text = "F";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripNotify;
            this.notifyIcon.Text = "SMS Search";
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // contextMenuStripNotify
            // 
            this.contextMenuStripNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStripNotify.Name = "contextMenuStripNotify";
            this.contextMenuStripNotify.Size = new System.Drawing.Size(93, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblRecordCnt,
            this.toolStripSeparator1,
            this.tscmbDbServer,
            this.tscmbDbDatabase,
            this.toolStripSeparator2,
            this.ReconnectDB,
            this.toolStripSeparator5,
            this.onTop,
            this.tsInfo,
            this.toolStripSeparator3,
            this.toolStripSeparator4,
            this.tsProgressBar,
            this.tslblInfo});
            this.toolStrip.Location = new System.Drawing.Point(0, 555);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(603, 25);
            this.toolStrip.TabIndex = 33;
            this.toolStrip.Text = "toolStrip";
            // 
            // tslblRecordCnt
            // 
            this.tslblRecordCnt.Name = "tslblRecordCnt";
            this.tslblRecordCnt.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.tslblRecordCnt.Size = new System.Drawing.Size(23, 22);
            this.tslblRecordCnt.Text = "0";
            this.tslblRecordCnt.ToolTipText = "Record count";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tscmbDbServer
            // 
            this.tscmbDbServer.AutoSize = false;
            this.tscmbDbServer.Enabled = false;
            this.tscmbDbServer.Name = "tscmbDbServer";
            this.tscmbDbServer.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.tscmbDbServer.Size = new System.Drawing.Size(0, 23);
            this.tscmbDbServer.ToolTipText = "Server";
            // 
            // tscmbDbDatabase
            // 
            this.tscmbDbDatabase.DropDownWidth = 180;
            this.tscmbDbDatabase.MaxDropDownItems = 30;
            this.tscmbDbDatabase.Name = "tscmbDbDatabase";
            this.tscmbDbDatabase.Size = new System.Drawing.Size(180, 25);
            this.tscmbDbDatabase.Sorted = true;
            this.tscmbDbDatabase.ToolTipText = "Database selector";
            this.tscmbDbDatabase.DropDown += new System.EventHandler(this.tscmbDbDatabase_DropDown);
            this.tscmbDbDatabase.SelectedIndexChanged += new System.EventHandler(this.tscmbDbDatabase_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ReconnectDB
            // 
            this.ReconnectDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReconnectDB.Image = ((System.Drawing.Image)(resources.GetObject("ReconnectDB.Image")));
            this.ReconnectDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReconnectDB.Name = "ReconnectDB";
            this.ReconnectDB.Size = new System.Drawing.Size(23, 22);
            this.ReconnectDB.Text = "Reconnect";
            this.ReconnectDB.ToolTipText = "Attempt to reconnect to selected Database";
            this.ReconnectDB.Click += new System.EventHandler(this.ReconnectDB_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // onTop
            // 
            this.onTop.AutoToolTip = false;
            this.onTop.CheckOnClick = true;
            this.onTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.onTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.onTop.Name = "onTop";
            this.onTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.onTop.Size = new System.Drawing.Size(96, 22);
            this.onTop.Text = "Always on top";
            this.onTop.ToolTipText = "Click to stay on top";
            this.onTop.Click += new System.EventHandler(this.onTop_Click);
            this.onTop.MouseEnter += new System.EventHandler(this.onTop_MouseEnter);
            // 
            // tsInfo
            // 
            this.tsInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsInfo.Name = "tsInfo";
            this.tsInfo.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(100, 22);
            this.tsProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.tsProgressBar.Visible = false;
            // 
            // tslblInfo
            // 
            this.tslblInfo.Name = "tslblInfo";
            this.tslblInfo.Size = new System.Drawing.Size(50, 22);
            this.tslblInfo.Text = "tslblInfo";
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnShowTarget);
            this.splitContainer.Panel1.Controls.Add(this.btnSetup);
            this.splitContainer.Panel1.Controls.Add(this.tabCtl);
            this.splitContainer.Panel1MinSize = 177;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnNextMatch);
            this.splitContainer.Panel2.Controls.Add(this.btnPrevMatch);
            this.splitContainer.Panel2.Controls.Add(this.lblMatchCount);
            this.splitContainer.Panel2.Controls.Add(this.btnExport);
            this.splitContainer.Panel2.Controls.Add(this.btnClearFilter);
            this.splitContainer.Panel2.Controls.Add(this.txtGridFilter);
            this.splitContainer.Panel2.Controls.Add(this.lblFilter);
            this.splitContainer.Panel2.Controls.Add(this.chkToggleDesc);
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Panel2.Controls.Add(this.dGrd);
            this.splitContainer.Panel2.Controls.Add(this.btnPopGrid);
            this.splitContainer.Panel2MinSize = 29;
            this.splitContainer.Size = new System.Drawing.Size(603, 554);
            this.splitContainer.SplitterDistance = 177;
            this.splitContainer.TabIndex = 102;
            this.splitContainer.TabStop = false;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // btnNextMatch
            // 
            this.btnNextMatch.Location = new System.Drawing.Point(500, 34);
            this.btnNextMatch.Name = "btnNextMatch";
            this.btnNextMatch.Size = new System.Drawing.Size(25, 25);
            this.btnNextMatch.TabIndex = 16;
            this.btnNextMatch.Text = ">";
            this.btnNextMatch.UseVisualStyleBackColor = true;
            this.btnNextMatch.Visible = false;
            this.btnNextMatch.Click += new System.EventHandler(this.btnNextMatch_Click);
            // 
            // btnPrevMatch
            // 
            this.btnPrevMatch.Location = new System.Drawing.Point(470, 34);
            this.btnPrevMatch.Name = "btnPrevMatch";
            this.btnPrevMatch.Size = new System.Drawing.Size(25, 25);
            this.btnPrevMatch.TabIndex = 15;
            this.btnPrevMatch.Text = "<";
            this.btnPrevMatch.UseVisualStyleBackColor = true;
            this.btnPrevMatch.Visible = false;
            this.btnPrevMatch.Click += new System.EventHandler(this.btnPrevMatch_Click);
            // 
            // lblMatchCount
            // 
            this.lblMatchCount.AutoSize = true;
            this.lblMatchCount.Location = new System.Drawing.Point(365, 39);
            this.lblMatchCount.Name = "lblMatchCount";
            this.lblMatchCount.Size = new System.Drawing.Size(0, 15);
            this.lblMatchCount.TabIndex = 14;
            this.lblMatchCount.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(280, 34);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 25);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(251, 34);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(23, 23);
            this.btnClearFilter.TabIndex = 13;
            this.btnClearFilter.Text = "x";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // txtGridFilter
            // 
            this.txtGridFilter.Location = new System.Drawing.Point(45, 35);
            this.txtGridFilter.Name = "txtGridFilter";
            this.txtGridFilter.Size = new System.Drawing.Size(200, 23);
            this.txtGridFilter.TabIndex = 11;
            this.txtGridFilter.TextChanged += new System.EventHandler(this.txtGridFilter_TextChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(5, 38);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(36, 15);
            this.lblFilter.TabIndex = 10;
            this.lblFilter.Text = "Filter:";
            // 
            // chkToggleDesc
            // 
            this.chkToggleDesc.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkToggleDesc.Location = new System.Drawing.Point(87, 4);
            this.chkToggleDesc.Name = "chkToggleDesc";
            this.chkToggleDesc.Size = new System.Drawing.Size(77, 23);
            this.chkToggleDesc.TabIndex = 2;
            this.chkToggleDesc.Text = "Show Desc.";
            this.chkToggleDesc.UseVisualStyleBackColor = true;
            this.chkToggleDesc.CheckedChanged += new System.EventHandler(this.chkToggleDesc_CheckedChanged);
            // 
            // frmMain
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(603, 580);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(600, 270);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SMS Search";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMain_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dGrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabCtl.ResumeLayout(false);
            this.tabFct.ResumeLayout(false);
            this.tabFct.PerformLayout();
            this.tabTlz.ResumeLayout(false);
            this.tabTlz.PerformLayout();
            this.tabFields.ResumeLayout(false);
            this.tabFields.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStripNotify.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTbl)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton ReconnectDB;
    }
}




