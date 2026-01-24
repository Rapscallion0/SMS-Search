using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Search
{
    public class Utils
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

        public static string Encrypt(string sLine)
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

        public static string Decrypt(string sLine)
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
    }
}
