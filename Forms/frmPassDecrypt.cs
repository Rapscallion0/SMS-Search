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
            txtEncrypted.Text = GeneralUtils.Encrypt(txtDecrypted.Text);
            
            if (txtEncrypted.Text != "")
            {
                // Copy Encrypted text to Windows clipboard
                Clipboard.SetText(txtEncrypted.Text);

                // Show toast message
                GeneralUtils.showToast(1, "Encrypted string copied to clipboard.");
            }*/
            txtDecrypted.Focus();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtDecrypted.Text = Utils.GeneralUtils.Decrypt(txtEncrypted.Text);

            if (txtDecrypted.Text != "")
            {
                // Copy Encrypted text to Windows clipboard
                Clipboard.SetText(txtDecrypted.Text);

                // Show toast message
                Utils.GeneralUtils.showToast(1, "Decrypted string copied to clipboard.");
            }
            txtEncrypted.Focus();
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
