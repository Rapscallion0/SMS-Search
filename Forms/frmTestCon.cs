using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace SMS_Search.Forms
{
	public partial class frmTestCon : Form
	{
		public frmTestCon()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
			Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
		}
	}
}
