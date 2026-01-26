namespace SMS_Search.Settings
{
    partial class DatabaseSettings
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
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDbServer = new System.Windows.Forms.ComboBox();
            this.chkScanNetwork = new System.Windows.Forms.CheckBox();
            this.btnTestConn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDbDatabase = new System.Windows.Forms.ComboBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.lblDbUser = new System.Windows.Forms.Label();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.lblDbPassword = new System.Windows.Forms.Label();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "DB Server:";
            //
            // cmbDbServer
            //
            this.cmbDbServer.FormattingEnabled = true;
            this.cmbDbServer.Location = new System.Drawing.Point(73, 12);
            this.cmbDbServer.Name = "cmbDbServer";
            this.cmbDbServer.Size = new System.Drawing.Size(220, 21);
            this.cmbDbServer.TabIndex = 1;
            //
            // chkScanNetwork
            //
            this.chkScanNetwork.AutoSize = true;
            this.chkScanNetwork.Enabled = false;
            this.chkScanNetwork.Location = new System.Drawing.Point(300, 14);
            this.chkScanNetwork.Name = "chkScanNetwork";
            this.chkScanNetwork.Size = new System.Drawing.Size(131, 17);
            this.chkScanNetwork.TabIndex = 2;
            this.chkScanNetwork.Text = "Scan network (slower)";
            this.chkScanNetwork.Visible = false;
            this.chkScanNetwork.UseVisualStyleBackColor = true;
            //
            // btnTestConn
            //
            this.btnTestConn.Location = new System.Drawing.Point(73, 150);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(115, 23);
            this.btnTestConn.TabIndex = 10;
            this.btnTestConn.Text = "Test DB connection";
            this.btnTestConn.UseVisualStyleBackColor = true;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "DB Name:";
            //
            // cmbDbDatabase
            //
            this.cmbDbDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDbDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDbDatabase.FormattingEnabled = true;
            this.cmbDbDatabase.Location = new System.Drawing.Point(73, 42);
            this.cmbDbDatabase.Name = "cmbDbDatabase";
            this.cmbDbDatabase.Size = new System.Drawing.Size(220, 21);
            this.cmbDbDatabase.Sorted = true;
            this.cmbDbDatabase.TabIndex = 4;
            //
            // chkWindowsAuth
            //
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Checked = true;
            this.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuth.Enabled = false;
            this.chkWindowsAuth.Location = new System.Drawing.Point(15, 80);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(141, 17);
            this.chkWindowsAuth.TabIndex = 5;
            this.chkWindowsAuth.Text = "Windows Authentication";
            this.chkWindowsAuth.Visible = false;
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            //
            // lblDbUser
            //
            this.lblDbUser.AutoSize = true;
            this.lblDbUser.Location = new System.Drawing.Point(12, 103);
            this.lblDbUser.Name = "lblDbUser";
            this.lblDbUser.Size = new System.Drawing.Size(61, 13);
            this.lblDbUser.TabIndex = 6;
            this.lblDbUser.Text = "User name:";
            //
            // txtDbUser
            //
            this.txtDbUser.Location = new System.Drawing.Point(79, 100);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(150, 20);
            this.txtDbUser.TabIndex = 7;
            //
            // lblDbPassword
            //
            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(12, 129);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(56, 13);
            this.lblDbPassword.TabIndex = 8;
            this.lblDbPassword.Text = "Password:";
            //
            // txtDbPassword
            //
            this.txtDbPassword.Location = new System.Drawing.Point(79, 126);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(150, 20);
            this.txtDbPassword.TabIndex = 9;
            //
            // lblConnStatus
            //
            this.lblConnStatus.AutoSize = true;
            this.lblConnStatus.Location = new System.Drawing.Point(200, 155);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(0, 13);
            this.lblConnStatus.TabIndex = 11;
            //
            // DatabaseSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblConnStatus);
            this.Controls.Add(this.txtDbPassword);
            this.Controls.Add(this.lblDbPassword);
            this.Controls.Add(this.txtDbUser);
            this.Controls.Add(this.lblDbUser);
            this.Controls.Add(this.chkWindowsAuth);
            this.Controls.Add(this.cmbDbDatabase);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnTestConn);
            this.Controls.Add(this.chkScanNetwork);
            this.Controls.Add(this.cmbDbServer);
            this.Controls.Add(this.label3);
            this.Name = "DatabaseSettings";
            this.Size = new System.Drawing.Size(430, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDbServer;
        private System.Windows.Forms.CheckBox chkScanNetwork;
        private System.Windows.Forms.Button btnTestConn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDbDatabase;
        private System.Windows.Forms.CheckBox chkWindowsAuth;
        private System.Windows.Forms.Label lblDbUser;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.Label lblDbPassword;
        private System.Windows.Forms.TextBox txtDbPassword;
        private System.Windows.Forms.Label lblConnStatus;
    }
}
