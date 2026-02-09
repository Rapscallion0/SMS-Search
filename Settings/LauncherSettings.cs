using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMS_Search.Utils;
using SMS_Search.Utils;
using System.Management;

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
        private const string LegacyLauncherExe = "SMSSearchLauncher.exe";
        private string _lastValidHotkey = "";
        private string _currentValidHotkey = "";
        private bool _isCurrentHotkeyValid = false;
        private bool _isLoaded = false;

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                CheckAndMigrateLegacy();
            }
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

        private void CheckAndMigrateLegacy()
        {
            string legacyPath = Path.Combine(Application.StartupPath, LegacyLauncherExe);
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string oldShortcut = Path.Combine(startupFolder, "SMS Search Launcher.lnk"); // Very old
            // string legacyShortcut = Path.Combine(startupFolder, "SMSSearchLauncher.lnk"); // Recent old - matches current new shortcut name!

            bool legacyExists = File.Exists(legacyPath) || File.Exists(oldShortcut);

            if (legacyExists)
            {
                _log.Logger(LogLevel.Info, "Legacy launcher detected. Migrating...");
                // We run unregister (cleans up legacy) then register (sets up new)
                // We do this async fire-and-forget or just invoke register button logic?
                // Invoke Register logic but need to be careful about UI thread.
                // Since this is Constructor/Load, we should probably do it carefully.
                // Let's defer to a method we can await or run on load.

                // For now, we will just queue it up.
                this.BeginInvoke(new Action(async () => {
                     await PerformMigration();
                }));
            }
        }

        private async Task PerformMigration()
        {
             try
             {
                 _log.Logger(LogLevel.Info, "Starting migration of legacy launcher...");
                 lblLauncherStatus.Text = "Status: Migrating...";
                 DrawStatusLight(Color.Orange);
                 btnRegister.Enabled = false;
                 btnUnregister.Enabled = false;

                 await Task.Run(async () =>
                 {
                     KillLauncher(); // Kills legacy and new
                     await DeleteLegacyLauncherAsync(); // Deletes legacy exe
                     RemoveStartupShortcut(); // Removes all shortcuts

                     // Now Register New
                     CreateStartupShortcut();
                     await StartLauncherAsync();
                 });
             }
             catch (Exception ex)
             {
                 _log.Logger(LogLevel.Error, "Migration failed: " + ex.Message);
             }
             finally
             {
                 UpdateLauncherStatusUI();
             }
        }

        private void WireUpEvents()
        {
            txtHotkey.KeyDown += txtHotkey_KeyDown;
            txtHotkey.KeyUp += txtHotkey_KeyUp;
            txtHotkey.Leave += txtHotkey_Leave;

            btnRegister.Click += btnRegister_Click;
            btnUnregister.Click += btnUnregister_Click;
        }

        private async void txtHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                 Keys def = HotkeyUtils.GetDefaultHotkey();
                 txtHotkey.Text = HotkeyUtils.ToString(def);
                 _currentValidHotkey = txtHotkey.Text;
                 _isCurrentHotkeyValid = true;
                 await SaveHotkeyIfValidAsync();
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

        private async void txtHotkey_KeyUp(object sender, KeyEventArgs e)
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
                    await SaveHotkeyIfValidAsync();
                }
            }
        }

        private async void txtHotkey_Leave(object sender, EventArgs e)
        {
             if (!_isCurrentHotkeyValid)
            {
                txtHotkey.Text = _lastValidHotkey;
                _currentValidHotkey = _lastValidHotkey;
                _isCurrentHotkeyValid = true;
                txtHotkey.BackColor = SystemColors.Window;
            }
            await SaveHotkeyIfValidAsync();
        }

        private async Task SaveHotkeyIfValidAsync()
        {
            if (_isCurrentHotkeyValid && _isLoaded && _config != null)
            {
                 _config.SetValue("LAUNCHER", "HOTKEY", txtHotkey.Text);
                 _config.Save();

                 // If running, restart to pick up new hotkey
                 await ReloadLauncherIfRunningAsync();
                 (this.ParentForm as frmConfig)?.FlashSaved();
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

                await Task.Run(async () =>
                {
                    KillLauncher();
                    CreateStartupShortcut();
                    await StartLauncherAsync();
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

                await Task.Run(async () =>
                {
                    RemoveStartupShortcut();
                    KillLauncher();
                    await DeleteLegacyLauncherAsync();
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

        private async Task DeleteLegacyLauncherAsync()
        {
            _log.Logger(LogLevel.Info, "Checking for legacy Launcher executable");
            string targetPath = Path.Combine(Application.StartupPath, LegacyLauncherExe);
            if (File.Exists(targetPath))
            {
                _log.Logger(LogLevel.Info, "Deleting legacy Launcher executable");
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
                            _log.Logger(LogLevel.Warning, "Failed to delete legacy launcher: " + ex.Message);
                            return;
                        }
                        retries--;
                        await Task.Delay(200);
                    }
                }
            }
        }

        private void CreateStartupShortcut()
        {
            _log.Logger(LogLevel.Info, "Creating Startup shortcut");
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "SMSSearchLauncher.lnk");
            string targetPath = Application.ExecutablePath; // Point to SELF

            // Cleanup old shortcut if exists
            string oldShortcut = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            if (File.Exists(oldShortcut))
            {
                try { File.Delete(oldShortcut); } catch { }
            }

            // Cleanup legacy shortcut
             string legacyShortcut = Path.Combine(startupFolder, "SMSSearchLauncher.lnk");
             // Actually this is the SAME name as the new one.
             // We will overwrite it.

            string script = String.Format("$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('{0}'); $Shortcut.TargetPath = '{1}'; $Shortcut.Arguments = '--listener'; $Shortcut.WorkingDirectory = '{2}'; $Shortcut.Save()",
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

            string shortcutPath = Path.Combine(startupFolder, "SMSSearchLauncher.lnk");
            if (File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
            }

            // Cleanup old shortcut
            string oldShortcut = Path.Combine(startupFolder, "SMS Search Launcher.lnk");
            if (File.Exists(oldShortcut))
            {
                File.Delete(oldShortcut);
            }
        }

        private async Task StartLauncherAsync()
        {
            string targetPath = Application.ExecutablePath;
            if (!IsLauncherRunning())
            {
                _log.Logger(LogLevel.Info, "Starting Launcher");
                Process.Start(targetPath, "--listener");

                // Wait for it to start
                int retries = 20; // 2 seconds
                while (retries > 0)
                {
                    if (IsLauncherRunning()) break;
                    await Task.Delay(100);
                    retries--;
                }
            }
        }

        private void KillLauncher()
        {
            // Kill legacy
            foreach (var name in new[] { "Launcher", "SMS Search Launcher", "SMSSearchLauncher" })
            {
                foreach (Process p in Process.GetProcessesByName(name))
                {
                    try
                    {
                         _log.Logger(LogLevel.Info, "Killing legacy process: " + name);
                         p.Kill();
                         p.WaitForExit(5000);
                    }
                    catch { }
                }
            }

            // Kill new listener
            try
            {
                // We need to identify SMSSearch.exe running with --listener
                using (var searcher = new ManagementObjectSearcher("SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name LIKE 'SMSSearch%'"))
                using (var objects = searcher.Get())
                {
                    foreach (var obj in objects)
                    {
                        string cmd = obj["CommandLine"]?.ToString();
                        if (cmd != null && cmd.Contains("--listener"))
                        {
                            int pid = Convert.ToInt32(obj["ProcessId"]);
                            try
                            {
                                Process p = Process.GetProcessById(pid);
                                _log.Logger(LogLevel.Info, "Killing listener process: " + pid);
                                p.Kill();
                                p.WaitForExit(5000);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 _log.Logger(LogLevel.Error, "Error killing listener: " + ex.Message);
            }
        }

        private bool IsLauncherRunning()
        {
             // Check Mutex
            try
            {
                // Note: Mutex.OpenExisting requires the exact name used in Program.cs which was "Global\SMSSearchListener" (prepend Global\ explicitly there? No, passed "SMSSearchListener" -> "Global\SMSSearchListener")
                // SingleApplication prepends Global\.
                // So the mutex name is "Global\SMSSearchListener".
                using (var mutex = System.Threading.Mutex.OpenExisting("Global\\SMSSearchListener"))
                {
                    return true;
                }
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task ReloadLauncherIfRunningAsync()
        {
            if (IsLauncherRunning())
            {
                KillLauncher();
                await StartLauncherAsync();
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
            string shortcutPath = Path.Combine(startupFolder, "SMSSearchLauncher.lnk");

            // Check if registered: Shortcut must exist.
            // AND target of shortcut should be us?
            // Simplified check: just shortcut existence + isListener running?

            bool isRegistered = File.Exists(shortcutPath);
            bool isRunning = IsLauncherRunning();

            if (isRegistered)
            {
                 btnRegister.Enabled = false;
                 btnUnregister.Enabled = true;
                 lblWarning.Visible = false;

                 if (isRunning)
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Running";
                      DrawStatusLight(Color.Green);
                 }
                 else
                 {
                      lblLauncherStatus.Text = "Status: Registered - Service Stopped";
                      DrawStatusLight(Color.Orange);
                 }
            }
            else
            {
                 lblLauncherStatus.Text = "Status: Not Registered";
                 DrawStatusLight(Color.Red);
                 btnRegister.Enabled = true;
                 btnUnregister.Enabled = false;
                 lblWarning.Visible = true;
            }
        }
    }
}
