using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMS_Search.Forms
{
    partial class frmClipboardOptions
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCopyContent = new System.Windows.Forms.Button();
            this.btnPreserveLayout = new System.Windows.Forms.Button();
            this.chkDontAsk = new System.Windows.Forms.CheckBox();
            this.cmbScope = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();

            //
            // lblMessage
            //
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(300, 30);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Your selection has gaps. How would you like to copy?\r\nChoose \"Content Only\" to ignore gaps or \"Preserve Layout\" to include them.";

            //
            // btnCopyContent
            //
            this.btnCopyContent.Location = new System.Drawing.Point(15, 60);
            this.btnCopyContent.Name = "btnCopyContent";
            this.btnCopyContent.Size = new System.Drawing.Size(120, 35);
            this.btnCopyContent.TabIndex = 1;
            this.btnCopyContent.Text = "Copy Content Only";
            this.btnCopyContent.UseVisualStyleBackColor = true;
            this.btnCopyContent.Click += new System.EventHandler(this.btnCopyContent_Click);

            //
            // btnPreserveLayout
            //
            this.btnPreserveLayout.Location = new System.Drawing.Point(197, 60);
            this.btnPreserveLayout.Name = "btnPreserveLayout";
            this.btnPreserveLayout.Size = new System.Drawing.Size(120, 35);
            this.btnPreserveLayout.TabIndex = 2;
            this.btnPreserveLayout.Text = "Preserve Layout";
            this.btnPreserveLayout.UseVisualStyleBackColor = true;
            this.btnPreserveLayout.Click += new System.EventHandler(this.btnPreserveLayout_Click);

            //
            // chkDontAsk
            //
            this.chkDontAsk.AutoSize = true;
            this.chkDontAsk.Location = new System.Drawing.Point(15, 110);
            this.chkDontAsk.Name = "chkDontAsk";
            this.chkDontAsk.Size = new System.Drawing.Size(100, 17);
            this.chkDontAsk.TabIndex = 3;
            this.chkDontAsk.Text = "Don't ask again";
            this.chkDontAsk.UseVisualStyleBackColor = true;
            this.chkDontAsk.CheckedChanged += new System.EventHandler(this.chkDontAsk_CheckedChanged);

            //
            // cmbScope
            //
            this.cmbScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScope.FormattingEnabled = true;
            this.cmbScope.Items.AddRange(new object[] {
            "This Session",
            "Forever"});
            this.cmbScope.Location = new System.Drawing.Point(140, 108);
            this.cmbScope.Name = "cmbScope";
            this.cmbScope.Size = new System.Drawing.Size(100, 21);
            this.cmbScope.TabIndex = 4;
            this.cmbScope.SelectedIndexChanged += new System.EventHandler(this.cmbScope_SelectedIndexChanged);

            //
            // frmClipboardOptions
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 151);
            this.Controls.Add(this.cmbScope);
            this.Controls.Add(this.chkDontAsk);
            this.Controls.Add(this.btnPreserveLayout);
            this.Controls.Add(this.btnCopyContent);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClipboardOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Disjointed Selection Detected";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCopyContent;
        private System.Windows.Forms.Button btnPreserveLayout;
        private System.Windows.Forms.CheckBox chkDontAsk;
        private System.Windows.Forms.ComboBox cmbScope;
    }
}
