using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace SMS_Search
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
            // Default to "SMS Search.ini" if directory is empty (current dir)
            if (string.IsNullOrEmpty(directory))
            {
                _iniFilePath = "SMS Search.ini";
            }
            else
            {
                _iniFilePath = Path.Combine(directory, "SMS Search.ini");
            }

            _config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            Load();
        }

        public void Load()
        {
            if (File.Exists(_jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    _config = SimpleJsonParser.Parse(json);
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
                string json = SimpleJsonParser.Serialize(_config);
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

    // Simple JSON Parser for Dictionary<string, Dictionary<string, string>> structure
    public static class SimpleJsonParser
    {
        public static string Serialize(Dictionary<string, Dictionary<string, string>> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            int sectionCount = 0;
            foreach (var section in data)
            {
                sb.Append("  \"");
                sb.Append(Escape(section.Key));
                sb.AppendLine("\": {");

                int keyCount = 0;
                foreach (var item in section.Value)
                {
                    sb.Append("    \"");
                    sb.Append(Escape(item.Key));
                    sb.Append("\": \"");
                    sb.Append(Escape(item.Value));
                    sb.Append("\"");

                    keyCount++;
                    if (keyCount < section.Value.Count)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }

                sb.Append("  }");
                sectionCount++;
                if (sectionCount < data.Count)
                    sb.AppendLine(",");
                else
                    sb.AppendLine();
            }
            sb.Append("}");
            return sb.ToString();
        }

        private static string Escape(string s)
        {
            if (s == null) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
        }

        // Very basic parser that assumes the structure generated above
        public static Dictionary<string, Dictionary<string, string>> Parse(string json)
        {
            var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(json)) return result;

            json = json.Trim();
            if (!json.StartsWith("{") || !json.EndsWith("}")) return result;

            // Remove outer braces
            json = json.Substring(1, json.Length - 2);

            int index = 0;
            while (index < json.Length)
            {
                // Find section key
                string sectionName = ParseString(json, ref index);
                if (sectionName == null) break;

                // Find colon
                SkipWhitespace(json, ref index);
                if (index < json.Length && json[index] == ':') index++;

                // Find section object start
                SkipWhitespace(json, ref index);
                if (index < json.Length && json[index] == '{')
                {
                    index++;
                    var sectionDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    while (index < json.Length)
                    {
                        SkipWhitespace(json, ref index);
                        if (index < json.Length && json[index] == '}')
                        {
                            index++; // End of section
                            break;
                        }

                        string key = ParseString(json, ref index);
                        if (key == null) break;

                        SkipWhitespace(json, ref index);
                        if (index < json.Length && json[index] == ':') index++;

                        string value = ParseString(json, ref index);
                        sectionDict[key] = value ?? "";

                        SkipWhitespace(json, ref index);
                        if (index < json.Length && json[index] == ',') index++;
                    }
                    result[sectionName] = sectionDict;
                }

                SkipWhitespace(json, ref index);
                if (index < json.Length && json[index] == ',') index++;
            }

            return result;
        }

        private static void SkipWhitespace(string json, ref int index)
        {
            while (index < json.Length && char.IsWhiteSpace(json[index]))
                index++;
        }

        private static string ParseString(string json, ref int index)
        {
            SkipWhitespace(json, ref index);
            if (index >= json.Length || json[index] != '"') return null;

            index++; // Skip opening quote
            StringBuilder sb = new StringBuilder();

            while (index < json.Length)
            {
                char c = json[index];
                if (c == '"')
                {
                    index++; // Skip closing quote
                    return sb.ToString();
                }

                if (c == '\\' && index + 1 < json.Length)
                {
                    index++;
                    char next = json[index];
                    switch (next)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        default: sb.Append(next); break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
                index++;
            }
            return sb.ToString();
        }
    }
}
