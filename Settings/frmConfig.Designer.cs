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
            System.Windows.Forms.TreeNode nodeApp = new System.Windows.Forms.TreeNode("Application");
            System.Windows.Forms.TreeNode nodeDisplay = new System.Windows.Forms.TreeNode("Display");
            System.Windows.Forms.TreeNode nodeLogging = new System.Windows.Forms.TreeNode("Logging");
            System.Windows.Forms.TreeNode nodeGeneral = new System.Windows.Forms.TreeNode("General", new System.Windows.Forms.TreeNode[] {
            nodeApp,
            nodeDisplay,
            nodeLogging});

            System.Windows.Forms.TreeNode nodeBehavior = new System.Windows.Forms.TreeNode("Behavior");
            System.Windows.Forms.TreeNode nodeCleanSql = new System.Windows.Forms.TreeNode("Clean SQL");
            System.Windows.Forms.TreeNode nodeSearch = new System.Windows.Forms.TreeNode("Search", new System.Windows.Forms.TreeNode[] {
            nodeBehavior,
            nodeCleanSql});

            System.Windows.Forms.TreeNode nodeDatabase = new System.Windows.Forms.TreeNode("Database");
            System.Windows.Forms.TreeNode nodeLauncher = new System.Windows.Forms.TreeNode("Launcher");

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
            this.splitConfig.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitConfig.Location = new System.Drawing.Point(0, 0);
            this.splitConfig.Name = "splitConfig";
            // 
            // splitConfig.Panel1
            // 
            this.splitConfig.Panel1.Controls.Add(this.tvSettings);
            this.splitConfig.Size = new System.Drawing.Size(600, 400);
            this.splitConfig.SplitterDistance = 150;
            this.splitConfig.TabIndex = 0;
            // 
            // tvSettings
            // 
            this.tvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSettings.HideSelection = false;
            this.tvSettings.ImageIndex = 0;
            this.tvSettings.ImageList = this.imgListIcons;
            this.tvSettings.Location = new System.Drawing.Point(0, 0);
            this.tvSettings.Name = "tvSettings";

            nodeApp.Name = "Application";
            nodeApp.Text = "Application";
            nodeApp.ImageKey = "Application";
            nodeApp.SelectedImageKey = "Application";

            nodeDisplay.Name = "Display";
            nodeDisplay.Text = "Display";
            nodeDisplay.ImageKey = "Display";
            nodeDisplay.SelectedImageKey = "Display";

            nodeLogging.Name = "Logging";
            nodeLogging.Text = "Logging";
            nodeLogging.ImageKey = "Logging";
            nodeLogging.SelectedImageKey = "Logging";

            nodeGeneral.Name = "General";
            nodeGeneral.Text = "General";
            nodeGeneral.ImageKey = "General";
            nodeGeneral.SelectedImageKey = "General";

            nodeBehavior.Name = "Behavior";
            nodeBehavior.Text = "Behavior";
            nodeBehavior.ImageKey = "Behavior";
            nodeBehavior.SelectedImageKey = "Behavior";

            nodeCleanSql.Name = "CleanSql";
            nodeCleanSql.Text = "Clean SQL";
            nodeCleanSql.ImageKey = "CleanSql";
            nodeCleanSql.SelectedImageKey = "CleanSql";

            nodeSearch.Name = "Search";
            nodeSearch.Text = "Search";
            nodeSearch.ImageKey = "Search";
            nodeSearch.SelectedImageKey = "Search";

            nodeDatabase.Name = "Database";
            nodeDatabase.Text = "Database";
            nodeDatabase.ImageKey = "Database";
            nodeDatabase.SelectedImageKey = "Database";

            nodeLauncher.Name = "Launcher";
            nodeLauncher.Text = "Launcher";
            nodeLauncher.ImageKey = "Launcher";
            nodeLauncher.SelectedImageKey = "Launcher";

            this.tvSettings.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            nodeGeneral,
            nodeSearch,
            nodeDatabase,
            nodeLauncher});
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
            this.btnRevert.Location = new System.Drawing.Point(356, 415);
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
            this.btnClose.Location = new System.Drawing.Point(513, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.toolTip1.SetToolTip(this.btnClose, "Close settings");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenConfig.Location = new System.Drawing.Point(12, 415);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(110, 23);
            this.btnOpenConfig.TabIndex = 3;
            this.btnOpenConfig.Text = "Open Settings File";
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            //
            // lblAutoSave
            //
            this.lblAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAutoSave.AutoSize = true;
            this.lblAutoSave.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblAutoSave.Location = new System.Drawing.Point(135, 419);
            this.lblAutoSave.Name = "lblAutoSave";
            this.lblAutoSave.Size = new System.Drawing.Size(180, 15);
            this.lblAutoSave.TabIndex = 4;
            this.lblAutoSave.Text = "Settings are saved automatically";
            //
            // lblSavedStatus
            //
            this.lblSavedStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSavedStatus.AutoSize = true;
            this.lblSavedStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSavedStatus.ForeColor = System.Drawing.Color.Green;
            this.lblSavedStatus.Location = new System.Drawing.Point(315, 419);
            this.lblSavedStatus.Name = "lblSavedStatus";
            this.lblSavedStatus.Size = new System.Drawing.Size(43, 15);
            this.lblSavedStatus.TabIndex = 5;
            this.lblSavedStatus.Text = "Saved!";
            this.lblSavedStatus.Visible = false;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.lblSavedStatus);
            this.Controls.Add(this.lblAutoSave);
            this.Controls.Add(this.btnOpenConfig);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.splitConfig);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.TreeView tvSettings;
        private System.Windows.Forms.ImageList imgListIcons;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.Label lblAutoSave;
        private System.Windows.Forms.Label lblSavedStatus;
        private System.Windows.Forms.SplitContainer splitConfig;
    }
}
