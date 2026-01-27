namespace SMS_Search.Settings
{
    partial class frmConfig
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Database");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Advanced");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Update");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Logging");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Clean SQL");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Launcher");
            this.splitConfig = new System.Windows.Forms.SplitContainer();
            this.tvSettings = new System.Windows.Forms.TreeView();
            this.imgListIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnRevert = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblConfigFilePath = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).BeginInit();
            this.splitConfig.Panel1.SuspendLayout();
            this.splitConfig.SuspendLayout();
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
            this.splitConfig.Size = new System.Drawing.Size(584, 400);
            this.splitConfig.SplitterDistance = 150;
            this.splitConfig.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitConfig.TabIndex = 0;
            //
            // tvSettings
            //
            this.tvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSettings.ImageIndex = 0;
            this.tvSettings.ImageList = this.imgListIcons;
            this.tvSettings.Location = new System.Drawing.Point(0, 0);
            this.tvSettings.Name = "tvSettings";
            treeNode1.ImageKey = "General";
            treeNode1.Name = "General";
            treeNode1.SelectedImageKey = "General";
            treeNode1.Text = "General";
            treeNode2.ImageKey = "Database";
            treeNode2.Name = "Database";
            treeNode2.SelectedImageKey = "Database";
            treeNode2.Text = "Database";
            treeNode3.ImageKey = "Advanced";
            treeNode3.Name = "Advanced";
            treeNode3.SelectedImageKey = "Advanced";
            treeNode3.Text = "Advanced";
            treeNode4.ImageKey = "Update";
            treeNode4.Name = "Update";
            treeNode4.SelectedImageKey = "Update";
            treeNode4.Text = "Update";
            treeNode5.ImageKey = "Logging";
            treeNode5.Name = "Logging";
            treeNode5.SelectedImageKey = "Logging";
            treeNode5.Text = "Logging";
            treeNode6.ImageKey = "CleanSql";
            treeNode6.Name = "CleanSql";
            treeNode6.SelectedImageKey = "CleanSql";
            treeNode6.Text = "Clean SQL";
            treeNode7.ImageKey = "Launcher";
            treeNode7.Name = "Launcher";
            treeNode7.SelectedImageKey = "Launcher";
            treeNode7.Text = "Launcher";
            this.tvSettings.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
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
            //
            // btnRevert
            //
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevert.Location = new System.Drawing.Point(340, 415);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(120, 23);
            this.btnRevert.TabIndex = 1;
            this.btnRevert.Text = "Revert to Default";
            this.toolTip1.SetToolTip(this.btnRevert, "Reset all settings to default values");
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(497, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.toolTip1.SetToolTip(this.btnClose, "Close settings");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // lblConfigFilePath
            //
            this.lblConfigFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblConfigFilePath.AutoEllipsis = true;
            this.lblConfigFilePath.AutoSize = true;
            this.lblConfigFilePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConfigFilePath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblConfigFilePath.Location = new System.Drawing.Point(12, 420);
            this.lblConfigFilePath.MaximumSize = new System.Drawing.Size(240, 13);
            this.lblConfigFilePath.Name = "lblConfigFilePath";
            this.lblConfigFilePath.Size = new System.Drawing.Size(85, 13);
            this.lblConfigFilePath.TabIndex = 3;
            this.lblConfigFilePath.Text = "lblConfigFilePath";
            this.lblConfigFilePath.Click += new System.EventHandler(this.lblConfigFilePath_Click);
            //
            // frmConfig
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(584, 450);
            this.Controls.Add(this.lblConfigFilePath);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.splitConfig);
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
            this.splitConfig.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).EndInit();
            this.splitConfig.ResumeLayout(false);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.SplitContainer splitConfig;
        private System.Windows.Forms.TreeView tvSettings;
        private System.Windows.Forms.ImageList imgListIcons;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblConfigFilePath;
    }
}
