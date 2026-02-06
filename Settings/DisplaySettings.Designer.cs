namespace SMS_Search.Settings
{
    partial class DisplaySettings
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkDescriptionColumns = new System.Windows.Forms.CheckBox();
            this.chkResizeColumns = new System.Windows.Forms.CheckBox();
            this.chkShowRowNumbers = new System.Windows.Forms.CheckBox();
            this.lblAutoResizeLimit = new System.Windows.Forms.Label();
            this.txtAutoResizeLimit = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblCopyDelimiter = new System.Windows.Forms.Label();
            this.cmbCopyDelimiter = new System.Windows.Forms.ComboBox();
            this.txtCustomDelimiter = new System.Windows.Forms.TextBox();
            this.lblCopyWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(15, 15);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(294, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Configure how data results are displayed in the grid.";
            // 
            // chkDescriptionColumns
            // 
            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(18, 50);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(169, 19);
            this.chkDescriptionColumns.TabIndex = 1;
            this.chkDescriptionColumns.Text = "Show description in header";
            this.toolTip1.SetToolTip(this.chkDescriptionColumns, "Show descriptive names in column headers instead of field codes.");
            this.chkDescriptionColumns.UseVisualStyleBackColor = true;
            // 
            // chkResizeColumns
            // 
            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.Location = new System.Drawing.Point(18, 100);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(107, 19);
            this.chkResizeColumns.TabIndex = 2;
            this.chkResizeColumns.Text = "Resize columns";
            this.toolTip1.SetToolTip(this.chkResizeColumns, "Automatically resize columns when executing a search or when toggling between fie" +
        "ld codes and descriptions.");
            this.chkResizeColumns.UseVisualStyleBackColor = true;
            // 
            // chkShowRowNumbers
            // 
            this.chkShowRowNumbers.AutoSize = true;
            this.chkShowRowNumbers.Location = new System.Drawing.Point(18, 75);
            this.chkShowRowNumbers.Name = "chkShowRowNumbers";
            this.chkShowRowNumbers.Size = new System.Drawing.Size(128, 19);
            this.chkShowRowNumbers.TabIndex = 3;
            this.chkShowRowNumbers.Text = "Show row numbers";
            this.toolTip1.SetToolTip(this.chkShowRowNumbers, "Display row numbers in the row header.");
            this.chkShowRowNumbers.UseVisualStyleBackColor = true;
            // 
            // lblAutoResizeLimit
            // 
            this.lblAutoResizeLimit.AutoSize = true;
            this.lblAutoResizeLimit.Location = new System.Drawing.Point(34, 122);
            this.lblAutoResizeLimit.Name = "lblAutoResizeLimit";
            this.lblAutoResizeLimit.Size = new System.Drawing.Size(183, 15);
            this.lblAutoResizeLimit.TabIndex = 4;
            this.lblAutoResizeLimit.Text = "Max rows for column auto-resize:";
            // 
            // txtAutoResizeLimit
            // 
            this.txtAutoResizeLimit.Location = new System.Drawing.Point(223, 119);
            this.txtAutoResizeLimit.Name = "txtAutoResizeLimit";
            this.txtAutoResizeLimit.Size = new System.Drawing.Size(60, 23);
            this.txtAutoResizeLimit.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtAutoResizeLimit, "Maximum number of rows to process when auto-resizing columns (improves performanc" +
        "e).");
            //
            // lblCopyDelimiter
            //
            this.lblCopyDelimiter.AutoSize = true;
            this.lblCopyDelimiter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyDelimiter.Location = new System.Drawing.Point(15, 160);
            this.lblCopyDelimiter.Name = "lblCopyDelimiter";
            this.lblCopyDelimiter.Size = new System.Drawing.Size(91, 15);
            this.lblCopyDelimiter.TabIndex = 6;
            this.lblCopyDelimiter.Text = "Copy delimiter:";
            //
            // cmbCopyDelimiter
            //
            this.cmbCopyDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopyDelimiter.FormattingEnabled = true;
            this.cmbCopyDelimiter.Location = new System.Drawing.Point(18, 180);
            this.cmbCopyDelimiter.Name = "cmbCopyDelimiter";
            this.cmbCopyDelimiter.Size = new System.Drawing.Size(150, 23);
            this.cmbCopyDelimiter.TabIndex = 7;
            this.toolTip1.SetToolTip(this.cmbCopyDelimiter, "Select delimiter for copy operations.");
            //
            // txtCustomDelimiter
            //
            this.txtCustomDelimiter.Location = new System.Drawing.Point(175, 180);
            this.txtCustomDelimiter.Name = "txtCustomDelimiter";
            this.txtCustomDelimiter.Size = new System.Drawing.Size(100, 23);
            this.txtCustomDelimiter.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtCustomDelimiter, "Enter custom delimiter characters.");
            this.txtCustomDelimiter.Visible = false;
            //
            // lblCopyWarning
            //
            this.lblCopyWarning.AutoSize = true;
            this.lblCopyWarning.ForeColor = System.Drawing.Color.DimGray;
            this.lblCopyWarning.Location = new System.Drawing.Point(15, 210);
            this.lblCopyWarning.Name = "lblCopyWarning";
            this.lblCopyWarning.Size = new System.Drawing.Size(383, 15);
            this.lblCopyWarning.TabIndex = 9;
            this.lblCopyWarning.Text = "Note: Custom delimiters perform a text-only copy (Excel formatting may be lost).";
            // 
            // DisplaySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblCopyWarning);
            this.Controls.Add(this.txtCustomDelimiter);
            this.Controls.Add(this.cmbCopyDelimiter);
            this.Controls.Add(this.lblCopyDelimiter);
            this.Controls.Add(this.txtAutoResizeLimit);
            this.Controls.Add(this.lblAutoResizeLimit);
            this.Controls.Add(this.chkShowRowNumbers);
            this.Controls.Add(this.chkResizeColumns);
            this.Controls.Add(this.chkDescriptionColumns);
            this.Controls.Add(this.lblDescription);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DisplaySettings";
            this.Size = new System.Drawing.Size(450, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkDescriptionColumns;
        private System.Windows.Forms.CheckBox chkResizeColumns;
        private System.Windows.Forms.CheckBox chkShowRowNumbers;
        private System.Windows.Forms.Label lblAutoResizeLimit;
        private System.Windows.Forms.TextBox txtAutoResizeLimit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblCopyDelimiter;
        private System.Windows.Forms.ComboBox cmbCopyDelimiter;
        private System.Windows.Forms.TextBox txtCustomDelimiter;
        private System.Windows.Forms.Label lblCopyWarning;
    }
}
