using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMS_Search;
using Log;

namespace SMS_Search.Settings
{
    public partial class LauncherSettings : UserControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private ConfigManager _config;
        private Logfile _log = new Logfile();
        private const string LauncherExe = "SMS Search Launcher.exe";
        private string _lastValidHotkey = "";
        private string _currentValidHotkey = "";
        private bool _isCurrentHotkeyValid = false;
        private bool _isLoaded = false;
        private ToolTip toolTip1 = new ToolTip();

        public LauncherSettings(ConfigManager config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
            UpdateLauncherStatusUI();
            WireUpEvents();
        }

        public LauncherSettings()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            LoadSettings();
            UpdateLauncherStatusUI();
        }

        private void LoadSettings()
        {
            if (_config == null) return;
            _isLoaded = false;

            string savedHotkey = _config.GetValue("LAUNCHER", "HOTKEY");
            if (string.IsNullOrEmpty(savedHotkey))
            {
                Keys defaultKey = HotkeyUtils.GetDefaultHotkey();
                _lastValidHotkey = HotkeyUtils.ToString(defaultKey);
            }
            else
            {
                Keys k = HotkeyUtils.Parse(savedHotkey);
                if (k == Keys.None)
                {
                     Keys defaultKey = HotkeyUtils.GetDefaultHotkey();
                     _lastValidHotkey = HotkeyUtils.ToString(defaultKey);
                }
                else
                {
                     _lastValidHotkey = HotkeyUtils.ToString(k);
                }
            }
            txtHotkey.Text = _lastValidHotkey;
            _currentValidHotkey = _lastValidHotkey;
            _isCurrentHotkeyValid = true;

            _isLoaded = true;
        }

        private void WireUpEvents()
        {
            txtHotkey.KeyDown += txtHotkey_KeyDown;
            txtHotkey.KeyUp += txtHotkey_KeyUp;
            txtHotkey.Leave += txtHotkey_Leave;

            btnRegister.Click += btnRegister_Click;
            btnUnregister.Click += btnUnregister_Click;
        }

        private void txtHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                 Keys def = HotkeyUtils.GetDefaultHotkey();
                 txtHotkey.Text = HotkeyUtils.ToString(def);
                 _currentValidHotkey = txtHotkey.Text;
                 _isCurrentHotkeyValid = true;
                 SaveHotkeyIfValid();
                 return;
            }

            string hotkeyStr = HotkeyUtils.ToString(e.KeyData);
            txtHotkey.Text = hotkeyStr;
            txtHotkey.SelectionStart = txtHotkey.Text.Length;

            if (HotkeyUtils.IsValid(e.KeyData))
            {
                if (HotkeyUtils.IsStandard(e.KeyData))
                {
                    txtHotkey.BackColor = Color.MistyRose;
                    _isCurrentHotkeyValid = false;
                    toolTip1.SetToolTip(txtHotkey, "This shortcut is a standard system shortcut and cannot be used.");
                }
                else
                {
                    if (CheckHotkeyAvailability(e.KeyData))
                    {
                        txtHotkey.BackColor = SystemColors.Window;
                        _currentValidHotkey = hotkeyStr;
                        _isCurrentHotkeyValid = true;
                        toolTip1.SetToolTip(txtHotkey, "");
                    }
                    else
                    {
                        txtHotkey.BackColor = Color.MistyRose;
                        _isCurrentHotkeyValid = false;
                         toolTip1.SetToolTip(txtHotkey, "This shortcut is already in use by another application.");
                    }
                }
            }
            else
            {
                txtHotkey.BackColor = SystemColors.Window;
                _isCurrentHotkeyValid = false;
            }
        }

        private void txtHotkey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.None || (e.KeyData & Keys.Modifiers) == Keys.None)
            {
                if (!_isCurrentHotkeyValid)
                {
                    txtHotkey.Text = _lastValidHotkey;
                    _currentValidHotkey = _lastValidHotkey;
                    _isCurrentHotkeyValid = true;
                    txtHotkey.BackColor = SystemColors.Window;
                }
                else
                {
                    _lastValidHotkey = _currentValidHotkey;
                    SaveHotkeyIfValid();
                }
            }
        }

        private void txtHotkey_Leave(object sender, EventArgs e)
        {
             if (!_isCurrentHotkeyValid)
            {
                txtHotkey.Text = _lastValidHotkey;
                _currentValidHotkey = _lastValidHotkey;
                _isCurrentHotkeyValid = true;
                txtHotkey.BackColor = SystemColors.Window;
            }
            SaveHotkeyIfValid();
        }

        private void SaveHotkeyIfValid()
        {
            if (_isCurrentHotkeyValid && _isLoaded && _config != null)
            {
                 _config.SetValue("LAUNCHER", "HOTKEY", txtHotkey.Text);
                 _config.Save();
            }
        }

        private bool CheckHotkeyAvailability(Keys keyData)
        {
            string saved = _config.GetValue("LAUNCHER", "HOTKEY");
            if (!string.IsNullOrEmpty(saved))
            {
                Keys savedKey = HotkeyUtils.Parse(saved);
                if (savedKey == keyData) return true;
            }

            int id = 0x9000;
            uint modifiers = 0;
            if ((keyData & Keys.Control) == Keys.Control) modifiers |= 0x0002;
            if ((keyData & Keys.Alt) == Keys.Alt) modifiers |= 0x0001;
            if ((keyData & Keys.Shift) == Keys.Shift) modifiers |= 0x0004;

            uint vk = (uint)(keyData & ~Keys.Modifiers);

            bool success = RegisterHotKey(this.Handle, id, modifiers, vk);
            if (success)
            {
                UnregisterHotKey(this.Handle, id);
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                _log.Logger(LogLevel.Info, "Registering Launcher service");
                Cursor = Cursors.WaitCursor;
                lblLauncherStatus.Text = "Status: Registering...";
                DrawStatusLight(Color.Orange);
                btnRegister.Enabled = false;
                btnUnregister.Enabled = false;

                await Task.Run(() =>
                {
                    KillLauncher();
                    ExtractLauncher();
                    CreateStartupShortcut();
                    StartLauncher();
                });
            }
            catch (Exception ex)
            {
                _log.Logger(LogLevel.Error, "Error registering launcher: " + ex.Message);
                MessageBox.Show("Error registering launcher: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateLauncherStatusUI();
                Cursor = Cursors.Default;
            }
        }

        private async void btnUnregister_Click(object sender, EventArgs e)
        {
            try
            {
                _log.Logger(LogLevel.Info, "Unregistering Launcher service");
                Cursor = Cursors.WaitCursor;
                lblLauncherStatus.Text = "Status: Unregistering...";
                DrawStatusLight(Color.Orange);
                btnRegister.Enabled = false;
                btnUnregister.Enabled = false;

                await Task.Run(() =>
                {
                    RemoveStartupShortcut();
                    KillLauncher();
                    DeleteLauncher();
                });
            }
            catch (Exception ex)
            {
                _log.Logger(LogLevel.Error, "Error unregistering launcher: " + ex.Message);
                MessageBox.Show("Error unregistering launcher: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateLauncherStatusUI();
                Cursor = Cursors.Default;
            }
        }

        private void ExtractLauncher()
        {
            _log.Logger(LogLevel.Info, "Extracting Launcher executable");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly(); // Note: This might be SMS_Search.dll/exe
                // The resource name might depend on namespace. "SMS_Search.Resources.SMS Search Launcher.exe"
                // But previously `GetExecutingAssembly` was `SMS_Search` (frmConfig).
                // Now `LauncherSettings` is in `SMS_Search.Settings`. Executing assembly should be same (project).

                string resourceName = "SMS_Search.Resources.SMS Search Launcher.exe";

                var resources = assembly.GetManifestResourceNames();
                var foundResource = Array.Find(resources, r => r.EndsWith("SMS Search Launcher.exe", StringComparison.OrdinalIgnoreCase));

                if (foundResource != null)
                {
                    resourceName = foundResource;
                }
                else
                {
                    string availableResources = string.Join(Environment.NewLine, resources);
                    throw new Exception("Embedded launcher resource not found.\nAvailable resources:\n" + availableResources);
                }

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        throw new Exception("Embedded launcher resource stream is null for '" + resourceName + "'.");
                    }
                    using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to extract launcher: " + ex.Message);
            }
        }

        private void DeleteLauncher()
        {
            _log.Logger(LogLevel.Info, "Deleting Launcher executable");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            if (File.Exists(targetPath))
            {
                int retries = 5;
                while (retries >= 0)
                {
                    try
                    {
                        File.Delete(targetPath);
                        return;
                    }
                    catch (Exception ex)
                    {
                        bool isRetryable = ex is IOException || ex is UnauthorizedAccessException;
                        if (retries == 0 || !isRetryable)
                        {
                            throw new Exception("Failed to delete launcher executable: " + ex.Message);
                        }
                        retries--;
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
        }

        private void CreateStartupShortcut()
        {
            _log.Logger(LogLevel.Info, "Creating Startup shortcut");
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);

            if (!File.Exists(targetPath))
            {
                throw new FileNotFoundException("Launcher executable not found at: " + targetPath);
            }

            string script = String.Format("$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('{0}'); $Shortcut.TargetPath = '{1}'; $Shortcut.WorkingDirectory = '{2}'; $Shortcut.Save()",
                shortcutPath, targetPath, Application.StartupPath);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell.exe";
            psi.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + script + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            using (Process p = Process.Start(psi))
            {
                p.WaitForExit();
            }
        }

        private void RemoveStartupShortcut()
        {
            _log.Logger(LogLevel.Info, "Removing Startup shortcut");
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");

            if (File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
            }
        }

        private void StartLauncher()
        {
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);
            if (File.Exists(targetPath))
            {
                if (!IsLauncherRunning())
                {
                    _log.Logger(LogLevel.Info, "Starting Launcher");
                    Process.Start(targetPath);
                }
            }
        }

        private void KillLauncher()
        {
            foreach (var name in new[] { "Launcher", "SMS Search Launcher" })
            {
                Process[] processes = Process.GetProcessesByName(name);
                foreach (Process p in processes)
                {
                    try
                    {
                        _log.Logger(LogLevel.Info, "Killing process: " + name);
                        p.Kill();
                        p.WaitForExit(5000);
                    }
                    catch { }
                }
            }
        }

        private bool IsLauncherRunning()
        {
            return Process.GetProcessesByName("Launcher").Length > 0 || Process.GetProcessesByName("SMS Search Launcher").Length > 0;
        }

        private void ReloadLauncherIfRunning()
        {
            if (IsLauncherRunning())
            {
                KillLauncher();
                StartLauncher();
            }
        }

        private void DrawStatusLight(bool isGreen)
        {
            DrawStatusLight(isGreen ? Color.Green : Color.Red);
        }

        private void DrawStatusLight(Color c)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(c))
                {
                    g.FillEllipse(b, 1, 1, 14, 14);
                }
            }
            if (pbLauncherStatus.Image != null) pbLauncherStatus.Image.Dispose();
            pbLauncherStatus.Image = bmp;
        }

        private void UpdateLauncherStatusUI()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            string targetPath = Path.Combine(Application.StartupPath, LauncherExe);

            bool isRegistered = File.Exists(shortcutPath) && File.Exists(targetPath);

            DrawStatusLight(isRegistered);

            if (isRegistered)
            {
                 btnRegister.Enabled = false;
                 btnUnregister.Enabled = true;

                 if (IsLauncherRunning())
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Running";
                 }
                 else
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Stopped";
                 }
            }
            else
            {
                 lblLauncherStatus.Text = "Status: Not Registered";
                 btnRegister.Enabled = true;
                 btnUnregister.Enabled = false;
            }
        }
    }
}
