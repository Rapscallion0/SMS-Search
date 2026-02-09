using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Search.Forms
{
    public partial class frmToast : Form
    {
        int toastX, toastY;
        int toastTimerDuration = 300;
        private Screen _targetScreen;

        public frmToast(int type, string message, string title, Screen screen = null)
        {
            InitializeComponent();
            
            _targetScreen = screen ?? Screen.PrimaryScreen;

            lblToastMessage.Text = message;

            switch (type)
            {
                case 0:
                    lblToastType.Text = "Success";
                    pnlToastBorder.BackColor = Color.FromArgb(57, 155, 53);
                    imgToastIcon.Image = Properties.Resources.Round_Check;

                    break;
                
                case 1:
                    lblToastType.Text = "Information";
                    pnlToastBorder.BackColor = Color.FromArgb(18, 136, 191);
                    imgToastIcon.Image = Properties.Resources.Round_Info;

                    break;

                case 2:
                    lblToastType.Text = "Error";
                    pnlToastBorder.BackColor = Color.FromArgb(227, 50, 45);
                    imgToastIcon.Image = Properties.Resources.Round_X;

                    break;
                
                case 3:
                    lblToastType.Text = "Warning";
                    pnlToastBorder.BackColor = Color.FromArgb(245, 171, 35);
                    imgToastIcon.Image = Properties.Resources.Round_Exclamation;

                    break;
            }

            if (!string.IsNullOrEmpty(title) && title != "Message")
            {
                lblToastType.Text = title;
            }

            // Adjust height based on content
            int padding = 10;
            int rightMargin = 10;
            lblToastMessage.MaximumSize = new Size(this.Width - lblToastMessage.Left - rightMargin, 0);

            // Use PerformLayout to ensure AutoSize calculates the new height
            this.PerformLayout();

            int requiredHeight = lblToastMessage.Bottom + padding;
            if (requiredHeight > 60)
            {
                this.Height = requiredHeight;
                pnlToastBorder.Height = this.Height;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                // WS_EX_NOACTIVATE prevents the window from becoming the active window
                baseParams.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE

                return baseParams;
            }
        }

        private void frmToast_Load(object sender, EventArgs e)
        {
            Position();
        }

        private void Position()
        {
            Rectangle workingArea = _targetScreen.WorkingArea;

            toastX = workingArea.Right - this.Width - 5;
            toastY = workingArea.Bottom;

            this.Location = new Point(toastX, toastY);
        }

        private void toastHide_Tick(object sender, EventArgs e)
        {
            toastTimerDuration--;
            if (toastTimerDuration <= 0)
            {
                toastY += 10;
                this.Location = new Point(toastX, toastY);
                if (toastY > _targetScreen.WorkingArea.Bottom)
                {
                    tmrToastHide.Stop();
                    toastTimerDuration = 100;
                    this.Close();
                }
            }
        }

        private void frmToast_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void toastTimer_Tick(object sender, EventArgs e)
        {
            toastY -= 10;
            this.Location = new Point(toastX, toastY);

            if (toastY <= _targetScreen.WorkingArea.Bottom - this.Height - 20)
            {
                tmrToastShow.Stop();
                tmrToastHide.Start();
            }
        }
    }
}
