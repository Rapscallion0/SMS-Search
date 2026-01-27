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
            this.grpGridSettings = new System.Windows.Forms.GroupBox();
            this.chkShowRowNumbers = new System.Windows.Forms.CheckBox();
            this.chkDescriptionColumns = new System.Windows.Forms.CheckBox();
            this.chkResizeColumns = new System.Windows.Forms.CheckBox();
            this.lblAutoResizeLimit = new System.Windows.Forms.Label();
            this.txtAutoResizeLimit = new System.Windows.Forms.TextBox();
            this.grpSearchSettings = new System.Windows.Forms.GroupBox();
            this.chkSearchAny = new System.Windows.Forms.CheckBox();
            this.lblTableLookup = new System.Windows.Forms.Label();
            this.cmbTableLookup = new System.Windows.Forms.ComboBox();
            this.chkHighlightMatches = new System.Windows.Forms.CheckBox();
            this.picHighlightWarning = new System.Windows.Forms.PictureBox();
            this.lblHighlightColor = new System.Windows.Forms.Label();
            this.btnHighlightColor = new System.Windows.Forms.Button();
            this.grpOtherSettings = new System.Windows.Forms.GroupBox();
            this.chkCopyCleanSql = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).BeginInit();
            this.grpGridSettings.SuspendLayout();
            this.grpSearchSettings.SuspendLayout();
            this.grpOtherSettings.SuspendLayout();
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
            // grpGridSettings
            //
            this.grpGridSettings.Controls.Add(this.chkShowRowNumbers);
            this.grpGridSettings.Controls.Add(this.chkDescriptionColumns);
            this.grpGridSettings.Controls.Add(this.chkResizeColumns);
            this.grpGridSettings.Controls.Add(this.lblAutoResizeLimit);
            this.grpGridSettings.Controls.Add(this.txtAutoResizeLimit);
            this.grpGridSettings.Location = new System.Drawing.Point(15, 45);
            this.grpGridSettings.Name = "grpGridSettings";
            this.grpGridSettings.Size = new System.Drawing.Size(400, 150);
            this.grpGridSettings.TabIndex = 1;
            this.grpGridSettings.TabStop = false;
            this.grpGridSettings.Text = "Grid Display";
            //
            // chkDescriptionColumns
            // 
            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(15, 25);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(143, 17);
            this.chkDescriptionColumns.TabIndex = 0;
            this.chkDescriptionColumns.Text = "Show header description";
            this.chkDescriptionColumns.UseVisualStyleBackColor = true;
            // 
            // chkResizeColumns
            // 
            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.Location = new System.Drawing.Point(15, 50);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(201, 17);
            this.chkResizeColumns.TabIndex = 1;
            this.chkResizeColumns.Text = "Resize columns on description toggle";
            this.chkResizeColumns.UseVisualStyleBackColor = true;
            // 
            // chkShowRowNumbers
            // 
            this.chkShowRowNumbers.AutoSize = true;
            this.chkShowRowNumbers.Location = new System.Drawing.Point(15, 75);
            this.chkShowRowNumbers.Name = "chkShowRowNumbers";
            this.chkShowRowNumbers.Size = new System.Drawing.Size(125, 17);
            this.chkShowRowNumbers.TabIndex = 2;
            this.chkShowRowNumbers.Text = "Show row numbers";
            this.chkShowRowNumbers.UseVisualStyleBackColor = true;
            //
            // lblAutoResizeLimit
            //
            this.lblAutoResizeLimit.AutoSize = true;
            this.lblAutoResizeLimit.Location = new System.Drawing.Point(15, 110);
            this.lblAutoResizeLimit.Name = "lblAutoResizeLimit";
            this.lblAutoResizeLimit.Size = new System.Drawing.Size(161, 13);
            this.lblAutoResizeLimit.TabIndex = 3;
            this.lblAutoResizeLimit.Text = "Max rows for column auto-resize:";
            //
            // txtAutoResizeLimit
            //
            this.txtAutoResizeLimit.Location = new System.Drawing.Point(180, 107);
            this.txtAutoResizeLimit.Name = "txtAutoResizeLimit";
            this.txtAutoResizeLimit.Size = new System.Drawing.Size(60, 20);
            this.txtAutoResizeLimit.TabIndex = 4;
            //
            // grpSearchSettings
            //
            this.grpSearchSettings.Controls.Add(this.chkSearchAny);
            this.grpSearchSettings.Controls.Add(this.lblTableLookup);
            this.grpSearchSettings.Controls.Add(this.cmbTableLookup);
            this.grpSearchSettings.Controls.Add(this.chkHighlightMatches);
            this.grpSearchSettings.Controls.Add(this.picHighlightWarning);
            this.grpSearchSettings.Controls.Add(this.lblHighlightColor);
            this.grpSearchSettings.Controls.Add(this.btnHighlightColor);
            this.grpSearchSettings.Location = new System.Drawing.Point(15, 210);
            this.grpSearchSettings.Name = "grpSearchSettings";
            this.grpSearchSettings.Size = new System.Drawing.Size(400, 160);
            this.grpSearchSettings.TabIndex = 2;
            this.grpSearchSettings.TabStop = false;
            this.grpSearchSettings.Text = "Search Behavior";
            // 
            // chkSearchAny
            // 
            this.chkSearchAny.AutoSize = true;
            this.chkSearchAny.Location = new System.Drawing.Point(15, 25);
            this.chkSearchAny.Name = "chkSearchAny";
            this.chkSearchAny.Size = new System.Drawing.Size(174, 17);
            this.chkSearchAny.TabIndex = 0;
            this.chkSearchAny.Text = "Search anywhere in description";
            this.chkSearchAny.UseVisualStyleBackColor = true;
            // 
            // lblTableLookup
            // 
            this.lblTableLookup.AutoSize = true;
            this.lblTableLookup.Location = new System.Drawing.Point(15, 60);
            this.lblTableLookup.Name = "lblTableLookup";
            this.lblTableLookup.Size = new System.Drawing.Size(69, 13);
            this.lblTableLookup.TabIndex = 1;
            this.lblTableLookup.Text = "Table lookup:";
            // 
            // cmbTableLookup
            // 
            this.cmbTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableLookup.FormattingEnabled = true;
            this.cmbTableLookup.Items.AddRange(new object[] {
            "Show Fields",
            "Show Records"});
            this.cmbTableLookup.Location = new System.Drawing.Point(100, 57);
            this.cmbTableLookup.Name = "cmbTableLookup";
            this.cmbTableLookup.Size = new System.Drawing.Size(120, 21);
            this.cmbTableLookup.TabIndex = 2;
            // 
            // chkHighlightMatches
            // 
            this.chkHighlightMatches.AutoSize = true;
            this.chkHighlightMatches.Location = new System.Drawing.Point(15, 95);
            this.chkHighlightMatches.Name = "chkHighlightMatches";
            this.chkHighlightMatches.Size = new System.Drawing.Size(105, 17);
            this.chkHighlightMatches.TabIndex = 3;
            this.chkHighlightMatches.Text = "Show filter count";
            this.chkHighlightMatches.UseVisualStyleBackColor = true;
            // 
            // picHighlightWarning
            // 
            this.picHighlightWarning.Location = new System.Drawing.Point(130, 95);
            this.picHighlightWarning.Name = "picHighlightWarning";
            this.picHighlightWarning.Size = new System.Drawing.Size(16, 16);
            this.picHighlightWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHighlightWarning.TabIndex = 4;
            this.picHighlightWarning.TabStop = false;
            // 
            // lblHighlightColor
            // 
            this.lblHighlightColor.AutoSize = true;
            this.lblHighlightColor.Location = new System.Drawing.Point(15, 125);
            this.lblHighlightColor.Name = "lblHighlightColor";
            this.lblHighlightColor.Size = new System.Drawing.Size(108, 13);
            this.lblHighlightColor.TabIndex = 5;
            this.lblHighlightColor.Text = "Match highlight color:";
            // 
            // btnHighlightColor
            // 
            this.btnHighlightColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighlightColor.Location = new System.Drawing.Point(130, 120);
            this.btnHighlightColor.Name = "btnHighlightColor";
            this.btnHighlightColor.Size = new System.Drawing.Size(40, 23);
            this.btnHighlightColor.TabIndex = 6;
            this.btnHighlightColor.UseVisualStyleBackColor = true;
            // 
            // grpOtherSettings
            //
            this.grpOtherSettings.Controls.Add(this.chkCopyCleanSql);
            this.grpOtherSettings.Location = new System.Drawing.Point(15, 385);
            this.grpOtherSettings.Name = "grpOtherSettings";
            this.grpOtherSettings.Size = new System.Drawing.Size(400, 60);
            this.grpOtherSettings.TabIndex = 3;
            this.grpOtherSettings.TabStop = false;
            this.grpOtherSettings.Text = "Miscellaneous";
            //
            // chkCopyCleanSql
            //
            this.chkCopyCleanSql.AutoSize = true;
            this.chkCopyCleanSql.Location = new System.Drawing.Point(15, 25);
            this.chkCopyCleanSql.Name = "chkCopyCleanSql";
            this.chkCopyCleanSql.Size = new System.Drawing.Size(202, 17);
            this.chkCopyCleanSql.TabIndex = 0;
            this.chkCopyCleanSql.Text = "Copy cleaned SQL query to clipboard";
            this.chkCopyCleanSql.UseVisualStyleBackColor = true;
            //
            // GeneralSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpGridSettings);
            this.Controls.Add(this.grpSearchSettings);
            this.Controls.Add(this.grpOtherSettings);
            this.Controls.Add(this.label6);
            this.Name = "GeneralSettings";
            this.Size = new System.Drawing.Size(430, 696);
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).EndInit();
            this.grpGridSettings.ResumeLayout(false);
            this.grpGridSettings.PerformLayout();
            this.grpSearchSettings.ResumeLayout(false);
            this.grpSearchSettings.PerformLayout();
            this.grpOtherSettings.ResumeLayout(false);
            this.grpOtherSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpGridSettings;
        private System.Windows.Forms.CheckBox chkDescriptionColumns;
        private System.Windows.Forms.CheckBox chkResizeColumns;
        private System.Windows.Forms.CheckBox chkShowRowNumbers;
        private System.Windows.Forms.Label lblAutoResizeLimit;
        private System.Windows.Forms.TextBox txtAutoResizeLimit;
        private System.Windows.Forms.GroupBox grpSearchSettings;
        private System.Windows.Forms.CheckBox chkSearchAny;
        private System.Windows.Forms.Label lblTableLookup;
        private System.Windows.Forms.ComboBox cmbTableLookup;
        private System.Windows.Forms.CheckBox chkHighlightMatches;
        private System.Windows.Forms.PictureBox picHighlightWarning;
        private System.Windows.Forms.Label lblHighlightColor;
        private System.Windows.Forms.Button btnHighlightColor;
        private System.Windows.Forms.GroupBox grpOtherSettings;
        private System.Windows.Forms.CheckBox chkCopyCleanSql;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
