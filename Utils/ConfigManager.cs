using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace SMS_Search.Utils
{
    public class ConfigManager
    {
        private string _jsonFilePath;
        private string _iniFilePath;
        private Dictionary<string, Dictionary<string, string>> _config;

        // P/Invoke for reading INI files
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, int nSize, string lpFileName);

        public ConfigManager(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            string directory = Path.GetDirectoryName(_jsonFilePath);
            // Default to "SMSSearch.ini" if directory is empty (current dir)
            if (string.IsNullOrEmpty(directory))
            {
                _iniFilePath = "SMSSearch.ini";
            }
            else
            {
                _iniFilePath = Path.Combine(directory, "SMSSearch.ini");
            }

            string dir = directory;
            if (string.IsNullOrEmpty(dir)) dir = AppDomain.CurrentDomain.BaseDirectory;

            // Migrate legacy INI if exists
            string legacyIniName = "SMS Search.ini";
            string legacyIniPath = Path.Combine(dir, legacyIniName);
            if (File.Exists(legacyIniPath) && !File.Exists(_iniFilePath))
            {
                try { File.Move(legacyIniPath, _iniFilePath); } catch { }
            }

            _config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            string currentJsonName = Path.GetFileName(_jsonFilePath);

            // Migration from SMS Search_settings.json (previous version) to SMSSearch_settings.json (current)
            string prevSettingsName = "SMS Search_settings.json";
            if (!string.Equals(prevSettingsName, currentJsonName, StringComparison.OrdinalIgnoreCase))
            {
                string prevPath = Path.Combine(dir, prevSettingsName);
                if (File.Exists(prevPath) && !File.Exists(_jsonFilePath))
                {
                    try
                    {
                        File.Move(prevPath, _jsonFilePath);
                    }
                    catch { }
                }
            }

            // Migration from SMS Search.json to SMSSearch_settings.json
            string legacyJsonName = "SMS Search.json";
            if (!string.Equals(legacyJsonName, currentJsonName, StringComparison.OrdinalIgnoreCase))
            {
                string legacyPath = Path.Combine(dir, legacyJsonName);
                if (File.Exists(legacyPath) && !File.Exists(_jsonFilePath))
                {
                    try
                    {
                        File.Move(legacyPath, _jsonFilePath);
                    }
                    catch { }
                }
            }

            Load();
        }

        public void Load()
        {
            if (File.Exists(_jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    var deserialized = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

                    if (deserialized != null)
                    {
                        // Rebuild dictionaries to ensure CaseInsensitive keys
                        _config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
                        foreach (var section in deserialized)
                        {
                            _config[section.Key] = new Dictionary<string, string>(section.Value, StringComparer.OrdinalIgnoreCase);
                        }
                    }
                    else
                    {
                        _config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
                    }
                }
                catch
                {
                    _config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
                }
            }
            else if (File.Exists(_iniFilePath))
            {
                ImportFromIni();
            }
        }

        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(_jsonFilePath, json);
            }
            catch (Exception)
            {
                // Handle or log error
            }
        }

        public string GetValue(string section, string key)
        {
            if (_config.ContainsKey(section) && _config[section].ContainsKey(key))
            {
                return _config[section][key];
            }
            return "";
        }

        public void SetValue(string section, string key, string value)
        {
            if (!_config.ContainsKey(section))
            {
                _config[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            _config[section][key] = value;
        }

        public void ClearSection(string section)
        {
            if (_config.ContainsKey(section))
            {
                _config.Remove(section);
            }
        }

        public Dictionary<string, Dictionary<string, string>> GetRawConfig()
        {
            return _config;
        }

        private void ImportFromIni()
        {
            // Read all sections
            string[] sections = GetIniSectionNames(_iniFilePath);
            foreach (string section in sections)
            {
                Dictionary<string, string> sectionData = GetIniSectionData(_iniFilePath, section);
                foreach (var kvp in sectionData)
                {
                    SetValue(section, kvp.Key, kvp.Value);
                }
            }

            // Save to JSON
            Save();

            // Rename INI file
            try
            {
                string oldIniPath = _iniFilePath + ".old";
                if (File.Exists(oldIniPath))
                {
                    File.Delete(oldIniPath);
                }
                File.Move(_iniFilePath, oldIniPath);
            }
            catch
            {
                // If rename fails, we proceed anyway, JSON is already saved.
            }
        }

        // Helper to read all section names from INI
        private string[] GetIniSectionNames(string iniFile)
        {
            IntPtr pBuffer = Marshal.AllocCoTaskMem(32768);
            try
            {
                int bytesRead = GetPrivateProfileSectionNames(pBuffer, 32768, iniFile);
                if (bytesRead == 0)
                    return new string[0];

                string allSections = Marshal.PtrToStringAnsi(pBuffer, bytesRead);
                return allSections.TrimEnd('\0').Split('\0');
            }
            finally
            {
                Marshal.FreeCoTaskMem(pBuffer);
            }
        }

        // Helper to read all keys in a section
        private Dictionary<string, string> GetIniSectionData(string iniFile, string section)
        {
            Dictionary<string, string> data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            IntPtr pBuffer = Marshal.AllocCoTaskMem(32768);
            try
            {
                int bytesRead = GetPrivateProfileSection(section, pBuffer, 32768, iniFile);
                if (bytesRead > 0)
                {
                    string allKeys = Marshal.PtrToStringAnsi(pBuffer, bytesRead);
                    string[] lines = allKeys.TrimEnd('\0').Split('\0');
                    foreach (string line in lines)
                    {
                        int equalIndex = line.IndexOf('=');
                        if (equalIndex > 0)
                        {
                            string key = line.Substring(0, equalIndex);
                            string val = line.Substring(equalIndex + 1);
                            data[key] = val;
                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(pBuffer);
            }
            return data;
        }
    }
}
