namespace SMS_Search.Settings
{
    partial class LoggingSettings
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
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.lblRetention = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblLogFile = new System.Windows.Forms.Label();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.btnOpenLogFolder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
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
            this.lblDescription.Text = "Configure application logging and log retention.";
            //
            // chkLogging
            //
            this.chkLogging.AutoSize = true;
            this.chkLogging.Location = new System.Drawing.Point(20, 50);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(96, 17);
            this.chkLogging.TabIndex = 1;
            this.chkLogging.Text = "Enable logging";
            this.toolTip1.SetToolTip(this.chkLogging, "Turn on logging to file.");
            this.chkLogging.UseVisualStyleBackColor = true;
            //
            // lblLogLevel
            //
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(20, 83);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(53, 13);
            this.lblLogLevel.TabIndex = 2;
            this.lblLogLevel.Text = "Log Level:";
            //
            // cmbLogLevel
            //
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Items.AddRange(new object[] {
            "Critical",
            "Error",
            "Warning",
            "Info",
            "Debug"});
            this.cmbLogLevel.Location = new System.Drawing.Point(80, 80);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(80, 21);
            this.cmbLogLevel.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cmbLogLevel, "Minimum level of logs to record.");
            //
            // lblRetention
            //
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(180, 83);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(100, 13);
            this.lblRetention.TabIndex = 4;
            this.lblRetention.Text = "Retention (Days):";
            //
            // numRetention
            //
            this.numRetention.Location = new System.Drawing.Point(290, 81);
            this.numRetention.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numRetention.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRetention.Name = "numRetention";
            this.numRetention.Size = new System.Drawing.Size(50, 20);
            this.numRetention.TabIndex = 5;
            this.toolTip1.SetToolTip(this.numRetention, "Number of days to keep log files.");
            this.numRetention.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            //
            // lblLogFile
            //
            this.lblLogFile.AutoSize = true;
            this.lblLogFile.Location = new System.Drawing.Point(20, 120);
            this.lblLogFile.Name = "lblLogFile";
            this.lblLogFile.Size = new System.Drawing.Size(84, 13);
            this.lblLogFile.TabIndex = 6;
            this.lblLogFile.Text = "Current Log File:";
            //
            // txtLogFile
            //
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.Location = new System.Drawing.Point(20, 136);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.ReadOnly = true;
            this.txtLogFile.Size = new System.Drawing.Size(410, 20);
            this.txtLogFile.TabIndex = 7;
            //
            // btnOpenLog
            //
            this.btnOpenLog.Location = new System.Drawing.Point(20, 165);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(75, 23);
            this.btnOpenLog.TabIndex = 8;
            this.btnOpenLog.Text = "Open Log";
            this.toolTip1.SetToolTip(this.btnOpenLog, "Open the current log file.");
            this.btnOpenLog.UseVisualStyleBackColor = true;
            //
            // btnOpenLogFolder
            //
            this.btnOpenLogFolder.Location = new System.Drawing.Point(101, 165);
            this.btnOpenLogFolder.Name = "btnOpenLogFolder";
            this.btnOpenLogFolder.Size = new System.Drawing.Size(90, 23);
            this.btnOpenLogFolder.TabIndex = 9;
            this.btnOpenLogFolder.Text = "Open Folder";
            this.toolTip1.SetToolTip(this.btnOpenLogFolder, "Open the directory containing log files.");
            this.btnOpenLogFolder.UseVisualStyleBackColor = true;
            //
            // LoggingSettings
            //
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnOpenLogFolder);
            this.Controls.Add(this.btnOpenLog);
            this.Controls.Add(this.txtLogFile);
            this.Controls.Add(this.lblLogFile);
            this.Controls.Add(this.numRetention);
            this.Controls.Add(this.lblRetention);
            this.Controls.Add(this.cmbLogLevel);
            this.Controls.Add(this.lblLogLevel);
            this.Controls.Add(this.chkLogging);
            this.Controls.Add(this.lblDescription);
            this.Name = "LoggingSettings";
            this.Size = new System.Drawing.Size(450, 400);
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkLogging;
        private System.Windows.Forms.Label lblLogLevel;
        private System.Windows.Forms.ComboBox cmbLogLevel;
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.NumericUpDown numRetention;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblLogFile;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Button btnOpenLogFolder;
    }
}
