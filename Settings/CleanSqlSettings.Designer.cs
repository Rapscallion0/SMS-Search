namespace SMS_Search.Settings
{
    partial class CleanSqlSettings
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
            this.dgvCleanSqlRules = new System.Windows.Forms.DataGridView();
            this.colRegex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnResetCleanSql = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(10, 10);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(350, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Define Regex rules to clean SQL queries before copying to clipboard.";
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
            this.dgvCleanSqlRules.Location = new System.Drawing.Point(10, 40);
            this.dgvCleanSqlRules.Name = "dgvCleanSqlRules";
            this.dgvCleanSqlRules.Size = new System.Drawing.Size(430, 320);
            this.dgvCleanSqlRules.TabIndex = 1;
            this.toolTip1.SetToolTip(this.dgvCleanSqlRules, "List of Regex patterns and their replacements.");
            // 
            // colRegex
            // 
            this.colRegex.HeaderText = "Regex Pattern";
            this.colRegex.Name = "colRegex";
            this.colRegex.Width = 200;
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
            this.btnResetCleanSql.Location = new System.Drawing.Point(355, 370);
            this.btnResetCleanSql.Name = "btnResetCleanSql";
            this.btnResetCleanSql.Size = new System.Drawing.Size(85, 23);
            this.btnResetCleanSql.TabIndex = 2;
            this.btnResetCleanSql.Text = "Reset Defaults";
            this.toolTip1.SetToolTip(this.btnResetCleanSql, "Restore the default set of cleaning rules.");
            this.btnResetCleanSql.UseVisualStyleBackColor = true;
            // 
            // CleanSqlSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnResetCleanSql);
            this.Controls.Add(this.dgvCleanSqlRules);
            this.Name = "CleanSqlSettings";
            this.Size = new System.Drawing.Size(450, 400);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCleanSqlRules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.DataGridView dgvCleanSqlRules;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReplace;
        private System.Windows.Forms.Button btnResetCleanSql;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
