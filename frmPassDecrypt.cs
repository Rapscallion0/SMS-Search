using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Search
{
    public partial class frmPassDecrypt : Form
    {
        public frmPassDecrypt()
        {
            InitializeComponent();

            this.ActiveControl = txtEncrypted;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Encryption feature is not yet available... Stay tuned!!","Feature not available",MessageBoxButtons.OK,MessageBoxIcon.Information);

            // To be enabled once Encryption is figured out
            /*
            txtEncrypted.Text = SmallEncrypt(txtDecrypted.Text);
            
            if (txtEncrypted.Text != "")
            {
                // Copy Encrypted text to Windows clipboard
                Clipboard.SetText(txtEncrypted.Text);

                // Show toast message
                Utils.showToast(1, "Encrypted string copied to clipboard.");
            }*/
            txtDecrypted.Focus();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtDecrypted.Text = SmallDecrypt(txtEncrypted.Text);

            if (txtDecrypted.Text != "")
            {
                // Copy Encrypted text to Windows clipboard
                Clipboard.SetText(txtDecrypted.Text);

                // Show toast message
                Utils.showToast(1, "Decrypted string copied to clipboard.");
            }
            txtEncrypted.Focus();
        }

        public static string SmallEncrypt(string sLine)
        {
            // Return an empty string for empty or null inputs
            if (string.IsNullOrEmpty(sLine))
                return "";

            Encoding encoding = Encoding.GetEncoding("Windows-1252");

            byte[] bytes = encoding.GetBytes(sLine);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(~bytes[i]); // Apply bitwise NOT
            }

            // Prefix the result with '#' for non-empty inputs
            return "#" + encoding.GetString(bytes);
        }

        public static string SmallDecrypt(string sLine)
        {
            if (string.IsNullOrEmpty(sLine) || sLine[0] != '#')
                return sLine;

            Encoding encoding = Encoding.GetEncoding("Windows-1252");

            byte[] bytes = encoding.GetBytes(sLine.Substring(1));
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(~bytes[i]); // Apply bitwise NOT
            }

            return encoding.GetString(bytes);
        }


        private void txtDecrypted_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEncrypt.PerformClick();
            }
        }

        private void txtEncrypted_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDecrypt.PerformClick();
            }
        }
    }
}
