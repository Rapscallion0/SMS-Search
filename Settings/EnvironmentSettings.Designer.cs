namespace SMS_Search.Settings
{
    partial class EnvironmentSettings
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
            this.lblStartTab = new System.Windows.Forms.Label();
            this.cmbStartTab = new System.Windows.Forms.ComboBox();
            this.lblStartupLocation = new System.Windows.Forms.Label();
            this.cmbStartupLocation = new System.Windows.Forms.ComboBox();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.chkShowInTray = new System.Windows.Forms.CheckBox();
            this.chkMultiInstance = new System.Windows.Forms.CheckBox();
            this.chkUnarchiveTarget = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblStartTab
            //
            this.lblStartTab.AutoSize = true;
            this.lblStartTab.Location = new System.Drawing.Point(15, 18);
            this.lblStartTab.Name = "lblStartTab";
            this.lblStartTab.Size = new System.Drawing.Size(59, 13);
            this.lblStartTab.TabIndex = 0;
            this.lblStartTab.Text = "Startup tab";
            //
            // cmbStartTab
            //
            this.cmbStartTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTab.FormattingEnabled = true;
            this.cmbStartTab.Items.AddRange(new object[] {
            "Function",
            "Totalizer",
            "Fields"});
            this.cmbStartTab.Location = new System.Drawing.Point(120, 15);
            this.cmbStartTab.Name = "cmbStartTab";
            this.cmbStartTab.Size = new System.Drawing.Size(121, 21);
            this.cmbStartTab.TabIndex = 1;
            //
            // lblStartupLocation
            //
            this.lblStartupLocation.AutoSize = true;
            this.lblStartupLocation.Location = new System.Drawing.Point(15, 48);
            this.lblStartupLocation.Name = "lblStartupLocation";
            this.lblStartupLocation.Size = new System.Drawing.Size(81, 13);
            this.lblStartupLocation.TabIndex = 2;
            this.lblStartupLocation.Text = "Startup location";
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
            this.cmbStartupLocation.Location = new System.Drawing.Point(120, 45);
            this.cmbStartupLocation.Name = "cmbStartupLocation";
            this.cmbStartupLocation.Size = new System.Drawing.Size(121, 21);
            this.cmbStartupLocation.TabIndex = 3;
            //
            // chkAlwaysOnTop
            //
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(18, 80);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.chkAlwaysOnTop.TabIndex = 4;
            this.chkAlwaysOnTop.Text = "Always on top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            //
            // chkShowInTray
            //
            this.chkShowInTray.AutoSize = true;
            this.chkShowInTray.Location = new System.Drawing.Point(18, 105);
            this.chkShowInTray.Name = "chkShowInTray";
            this.chkShowInTray.Size = new System.Drawing.Size(119, 17);
            this.chkShowInTray.TabIndex = 5;
            this.chkShowInTray.Text = "Show in system tray";
            this.toolTip1.SetToolTip(this.chkShowInTray, "Will not show in task bar");
            this.chkShowInTray.UseVisualStyleBackColor = true;
            //
            // chkMultiInstance
            //
            this.chkMultiInstance.AutoSize = true;
            this.chkMultiInstance.Location = new System.Drawing.Point(18, 130);
            this.chkMultiInstance.Name = "chkMultiInstance";
            this.chkMultiInstance.Size = new System.Drawing.Size(137, 17);
            this.chkMultiInstance.TabIndex = 6;
            this.chkMultiInstance.Text = "Allow multiple instances";
            this.chkMultiInstance.UseVisualStyleBackColor = true;
            //
            // chkUnarchiveTarget
            //
            this.chkUnarchiveTarget.AutoSize = true;
            this.chkUnarchiveTarget.Location = new System.Drawing.Point(18, 155);
            this.chkUnarchiveTarget.Name = "chkUnarchiveTarget";
            this.chkUnarchiveTarget.Size = new System.Drawing.Size(183, 17);
            this.chkUnarchiveTarget.TabIndex = 7;
            this.chkUnarchiveTarget.Text = "Show unarchive target on startup";
            this.chkUnarchiveTarget.UseVisualStyleBackColor = true;
            //
            // EnvironmentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkUnarchiveTarget);
            this.Controls.Add(this.chkMultiInstance);
            this.Controls.Add(this.chkShowInTray);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.cmbStartupLocation);
            this.Controls.Add(this.lblStartupLocation);
            this.Controls.Add(this.cmbStartTab);
            this.Controls.Add(this.lblStartTab);
            this.Name = "EnvironmentSettings";
            this.Size = new System.Drawing.Size(334, 259);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStartTab;
        private System.Windows.Forms.ComboBox cmbStartTab;
        private System.Windows.Forms.Label lblStartupLocation;
        private System.Windows.Forms.ComboBox cmbStartupLocation;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.CheckBox chkShowInTray;
        private System.Windows.Forms.CheckBox chkMultiInstance;
        private System.Windows.Forms.CheckBox chkUnarchiveTarget;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
