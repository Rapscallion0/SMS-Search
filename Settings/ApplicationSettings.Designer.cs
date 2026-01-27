namespace SMS_Search.Settings
{
    partial class ApplicationSettings
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
            this.lblStartTab = new System.Windows.Forms.Label();
            this.cmbStartTab = new System.Windows.Forms.ComboBox();
            this.lblStartupLocation = new System.Windows.Forms.Label();
            this.cmbStartupLocation = new System.Windows.Forms.ComboBox();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.chkShowInTray = new System.Windows.Forms.CheckBox();
            this.chkMultiInstance = new System.Windows.Forms.CheckBox();
            this.chkUnarchiveTarget = new System.Windows.Forms.CheckBox();
            this.chkCheckUpdate = new System.Windows.Forms.CheckBox();
            this.btnChkUpdate = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(15, 15);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(262, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "General application settings and startup behavior.";
            //
            // lblStartTab
            //
            this.lblStartTab.AutoSize = true;
            this.lblStartTab.Location = new System.Drawing.Point(15, 50);
            this.lblStartTab.Name = "lblStartTab";
            this.lblStartTab.Size = new System.Drawing.Size(62, 13);
            this.lblStartTab.TabIndex = 1;
            this.lblStartTab.Text = "Startup tab:";
            //
            // cmbStartTab
            //
            this.cmbStartTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTab.FormattingEnabled = true;
            this.cmbStartTab.Items.AddRange(new object[] {
            "Function",
            "Totalizer",
            "Fields"});
            this.cmbStartTab.Location = new System.Drawing.Point(120, 47);
            this.cmbStartTab.Name = "cmbStartTab";
            this.cmbStartTab.Size = new System.Drawing.Size(150, 21);
            this.cmbStartTab.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cmbStartTab, "Select which tab is active when the application starts.");
            //
            // lblStartupLocation
            //
            this.lblStartupLocation.AutoSize = true;
            this.lblStartupLocation.Location = new System.Drawing.Point(15, 80);
            this.lblStartupLocation.Name = "lblStartupLocation";
            this.lblStartupLocation.Size = new System.Drawing.Size(84, 13);
            this.lblStartupLocation.TabIndex = 3;
            this.lblStartupLocation.Text = "Startup location:";
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
            this.cmbStartupLocation.Location = new System.Drawing.Point(120, 77);
            this.cmbStartupLocation.Name = "cmbStartupLocation";
            this.cmbStartupLocation.Size = new System.Drawing.Size(150, 21);
            this.cmbStartupLocation.TabIndex = 4;
            this.toolTip1.SetToolTip(this.cmbStartupLocation, "Select where the window appears when the application starts.");
            //
            // chkAlwaysOnTop
            //
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(18, 115);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.chkAlwaysOnTop.TabIndex = 5;
            this.chkAlwaysOnTop.Text = "Always on top";
            this.toolTip1.SetToolTip(this.chkAlwaysOnTop, "Keep the application window above all other windows.");
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            //
            // chkShowInTray
            //
            this.chkShowInTray.AutoSize = true;
            this.chkShowInTray.Location = new System.Drawing.Point(18, 140);
            this.chkShowInTray.Name = "chkShowInTray";
            this.chkShowInTray.Size = new System.Drawing.Size(119, 17);
            this.chkShowInTray.TabIndex = 6;
            this.chkShowInTray.Text = "Show in system tray";
            this.toolTip1.SetToolTip(this.chkShowInTray, "Minimize the application to the system tray instead of the taskbar.");
            this.chkShowInTray.UseVisualStyleBackColor = true;
            //
            // chkMultiInstance
            //
            this.chkMultiInstance.AutoSize = true;
            this.chkMultiInstance.Location = new System.Drawing.Point(18, 165);
            this.chkMultiInstance.Name = "chkMultiInstance";
            this.chkMultiInstance.Size = new System.Drawing.Size(137, 17);
            this.chkMultiInstance.TabIndex = 7;
            this.chkMultiInstance.Text = "Allow multiple instances";
            this.toolTip1.SetToolTip(this.chkMultiInstance, "Allow more than one instance of SMS Search to run simultaneously.");
            this.chkMultiInstance.UseVisualStyleBackColor = true;
            //
            // chkUnarchiveTarget
            //
            this.chkUnarchiveTarget.AutoSize = true;
            this.chkUnarchiveTarget.Location = new System.Drawing.Point(18, 190);
            this.chkUnarchiveTarget.Name = "chkUnarchiveTarget";
            this.chkUnarchiveTarget.Size = new System.Drawing.Size(183, 17);
            this.chkUnarchiveTarget.TabIndex = 8;
            this.chkUnarchiveTarget.Text = "Show unarchive target on startup";
            this.toolTip1.SetToolTip(this.chkUnarchiveTarget, "Automatically show the unarchive target dialog when the application starts.");
            this.chkUnarchiveTarget.UseVisualStyleBackColor = true;
            //
            // chkCheckUpdate
            //
            this.chkCheckUpdate.AutoSize = true;
            this.chkCheckUpdate.Location = new System.Drawing.Point(18, 215);
            this.chkCheckUpdate.Name = "chkCheckUpdate";
            this.chkCheckUpdate.Size = new System.Drawing.Size(222, 17);
            this.chkCheckUpdate.TabIndex = 9;
            this.chkCheckUpdate.Text = "Automatically check for update on startup";
            this.toolTip1.SetToolTip(this.chkCheckUpdate, "Check for new versions of SMS Search when the application launches.");
            this.chkCheckUpdate.UseVisualStyleBackColor = true;
            //
            // btnChkUpdate
            //
            this.btnChkUpdate.Location = new System.Drawing.Point(18, 240);
            this.btnChkUpdate.Name = "btnChkUpdate";
            this.btnChkUpdate.Size = new System.Drawing.Size(140, 30);
            this.btnChkUpdate.TabIndex = 10;
            this.btnChkUpdate.Text = "Check for update";
            this.toolTip1.SetToolTip(this.btnChkUpdate, "Manually check for updates now.");
            this.btnChkUpdate.UseVisualStyleBackColor = true;
            //
            // ApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnChkUpdate);
            this.Controls.Add(this.chkCheckUpdate);
            this.Controls.Add(this.chkUnarchiveTarget);
            this.Controls.Add(this.chkMultiInstance);
            this.Controls.Add(this.chkShowInTray);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.cmbStartupLocation);
            this.Controls.Add(this.lblStartupLocation);
            this.Controls.Add(this.cmbStartTab);
            this.Controls.Add(this.lblStartTab);
            this.Controls.Add(this.lblDescription);
            this.Name = "ApplicationSettings";
            this.Size = new System.Drawing.Size(450, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblStartTab;
        private System.Windows.Forms.ComboBox cmbStartTab;
        private System.Windows.Forms.Label lblStartupLocation;
        private System.Windows.Forms.ComboBox cmbStartupLocation;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.CheckBox chkShowInTray;
        private System.Windows.Forms.CheckBox chkMultiInstance;
        private System.Windows.Forms.CheckBox chkUnarchiveTarget;
        private System.Windows.Forms.CheckBox chkCheckUpdate;
        private System.Windows.Forms.Button btnChkUpdate;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
