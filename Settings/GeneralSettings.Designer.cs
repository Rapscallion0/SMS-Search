namespace SMS_Search.Settings
{
    partial class GeneralSettings
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.lblAutoResizeLimit = new System.Windows.Forms.Label();
            this.txtAutoResizeLimit = new System.Windows.Forms.TextBox();
            this.chkHighlightMatches = new System.Windows.Forms.CheckBox();
            this.picHighlightWarning = new System.Windows.Forms.PictureBox();
            this.lblHighlightColor = new System.Windows.Forms.Label();
            this.btnHighlightColor = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Set SMS Search defaults";
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(18, 318);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.chkAlwaysOnTop.TabIndex = 1;
            this.chkAlwaysOnTop.Text = "Always on top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // chkDescriptionColumns
            // 
            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(220, 45);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(143, 17);
            this.chkDescriptionColumns.TabIndex = 2;
            this.chkDescriptionColumns.Text = "Show header description";
            this.chkDescriptionColumns.UseVisualStyleBackColor = true;
            // 
            // chkResizeColumns
            // 
            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkResizeColumns.Location = new System.Drawing.Point(220, 75);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(201, 17);
            this.chkResizeColumns.TabIndex = 3;
            this.chkResizeColumns.Text = "Resize columns on description toggle";
            this.chkResizeColumns.UseVisualStyleBackColor = true;
            // 
            // chkShowInTray
            // 
            this.chkShowInTray.AutoSize = true;
            this.chkShowInTray.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowInTray.Location = new System.Drawing.Point(131, 318);
            this.chkShowInTray.Name = "chkShowInTray";
            this.chkShowInTray.Size = new System.Drawing.Size(119, 17);
            this.chkShowInTray.TabIndex = 4;
            this.chkShowInTray.Text = "Show in system tray";
            this.chkShowInTray.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkShowInTray, "Will not show in task bar");
            this.chkShowInTray.UseVisualStyleBackColor = true;
            // 
            // chkCopyCleanSql
            // 
            this.chkCopyCleanSql.AutoSize = true;
            this.chkCopyCleanSql.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCopyCleanSql.Location = new System.Drawing.Point(220, 105);
            this.chkCopyCleanSql.Name = "chkCopyCleanSql";
            this.chkCopyCleanSql.Size = new System.Drawing.Size(202, 17);
            this.chkCopyCleanSql.TabIndex = 5;
            this.chkCopyCleanSql.Text = "Copy cleaned SQL query to clipboard";
            this.chkCopyCleanSql.UseVisualStyleBackColor = true;
            // 
            // chkSearchAny
            // 
            this.chkSearchAny.AutoSize = true;
            this.chkSearchAny.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSearchAny.Location = new System.Drawing.Point(18, 135);
            this.chkSearchAny.Name = "chkSearchAny";
            this.chkSearchAny.Size = new System.Drawing.Size(174, 17);
            this.chkSearchAny.TabIndex = 6;
            this.chkSearchAny.Text = "Search anywhere in description";
            this.chkSearchAny.UseVisualStyleBackColor = true;
            // 
            // chkMultiInstance
            // 
            this.chkMultiInstance.AutoSize = true;
            this.chkMultiInstance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMultiInstance.Location = new System.Drawing.Point(256, 318);
            this.chkMultiInstance.Name = "chkMultiInstance";
            this.chkMultiInstance.Size = new System.Drawing.Size(137, 17);
            this.chkMultiInstance.TabIndex = 7;
            this.chkMultiInstance.Text = "Allow multiple instances";
            this.chkMultiInstance.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(242, 287);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Startup tab";
            // 
            // cmbStartTab
            // 
            this.cmbStartTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTab.FormattingEnabled = true;
            this.cmbStartTab.Items.AddRange(new object[] {
            "Function",
            "Totalizer",
            "Fields"});
            this.cmbStartTab.Location = new System.Drawing.Point(317, 284);
            this.cmbStartTab.Name = "cmbStartTab";
            this.cmbStartTab.Size = new System.Drawing.Size(85, 21);
            this.cmbStartTab.TabIndex = 9;
            // 
            // chkUnarchiveTarget
            // 
            this.chkUnarchiveTarget.AutoSize = true;
            this.chkUnarchiveTarget.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUnarchiveTarget.Location = new System.Drawing.Point(19, 352);
            this.chkUnarchiveTarget.Name = "chkUnarchiveTarget";
            this.chkUnarchiveTarget.Size = new System.Drawing.Size(183, 17);
            this.chkUnarchiveTarget.TabIndex = 10;
            this.chkUnarchiveTarget.Text = "Show unarchive target on startup";
            this.chkUnarchiveTarget.UseVisualStyleBackColor = true;
            // 
            // lblTableLookup
            // 
            this.lblTableLookup.AutoSize = true;
            this.lblTableLookup.Location = new System.Drawing.Point(15, 200);
            this.lblTableLookup.Name = "lblTableLookup";
            this.lblTableLookup.Size = new System.Drawing.Size(69, 13);
            this.lblTableLookup.TabIndex = 11;
            this.lblTableLookup.Text = "Table lookup";
            // 
            // cmbTableLookup
            // 
            this.cmbTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableLookup.FormattingEnabled = true;
            this.cmbTableLookup.Items.AddRange(new object[] {
            "Show Fields",
            "Show Records"});
            this.cmbTableLookup.Location = new System.Drawing.Point(90, 197);
            this.cmbTableLookup.Name = "cmbTableLookup";
            this.cmbTableLookup.Size = new System.Drawing.Size(102, 21);
            this.cmbTableLookup.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 353);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Startup location";
            // 
            // cmbStartupLocation
            // 
            this.cmbStartupLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartupLocation.FormattingEnabled = true;
            this.cmbStartupLocation.Items.AddRange(new object[] {
            "Last location",
            "Primary display",
            "Active display",
            "Cursor location"});
            this.cmbStartupLocation.Location = new System.Drawing.Point(307, 350);
            this.cmbStartupLocation.Name = "cmbStartupLocation";
            this.cmbStartupLocation.Size = new System.Drawing.Size(100, 21);
            this.cmbStartupLocation.TabIndex = 14;
            // 
            // lblAutoResizeLimit
            // 
            this.lblAutoResizeLimit.AutoSize = true;
            this.lblAutoResizeLimit.Location = new System.Drawing.Point(220, 200);
            this.lblAutoResizeLimit.Name = "lblAutoResizeLimit";
            this.lblAutoResizeLimit.Size = new System.Drawing.Size(161, 13);
            this.lblAutoResizeLimit.TabIndex = 15;
            this.lblAutoResizeLimit.Text = "Max rows for column auto-resize:";
            // 
            // txtAutoResizeLimit
            // 
            this.txtAutoResizeLimit.Location = new System.Drawing.Point(355, 197);
            this.txtAutoResizeLimit.Name = "txtAutoResizeLimit";
            this.txtAutoResizeLimit.Size = new System.Drawing.Size(60, 20);
            this.txtAutoResizeLimit.TabIndex = 16;
            // 
            // chkHighlightMatches
            // 
            this.chkHighlightMatches.AutoSize = true;
            this.chkHighlightMatches.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHighlightMatches.Location = new System.Drawing.Point(18, 230);
            this.chkHighlightMatches.Name = "chkHighlightMatches";
            this.chkHighlightMatches.Size = new System.Drawing.Size(105, 17);
            this.chkHighlightMatches.TabIndex = 17;
            this.chkHighlightMatches.Text = "Show filter count";
            this.chkHighlightMatches.UseVisualStyleBackColor = true;
            // 
            // picHighlightWarning
            // 
            this.picHighlightWarning.Location = new System.Drawing.Point(131, 230);
            this.picHighlightWarning.Name = "picHighlightWarning";
            this.picHighlightWarning.Size = new System.Drawing.Size(16, 16);
            this.picHighlightWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHighlightWarning.TabIndex = 18;
            this.picHighlightWarning.TabStop = false;
            // 
            // lblHighlightColor
            // 
            this.lblHighlightColor.AutoSize = true;
            this.lblHighlightColor.Location = new System.Drawing.Point(15, 260);
            this.lblHighlightColor.Name = "lblHighlightColor";
            this.lblHighlightColor.Size = new System.Drawing.Size(108, 13);
            this.lblHighlightColor.TabIndex = 19;
            this.lblHighlightColor.Text = "Match highlight color:";
            // 
            // btnHighlightColor
            // 
            this.btnHighlightColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighlightColor.Location = new System.Drawing.Point(131, 255);
            this.btnHighlightColor.Name = "btnHighlightColor";
            this.btnHighlightColor.Size = new System.Drawing.Size(40, 23);
            this.btnHighlightColor.TabIndex = 20;
            this.btnHighlightColor.UseVisualStyleBackColor = true;
            // 
            // GeneralSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHighlightColor);
            this.Controls.Add(this.lblHighlightColor);
            this.Controls.Add(this.picHighlightWarning);
            this.Controls.Add(this.chkHighlightMatches);
            this.Controls.Add(this.txtAutoResizeLimit);
            this.Controls.Add(this.lblAutoResizeLimit);
            this.Controls.Add(this.cmbStartupLocation);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbTableLookup);
            this.Controls.Add(this.lblTableLookup);
            this.Controls.Add(this.chkUnarchiveTarget);
            this.Controls.Add(this.cmbStartTab);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkMultiInstance);
            this.Controls.Add(this.chkSearchAny);
            this.Controls.Add(this.chkCopyCleanSql);
            this.Controls.Add(this.chkShowInTray);
            this.Controls.Add(this.chkResizeColumns);
            this.Controls.Add(this.chkDescriptionColumns);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.label6);
            this.Name = "GeneralSettings";
            this.Size = new System.Drawing.Size(430, 400);
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.CheckBox chkDescriptionColumns;
        private System.Windows.Forms.CheckBox chkResizeColumns;
        private System.Windows.Forms.CheckBox chkShowInTray;
        private System.Windows.Forms.CheckBox chkCopyCleanSql;
        private System.Windows.Forms.CheckBox chkSearchAny;
        private System.Windows.Forms.CheckBox chkMultiInstance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStartTab;
        private System.Windows.Forms.CheckBox chkUnarchiveTarget;
        private System.Windows.Forms.Label lblTableLookup;
        private System.Windows.Forms.ComboBox cmbTableLookup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbStartupLocation;
        private System.Windows.Forms.Label lblAutoResizeLimit;
        private System.Windows.Forms.TextBox txtAutoResizeLimit;
        private System.Windows.Forms.CheckBox chkHighlightMatches;
        private System.Windows.Forms.PictureBox picHighlightWarning;
        private System.Windows.Forms.Label lblHighlightColor;
        private System.Windows.Forms.Button btnHighlightColor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
