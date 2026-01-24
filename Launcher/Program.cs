using System;
using System.Windows.Forms;
using System.IO;
using SMS_Search;

namespace SMS_Search_Launcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // We do not show a form. We run a hidden context.
            // Using a hidden window to handle messages.
            Application.Run(new HiddenWindow());
        }
    }
}
