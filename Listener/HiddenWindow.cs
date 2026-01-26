using System;
using System.Windows.Forms;
using System.IO;
using SMS_Search; // For ConfigManager
using Log;

namespace SMS_Search.Listener
{
    public class HiddenWindow : Form
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 1;
        private ConfigManager _config;
        private Logfile log = new Logfile("Listener"); // Changed log name

        public HiddenWindow()
        {
            // Set title for identification
            this.Text = "SMSSearch Listener Service";

            // Hide the form completely
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = System.Drawing.Size.Empty;

            // Load Config
            LoadConfigAndRegisterHotKey();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Prevent window from becoming visible
            base.SetVisibleCore(false);
        }

        private void LoadConfigAndRegisterHotKey()
        {
            // Updated to use standardized filename
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SMSSearch_settings.json");
            _config = new ConfigManager(configPath);

            string hotkeyStr = _config.GetValue("LAUNCHER", "HOTKEY");
            if (!string.IsNullOrEmpty(hotkeyStr))
            {
                try
                {
                    var keyData = HotkeyUtils.Parse(hotkeyStr);
                    if (keyData != Keys.None)
                    {
                        RegisterHotKeyFromKeys(keyData);
                        log.Logger(LogLevel.Info, "Hotkey registered: " + hotkeyStr);
                    }
                    else
                    {
                         log.Logger(LogLevel.Warning, "Parsed hotkey is None for: " + hotkeyStr);
                    }
                }
                catch (Exception ex)
                {
                    log.Logger(LogLevel.Error, "Error parsing hotkey config: " + ex.Message);
                }
            }
            else
            {
                log.Logger(LogLevel.Warning, "No hotkey configured in LAUNCHER section");
            }
        }

        private void RegisterHotKeyFromKeys(Keys keyData)
        {
            HotKeyManager.KeyModifiers modifiers = HotKeyManager.KeyModifiers.None;

            if ((keyData & Keys.Control) == Keys.Control)
                modifiers |= HotKeyManager.KeyModifiers.Control;
            if ((keyData & Keys.Alt) == Keys.Alt)
                modifiers |= HotKeyManager.KeyModifiers.Alt;
            if ((keyData & Keys.Shift) == Keys.Shift)
                modifiers |= HotKeyManager.KeyModifiers.Shift;

            // Extract the actual key (remove modifiers)
            Keys key = keyData & ~Keys.Modifiers;

            HotKeyManager.RegisterHotKey(this.Handle, HOTKEY_ID, modifiers, key);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                log.Logger(LogLevel.Info, "Hotkey triggered");
                AppSwitcher.SwitchToOrStartApp();
            }
            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            log.Logger(LogLevel.Info, "Unregistering hotkey and closing hidden window");
            HotKeyManager.UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosing(e);
        }
    }
}
