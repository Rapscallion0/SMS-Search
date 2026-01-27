namespace SMS_Search.Settings
{
    partial class LauncherSettings
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
            this.lblLauncherInfo = new System.Windows.Forms.Label();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUnregister = new System.Windows.Forms.Button();
            this.lblLauncherStatus = new System.Windows.Forms.Label();
            this.pbLauncherStatus = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblWarning = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(16, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(277, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Configure background service and global hotkey.";
            // 
            // lblLauncherInfo
            // 
            this.lblLauncherInfo.AutoSize = true;
            this.lblLauncherInfo.Location = new System.Drawing.Point(16, 46);
            this.lblLauncherInfo.Name = "lblLauncherInfo";
            this.lblLauncherInfo.Size = new System.Drawing.Size(384, 13);
            this.lblLauncherInfo.TabIndex = 1;
            this.lblLauncherInfo.Text = "Update the hidden launcher settings to switch to SMS Search via global hotkey.";
            // 
            // lblHotkey
            // 
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(16, 80);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(44, 13);
            this.lblHotkey.TabIndex = 2;
            this.lblHotkey.Text = "Hotkey:";
            // 
            // txtHotkey
            // 
            this.txtHotkey.Location = new System.Drawing.Point(66, 77);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.Size = new System.Drawing.Size(200, 20);
            this.txtHotkey.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtHotkey, "Enter the global hotkey combination.");
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(19, 120);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 23);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "Register Service";
            this.toolTip1.SetToolTip(this.btnRegister, "Register and start the background listener service.");
            this.btnRegister.UseVisualStyleBackColor = true;
            // 
            // btnUnregister
            // 
            this.btnUnregister.Location = new System.Drawing.Point(150, 120);
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(120, 23);
            this.btnUnregister.TabIndex = 5;
            this.btnUnregister.Text = "Unregister Service";
            this.toolTip1.SetToolTip(this.btnUnregister, "Stop and unregister the background listener service.");
            this.btnUnregister.UseVisualStyleBackColor = true;
            // 
            // lblLauncherStatus
            // 
            this.lblLauncherStatus.AutoSize = true;
            this.lblLauncherStatus.Location = new System.Drawing.Point(40, 162);
            this.lblLauncherStatus.Name = "lblLauncherStatus";
            this.lblLauncherStatus.Size = new System.Drawing.Size(0, 13);
            this.lblLauncherStatus.TabIndex = 7;
            // 
            // pbLauncherStatus
            // 
            this.pbLauncherStatus.Location = new System.Drawing.Point(19, 160);
            this.pbLauncherStatus.Name = "pbLauncherStatus";
            this.pbLauncherStatus.Size = new System.Drawing.Size(16, 16);
            this.pbLauncherStatus.TabIndex = 6;
            this.pbLauncherStatus.TabStop = false;
            this.toolTip1.SetToolTip(this.pbLauncherStatus, "Status of the background listener service.");
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(63, 100);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(227, 13);
            this.lblWarning.TabIndex = 8;
            this.lblWarning.Text = "* Service must be registered for hotkey to work";
            this.lblWarning.Visible = false;
            // 
            // LauncherSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblLauncherStatus);
            this.Controls.Add(this.pbLauncherStatus);
            this.Controls.Add(this.btnUnregister);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtHotkey);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.lblLauncherInfo);
            this.Name = "LauncherSettings";
            this.Size = new System.Drawing.Size(450, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblLauncherInfo;
        private System.Windows.Forms.Label lblHotkey;
        private System.Windows.Forms.TextBox txtHotkey;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnUnregister;
        private System.Windows.Forms.Label lblLauncherStatus;
        private System.Windows.Forms.PictureBox pbLauncherStatus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblWarning;
    }
}
