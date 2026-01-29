using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SMS_Search
{
    public class SearchTextBox : UserControl
    {
        private TextBox txtValue;
        private Button btnClear;

        public SearchTextBox()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        private void InitializeComponent()
        {
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // btnClear
            //
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(180, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(20, 20);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "";
            this.btnClear.BackgroundImage = SMS_Search.Properties.Resources.Round_X;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClear.BackColor = System.Drawing.SystemColors.Window;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.TabStop = false;
            //
            // txtValue
            //
            this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Location = new System.Drawing.Point(2, 2);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(178, 16);
            this.txtValue.TabIndex = 0;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValue_KeyDown);
            //
            // SearchTextBox
            //
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnClear);
            this.Name = "SearchTextBox";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(200, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            btnClear.Visible = !string.IsNullOrEmpty(txtValue.Text);
            OnTextChanged(e);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
            txtValue.Focus();
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            txtValue.Focus();
        }
    }
}
