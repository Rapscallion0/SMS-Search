// using Ini;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace SMS_Search.Forms
{
    /// <summary>
    /// Form that displays the End-User License Agreement (EULA) and requires acceptance.
    /// </summary>
	public class frmEula : Form
	{
		private IContainer components = null;
		private Button btnOk;
		private Button btnCancel;
		private CheckBox chkAgree;
		private RichTextBox txtEula;
		private Label label1;
		private Label label2;
		private Panel panel1;
		private static string ConfigFilePath = Path.Combine(Application.StartupPath, "SMSSearch_settings.json");
		private Utils.ConfigManager config = new Utils.ConfigManager(frmEula.ConfigFilePath);
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
                components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            btnOk = new Button();
            btnCancel = new Button();
            chkAgree = new CheckBox();
            txtEula = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            panel1.SuspendLayout();
			base.SuspendLayout();
            btnOk.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btnOk.Location = new Point(247, 241);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "&OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btnCancel.Location = new Point(328, 241);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            chkAgree.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            chkAgree.AutoSize = true;
            chkAgree.Location = new Point(12, 217);
            chkAgree.Name = "chkAgree";
            chkAgree.Size = new Size(234, 17);
            chkAgree.TabIndex = 3;
            chkAgree.Text = "I accept the terms in the License Agreement";
            chkAgree.UseVisualStyleBackColor = true;
            chkAgree.CheckedChanged += new EventHandler(chkAgree_CheckedChanged);
            txtEula.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            txtEula.BackColor = SystemColors.Window;
            txtEula.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEula.Location = new Point(12, 14);
            txtEula.Name = "txtEula";
            txtEula.ReadOnly = true;
            txtEula.Size = new Size(392, 117);
            txtEula.TabIndex = 4;
            txtEula.Text = "";
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(210, 16);
            label1.TabIndex = 5;
            label1.Text = "End-User License Agreement";
            label2.AutoSize = true;
            label2.Location = new Point(28, 30);
            label2.Name = "label2";
            label2.Size = new Size(256, 13);
            label2.TabIndex = 6;
            label2.Text = "Please read the following license agreement carefully";
            panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(txtEula);
            panel1.Location = new Point(-3, 55);
            panel1.Name = "panel1";
            panel1.Size = new Size(421, 149);
            panel1.TabIndex = 7;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(415, 276);
			base.ControlBox = false;
			base.Controls.Add(panel1);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.Controls.Add(chkAgree);
			base.Controls.Add(btnCancel);
			base.Controls.Add(btnOk);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "frmEula";
			base.StartPosition = FormStartPosition.CenterParent;
            Text = "SMS Search - End User License Agreement";
			base.TopMost = true;
			base.Load += new EventHandler(frmEula_Load);
            panel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public frmEula()
		{
            InitializeComponent();
		}
		private void frmEula_Load(object sender, EventArgs e)
		{
            btnOk.Enabled = false;
            txtEula.Text = "There is no warranty for the program, to the extent permitted by applicable law. Except when otherwise stated in writing, the copyright holders and/or other parties provide the program “AS IS” without warranty of any kind, either expressed or implied, including, but not limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.";
		}
		private void btnOk_Click(object sender, EventArgs e)
		{
            config.SetValue("GENERAL", "EULA", "1");
            config.Save();
			base.Close();
		}
		private void chkAgree_CheckedChanged(object sender, EventArgs e)
		{
			if (chkAgree.Checked)
			{
                btnOk.Enabled = true;
				return;
			}
            btnOk.Enabled = false;
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
			Application.Exit();
		}
	}
}
