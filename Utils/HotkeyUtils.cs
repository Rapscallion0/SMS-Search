using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMS_Search.Utils
{
    public static class HotkeyUtils
    {
        private static readonly Keys[] RestrictedKeys = new Keys[]
        {
            Keys.Control | Keys.C,
            Keys.Control | Keys.V,
            Keys.Control | Keys.X,
            Keys.Control | Keys.Z,
            Keys.Control | Keys.A,
            Keys.Control | Keys.Y,
            Keys.Alt | Keys.F4
        };

        public static string ToString(Keys keyData)
        {
            var sb = new StringBuilder();
            var modifiers = new List<string>();

            // Check modifiers
            if ((keyData & Keys.Control) == Keys.Control) modifiers.Add("<CTRL>");
            if ((keyData & Keys.Alt) == Keys.Alt) modifiers.Add("<ALT>");
            if ((keyData & Keys.Shift) == Keys.Shift) modifiers.Add("<SHIFT>");

            // Extract key code (remove modifiers)
            Keys keyCode = keyData & ~Keys.Modifiers;

            if (modifiers.Count > 0)
            {
                sb.Append(string.Join(" + ", modifiers));
            }

            // If there is a key pressed (and it's not just a modifier key itself)
            if (keyCode != Keys.None && keyCode != Keys.ControlKey && keyCode != Keys.ShiftKey && keyCode != Keys.Menu && keyCode != Keys.LWin && keyCode != Keys.RWin)
            {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append(GetKeyDisplayName(keyCode));
            }

            return sb.ToString();
        }

        public static Keys Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Keys.None;

            // Try parsing new format
            if (input.Contains("<") || input.Contains("+"))
            {
                Keys result = Keys.None;
                string[] parts = input.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in parts)
                {
                    string p = part.Trim();
                    if (p == "<CTRL>") result |= Keys.Control;
                    else if (p == "<ALT>") result |= Keys.Alt;
                    else if (p == "<SHIFT>") result |= Keys.Shift;
                    else
                    {
                        // It's a key
                        string keyName = p.Replace("<", "").Replace(">", "");
                        if (keyName == "`") result |= Keys.Oemtilde;
                        else if (keyName.Length == 1 && char.IsDigit(keyName[0]))
                        {
                            // Handle digits 0-9
                            result |= (Keys)Enum.Parse(typeof(Keys), "D" + keyName);
                        }
                        else
                        {
                            try
                            {
                                result |= (Keys)Enum.Parse(typeof(Keys), keyName, true);
                            }
                            catch
                            {
                                // Try mapping special cases if needed
                            }
                        }
                    }
                }
                return result;
            }

            // Fallback to legacy format
            try
            {
                return (Keys)Enum.Parse(typeof(Keys), input);
            }
            catch
            {
                return Keys.None;
            }
        }

        public static bool IsValid(Keys keyData)
        {
            // Must have at least one modifier
            bool hasModifier = (keyData & Keys.Modifiers) != 0;
            if (!hasModifier) return false;

            // Must have a non-modifier key
            Keys keyCode = keyData & ~Keys.Modifiers;
            if (keyCode == Keys.None) return false;

            // Ensure the key itself is not a modifier key
            if (keyCode == Keys.ControlKey || keyCode == Keys.ShiftKey || keyCode == Keys.Menu) return false;

            return true;
        }

        public static bool IsStandard(Keys keyData)
        {
            // Exact match check
            return RestrictedKeys.Contains(keyData);
        }

        private static string GetKeyDisplayName(Keys key)
        {
            // Handle digits
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return ((int)key - (int)Keys.D0).ToString();
            }

            // Handle special keys
            switch (key)
            {
                case Keys.Oemtilde: return "`";
                case Keys.Oemcomma: return ",";
                case Keys.OemPeriod: return ".";
                case Keys.OemQuestion: return "/";
                case Keys.Oem1: return ";"; // Semicolon
                case Keys.Oem7: return "'"; // Quote
                case Keys.OemOpenBrackets: return "[";
                case Keys.OemCloseBrackets: return "]";
                case Keys.Oem5: return "\\";
                case Keys.OemMinus: return "-";
                case Keys.Oemplus: return "=";
                // Add more as needed
            }

            string name = key.ToString();

            // If it's a single letter or digit, return it
            if (name.Length == 1) return name;

            // Otherwise wrap in brackets
            return "<" + name + ">";
        }

        public static Keys GetDefaultHotkey()
        {
             return Keys.Control | Keys.Shift | Keys.Oemtilde;
        }
    }
}
