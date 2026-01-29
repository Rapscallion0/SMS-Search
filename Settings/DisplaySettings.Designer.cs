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
            this.SuspendLayout();
            //
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(15, 15);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(268, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Configure how data results are displayed in the grid.";
            //
            // chkDescriptionColumns
            //
            this.chkDescriptionColumns.AutoSize = true;
            this.chkDescriptionColumns.Location = new System.Drawing.Point(18, 50);
            this.chkDescriptionColumns.Name = "chkDescriptionColumns";
            this.chkDescriptionColumns.Size = new System.Drawing.Size(143, 17);
            this.chkDescriptionColumns.TabIndex = 1;
            this.chkDescriptionColumns.Text = "Show description in header";
            this.toolTip1.SetToolTip(this.chkDescriptionColumns, "Show descriptive names in column headers instead of field codes.");
            this.chkDescriptionColumns.UseVisualStyleBackColor = true;
            //
            // chkResizeColumns
            //
            this.chkResizeColumns.AutoSize = true;
            this.chkResizeColumns.Location = new System.Drawing.Point(18, 75);
            this.chkResizeColumns.Name = "chkResizeColumns";
            this.chkResizeColumns.Size = new System.Drawing.Size(201, 17);
            this.chkResizeColumns.TabIndex = 2;
            this.chkResizeColumns.Text = "Resize columns on description toggle";
            this.toolTip1.SetToolTip(this.chkResizeColumns, "Automatically resize columns when toggling between field codes and descriptions.");
            this.chkResizeColumns.UseVisualStyleBackColor = true;
            //
            // chkShowRowNumbers
            //
            this.chkShowRowNumbers.AutoSize = true;
            this.chkShowRowNumbers.Location = new System.Drawing.Point(18, 100);
            this.chkShowRowNumbers.Name = "chkShowRowNumbers";
            this.chkShowRowNumbers.Size = new System.Drawing.Size(125, 17);
            this.chkShowRowNumbers.TabIndex = 3;
            this.chkShowRowNumbers.Text = "Show row numbers";
            this.toolTip1.SetToolTip(this.chkShowRowNumbers, "Display row numbers in the row header.");
            this.chkShowRowNumbers.UseVisualStyleBackColor = true;
            //
            // lblAutoResizeLimit
            //
            this.lblAutoResizeLimit.AutoSize = true;
            this.lblAutoResizeLimit.Location = new System.Drawing.Point(15, 130);
            this.lblAutoResizeLimit.Name = "lblAutoResizeLimit";
            this.lblAutoResizeLimit.Size = new System.Drawing.Size(161, 13);
            this.lblAutoResizeLimit.TabIndex = 4;
            this.lblAutoResizeLimit.Text = "Max rows for column auto-resize:";
            //
            // txtAutoResizeLimit
            //
            this.txtAutoResizeLimit.Location = new System.Drawing.Point(180, 127);
            this.txtAutoResizeLimit.Name = "txtAutoResizeLimit";
            this.txtAutoResizeLimit.Size = new System.Drawing.Size(60, 20);
            this.txtAutoResizeLimit.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtAutoResizeLimit, "Maximum number of rows to process when auto-resizing columns (improves performance).");
            //
            // DisplaySettings
            //
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.txtAutoResizeLimit);
            this.Controls.Add(this.lblAutoResizeLimit);
            this.Controls.Add(this.chkShowRowNumbers);
            this.Controls.Add(this.chkResizeColumns);
            this.Controls.Add(this.chkDescriptionColumns);
            this.Controls.Add(this.lblDescription);
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
    }
}
