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
            this.components = new System.ComponentModel.Container();
            this.lblDescription = new System.Windows.Forms.Label();
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(12, 15);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(204, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Configure the database connection.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "DB Server:";
            // 
            // cmbDbServer
            // 
            this.cmbDbServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDbServer.FormattingEnabled = true;
            this.cmbDbServer.Location = new System.Drawing.Point(73, 42);
            this.cmbDbServer.Name = "cmbDbServer";
            this.cmbDbServer.Size = new System.Drawing.Size(284, 21);
            this.cmbDbServer.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cmbDbServer, "Select or enter the SQL Server instance name.");
            // 
            // chkScanNetwork
            // 
            this.chkScanNetwork.AutoSize = true;
            this.chkScanNetwork.Enabled = false;
            this.chkScanNetwork.Location = new System.Drawing.Point(15, 193);
            this.chkScanNetwork.Name = "chkScanNetwork";
            this.chkScanNetwork.Size = new System.Drawing.Size(131, 17);
            this.chkScanNetwork.TabIndex = 6;
            this.chkScanNetwork.Text = "Scan network (slower)";
            this.chkScanNetwork.UseVisualStyleBackColor = true;
            this.chkScanNetwork.Visible = false;
            // 
            // btnTestConn
            // 
            this.btnTestConn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConn.Location = new System.Drawing.Point(363, 42);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(71, 51);
            this.btnTestConn.TabIndex = 5;
            this.btnTestConn.Text = "Test DB connection";
            this.toolTip1.SetToolTip(this.btnTestConn, "Test the connection to the specified database.");
            this.btnTestConn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "DB Name:";
            // 
            // cmbDbDatabase
            // 
            this.cmbDbDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDbDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDbDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDbDatabase.FormattingEnabled = true;
            this.cmbDbDatabase.Location = new System.Drawing.Point(73, 72);
            this.cmbDbDatabase.Name = "cmbDbDatabase";
            this.cmbDbDatabase.Size = new System.Drawing.Size(284, 21);
            this.cmbDbDatabase.Sorted = true;
            this.cmbDbDatabase.TabIndex = 4;
            this.toolTip1.SetToolTip(this.cmbDbDatabase, "Select or enter the database name.");
            // 
            // chkWindowsAuth
            // 
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Checked = true;
            this.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuth.Location = new System.Drawing.Point(15, 110);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(141, 17);
            this.chkWindowsAuth.TabIndex = 7;
            this.chkWindowsAuth.Text = "Windows Authentication";
            this.toolTip1.SetToolTip(this.chkWindowsAuth, "Use current Windows credentials to connect.");
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            // 
            // lblDbUser
            // 
            this.lblDbUser.AutoSize = true;
            this.lblDbUser.Location = new System.Drawing.Point(12, 133);
            this.lblDbUser.Name = "lblDbUser";
            this.lblDbUser.Size = new System.Drawing.Size(61, 13);
            this.lblDbUser.TabIndex = 8;
            this.lblDbUser.Text = "User name:";
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(79, 130);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(150, 20);
            this.txtDbUser.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtDbUser, "SQL Server username.");
            // 
            // lblDbPassword
            // 
            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(12, 159);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(56, 13);
            this.lblDbPassword.TabIndex = 10;
            this.lblDbPassword.Text = "Password:";
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(79, 156);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(150, 20);
            this.txtDbPassword.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtDbPassword, "SQL Server password.");
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnStatus.Location = new System.Drawing.Point(236, 96);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(198, 17);
            this.lblConnStatus.TabIndex = 12;
            this.lblConnStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DatabaseSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDescription);
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
            this.Size = new System.Drawing.Size(450, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
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
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
