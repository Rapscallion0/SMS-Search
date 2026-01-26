namespace SMS_Search.Settings
{
    partial class UpdateSettings
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
            this.chkCheckUpdate = new System.Windows.Forms.CheckBox();
            this.btnChkUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // chkCheckUpdate
            //
            this.chkCheckUpdate.AutoSize = true;
            this.chkCheckUpdate.Location = new System.Drawing.Point(20, 20);
            this.chkCheckUpdate.Name = "chkCheckUpdate";
            this.chkCheckUpdate.Size = new System.Drawing.Size(222, 17);
            this.chkCheckUpdate.TabIndex = 0;
            this.chkCheckUpdate.Text = "Automatically check for update on startup";
            this.chkCheckUpdate.UseVisualStyleBackColor = true;
            //
            // btnChkUpdate
            //
            this.btnChkUpdate.Location = new System.Drawing.Point(20, 50);
            this.btnChkUpdate.Name = "btnChkUpdate";
            this.btnChkUpdate.Size = new System.Drawing.Size(140, 30);
            this.btnChkUpdate.TabIndex = 1;
            this.btnChkUpdate.Text = "Check for update now";
            this.btnChkUpdate.UseVisualStyleBackColor = true;
            //
            // UpdateSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnChkUpdate);
            this.Controls.Add(this.chkCheckUpdate);
            this.Name = "UpdateSettings";
            this.Size = new System.Drawing.Size(430, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCheckUpdate;
        private System.Windows.Forms.Button btnChkUpdate;
    }
}
