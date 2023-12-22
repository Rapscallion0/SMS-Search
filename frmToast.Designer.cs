namespace SMS_Search
{
    partial class frmToast
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlToastBorder = new System.Windows.Forms.Panel();
            this.imgToastIcon = new System.Windows.Forms.PictureBox();
            this.lblToastType = new System.Windows.Forms.Label();
            this.lblToastMessage = new System.Windows.Forms.Label();
            this.tmrToastShow = new System.Windows.Forms.Timer(this.components);
            this.tmrToastHide = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imgToastIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlToastBorder
            // 
            this.pnlToastBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(155)))), ((int)(((byte)(53)))));
            this.pnlToastBorder.Location = new System.Drawing.Point(-6, 0);
            this.pnlToastBorder.Name = "pnlToastBorder";
            this.pnlToastBorder.Size = new System.Drawing.Size(10, 60);
            this.pnlToastBorder.TabIndex = 0;
            // 
            // imgToastIcon
            // 
            this.imgToastIcon.Image = global::SMS_Search.Properties.Resources.Round_Check;
            this.imgToastIcon.Location = new System.Drawing.Point(22, 15);
            this.imgToastIcon.Name = "imgToastIcon";
            this.imgToastIcon.Size = new System.Drawing.Size(25, 25);
            this.imgToastIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgToastIcon.TabIndex = 0;
            this.imgToastIcon.TabStop = false;
            // 
            // lblToastType
            // 
            this.lblToastType.AutoSize = true;
            this.lblToastType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToastType.Location = new System.Drawing.Point(64, 8);
            this.lblToastType.Name = "lblToastType";
            this.lblToastType.Size = new System.Drawing.Size(69, 17);
            this.lblToastType.TabIndex = 1;
            this.lblToastType.Text = "Toast Title";
            // 
            // lblToastMessage
            // 
            this.lblToastMessage.AutoSize = true;
            this.lblToastMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToastMessage.Location = new System.Drawing.Point(64, 31);
            this.lblToastMessage.Name = "lblToastMessage";
            this.lblToastMessage.Size = new System.Drawing.Size(82, 13);
            this.lblToastMessage.TabIndex = 2;
            this.lblToastMessage.Text = "Toast Message";
            // 
            // tmrToastShow
            // 
            this.tmrToastShow.Enabled = true;
            this.tmrToastShow.Interval = 10;
            this.tmrToastShow.Tick += new System.EventHandler(this.toastTimer_Tick);
            // 
            // tmrToastHide
            // 
            this.tmrToastHide.Interval = 10;
            this.tmrToastHide.Tick += new System.EventHandler(this.toastHide_Tick);
            // 
            // frmToast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 60);
            this.Controls.Add(this.lblToastMessage);
            this.Controls.Add(this.lblToastType);
            this.Controls.Add(this.imgToastIcon);
            this.Controls.Add(this.pnlToastBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmToast";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmToast";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmToast_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmToast_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.imgToastIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlToastBorder;
        private System.Windows.Forms.PictureBox imgToastIcon;
        private System.Windows.Forms.Label lblToastType;
        private System.Windows.Forms.Label lblToastMessage;
        private System.Windows.Forms.Timer tmrToastShow;
        private System.Windows.Forms.Timer tmrToastHide;
    }
}