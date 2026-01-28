using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace SMS_Search
{
    partial class frmUnarchive
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
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        //private IContainer components;
        private PictureBox picTarget;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem closeTargetToolStripMenuItem;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUnarchive));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picTarget = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeTargetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 26);
            // 
            // closeTargetToolStripMenuItem
            // 
            this.closeTargetToolStripMenuItem.Name = "closeTargetToolStripMenuItem";
            this.closeTargetToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.closeTargetToolStripMenuItem.Text = "Close Target";
            this.closeTargetToolStripMenuItem.Click += new System.EventHandler(this.closeTargetToolStripMenuItem_Click);
            // 
            // picTarget
            // 
            this.picTarget.BackColor = System.Drawing.Color.Transparent;
            this.picTarget.ContextMenuStrip = this.contextMenuStrip1;
            this.picTarget.Image = ((System.Drawing.Image)(resources.GetObject("picTarget.Image")));
            this.picTarget.Location = new System.Drawing.Point(0, 0);
            this.picTarget.Name = "picTarget";
            this.picTarget.Size = new System.Drawing.Size(117, 116);
            this.picTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTarget.TabIndex = 0;
            this.picTarget.TabStop = false;
            this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseDown);
            // 
            // frmUnarchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.DarkRed;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(117, 117);
            this.ControlBox = false;
            this.Controls.Add(this.picTarget);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUnarchive";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUnarchive_FormClosing);
            this.Load += new System.EventHandler(this.frmUnarchive_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmUnarchive_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmUnarchive_DragEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTarget)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
