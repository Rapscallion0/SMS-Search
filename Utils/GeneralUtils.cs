using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using SMS_Search.Forms;

namespace SMS_Search.Utils
{
    public class GeneralUtils
    {
        /// <summary>
        /// Display a toast message
        /// </summary>
        /// <param name="type"><br />
        /// 0: Success <br />
        /// 1: Information <br />
        /// 2: Error <br />
        /// 3: Warning</param>
        /// <param name="message">Message detail</param>
        /// <param name="title">Optional - Override default type title</param>
        /// <param name="screen">Optional - Target screen for the toast</param>
        public static void showToast(int type, string message, string title = "Message", Screen screen = null)
        { 
            if (screen == null)
            {
                if (Form.ActiveForm != null)
                {
                    screen = Screen.FromControl(Form.ActiveForm);
                }
                else
                {
                    screen = Screen.FromPoint(Cursor.Position);
                }
            }

            frmToast toast = new frmToast(type, message, title, screen);
            toast.Show();
        }

        /// <summary>
        /// Encrypts a string using Windows Data Protection (current user scope).
        /// </summary>
        /// <returns>Base64 encoded encrypted string.</returns>
        public static string Encrypt(string sLine)
        {
            if (string.IsNullOrEmpty(sLine)) return "";
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(sLine);
                byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(encrypted);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Decrypts a string using Windows Data Protection.
        /// </summary>
        public static string Decrypt(string sLine)
        {
            if (string.IsNullOrEmpty(sLine)) return "";

            // Legacy check: Old encryption started with #. Force re-entry.
            if (sLine.StartsWith("#")) return "";

            try
            {
                byte[] data = Convert.FromBase64String(sLine);
                byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                // Decryption failed (invalid format or different user/machine)
                return "";
            }
        }
    }
}
