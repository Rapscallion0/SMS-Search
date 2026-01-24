using System;
using System.Windows.Forms;
using System.IO;
using SMS_Search; // For ConfigManager

namespace SMS_Search_Launcher
{
    public class HiddenWindow : Form
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 1;
        private ConfigManager _config;

        public HiddenWindow()
        {
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
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SMS Search.json");
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
                    }
                }
                catch
                {
                    // Invalid config, ignore
                }
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
                AppSwitcher.SwitchToOrStartApp();
            }
            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            HotKeyManager.UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosing(e);
        }
    }
}
