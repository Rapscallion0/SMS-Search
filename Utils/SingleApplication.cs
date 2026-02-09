using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SMS_Search.Utils
{
    /// <summary>
    /// Enforces single application instance logic using a named Mutex.
    /// Can bring the existing instance to the foreground if a new instance is started.
    /// </summary>
	public class SingleApplication
	{
		private const int SW_RESTORE = 9;
		private static Mutex mutex;
		[DllImport("user32.dll")]
		private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll")]
		private static extern int SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		private static extern int IsIconic(IntPtr hWnd);
		private static IntPtr GetCurrentInstanceWindowHandle()
		{
			IntPtr result = IntPtr.Zero;
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
			Process[] array = processesByName;
			for (int i = 0; i < array.Length; i++)
			{
				Process process = array[i];
                if (process.Id != currentProcess.Id && process.MainModule.FileName == currentProcess.MainModule.FileName && process.MainWindowHandle != IntPtr.Zero)
				{
					result = process.MainWindowHandle;
					break;
				}
			}
			return result;
		}
		private static void SwitchToCurrentInstance()
		{
			IntPtr currentInstanceWindowHandle = SingleApplication.GetCurrentInstanceWindowHandle();
			if (currentInstanceWindowHandle != IntPtr.Zero)
			{
				if (SingleApplication.IsIconic(currentInstanceWindowHandle) != 0)
				{
					SingleApplication.ShowWindow(currentInstanceWindowHandle, 9);
				}
				SingleApplication.SetForegroundWindow(currentInstanceWindowHandle);
			}
		}

        /// <summary>
        /// Runs the application if it's the only instance. Otherwise, switches to the existing instance.
        /// </summary>
        /// <param name="frmMain">The main form to run.</param>
        /// <param name="mutexName">Unique mutex name (defaults to executable name).</param>
        /// <returns>True if the application started; false if it was already running.</returns>
		public static bool Run(Form frmMain, string mutexName = null)
		{
			if (SingleApplication.IsAlreadyRunning(mutexName))
			{
				SingleApplication.SwitchToCurrentInstance();
				return false;
			}
			Application.Run(frmMain);
			return true;
		}

		public static bool Run(string mutexName = null)
		{
			return !SingleApplication.IsAlreadyRunning(mutexName);
		}

		private static bool IsAlreadyRunning(string mutexName = null)
		{
            string name = mutexName;
            if (string.IsNullOrEmpty(name))
            {
			    string location = Assembly.GetExecutingAssembly().Location;
			    FileSystemInfo fileSystemInfo = new FileInfo(location);
			    name = fileSystemInfo.Name;
            }

			bool flag;
			SingleApplication.mutex = new Mutex(true,"Global\\" + name,out flag);
			if (flag)
			{
				SingleApplication.mutex.ReleaseMutex();
			}
			return !flag;
		}
	}
}
