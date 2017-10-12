using Ini;
using Log;
using SMS_Search.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace SMS_Search
{
	public partial class frmUnarchive : Form
	{
		private static string ConfigFilePath = ".\\SMS Search.ini";
		private IniFile ini = new IniFile(frmUnarchive.ConfigFilePath);
		private Logfile log = new Logfile();

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();
		
        public frmUnarchive()
		{
			InitializeComponent();
		}
		
        private void frmUnarchive_Load(object sender, EventArgs e)
		{
			bool flag = false;
			StartPosition = FormStartPosition.Manual;
			Screen[] allScreens = Screen.AllScreens;
			Screen[] array = allScreens;
			for (int i = 0; i < array.Length; i++)
			{
				Screen screen = array[i];
				Rectangle rect = new Rectangle(10, 10, Width, Height);
				if (screen.WorkingArea.Contains(rect))
				{
					flag = true;
				}
			}
			if (ini.IniReadValue("UNARCHIVE", "LOCATIONY") == "" || ini.IniReadValue("UNARCHIVE", "LOCATIONX") == "" || !flag)
			{
				Top = 100;
				Left = 100;
				if (!flag)
				{
					MessageBox.Show("The Unarchive target was positioned outside the viewable area of the screen, and will been repositioned to the default location.", "Unarchived target retrieved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					log.Logger(1, "Target location reset");
				}
			}
			else
			{
				Top = Convert.ToInt32(ini.IniReadValue("UNARCHIVE", "LOCATIONY"));
				Left = Convert.ToInt32(ini.IniReadValue("UNARCHIVE", "LOCATIONX"));
			}
			log.Logger(0, "Target initialized at X: " + ini.IniReadValue("UNARCHIVE", "LOCATIONX") + "; Y: " + ini.IniReadValue("UNARCHIVE", "LOCATIONY"));
		}
		
        private void picTarget_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				frmUnarchive.ReleaseCapture();
				frmUnarchive.SendMessage(Handle, 161u, 2, 0);
			}
		}
		
        private void closeTargetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		
        private void frmUnarchive_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}
		
        private void frmUnarchive_DragDrop(object sender, DragEventArgs e)
		{
			int num = 0;
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (File.GetAttributes(text).ToString() == "Directory")
				{
					string[] files = Directory.GetFiles(text, "*.*", SearchOption.AllDirectories);
					string[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						string text2 = array3[j];
						File.SetAttributes(text2, File.GetAttributes(text2) & ~(FileAttributes.ReadOnly | FileAttributes.Archive));
						num++;
						log.Logger(0, "File unarchived: " + text2.ToString());
					}
				}
				else
				{
					File.SetAttributes(text, File.GetAttributes(text) & ~(FileAttributes.ReadOnly | FileAttributes.Archive));
					num++;
					log.Logger(0, "File unarchived: " + text.ToString());
				}
			}
		}
		
        private void frmUnarchive_FormClosing(object sender, FormClosingEventArgs e)
		{
			ini.IniWriteValue("UNARCHIVE", "LOCATIONX", Location.X.ToString());
			ini.IniWriteValue("UNARCHIVE", "LOCATIONY", Location.Y.ToString());
			log.Logger(0, "Target terminated at X: " + ini.IniReadValue("UNARCHIVE", "LOCATIONX") + "; Y: " + ini.IniReadValue("UNARCHIVE", "LOCATIONY"));
		}

	}
}
