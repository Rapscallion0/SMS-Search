using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SMS_Search
{
    public class UpdateInfo
    {
        public string Version { get; set; }
        public string DownloadUrl { get; set; }
        public string ReleaseUrl { get; set; }
        public string Changelog { get; set; }
        public bool IsNewer { get; set; }
    }

    public class UpdateChecker
    {
        private const string RepoOwner = "Rapscallion0";
        private const string RepoName = "SMS-Search";
        private const string GitHubApiUrl = "https://api.github.com/repos/" + RepoOwner + "/" + RepoName + "/releases/latest";

        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // GitHub API requires a User-Agent
                    client.DefaultRequestHeaders.Add("User-Agent", "SMS-Search-Updater");
                    client.Timeout = TimeSpan.FromSeconds(10);

                    string json = await client.GetStringAsync(GitHubApiUrl);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var release = serializer.Deserialize<Dictionary<string, object>>(json);

                    if (release != null && release.ContainsKey("tag_name"))
                    {
                        string tagName = release["tag_name"] as string;
                        string releaseUrl = release.ContainsKey("html_url") ? release["html_url"] as string : "";
                        string body = release.ContainsKey("body") ? release["body"] as string : "";

                        if (string.IsNullOrEmpty(tagName)) return new UpdateInfo { IsNewer = false };

                        string versionStr = tagName.TrimStart('v', 'V');
                        Version remoteVersion;
                        if (Version.TryParse(versionStr, out remoteVersion))
                        {
                            Version currentVersion = new Version(Application.ProductVersion);

                            if (remoteVersion > currentVersion)
                            {
                                string downloadUrl = null;
                                if (release.ContainsKey("assets"))
                                {
                                    var assets = release["assets"] as object[];
                                    if (assets == null && release["assets"] is System.Collections.ArrayList al)
                                    {
                                        assets = al.ToArray();
                                    }

                                    if (assets != null)
                                    {
                                        foreach (var assetObj in assets)
                                        {
                                            var asset = assetObj as Dictionary<string, object>;
                                            if (asset != null && asset.ContainsKey("name") && asset.ContainsKey("browser_download_url"))
                                            {
                                                string name = asset["name"] as string;
                                                if (!string.IsNullOrEmpty(name) && name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    downloadUrl = asset["browser_download_url"] as string;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                return new UpdateInfo
                                {
                                    Version = tagName,
                                    DownloadUrl = downloadUrl,
                                    ReleaseUrl = releaseUrl,
                                    Changelog = body,
                                    IsNewer = true
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail or log if logging mechanism available
            }

            return new UpdateInfo { IsNewer = false };
        }

        public async Task PerformUpdate(UpdateInfo info)
        {
            if (string.IsNullOrEmpty(info.DownloadUrl))
            {
                if (!string.IsNullOrEmpty(info.ReleaseUrl))
                {
                    Process.Start(info.ReleaseUrl);
                }
                else
                {
                    MessageBox.Show("No download URL found for the new version.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            string tempPath = Path.GetTempPath();
            string installerPath = Path.Combine(tempPath, "SMS_Search_Update.exe");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "SMS-Search-Updater");
                    var data = await client.GetByteArrayAsync(info.DownloadUrl);
                    File.WriteAllBytes(installerPath, data);
                }

                string currentExe = Application.ExecutablePath;
                string batchPath = Path.Combine(tempPath, "sms_update.bat");
                string pid = Process.GetCurrentProcess().Id.ToString();

                // Batch script to wait for process exit, replace file, and restart
                string batchContent =
                    "@echo off\r\n" +
                    "timeout /t 2 /nobreak > NUL\r\n" +
                    ":loop\r\n" +
                    "tasklist /FI \"PID eq " + pid + "\" 2>NUL | find /I /N \"" + pid + "\">NUL\r\n" +
                    "if \"%ERRORLEVEL%\"==\"0\" (\r\n" +
                    "    timeout /t 1 /nobreak > NUL\r\n" +
                    "    goto loop\r\n" +
                    ")\r\n" +
                    "copy /Y \"" + installerPath + "\" \"" + currentExe + "\"\r\n" +
                    "start \"\" \"" + currentExe + "\"\r\n" +
                    "del \"" + installerPath + "\"\r\n" +
                    "del \"%~f0\"\r\n";

                File.WriteAllText(batchPath, batchContent);

                ProcessStartInfo psi = new ProcessStartInfo(batchPath);
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                Process.Start(psi);

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
