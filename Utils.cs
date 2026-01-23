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
    }
}
