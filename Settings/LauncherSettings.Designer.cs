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
            this.lblLauncherInfo = new System.Windows.Forms.Label();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUnregister = new System.Windows.Forms.Button();
            this.lblLauncherStatus = new System.Windows.Forms.Label();
            this.pbLauncherStatus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).BeginInit();
            this.SuspendLayout();
            //
            // lblLauncherInfo
            //
            this.lblLauncherInfo.AutoSize = true;
            this.lblLauncherInfo.Location = new System.Drawing.Point(16, 16);
            this.lblLauncherInfo.Name = "lblLauncherInfo";
            this.lblLauncherInfo.Size = new System.Drawing.Size(400, 13);
            this.lblLauncherInfo.TabIndex = 0;
            this.lblLauncherInfo.Text = "Update the hidden launcher settings to switch to SMS Search via global hotkey.";
            //
            // lblHotkey
            //
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(16, 50);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(44, 13);
            this.lblHotkey.TabIndex = 1;
            this.lblHotkey.Text = "Hotkey:";
            //
            // txtHotkey
            //
            this.txtHotkey.Location = new System.Drawing.Point(66, 47);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.Size = new System.Drawing.Size(200, 20);
            this.txtHotkey.TabIndex = 2;
            //
            // btnRegister
            //
            this.btnRegister.Location = new System.Drawing.Point(19, 90);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 23);
            this.btnRegister.TabIndex = 3;
            this.btnRegister.Text = "Register Service";
            this.btnRegister.UseVisualStyleBackColor = true;
            //
            // btnUnregister
            //
            this.btnUnregister.Location = new System.Drawing.Point(150, 90);
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(120, 23);
            this.btnUnregister.TabIndex = 4;
            this.btnUnregister.Text = "Unregister Service";
            this.btnUnregister.UseVisualStyleBackColor = true;
            //
            // lblLauncherStatus
            //
            this.lblLauncherStatus.AutoSize = true;
            this.lblLauncherStatus.Location = new System.Drawing.Point(40, 132);
            this.lblLauncherStatus.Name = "lblLauncherStatus";
            this.lblLauncherStatus.Size = new System.Drawing.Size(0, 13);
            this.lblLauncherStatus.TabIndex = 6;
            //
            // pbLauncherStatus
            //
            this.pbLauncherStatus.Location = new System.Drawing.Point(19, 130);
            this.pbLauncherStatus.Name = "pbLauncherStatus";
            this.pbLauncherStatus.Size = new System.Drawing.Size(16, 16);
            this.pbLauncherStatus.TabIndex = 5;
            this.pbLauncherStatus.TabStop = false;
            //
            // LauncherSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLauncherStatus);
            this.Controls.Add(this.pbLauncherStatus);
            this.Controls.Add(this.btnUnregister);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtHotkey);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.lblLauncherInfo);
            this.Name = "LauncherSettings";
            this.Size = new System.Drawing.Size(430, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pbLauncherStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLauncherInfo;
        private System.Windows.Forms.Label lblHotkey;
        private System.Windows.Forms.TextBox txtHotkey;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnUnregister;
        private System.Windows.Forms.Label lblLauncherStatus;
        private System.Windows.Forms.PictureBox pbLauncherStatus;
    }
}
