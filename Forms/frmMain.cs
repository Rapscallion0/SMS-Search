using SMS_Search.Data;
// using Ini;
using SMS_Search.Utils;
using SMS_Search.Controls;
using SMS_Search.Forms;
// using Microsoft.SqlServer.TransactSql.ScriptDom;
using SMS_Search.Properties;
using SMS_Search.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Search.Forms
{
    /// <summary>
    /// Main application form handling the primary UI, search logic, grid display, and interaction.
    /// </summary>
	public partial class frmMain : Form
	{
        /// <summary>
        /// Custom TextBox that handles Ctrl+A for "Select All".
        /// </summary>
		public class MyTextBox : TextBox
		{
			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (e.Control && e.KeyCode == Keys.A)
				{
					SelectAll();
					e.SuppressKeyPress = true;
					e.Handled = true;
					return;
				}
				OnKeyDown(e);
			}
		}

		private struct ReplacementRule
		{
			public Regex Regex;
			public string Replacement;
		}

		private List<ReplacementRule> _cleanSqlRules = new List<ReplacementRule>();
        private frmClipboardOptions.CopyAction? _clipboardSessionPreference = null;

		private int FormHeightMin = 300;
		private int FormHeightExpanded = 600;
		private int FormWidthMin = 600 + SystemInformation.FrameBorderSize.Width * 2;
		private int GridMinRowHeight = 100;
		private bool minimize = true;
		private bool keyPressHandled;
		private Logfile log = new Logfile();
		private static string ConfigFilePath = Path.Combine(Application.StartupPath, "SMSSearch_settings.json");
		private ConfigManager config = new ConfigManager(frmMain.ConfigFilePath);
		private static UpdateChecker Versions = new UpdateChecker();
		private dbConnector dbConn = new dbConnector();
        private DataRepository _repo = new DataRepository();
        private VirtualGridContext _gridContext;
        private QueryBuilder _queryBuilder;
		private ArrayList arrayGrdFld = new ArrayList();
		private ArrayList arrayGrdDesc = new ArrayList();

        // Removed fields moved to Designer: _filterDebounceTimer, _cellContextMenu, _columnHeaderMenu, _rowHeaderMenu, btnCancel

        private CancellationTokenSource _cts;

        private bool _highlightMatches = false;
        private Color _matchHighlightColor = Color.Yellow;
        private long _lastTotalMatchCount = 0;
        private bool _showRowNumbers = false;
        private bool _showDescriptions = false;

        private enum ExportFormat { Csv, Json, ExcelXml }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        /// <param name="Params">Command line parameters.</param>
		public frmMain(string[] Params)
		{
			InitializeComponent();
            _gridContext = new VirtualGridContext(_repo);
            _gridContext.DataReady += _gridContext_DataReady;
            _gridContext.LoadError += _gridContext_LoadError;

            // Wire up DataGridView events
            dGrd.CellValueNeeded += dGrd_CellValueNeeded;
            dGrd.ColumnHeaderMouseClick += dGrd_ColumnHeaderMouseClick;
            dGrd.RowPrePaint += dGrd_RowPrePaint;
            dGrd.RowPostPaint += dGrd_RowPostPaint;
            dGrd.CellPainting += dGrd_CellPainting;
            dGrd.CurrentCellChanged += dGrd_CurrentCellChanged;
            dGrd.RowHeaderMouseClick += dGrd_RowHeaderMouseClick;
            dGrd.ShowEditingIcon = false;
            dGrd.CellMouseClick += dGrd_CellMouseClick;

            // Cancel Button is now initialized in Designer
            btnCancel.BringToFront();

            // Initialize History
            QueryHistoryManager.Instance.Initialize(config);

			log.Logger(LogLevel.Info, "SMS Search V" + Application.ProductVersion + " initialized");

            // Calculate required height for the search panel (Panel1)
            int requiredPanel1Height = GetRequiredSearchPanelHeight();

            // Calculate window chrome overhead (Title bar, borders, etc.)
            int frameOverhead = this.Height - this.ClientSize.Height;

            // Set FormHeightMin dynamically
            // Panel1 content + ToolStrip + Window Frame + Padding
            FormHeightMin = requiredPanel1Height + toolStrip.Height + frameOverhead + 5;

			MinimumSize = new Size(FormWidthMin, FormHeightMin);
			Height = FormHeightMin;
			Width = FormWidthMin;

            // Hide the results panel initially
            splitContainer.Panel2Collapsed = true;
            try
            {
                splitContainer.SplitterDistance = requiredPanel1Height;
            }
            catch { } // Ignore if splitter distance invalid initially

			StartPosition = FormStartPosition.Manual;
            toolStrip.Renderer = new ToolStripSystemRenderer();

            // Handle startup location preference
            string startupLoc = config.GetValue("GENERAL", "STARTUP_LOCATION");

            if (startupLoc == "PRIMARY")
            {
			    Top = (Screen.PrimaryScreen.WorkingArea.Height - FormHeightExpanded) / 2;
			    Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            }
            else if (startupLoc == "ACTIVE")
            {
                // Active display (based on cursor)
                Screen screen = Screen.FromPoint(Cursor.Position);
			    Top = screen.WorkingArea.Top + (screen.WorkingArea.Height - FormHeightExpanded) / 2;
			    Left = screen.WorkingArea.Left + (screen.WorkingArea.Width - Width) / 2;
            }
            else if (startupLoc == "CURSOR")
            {
                // Center around cursor
                Top = Cursor.Position.Y - (FormHeightExpanded / 2);
                Left = Cursor.Position.X - (Width / 2);
            }
            else
            {
                // Default to Last Location (LAST)
                int lastTop, lastLeft;
                bool validTop = int.TryParse(config.GetValue("GENERAL", "LAST_TOP"), out lastTop);
                bool validLeft = int.TryParse(config.GetValue("GENERAL", "LAST_LEFT"), out lastLeft);

                if (validTop && validLeft)
                {
                    // Check if the last location is visible on any screen
                    bool isVisible = false;
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        if (screen.Bounds.Contains(new Point(lastLeft + 20, lastTop + 20)))
                        {
                            isVisible = true;
                            break;
                        }
                    }

                    if (isVisible)
                    {
                        Top = lastTop;
                        Left = lastLeft;
                    }
                    else
                    {
                        // Fallback to primary if off-screen
                        Top = (Screen.PrimaryScreen.WorkingArea.Height - FormHeightExpanded) / 2;
			            Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
                    }
                }
                else
                {
                    // Fallback to primary if no last location saved
                    Top = (Screen.PrimaryScreen.WorkingArea.Height - FormHeightExpanded) / 2;
			        Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
                }
            }

			txtNumFct.Focus();
		}

        /// <summary>
        /// Handles the Click event of the History button for Function search.
        /// </summary>
        private void btnHistFct_Click(object sender, EventArgs e)
        {
            ShowHistoryMenu(sender as Button, txtCustSqlFct, "Function");
        }

        /// <summary>
        /// Handles the Click event of the History button for Totalizer search.
        /// </summary>
        private void btnHistTlz_Click(object sender, EventArgs e)
        {
            ShowHistoryMenu(sender as Button, txtCustSqlTlz, "Totalizer");
        }

        /// <summary>
        /// Handles the Click event of the History button for Field search.
        /// </summary>
        private void btnHistFld_Click(object sender, EventArgs e)
        {
            ShowHistoryMenu(sender as Button, txtCustSqlFld, "Field");
        }

        /// <summary>
        /// Displays the history context menu for the specified search type.
        /// </summary>
        /// <param name="btn">The button that triggered the menu.</param>
        /// <param name="targetInput">The input control to populate when a history item is selected.</param>
        /// <param name="type">The type of history to retrieve (Function, Totalizer, Field).</param>
        private void ShowHistoryMenu(Button btn, Control targetInput, string type)
        {
            var history = QueryHistoryManager.Instance.GetHistory(type);
            if (history.Count == 0)
            {
                ContextMenuStrip emptyMenu = new ContextMenuStrip();
                emptyMenu.Items.Add("No history").Enabled = false;
                emptyMenu.Show(btn, new Point(0, btn.Height));
                return;
            }

            ContextMenuStrip menu = new ContextMenuStrip();
            foreach (var sql in history)
            {
                string display = sql.Length > 50 ? sql.Substring(0, 47) + "..." : sql;
                // Replace newlines for display
                display = display.Replace("\r", " ").Replace("\n", " ");

                var item = menu.Items.Add(display);
                item.ToolTipText = sql;
                item.Tag = sql;
                item.Click += (s, e) =>
                {
                    if (targetInput is TextBox tb) tb.Text = (s as ToolStripItem).Tag.ToString();
                    else if (targetInput is SqlRichTextBox rtb) rtb.Text = (s as ToolStripItem).Tag.ToString();
                    targetInput.Focus();
                };
            }

            menu.Items.Add(new ToolStripSeparator());
            var clearItem = menu.Items.Add("Clear History");
            clearItem.Click += (s, e) =>
            {
                QueryHistoryManager.Instance.ClearHistory(type);
                GeneralUtils.showToast(0, "History cleared", "History", Screen.FromControl(this));
            };

            menu.Show(btn, new Point(0, btn.Height));
        }

        /// <summary>
        /// Calculates the required height for the search criteria panel based on tab content.
        /// </summary>
        /// <returns>The calculated height in pixels.</returns>
        private int GetRequiredSearchPanelHeight()
        {
            int maxBottom = 0;
            if (tabCtl != null && tabCtl.TabPages.Count > 0)
            {
                foreach (TabPage tab in tabCtl.TabPages)
                {
                    foreach (Control c in tab.Controls)
                    {
                        if (c.Bottom > maxBottom) maxBottom = c.Bottom;
                    }
                }
                // Calculate tab control chrome (headers, borders)
                int tabChrome = tabCtl.Height - tabCtl.DisplayRectangle.Height;
                // Required height for TabControl = Content Bottom + Chrome + Padding
                int requiredTabHeight = maxBottom + tabChrome + 12;

                // Panel 1 minimum height (tabCtl.Top is the offset from Panel1 top)
                // Add extra height for the Execute button below the tab control
                return requiredTabHeight + tabCtl.Top + 12 + 40;
            }
            return 200; // Fallback
        }

        /*private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			//FormHeightMin = splitContainer.SplitterDistance + 95;
		}*/

        /// <summary>
        /// Event handler for Form Load. Initializes settings, updates, and UI state.
        /// </summary>
        private void frmMain_Load(object sender, EventArgs e)
		{
            // Check EULA acceptance
			if (config.GetValue("GENERAL", "EULA") != "1")
			{
				frmEula frmEula = new frmEula();
				frmEula.ShowDialog();
			}

            // Cleanup old logs
            try
            {
                string retentionStr = config.GetValue("GENERAL", "LOG_RETENTION");
                int retention = 14;
                if (!string.IsNullOrEmpty(retentionStr) && int.TryParse(retentionStr, out int r)) retention = r;
                log.CleanupLogs(retention);
            }
            catch { }

            // Check for updates if enabled
            if (config.GetValue("GENERAL", "CHECKUPDATE") == "1")
            {
                CheckUpdateAsync();
            }

            // Apply UI settings immediately
            ApplyConfigSettings();

			splitContainer.Paint += new PaintEventHandler(SplitContainer_Paint);
			tslblInfo.Text = "";
			setJulianDate();
			Text = Text + " - v" + Application.ProductVersion;

            // Restore last selected tab/mode
			string a;
			if ((a = config.GetValue("GENERAL", "TABLE_LOOKUP")) != null)
			{
				if (a == "FIELDS")
				{
					rdbShowFields.Select();
					goto IL_F6;
				}
				if (a == "RECORDS")
				{
					rdbShowRecords.Select();
					goto IL_F6;
				}
			}
			rdbShowFields.Select();
			IL_F6:
			string a2;
			if ((a2 = config.GetValue("GENERAL", "START_TAB")) != null)
			{
				if (a2 == "FCT_TAB")
				{
					tabCtl.SelectedTab = tabFct;
					goto IL_182;
				}
				if (a2 == "TLZ_TAB")
				{
					tabCtl.SelectedTab = tabTlz;
					goto IL_182;
				}
				if (a2 == "FIELDS")
				{
					tabCtl.SelectedTab = tabFields;
					goto IL_182;
				}
			}
			tabCtl.SelectedTab = tabFct;
			IL_182:
            // Show Unarchive target window if previously open
			if (config.GetValue("UNARCHIVE", "SHOWTARGET") == "1")
			{
				frmUnarchive frmUnarchive = new frmUnarchive();
				frmUnarchive.Show();
				btnShowTarget.Checked = true;
			}

            // Enable Double Buffering on DataGridView via reflection to reduce flicker
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dGrd, new object[] { true });
		}

        /// <summary>
        /// Checks for application updates asynchronously.
        /// </summary>
		private static async void CheckUpdateAsync()
		{
            UpdateInfo updateInfo = await Versions.CheckForUpdatesAsync();

            if (updateInfo.IsNewer)
            {
                string text = "There is an update available for download.\n\nCurrent Version:\t" +
                    Application.ProductVersion +
                    "\nNew Version:\t" +
                    updateInfo.Version +
                    "\n\nWould you like to update now?";

                if (MessageBox.Show(text, "SMS Search update checker", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await Versions.PerformUpdate(updateInfo);
                }
            }
		}

        /// <summary>
        /// Called when the form is shown. Handles database initialization and version checks.
        /// </summary>
        protected override async void OnShown(EventArgs e)
		{
            base.OnShown(e);
            log.Logger(LogLevel.Info, "frmMain_OnShown: Executing");

            string lastRunVersion = config.GetValue("GENERAL", "LAST_RUN_VERSION");
            string currentVersion = Application.ProductVersion;

            if (lastRunVersion != currentVersion)
            {
                if (!string.IsNullOrEmpty(lastRunVersion))
                {
                    GeneralUtils.showToast(0, "Updated from v" + lastRunVersion + " to v" + currentVersion, "Update", Screen.FromControl(this));
                }
                config.SetValue("GENERAL", "LAST_RUN_VERSION", currentVersion);
                config.Save();
            }

            await InitializeDatabaseAsync(true);
			setTabTextFocus();
		}

        /// <summary>
        /// Toggles the UI busy state, showing/hiding progress indicators and enabling/disabling controls.
        /// </summary>
        /// <param name="busy">If set to <c>true</c>, the UI enters a busy state.</param>
        private void SetBusy(bool busy)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetBusy(busy)));
                return;
            }

            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            tsProgressBar.Visible = busy;

            btnPopGrid.Visible = !busy;
            btnPopGrid.Enabled = !busy;

            btnCancel.Visible = busy;
            btnCancel.Enabled = busy;

            picRefresh.Enabled = !busy;
            tscmbDbDatabase.Enabled = !busy;
        }

        /// <summary>
        /// Handles the Cancel button click to abort the current operation.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
            btnCancel.Enabled = false; // Prevent multiple clicks
            tslblInfo.Text = "Cancelling...";
        }

        /// <summary>
        /// Constructs a SQL connection string based on current configuration and parameters.
        /// </summary>
        public string GetConnString(string DbServer, string DbDatabase)
		{
            if (config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH")))
            {
			    return "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" + DbDatabase + ";Data Source=" + DbServer;
            }
            else
            {
                string user = config.GetValue("CONNECTION", "SQLUSER");
                string pass = GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));
                return "Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase + ";User ID=" + user + ";Password=" + pass + ";Persist Security Info=False;";
            }
		}

        /// <summary>
        /// Populates the table list combobox from the database asynchronously.
        /// </summary>
        private async Task PopulateTableList()
		{
			string text = cmbTableFld.Text.ToString();
            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;

			SetBusy(true);

            try
            {
                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

                var tables = await _repo.GetTablesAsync(server, database, user, pass);

                DataTable dt = new DataTable();
                dt.Columns.Add("NAME");
                foreach (var t in tables) dt.Rows.Add(t);

                bindingSourceTbl.DataSource = dt;
                cmbTableFld.DataSource = bindingSourceTbl;
                cmbTableFld.DisplayMember = "NAME";
                cmbTableFld.ValueMember = "NAME";

                int num = cmbTableFld.FindString(text);
                if (num != -1)
                {
                    cmbTableFld.Text = text;
                }
                else
                {
                    if (cmbTableFld.Items.Count > 0) cmbTableFld.SelectedIndex = 0;
                }
                log.Logger(LogLevel.Info, $"PopulateTableList: Loaded {tables.Count()} tables");
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "PopulateTableList: error - " + ex.Message);
            }
            finally
            {
			    setTabTextFocus();
                SetBusy(false);
            }
		}

        /// <summary>
        /// Applies configuration settings to the UI controls.
        /// </summary>
        private void ApplyConfigSettings()
        {
            // Read connection values and update UI
            string server = config.GetValue("CONNECTION", "SERVER");
            string database = config.GetValue("CONNECTION", "DATABASE");

            // Defensive UI updates: only touch controls if form and controls are valid
            if (!this.IsDisposed && !this.Disposing)
            {
                if (tscmbDbServer != null) tscmbDbServer.Text = server ?? string.Empty;
                if (tscmbDbDatabase != null) tscmbDbDatabase.Text = database ?? string.Empty;
            }

            if (config.GetValue("GENERAL", "SHOWINTRAY") == "1")
            {
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
                MinimizeBox = false;
            }
            else
            {
                notifyIcon.Visible = false;
                ShowInTaskbar = true;
                MinimizeBox = true;
            }

            if (config.GetValue("GENERAL", "ALWAYSONTOP") == "1")
            {
                TopMost = true;
                onTop.Checked = true;
            }
            else
            {
                TopMost = false;
                onTop.Checked = false;
            }

            if (config.GetValue("GENERAL", "SEARCHANY") == "1")
            {
                chkSearchAnyFct.Checked = true;
                chkSearchAnyTlz.Checked = true;
                chkSearchAnyFld.Checked = true;
            }
            else
            {
                chkSearchAnyFct.Checked = false;
                chkSearchAnyTlz.Checked = false;
                chkSearchAnyFld.Checked = false;
            }

            if (config.GetValue("GENERAL", "DESCRIPTIONCOLUMNS") == "1")
            {
                _showDescriptions = true;
            }
            else
            {
                _showDescriptions = false;
            }

            string fctFields = config.GetValue("QUERY", "FUNCTION");
            string tlzFields = config.GetValue("QUERY", "TOTALIZER");
            _queryBuilder = new QueryBuilder(fctFields, tlzFields);

            LoadCleanSqlRules();

            _highlightMatches = config.GetValue("GENERAL", "HIGHLIGHT_MATCHES") == "1";
            string colorVal = config.GetValue("GENERAL", "MATCH_HIGHLIGHT_COLOR");
            if (!string.IsNullOrEmpty(colorVal) && int.TryParse(colorVal, out int argb))
            {
                _matchHighlightColor = Color.FromArgb(argb);
            }
            else
            {
                _matchHighlightColor = Color.Yellow;
            }

            _showRowNumbers = config.GetValue("GENERAL", "SHOW_ROW_NUMBERS") == "1";
            UpdateRowHeaderWidth();

            // Refresh filter UI state (visibility of buttons/count) if a filter is active
            if (!string.IsNullOrWhiteSpace(txtGridFilter.Text))
            {
                _filterDebounceTimer.Stop();
                _filterDebounceTimer.Start();
            }
        }

        /// <summary>
        /// Initializes the database connection, prompting for configuration if necessary.
        /// </summary>
        /// <param name="isStartup">Indicates if this is the application startup sequence.</param>
        private async Task InitializeDatabaseAsync(bool isStartup)
        {
            SetBusy(true);

            bool connected = false;

            while (!connected)
            {
                string server = config.GetValue("CONNECTION", "SERVER");
                string database = config.GetValue("CONNECTION", "DATABASE");
                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

                // Attempt to connect
                connected = await dbConn.TestDbConnAsync(server, database, false, user, pass);

                if (!connected || !File.Exists(frmMain.ConfigFilePath))
                {
                    // Show config dialog
                    using (var cfg = new frmConfig())
                    {
                        if (isStartup) cfg.ForceDatabaseSetup = true;

                        var result = cfg.ShowDialog(this);

                        if (isStartup && result != DialogResult.OK)
                        {
                            // User cancelled initial setup
                            Application.Exit();
                            return;
                        }

                        if (!isStartup && result != DialogResult.OK)
                        {
                            // User cancelled re-configuration, stop trying to connect
                            SetBusy(false);
                            return;
                        }
                    }

                    // Reload config
                    config.Load();
                    log.ReloadConfig();

                    // Re-apply settings (e.g. server name in UI)
                    ApplyConfigSettings();
                }
            }

            // Connection successful
            await PopulateTableList();
            SetBusy(false);
        }

        /// <summary>
        /// Loads the regex replacement rules for SQL cleaning from configuration.
        /// </summary>
        private void LoadCleanSqlRules()
        {
            _cleanSqlRules.Clear();
            string countStr = config.GetValue("CLEAN_SQL", "Count");
            int count = 0;
            List<SqlCleaningRule> rulesToLoad = new List<SqlCleaningRule>();

            if (!string.IsNullOrEmpty(countStr) && int.TryParse(countStr, out count) && count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    string pattern = config.GetValue("CLEAN_SQL", "Rule_" + i + "_Regex");
                    string replacement = config.GetValue("CLEAN_SQL", "Rule_" + i + "_Replace");
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        rulesToLoad.Add(new SqlCleaningRule { Pattern = pattern, Replacement = replacement });
                    }
                }
            }
            else
            {
                rulesToLoad = SqlCleaner.DefaultRules;
            }

            foreach (var rule in rulesToLoad)
            {
                try
                {
                    _cleanSqlRules.Add(new ReplacementRule
                    {
                        Regex = new Regex(rule.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase),
                        Replacement = rule.Replacement
                    });
                }
                catch (Exception ex)
                {
                    log.Logger(LogLevel.Error, "Error compiling regex for Clean SQL rule: " + rule.Pattern + " - " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Creates a new configuration file with current settings.
        /// </summary>
        private void CreateConfigFile()
		{
			if (File.Exists(frmMain.ConfigFilePath))
			{
				File.Delete(frmMain.ConfigFilePath);
			}
			WriteConfigConn(tscmbDbServer.Text, tscmbDbDatabase.Text);
		}

        /// <summary>
        /// Executes the search query and populates the grid.
        /// </summary>
        private async void btnPopGrid_Click(object sender, EventArgs e)
		{
			SetBusy(true);
			tslblInfo.Text = "";

            // Initialize Cancellation Token
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;

            bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
            string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
            string pass = useWinAuth ? null : GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

            _gridContext.SetConnection(server, database, user, pass);

            try
            {
			    bool SQLConnected = await _repo.TestConnectionAsync(server, database, user, pass, token);
			    arrayGrdDesc.Clear();
			    arrayGrdDesc.TrimToSize();
			    arrayGrdFld.Clear();
			    arrayGrdFld.TrimToSize();
			    dGrd.DataSource = null;
                dGrd.Rows.Clear();
                dGrd.Columns.Clear();
			    tslblRecordCnt.Text = "0";

			    if (SQLConnected)
			    {
                    var criteria = GetSearchCriteriaFromUI();

                    // Add to history if it's a custom SQL query
                    if (criteria.Type == SearchType.CustomSql && !string.IsNullOrWhiteSpace(criteria.Value))
                    {
                        QueryHistoryManager.Instance.AddQuery(criteria.Mode.ToString(), criteria.Value);
                    }

                    var queryResult = _queryBuilder.Build(criteria);

                    log.Logger(LogLevel.Info, "Starting search...");

                    Stopwatch sw = Stopwatch.StartNew();

				    try
				    {
                        // 1. Get Schema to setup columns
                        log.Logger(LogLevel.Debug, "Fetching schema...");
                        var schemaDt = await _gridContext.GetSchemaAsync(queryResult.Sql, queryResult.Parameters, token);
                        log.Logger(LogLevel.Debug, $"Schema fetched. Columns: {schemaDt.Columns.Count}");

                        foreach (DataColumn col in schemaDt.Columns)
                        {
                            dGrd.Columns.Add(new DataGridViewTextBoxColumn { Name = col.ColumnName, HeaderText = col.ColumnName });
                        }

                        // 2. Load Data (Count)
                        dGrd.VirtualMode = true;
                        tslblRecordCnt.Text = "Loading...";
                        string defaultSort = dGrd.Columns.Count > 0 ? dGrd.Columns[0].Name : null;

                        log.Logger(LogLevel.Debug, "Loading data (Count)...");
                        await _gridContext.LoadAsync(queryResult.Sql, queryResult.Parameters, defaultSort, token);
                        log.Logger(LogLevel.Debug, $"LoadAsync complete. TotalCount: {_gridContext.TotalCount}");

                        if (dGrd.Columns.Count > 0)
                        {
                             dGrd.Columns[0].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }

                        sw.Stop();
                        log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, _gridContext.TotalCount, true);
                        tslblInfo.Text = $"Executed in {sw.ElapsedMilliseconds} ms";
                        tslblInfo.ForeColor = SystemColors.ControlText;
				    }
				    catch (Exception ex)
				    {
                        if (ex is OperationCanceledException) throw; // Bubble up

                        // Fallback to legacy load if virtual mode fails
                        log.Logger(LogLevel.Warning, "Virtual Mode Load failed, attempting legacy load: " + ex.ToString());
                        try
                        {
                            dGrd.VirtualMode = false;
                            dGrd.Rows.Clear();
                            dGrd.Columns.Clear();

                            var dataTable = await _repo.ExecuteQueryAsync(server, database, user, pass, queryResult.Sql, queryResult.Parameters, token);

                            bindingSource.DataSource = dataTable;
                            dGrd.DataSource = bindingSource;
                            UpdateRowHeaderWidth();

                            sw.Stop();
                            tslblRecordCnt.Text = dataTable.Rows.Count.ToString();
                            log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, dataTable.Rows.Count, true);
                            tslblInfo.Text = $"Executed in {sw.ElapsedMilliseconds} ms";
                        }
                        catch (Exception ex2)
                        {
                            if (ex2 is OperationCanceledException) throw; // Bubble up

                            sw.Stop();
                            MessageBox.Show("Virtual Load Error: " + ex.Message + "\n\nLegacy Load Error: " + ex2.Message, "SQL error encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, 0, false, ex2.Message);
                        }
				    }
				    finally
				    {
                        if (!token.IsCancellationRequested)
                        {
                            // Resize columns based on headers first
					        dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                        }
				    }
				
                    if (dGrd.RowCount > 0)
				    {
                        // Check if we are opening the grid for the first time (or after it was hidden)
                        if (splitContainer.Panel2Collapsed)
                        {
                            // User wants to preserve the current size of the top panel (Search criteria)
                            // and add the grid below it.
                            int currentTopPanelHeight = splitContainer.Height;

                            // Determine how much height to add for the grid
                            // Use the difference between Expanded and Min, or a safe default (e.g. 300)
                            int gridHeightToAdd = Math.Max(FormHeightExpanded - FormHeightMin, 300);

                            if (this.WindowState != FormWindowState.Maximized)
                            {
                                this.Height += gridHeightToAdd;
                            }

                            // Recalculate based on actual new height
                            int maxSplitter = splitContainer.Height - splitContainer.Panel2MinSize;
                            int safeSplitter;

                            // If we couldn't expand (e.g. Maximized or Screen bounds), ensure Panel2 has space
                            if (currentTopPanelHeight > maxSplitter - 100) // If panel2 would be tiny (<100px)
                            {
                                // Give Panel2 some breathing room (e.g. 300px), shrinking Panel1
                                safeSplitter = splitContainer.Height - 300;
                            }
                            else
                            {
                                safeSplitter = currentTopPanelHeight;
                            }

                            safeSplitter = Math.Min(safeSplitter, maxSplitter);

                            if (safeSplitter < splitContainer.Panel1MinSize)
                                safeSplitter = splitContainer.Panel1MinSize;

                            try
                            {
                                splitContainer.SplitterDistance = safeSplitter;
                            }
                            catch (Exception ex)
                            {
                                log.Logger(LogLevel.Warning, "Failed to restore splitter distance: " + ex.Message);
                            }

                            splitContainer.Panel2Collapsed = false;
                        }
                        else
                        {
                            // Grid is already visible. Ensure minimum height is maintained if user shrunk it too much.
                            if (Height < FormHeightMin + GridMinRowHeight)
                            {
					            Height = FormHeightExpanded;
                            }
                            splitContainer.Panel2Collapsed = false;
                        }
				    }

				    if (dGrd.RowCount < 1 && !token.IsCancellationRequested)
				    {
					    tslblInfo.Text = "Query returned no records!";
					    tslblInfo.ForeColor = Color.Red;
				    }
			    }
			    else
			    {
				    tslblInfo.Text = "Connection failed!";
				    tslblInfo.ForeColor = Color.Red;
                    GeneralUtils.showToast(2, "Failed to connect to data source.\nPlease check your connection settings.", "SQL connection error", Screen.FromControl(this));
			    }
			    setTabTextFocus();
                if (!token.IsCancellationRequested)
                {
			        await setColumnArrayAsync();
			        await setHeadersAsync();
                }
            }
            catch (OperationCanceledException)
            {
                tslblInfo.Text = "Operation cancelled.";
                tslblInfo.ForeColor = Color.OrangeRed;
            }
            finally
            {
			    SetBusy(false);
            }
		}

        /// <summary>
        /// Provides cell values for Virtual Mode.
        /// </summary>
        private void dGrd_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < _gridContext.TotalCount)
            {
                e.Value = _gridContext.GetValue(e.RowIndex, e.ColumnIndex);
            }
        }

        /// <summary>
        /// Callback when new data pages are ready to be displayed.
        /// </summary>
        private void _gridContext_DataReady(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => _gridContext_DataReady(sender, e)));
                return;
            }
            dGrd.RowCount = _gridContext.TotalCount;
            UpdateRowHeaderWidth();

            if (!string.IsNullOrEmpty(_gridContext.FilterText))
            {
                tslblRecordCnt.Text = $"{_gridContext.TotalCount} / {_gridContext.UnfilteredCount}";
            }
            else
            {
                tslblRecordCnt.Text = _gridContext.TotalCount.ToString();
            }

            dGrd.Invalidate();
        }

        private void _gridContext_LoadError(object sender, string errorMessage)
        {
             if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => _gridContext_LoadError(sender, errorMessage)));
                return;
            }
            GeneralUtils.showToast(2, errorMessage, "Error loading data", Screen.FromControl(this));
        }

        /// <summary>
        /// Handles click on column header to trigger sorting or context menu.
        /// </summary>
        private async void dGrd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _columnHeaderMenu.Show(Cursor.Position);
                return;
            }

            var col = dGrd.Columns[e.ColumnIndex];
            await _gridContext.ApplySortAsync(col.Name);

            foreach (DataGridViewColumn c in dGrd.Columns)
            {
                c.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
            }

            if (_gridContext.SortDirection == "ASC")
                col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
            else
                col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
        }

        /// <summary>
        /// Builds the search criteria object based on the currently selected tab and inputs.
        /// </summary>
        private SearchCriteria GetSearchCriteriaFromUI()
        {
            var criteria = new SearchCriteria();
            string tabName = tabCtl.SelectedTab.Name;

            if (tabName == "tabFct")
            {
                criteria.Mode = SearchMode.Function;
                if (rdbNumFct.Checked) { criteria.Type = SearchType.Number; criteria.Value = txtNumFct.Text; }
                else if (rdbDescFct.Checked) { criteria.Type = SearchType.Description; criteria.Value = txtDescFct.Text; criteria.AnyMatch = chkSearchAnyFct.Checked; }
                else if (rdbCustSqlFct.Checked) { criteria.Type = SearchType.CustomSql; criteria.Value = !string.IsNullOrEmpty(txtCustSqlFct.SelectedText) ? txtCustSqlFct.SelectedText : txtCustSqlFct.Text; }
            }
            else if (tabName == "tabTlz")
            {
                criteria.Mode = SearchMode.Totalizer;
                if (rdbNumTlz.Checked) { criteria.Type = SearchType.Number; criteria.Value = txtNumTlz.Text; }
                else if (rdbDescTlz.Checked) { criteria.Type = SearchType.Description; criteria.Value = txtDescTlz.Text; criteria.AnyMatch = chkSearchAnyTlz.Checked; }
                else if (rdbCustSqlTlz.Checked) { criteria.Type = SearchType.CustomSql; criteria.Value = !string.IsNullOrEmpty(txtCustSqlTlz.SelectedText) ? txtCustSqlTlz.SelectedText : txtCustSqlTlz.Text; }
            }
            else if (tabName == "tabFields")
            {
                criteria.Mode = SearchMode.Field;
                if (rdbNumFld.Checked) { criteria.Type = SearchType.Number; criteria.Value = txtNumFld.Text; }
                else if (rdbDescFld.Checked) { criteria.Type = SearchType.Description; criteria.Value = txtDescFld.Text; criteria.AnyMatch = chkSearchAnyFld.Checked; }
                else if (rdbTableFld.Checked)
                {
                    criteria.Type = SearchType.Table;
                    criteria.Value = cmbTableFld.Text;
                    criteria.ShowFields = rdbShowFields.Checked;
                    criteria.LastTransaction = chkLastTransaction.Checked && chkLastTransaction.Visible;
                }
                else if (rdbCustSqlFld.Checked) { criteria.Type = SearchType.CustomSql; criteria.Value = !string.IsNullOrEmpty(txtCustSqlFld.SelectedText) ? txtCustSqlFld.SelectedText : txtCustSqlFld.Text; }
            }

            return criteria;
        }

        /// <summary>
        /// Interpolates parameters into the SQL string for display purposes only.
        /// </summary>
        private string GetInterpolatedSql(QueryResult query)
        {
            // Simple helper to visualize SQL for the Build buttons
            string sql = query.Sql;
            foreach (var name in query.Parameters.ParameterNames)
            {
                var val = query.Parameters.Get<object>(name);
                sql = sql.Replace("@" + name, $"'{val}'"); // Very basic interpolation for display only
            }
            return sql;
        }

        #region Radio Button Control and Focus
        private void txtDescFct_Enter(object sender, EventArgs e)
		{
			rdbDescFct.Checked = true;
		}

		private void txtNumFct_Enter(object sender, EventArgs e)
		{
			rdbNumFct.Checked = true;
		}

		private void txtSqlFct_Enter(object sender, EventArgs e)
		{
			rdbCustSqlFct.Checked = true;
		}

		private void rdbDescFct_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbDescFct.Checked)
			{
				txtDescFct.Focus();
			}
		}

		private void rdbNumFct_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbNumFct.Checked)
			{
				txtNumFct.Focus();
			}
		}

		private void rdbCustSqlFct_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbCustSqlFct.Checked)
			{
				txtCustSqlFct.Focus();
			}
		}

		private void txtDescTlz_Enter(object sender, EventArgs e)
		{
			rdbDescTlz.Checked = true;
		}

		private void txtNumTlz_Enter(object sender, EventArgs e)
		{
			rdbNumTlz.Checked = true;
		}

		private void txtCustSqlTlz_Enter(object sender, EventArgs e)
		{
			rdbCustSqlTlz.Checked = true;
		}

		private void rdbDescTlz_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbDescTlz.Checked)
			{
				txtDescTlz.Focus();
			}
		}

		private void rdbNumTlz_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbNumTlz.Checked)
			{
				txtNumTlz.Focus();
			}
		}

		private void rdbCustSqlTlz_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbCustSqlTlz.Checked)
			{
				txtCustSqlTlz.Focus();
			}
		}

		private void txtNumFld_Enter(object sender, EventArgs e)
		{
			rdbNumFld.Checked = true;
		}

		private void txtDescFld_Enter(object sender, EventArgs e)
		{
			rdbDescFld.Checked = true;
		}

		private void cmbTableFld_Enter(object sender, EventArgs e)
		{
			rdbTableFld.Checked = true;
		}

		private void txtCustSqlFld_Enter(object sender, EventArgs e)
		{
			rdbCustSqlFld.Checked = true;
		}

		private void rdbNumFld_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbNumFld.Checked)
			{
				txtNumFld.Focus();
			}
		}

		private void rdbDescFld_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbDescFld.Checked)
			{
				txtDescFld.Focus();
			}
		}

		private void rdbTableFld_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbTableFld.Checked)
			{
				cmbTableFld.Focus();
			}
		}

		private void rdbCustSqlFld_CheckedChanged(object sender, EventArgs e)
		{
			if (rdbCustSqlFld.Checked)
			{
				txtCustSqlFld.Focus();
			}
		}

        private void rdbSelectByFields_CheckedChanged(object sender, EventArgs e)
        {
            rdbTableFld.Checked = true;
            setTabTextFocus();
        }

        private void rdbSelectByTable_CheckedChanged(object sender, EventArgs e)
        {
            rdbTableFld.Checked = true;
            setTabTextFocus();
        }
        #endregion

        #region Reset forms
        private void btnClearFct_Click(object sender, EventArgs e)
		{
			txtNumFct.Clear();
			txtDescFct.Clear();
			txtCustSqlFct.Clear();
			setTabTextFocus();
		}

		private void btnClearTlz_Click(object sender, EventArgs e)
		{
			txtNumTlz.Clear();
			txtDescTlz.Clear();
			txtCustSqlTlz.Clear();
			setTabTextFocus();
		}

		private void btnClearFld_Click(object sender, EventArgs e)
		{
			txtNumFld.Clear();
			txtDescFld.Clear();
			txtCustSqlFld.Clear();
			setTabTextFocus();
		}
        #endregion

        #region Build query buttons
        private void btnBuildQryFct_Click(object sender, EventArgs e)
		{
            var criteria = GetSearchCriteriaFromUI();
            var res = _queryBuilder.Build(criteria);
			txtCustSqlFct.Text = GetInterpolatedSql(res);
			setTabTextFocus();
		}

		private void btnBuildQryTlz_Click(object sender, EventArgs e)
		{
            var criteria = GetSearchCriteriaFromUI();
            var res = _queryBuilder.Build(criteria);
			txtCustSqlTlz.Text = GetInterpolatedSql(res);
			setTabTextFocus();
		}

		private void btnBuildQryFld_Click(object sender, EventArgs e)
		{
            var criteria = GetSearchCriteriaFromUI();
            var res = _queryBuilder.Build(criteria);
			txtCustSqlFld.Text = GetInterpolatedSql(res);
			setTabTextFocus();
		}
        #endregion

        /// <summary>
        /// Saves the connection parameters to the configuration file.
        /// </summary>
        private void WriteConfigConn(string DbServer, string DbDatabase)
		{
			config.SetValue("CONNECTION", "SERVER", DbServer);
			config.SetValue("CONNECTION", "DATABASE", DbDatabase);
            config.Save();
		}

        #region Keypress handling
        private void Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = onlyNum(e);
        }

        private bool onlyNum(KeyPressEventArgs e)
        {
            return !char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '*' && e.KeyChar != '?';
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (keyPressHandled)
			{
				e.Handled = true;
			}
			keyPressHandled = false;
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.Control && e.KeyCode == Keys.C)
            {
                // Only intercept if grid has focus or contains focus, and not in editing mode?
                // Actually if dGrd is focused, we want to override default behavior.
                if (dGrd.Focused)
                {
                    CopyToClipboard(false);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }

			if (e.Control && e.KeyCode.ToString() == "A")
			{
				if (txtCustSqlFct.Focused)
				{
					txtCustSqlFct.SelectAll();
					keyPressHandled = true;
				}
				if (txtCustSqlTlz.Focused)
				{
					txtCustSqlTlz.SelectAll();
					keyPressHandled = true;
				}
				if (txtCustSqlFld.Focused)
				{
					txtCustSqlFld.SelectAll();
					keyPressHandled = true;
				}
			}
			if (e.Control && e.KeyCode.ToString() == "D1")
			{
				tabCtl.SelectedTab = tabFct;
			}
			if (e.Control && e.KeyCode.ToString() == "D2")
			{
				tabCtl.SelectedTab = tabTlz;
			}
			if (e.Control && e.KeyCode.ToString() == "D3")
			{
				tabCtl.SelectedTab = tabFields;
			}
			if (e.Control && e.KeyCode.ToString() == "T")
			{
				e.Handled = true;
				toggleTarget();
			}
			if (e.KeyCode == Keys.F5 || (e.KeyCode == Keys.Return && (txtNumFct.Focused || txtDescFct.Focused || txtNumTlz.Focused || txtDescTlz.Focused || txtNumFld.Focused || txtDescFld.Focused || cmbTableFld.Focused)))
			{
				btnPopGrid.PerformClick();
				if (e.KeyCode == Keys.Return)
				{
					e.SuppressKeyPress = true;
				}
			}
		}
        #endregion

        #region Date conversion
        private void txtJulian_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Return) && txtJulian.Focused)
			{
				setGregorianDate();
				log.Logger(LogLevel.Info, "Date converted: Julian \"" + txtJulian.Text + "\" to Gregorian \"" + dateGregorian.Text);
			}
		}

		private void dateGregorian_ValueChanged(object sender, EventArgs e)
		{
			if (dateGregorian.Focused)
			{
				setJulianDate();
				log.Logger(LogLevel.Info, "Date converted: Gregorian \"" + dateGregorian.Text + "\" to Julian \"" + txtJulian.Text);
			}
		}

		private void setJulianDate()
		{
			DateTime dateTime = dateGregorian.Value.AddDays(1.0);
			DateTime value = new DateTime(dateTime.Year, 1, 1);
			int days = dateTime.Subtract(value).Days;
			txtJulian.Text = dateTime.Year.ToString() + days.ToString().PadLeft(3, '0');
		}

		private void setGregorianDate()
		{
			try
			{
				int year = Convert.ToInt32(txtJulian.Text.Substring(0, 4));
				int num = Convert.ToInt32(txtJulian.Text.Substring(4, 3));
				DateTime dateTime = new DateTime(year, 1, 1) + new TimeSpan(num - 1, 0, 0, 0);
				dateGregorian.Text = dateTime.ToString("d");
			}
			catch
			{
                GeneralUtils.showToast(3, "You must specify a valid Julian date (YYYYDDD).", "Julian date error", Screen.FromControl(this));
			}
		}
        #endregion

        private void onTop_Click(object sender, EventArgs e)
		{
			if (TopMost)
			{
				onTop.Checked = false;
				TopMost = false;
				return;
			}
			onTop.Checked = true;
			TopMost = true;
		}

		private void tabCtl_SelectedIndexChanged(object sender, EventArgs e)
		{
			setTabTextFocus();
		}

		private void setTabTextFocus()
		{
			string TabName;
			if ((TabName = tabCtl.SelectedTab.Name) != null)
			{
				if (!(TabName == "tabFct"))
				{
					if (!(TabName == "tabTlz"))
					{
						if (!(TabName == "tabFields"))
						{
							return;
						}
						if (rdbNumFld.Checked)
						{
							txtNumFld.Focus();
							txtNumFld.SelectAll();
						}
						if (rdbDescFld.Checked)
						{
							txtDescFld.Focus();
						}
						if (rdbCustSqlFld.Checked)
						{
							txtCustSqlFld.Focus();
						}
						if (rdbTableFld.Checked)
						{
							cmbTableFld.Focus();
						}
					}
					else
					{
						if (rdbNumTlz.Checked)
						{
							txtNumTlz.Focus();
							txtNumTlz.SelectAll();
						}
						if (rdbDescTlz.Checked)
						{
							txtDescTlz.Focus();
						}
						if (rdbCustSqlTlz.Checked)
						{
							txtCustSqlTlz.Focus();
							return;
						}
					}
				}
				else
				{
					if (rdbNumFct.Checked)
					{
						txtNumFct.Focus();
						txtNumFct.SelectAll();
					}
					if (rdbDescFct.Checked)
					{
						txtDescFct.Focus();
					}
					if (rdbCustSqlFct.Checked)
					{
						txtCustSqlFct.Focus();
						return;
					}
				}
			}
		}

        private void ClearResults()
        {
            dGrd.DataSource = null;
            UpdateRowHeaderWidth();
            Height = FormHeightMin;
            tslblRecordCnt.Text = "0";
            splitContainer.Panel2Collapsed = true;
        }

        // SetupContextMenus removed - logic moved to Designer and event handlers below

        private void dGrd_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // If the clicked row is NOT part of the current selection, select it exclusively.
                // Otherwise (if it IS selected), keep the selection (allows right-clicking a multi-selection).
                if (!dGrd.Rows[e.RowIndex].Selected)
                {
                    dGrd.ClearSelection();
                    dGrd.Rows[e.RowIndex].Selected = true;
                }

                _rowHeaderMenu.Show(Cursor.Position);
            }
        }

        private void dGrd_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                _cellContextMenu.Show(Cursor.Position);
            }
        }

        private void FilterBySelection_Click(object sender, EventArgs e)
        {
            if (dGrd.CurrentCell != null && dGrd.CurrentCell.Value != null)
            {
                txtGridFilter.Text = dGrd.CurrentCell.Value.ToString();
            }
        }

        #region Context Menu Handlers (Designer)

        private void ctxMenuCell_SelectAll_Click(object sender, EventArgs e)
        {
            dGrd.SelectAll();
        }

        private void ctxMenuCell_Copy_Click(object sender, EventArgs e)
        {
            CopyToClipboard(false);
        }

        private void ctxMenuCell_CopyWithHeaders_Click(object sender, EventArgs e)
        {
            CopyToClipboard(true);
        }

        private async void ctxMenuCell_AdvCopy_Content_Click(object sender, EventArgs e)
        {
             string del = config.GetValue("GENERAL", "COPY_DELIMITER");
             if (string.IsNullOrEmpty(del)) del = "TAB";
             await CopyContentOnlyAsync(false, del);
        }

        private async void ctxMenuCell_AdvCopy_Layout_Click(object sender, EventArgs e)
        {
             string del = config.GetValue("GENERAL", "COPY_DELIMITER");
             if (string.IsNullOrEmpty(del)) del = "TAB";
             await CopyPreserveLayoutAsync(false, del);
        }

        private void ctxMenuCell_Resize_Click(object sender, EventArgs e)
        {
            dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void ctxMenuCell_Clear_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        private void ctxMenuCell_ExportSelected_Click(object sender, EventArgs e)
        {
            ExportSelectedCellsToCsv();
        }

        private void ctxMenuCell_ExportAll_Csv_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Csv);
        }

        private void ctxMenuCell_ExportAll_Json_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Json);
        }

        private void ctxMenuCell_ExportAll_Excel_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.ExcelXml);
        }

        private void ctxMenuCell_Opening(object sender, CancelEventArgs e)
        {
            // Only enable "Filter by selection" if a single cell is active and has a value
            bool canFilter = dGrd.CurrentCell != null && dGrd.CurrentCell.Value != null && !string.IsNullOrEmpty(dGrd.CurrentCell.Value.ToString());

            if (ctxMenuCell_Filter != null) ctxMenuCell_Filter.Enabled = canFilter;

            if (ctxMenuCell_AdvCopy != null) ctxMenuCell_AdvCopy.Visible = IsSelectionDisjointed();
        }

        // Column Header Menu

        private async void ctxMenuCol_ToggleDesc_Click(object sender, EventArgs e)
        {
            _showDescriptions = !_showDescriptions;
            await setHeadersAsync();
            setTabTextFocus();
        }

        private void ctxMenuCol_ExportAll_Csv_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Csv);
        }

        private void ctxMenuCol_ExportAll_Json_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Json);
        }

        private void ctxMenuCol_ExportAll_Excel_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.ExcelXml);
        }

        private void ctxMenuCol_Clear_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        private void ctxMenuCol_Opening(object sender, CancelEventArgs e)
        {
            if (ctxMenuCol_ToggleDesc != null)
            {
                if (_showDescriptions)
                    ctxMenuCol_ToggleDesc.Text = "Show field name in header";
                else
                    ctxMenuCol_ToggleDesc.Text = "Show description in header";
            }
        }

        // Row Header Menu

        private void ctxMenuRow_Copy_Click(object sender, EventArgs e)
        {
            CopyToClipboard(false);
        }

        private void ctxMenuRow_CopyWithHeaders_Click(object sender, EventArgs e)
        {
            CopyToClipboard(true);
        }

        private void ctxMenuRow_CopyInsert_Click(object sender, EventArgs e)
        {
            CopyAsSqlInsert();
        }

        private void ctxMenuRow_ExportSelectedCsv_Click(object sender, EventArgs e)
        {
            ExportSelectedRowsToCsv();
        }

        private void ctxMenuRow_ExportAll_Csv_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Csv);
        }

        private void ctxMenuRow_ExportAll_Json_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.Json);
        }

        private void ctxMenuRow_ExportAll_Excel_Click(object sender, EventArgs e)
        {
            ExportAllResultsAsync(ExportFormat.ExcelXml);
        }

        private void ctxMenuRow_Clear_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        #endregion

        /// <summary>
        /// Generates and copies SQL INSERT statements for the selected rows.
        /// </summary>
        private void CopyAsSqlInsert()
        {
            if (dGrd.SelectedCells.Count == 0) return;

            // Get unique rows
            var rowIndices = dGrd.SelectedCells.Cast<DataGridViewCell>()
                                .Select(c => c.RowIndex)
                                .Distinct()
                                .OrderBy(r => r)
                                .ToList();

            // Determine Table Name
            string tableName = "[TableName]";
            if (tabCtl.SelectedTab == tabFields && rdbTableFld.Checked && !string.IsNullOrWhiteSpace(cmbTableFld.Text))
            {
                tableName = cmbTableFld.Text;
                if (!tableName.StartsWith("[") && !tableName.Contains(" ")) tableName = "[" + tableName + "]";
            }

            // Get Visible Columns
            var cols = dGrd.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).ToList();
            if (cols.Count == 0) return;

            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO {tableName} (");
            sb.Append(string.Join(", ", cols.Select(c => $"[{c.Name}]")));
            sb.AppendLine(") VALUES");

            for (int i = 0; i < rowIndices.Count; i++)
            {
                int rowIndex = rowIndices[i];
                if (rowIndex >= dGrd.RowCount) continue;

                sb.Append("(");
                var cellValues = new List<string>();
                foreach (var col in cols)
                {
                    var val = dGrd[col.Index, rowIndex].Value;
                    cellValues.Add(FormatSqlValue(val));
                }
                sb.Append(string.Join(", ", cellValues));
                sb.Append(")");

                if (i < rowIndices.Count - 1)
                {
                    sb.AppendLine(",");
                }
                else
                {
                    sb.AppendLine(";");
                }
            }

            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());
                GeneralUtils.showToast(0, "INSERT statements copied", "Copy", Screen.FromControl(this));
            }
        }

        private string FormatSqlValue(object value)
        {
            if (value == null || value == DBNull.Value) return "NULL";

            if (value is bool b) return b ? "1" : "0";
            if (IsNumeric(value)) return value.ToString();
            if (value is DateTime dt) return $"'{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")}'";

            // Default: Quote string and escape
            return $"'{value.ToString().Replace("'", "''")}'";
        }

        private bool IsNumeric(object value)
        {
            return value is sbyte || value is byte || value is short || value is ushort ||
                   value is int || value is uint || value is long || value is ulong ||
                   value is float || value is double || value is decimal;
        }

        /// <summary>
        /// Handles copying selected grid content to clipboard, with support for advanced formatting (disjoint selection, custom delimiters).
        /// </summary>
        /// <param name="includeHeaders">Whether to include column headers in the copy.</param>
        private async void CopyToClipboard(bool includeHeaders)
        {
            if (dGrd.SelectedCells.Count == 0) return;

            string delimiterSetting = config.GetValue("GENERAL", "COPY_DELIMITER");
            if (string.IsNullOrEmpty(delimiterSetting)) delimiterSetting = "TAB";

            // Determine if disjointed
            bool isDisjointed = IsSelectionDisjointed();

            // Check preference
            var action = frmClipboardOptions.CopyAction.CopyContent; // Default for non-disjointed

            if (isDisjointed)
            {
                if (IsFullRowSelection())
                {
                    action = frmClipboardOptions.CopyAction.CopyContent;
                }
                else if (_clipboardSessionPreference.HasValue)
                {
                    // Check Session Preference
                    action = _clipboardSessionPreference.Value;
                }
                else
                {
                    // Check Forever Preference
                    string foreverPref = config.GetValue("GENERAL", "CLIPBOARD_DISJOINTED_ACTION");
                    if (!string.IsNullOrEmpty(foreverPref) && Enum.TryParse(foreverPref, out frmClipboardOptions.CopyAction parsedAction))
                    {
                        action = parsedAction;
                    }
                    else
                    {
                        // Ask User
                        using (var form = new frmClipboardOptions())
                        {
                            form.ShowDialog(this);
                            if (form.SelectedAction == frmClipboardOptions.CopyAction.Cancel) return;
                            action = form.SelectedAction;

                            if (form.RememberChoice)
                            {
                                if (form.RememberScope == frmClipboardOptions.MemoryScope.Session)
                                {
                                    _clipboardSessionPreference = action;
                                }
                                else
                                {
                                    config.SetValue("GENERAL", "CLIPBOARD_DISJOINTED_ACTION", action.ToString());
                                    config.Save();

                                    // Log
                                    log.Logger(LogLevel.Info, $"User selected 'Forever' clipboard preference: {action}");
                                }
                            }
                        }
                    }
                }
            }

            // Execute Action
            try
            {
                if (action == frmClipboardOptions.CopyAction.PreserveLayout)
                {
                    await CopyPreserveLayoutAsync(includeHeaders, delimiterSetting);
                }
                else
                {
                    // Copy Content (or Default)
                    // If TAB delimiter and not disjointed (standard case), use native efficient copy
                    if (delimiterSetting == "TAB" && !isDisjointed)
                    {
                        var oldMode = dGrd.ClipboardCopyMode;
                        try
                        {
                            dGrd.ClipboardCopyMode = includeHeaders
                                ? DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
                                : DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

                            DataObject dataObj = dGrd.GetClipboardContent();
                            if (dataObj != null)
                                Clipboard.SetDataObject(dataObj);
                        }
                        finally
                        {
                            dGrd.ClipboardCopyMode = oldMode;
                        }
                    }
                    else
                    {
                        // Custom delimiter OR Disjointed Content Only
                        await CopyContentOnlyAsync(includeHeaders, delimiterSetting);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "Clipboard Error: " + ex.Message);
                GeneralUtils.showToast(2, "Error copying to clipboard: " + ex.Message, "Copy Error", Screen.FromControl(this));
            }
        }

        private bool IsSelectionDisjointed()
        {
            if (dGrd.SelectedCells.Count <= 1) return false;

            int minRow = int.MaxValue, maxRow = int.MinValue;
            int minCol = int.MaxValue, maxCol = int.MinValue;

            foreach (DataGridViewCell cell in dGrd.SelectedCells)
            {
                if (cell.RowIndex < minRow) minRow = cell.RowIndex;
                if (cell.RowIndex > maxRow) maxRow = cell.RowIndex;
                if (cell.ColumnIndex < minCol) minCol = cell.ColumnIndex;
                if (cell.ColumnIndex > maxCol) maxCol = cell.ColumnIndex;
            }

            long expectedCount = (long)(maxRow - minRow + 1) * (maxCol - minCol + 1);
            return dGrd.SelectedCells.Count != expectedCount;
        }

        private bool IsFullRowSelection()
        {
            if (dGrd.SelectedRows.Count == 0) return false;

            var selectedRowIndices = new HashSet<int>();
            foreach (DataGridViewRow row in dGrd.SelectedRows)
            {
                selectedRowIndices.Add(row.Index);
            }

            foreach (DataGridViewCell cell in dGrd.SelectedCells)
            {
                if (!selectedRowIndices.Contains(cell.RowIndex))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetDelimiter(string delimiterSetting)
        {
            switch (delimiterSetting)
            {
                case "Comma (,)": return ",";
                case "Pipe (|)": return "|";
                case "Semicolon (;)": return ";";
                case "Custom...": return config.GetValue("GENERAL", "COPY_DELIMITER_CUSTOM");
                default: return "\t";
            }
        }

        /// <summary>
        /// Copies selection while preserving the visual grid layout (filling gaps).
        /// </summary>
        private async Task CopyPreserveLayoutAsync(bool includeHeaders, string delimiterSetting)
        {
            // 1. Capture data on UI thread
            var cells = dGrd.SelectedCells.Cast<DataGridViewCell>().ToList();
            if (cells.Count == 0) return;

            int minRow = cells.Min(c => c.RowIndex);
            int maxRow = cells.Max(c => c.RowIndex);
            int minCol = cells.Min(c => c.ColumnIndex);
            int maxCol = cells.Max(c => c.ColumnIndex);

            // Get columns info for the range
            var colInfo = new List<(int Index, string Header, bool Visible)>();
            for (int c = minCol; c <= maxCol; c++)
            {
                colInfo.Add((c, dGrd.Columns[c].HeaderText, dGrd.Columns[c].Visible));
            }

            // Capture cell values in a dictionary for fast lookup
            var cellValues = new Dictionary<(int r, int c), string>();
            foreach(var cell in cells)
            {
                cellValues[(cell.RowIndex, cell.ColumnIndex)] = cell.FormattedValue?.ToString() ?? "";
            }

            string delimiter = GetDelimiter(delimiterSetting);

            // 2. Build string in background
            string text = await Task.Run(() =>
            {
                var sb = new StringBuilder();

                if (includeHeaders)
                {
                    var headers = colInfo.Where(c => c.Visible).Select(c => c.Header);
                    sb.AppendLine(string.Join(delimiter, headers));
                }

                for (int r = minRow; r <= maxRow; r++)
                {
                    var rowVals = new List<string>();
                    foreach(var col in colInfo)
                    {
                        if (!col.Visible) continue;

                        if (cellValues.TryGetValue((r, col.Index), out string val))
                        {
                            rowVals.Add(val);
                        }
                        else
                        {
                            rowVals.Add(""); // Gap
                        }
                    }
                    sb.AppendLine(string.Join(delimiter, rowVals));
                }
                return sb.ToString();
            });

            if (!string.IsNullOrEmpty(text))
                Clipboard.SetText(text);
        }

        /// <summary>
        /// Copies only the content of selected cells, collapsing gaps.
        /// </summary>
        private async Task CopyContentOnlyAsync(bool includeHeaders, string delimiterSetting)
        {
             // 1. Capture data on UI thread
            var cells = dGrd.SelectedCells.Cast<DataGridViewCell>().ToList();
            if (cells.Count == 0) return;

            // Group by Row
            var rows = cells.GroupBy(c => c.RowIndex)
                            .OrderBy(g => g.Key)
                            .Select(g => new
                            {
                                RowIndex = g.Key,
                                Cells = g.OrderBy(c => c.ColumnIndex).Select(c => new { Val = c.FormattedValue?.ToString() ?? "", ColIdx = c.ColumnIndex }).ToList()
                            })
                            .ToList();

            var uniqueColIndices = cells.Select(c => c.ColumnIndex).Distinct().OrderBy(c => c).ToList();
            var headerList = new List<string>();
            if (includeHeaders)
            {
                 foreach(var idx in uniqueColIndices)
                 {
                     headerList.Add(dGrd.Columns[idx].HeaderText);
                 }
            }

            string delimiter = GetDelimiter(delimiterSetting);

            // 2. Build string in background
            string text = await Task.Run(() =>
            {
                var sb = new StringBuilder();

                if (includeHeaders)
                {
                    sb.AppendLine(string.Join(delimiter, headerList));
                }

                foreach (var row in rows)
                {
                    var vals = row.Cells.Select(c => c.Val);
                    sb.AppendLine(string.Join(delimiter, vals));
                }
                return sb.ToString();
            });

             if (!string.IsNullOrEmpty(text))
                Clipboard.SetText(text);
        }

        /// <summary>
        /// Opens the configuration dialog or the secret ROT13 tool (Ctrl+Shift).
        /// </summary>
        private async void btnSetup_Click(object sender, EventArgs e)
		{
			// Check if both Ctrl and Shift are pressed
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
                // Open the second form
                frmPassDecrypt rot13Form = new frmPassDecrypt();
                rot13Form.ShowDialog();
			}
			else
			{
				frmConfig frmConfig = new frmConfig();
				frmConfig.StartPosition = FormStartPosition.CenterParent;
				frmConfig.ShowDialog(this);

                // Note: The original code re-checked DB connection manually here too.
                // We'll rely on InitializeDatabaseAsync to handle re-validation and table refresh.

                config.Load(); // Re-load config in case it was changed
                log.ReloadConfig();

				ApplyConfigSettings();
                await InitializeDatabaseAsync(false);
			}
		}


        private async Task setColumnArrayAsync()
		{
            var columns = new List<string>();
            foreach (DataGridViewColumn col in dGrd.Columns)
            {
                if (col.HeaderText != null) columns.Add(col.HeaderText);
            }

            arrayGrdFld.Clear();
            arrayGrdDesc.Clear();

            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;
            bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
            string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
            string pass = useWinAuth ? null : GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

            try
            {
                var descriptions = await _repo.GetColumnDescriptionsAsync(server, database, user, pass, columns);

			    foreach (DataGridViewColumn dataGridViewColumn in dGrd.Columns)
			    {
				    arrayGrdFld.Add(dataGridViewColumn.Name);
                    if (dataGridViewColumn.Index < descriptions.Count)
                    {
				        arrayGrdDesc.Add(descriptions[dataGridViewColumn.Index]);
                    }
                    else
                    {
                        arrayGrdDesc.Add("");
                    }
			    }
            }
            catch (Exception ex)
            {
                log.Logger(LogLevel.Error, "setColumnArrayAsync error: " + ex.Message);
            }
		}

		private async Task setHeadersAsync()
		{
            bool showDesc = _showDescriptions;

            for (int i = 0; i < dGrd.Columns.Count; i++)
            {
                if (i < arrayGrdFld.Count && i < arrayGrdDesc.Count)
                {
                    string field = arrayGrdFld[i].ToString();
                    string desc = arrayGrdDesc[i].ToString();

                    if (showDesc)
                    {
                        dGrd.Columns[i].HeaderText = !string.IsNullOrEmpty(desc) ? desc : field;
                        dGrd.Columns[i].ToolTipText = field;
                    }
                    else
                    {
                        dGrd.Columns[i].HeaderText = field;
                        dGrd.Columns[i].ToolTipText = desc;
                    }
                }
            }

			if (config.GetValue("GENERAL", "RESIZECOLUMNS") == "1")
			{
                string limitStr = config.GetValue("GENERAL", "AUTO_RESIZE_LIMIT");
                int limit = 5000;
                if (string.IsNullOrEmpty(limitStr) || !int.TryParse(limitStr, out limit)) limit = 5000;

                if (dGrd.RowCount <= limit)
                {
                    if (dGrd.VirtualMode)
                    {
                        await _gridContext.EnsureRangeLoadedAsync(0, dGrd.RowCount);
                    }
				    dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                else
                {
                    await ResizeColumnsBasedOnFirstRowsAsync(100);
                }
			}
		}

        /// <summary>
        /// Resizes columns based on the content of the first N rows for performance optimization.
        /// </summary>
        private async Task ResizeColumnsBasedOnFirstRowsAsync(int rowLimit)
        {
            if (dGrd.RowCount == 0) return;

            int rowsToCheck = Math.Min(dGrd.RowCount, rowLimit);

            // In Virtual Mode, we must ensure data is loaded before measuring
            if (dGrd.VirtualMode)
            {
                await _gridContext.EnsureRangeLoadedAsync(0, rowsToCheck);
            }

            const int padding = 14; // Approximate padding for cell content

            // Cache font to avoid property access overhead in loop
            Font font = dGrd.DefaultCellStyle.Font ?? Font;

            foreach (DataGridViewColumn col in dGrd.Columns)
            {
                if (!col.Visible) continue;

                // Measure Header
                int maxWidth = col.HeaderCell.PreferredSize.Width;

                // Measure Cells
                for (int i = 0; i < rowsToCheck; i++)
                {
                    string valueStr = "";
                    if (dGrd.VirtualMode)
                    {
                        var value = _gridContext.GetValue(i, col.Index);
                        valueStr = value?.ToString() ?? "";
                    }
                    else
                    {
                        // Fallback for non-virtual mode
                        var value = dGrd.Rows[i].Cells[col.Index].Value;
                        valueStr = value?.ToString() ?? "";
                    }

                    if (!string.IsNullOrEmpty(valueStr))
                    {
                        int cellWidth = TextRenderer.MeasureText(valueStr, font).Width + padding;
                        if (cellWidth > maxWidth) maxWidth = cellWidth;
                    }
                }

                col.Width = maxWidth;
            }
        }

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (minimize && config.GetValue("GENERAL", "SHOWINTRAY") == "1")
			{
				if (e.CloseReason != CloseReason.TaskManagerClosing && e.CloseReason != CloseReason.WindowsShutDown)
				{
					e.Cancel = true;
				}
				Visible = false;
				return;
			}

            // Save last location
            if (WindowState == FormWindowState.Normal)
            {
                config.SetValue("GENERAL", "LAST_TOP", Top.ToString());
                config.SetValue("GENERAL", "LAST_LEFT", Left.ToString());
                config.Save();
            }

			foreach (Form form in Application.OpenForms)
			{
				if (form is frmUnarchive)
				{
					form.Close();
					break;
				}
			}
			log.Logger(LogLevel.Info, "SMS Search V" + Application.ProductVersion + " terminated");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			minimize = false;
			foreach (Form form in Application.OpenForms)
			{
				if (form is frmUnarchive)
				{
					form.Close();
					break;
				}
			}
			Application.Exit();
		}

		private async void picRefresh_Click(object sender, EventArgs e)
		{
			await PopulateTableList();
		}

		private void picRefresh_MouseEnter(object sender, EventArgs e)
		{
			picRefresh.Image = Resources.refresh2;
		}

		private void picRefresh_MouseLeave(object sender, EventArgs e)
		{
			picRefresh.Image = Resources.refresh;
		}

		private void dGrd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (tabCtl.SelectedTab.Name == "tabFields" && dGrd.Columns.Contains("Table") && e.RowIndex >= 0)
			{
				cmbTableFld.Text = dGrd.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
				rdbTableFld.Checked = true;
				setTabTextFocus();
				btnPopGrid.PerformClick();
			}
		}

		private void tscmbDbDatabase_DropDown(object sender, EventArgs e)
		{
			getDbNames();
		}

		private async void tscmbDbDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			await PopulateTableList();
		}

		private async void getDbNames()
		{
			tscmbDbDatabase.Items.Clear();
			SetBusy(true);

            string server = tscmbDbServer.Text;

            try
            {
                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : GeneralUtils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

                var dbs = await _repo.GetDatabasesAsync(server, user, pass);
                tscmbDbDatabase.Items.AddRange(dbs.ToArray());
                log.Logger(LogLevel.Info, $"getDbNames: Loaded {dbs.Count()} databases");
            }
            catch (Exception ex)
            {
                 log.Logger(LogLevel.Error, "getDbNames error: " + ex.Message);
                 GeneralUtils.showToast(2, "Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", Screen.FromControl(this));
            }
            finally
            {
                SetBusy(false);
            }
		}

        /// <summary>
        /// Cleans SQL text using configured regex rules.
        /// </summary>
		private string CleanSql(string toClean)
		{
            /*
			TSql110Parser _parser;
            Sql110ScriptGenerator _scriptGen;

            bool fQuotedIdenfifiers = false;
            _parser = new TSql110Parser(fQuotedIdenfifiers);
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions();
            options.SqlVersion = SqlVersion.Sql110;
            options.KeywordCasing = KeywordCasing.Uppercase;
            //options.IndentationSize = 1;
            
            
            _scriptGen = new Sql110ScriptGenerator(options);
			*/

			foreach (var rule in _cleanSqlRules)
			{
				toClean = rule.Regex.Replace(toClean, rule.Replacement);
			}

            /*
            // SQL cleanup
            TSqlFragment fragment;
            IList<ParseError> errors;
            StringReader sr = new StringReader(toClean);

            fragment = _parser.Parse(sr, out errors);

            if (errors != null && errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in errors)
                {
                    sb.AppendLine(error.Message);
                    sb.AppendLine("offset " + error.Offset.ToString());
                }
                toClean = sb.ToString();
            }
            else
            {
                String script;
                _scriptGen.GenerateScript(fragment, out script);
                toClean = script;
            }
            */
            //toClean = Regex.Replace(toClean, "(?i)( |)\\b(WHEN)\\b", "\r\n\t$2");
            
        return toClean;
        }

		private void btnCleanSqlFct_Click(object sender, EventArgs e)
		{
			txtCustSqlFct.Text = CleanSql(txtCustSqlFct.Text);
			if (config.GetValue("GENERAL", "COPYCLEANSQL") == "1" && txtCustSqlFct.Text != "")
			{
				Clipboard.SetText(txtCustSqlFct.Text);
				tsInfo.Text = "\"Cleaned SQL\" copied to clipboard";
			}
		}

		private void btnCleanSqlTlz_Click(object sender, EventArgs e)
		{
			txtCustSqlTlz.Text = CleanSql(txtCustSqlTlz.Text);
			if (config.GetValue("GENERAL", "COPYCLEANSQL") == "1" && txtCustSqlTlz.Text != "")
			{
				Clipboard.SetText(txtCustSqlTlz.Text);
				tsInfo.Text = "\"Cleaned SQL\" copied to clipboard";
			}
		}

		private void btnCleanSqlFld_Click(object sender, EventArgs e)
		{
			txtCustSqlFld.Text = CleanSql(txtCustSqlFld.Text);
			if (config.GetValue("GENERAL", "COPYCLEANSQL") == "1" && txtCustSqlFld.Text != "")
			{
				Clipboard.SetText(txtCustSqlFld.Text);
				tsInfo.Text = "\"Cleaned SQL\" copied to clipboard";
			}
		}

		private void btnShowTarget_Click(object sender, EventArgs e)
		{
			toggleTarget();
		}

		private void toggleTarget()
		{
			bool ShowTarget = false;
			foreach (Form form in Application.OpenForms)
			{
				if (form is frmUnarchive)
				{
					ShowTarget = true;
					form.Close();
					btnShowTarget.Checked = false;
					break;
				}
			}
			if (!ShowTarget)
			{
				frmUnarchive frmUnarchive = new frmUnarchive();
				frmUnarchive.Show();
				btnShowTarget.Checked = true;
			}
			setTabTextFocus();
		}

		private void SplitContainer_Paint(object sender, PaintEventArgs e)
		{
			SplitContainer splitContainer = sender as SplitContainer;
			Point[] array = new Point[3];
			Rectangle arg_13_0 = Rectangle.Empty;
			int width = splitContainer.Width;
			int height = splitContainer.Height;
			int splitterDistance = splitContainer.SplitterDistance;
			int splitterWidth = splitContainer.SplitterWidth;
			if (splitContainer.Orientation == Orientation.Horizontal)
			{
				array[0] = new Point(width / 2, splitterDistance + splitterWidth / 2);
				array[1] = new Point(array[0].X - 10, array[0].Y);
				array[2] = new Point(array[0].X + 10, array[0].Y);
				new Rectangle(array[1].X - 2, array[1].Y - 2, 25, 5);
			}
			else
			{
				array[0] = new Point(splitterDistance + splitterWidth / 2, height / 2);
				array[1] = new Point(array[0].X, array[0].Y - 10);
				array[2] = new Point(array[0].X, array[0].Y + 10);
				new Rectangle(array[1].X - 2, array[1].Y - 2, 5, 25);
			}
			Point[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Point location = array2[i];
				location.Offset(-2, -2);
				e.Graphics.FillEllipse(SystemBrushes.ControlDark, new Rectangle(location, new Size(3, 3)));
				location.Offset(1, 1);
				e.Graphics.FillEllipse(SystemBrushes.ControlLight, new Rectangle(location, new Size(3, 3)));
			}
		}

		private void cmbTableFld_TextChanged(object sender, EventArgs e)
		{
			string pattern = "^(SAL|INV|REC)_(HDR|REG|TTL)$";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			MatchCollection matchCollection = regex.Matches(cmbTableFld.Text);
			if (matchCollection.Count == 1)
			{
				chkLastTransaction.Visible = true;
				return;
			}
			chkLastTransaction.Visible = false;
		}

		private void chkLastTransaction_CheckedChanged(object sender, EventArgs e)
		{
			if (chkLastTransaction.Checked)
			{
				rdbTableFld.Checked = true;
				rdbShowRecords.Checked = true;
				setTabTextFocus();
			}
		}

		private void onTop_MouseEnter(object sender, EventArgs e)
		{
			Activate();
		}

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			MouseButtons button = e.Button;
			if (button != MouseButtons.Left)
			{
				return;
			}
			Show();
			BringToFront();
			Focus();
			if (!onTop.Checked)
			{
				TopMost = false;
				return;
			}
			TopMost = true;
		}

        private void txtGridFilter_TextChanged(object sender, EventArgs e)
        {
            _filterDebounceTimer.Stop();
            _filterDebounceTimer.Start();
        }

        private async void _filterDebounceTimer_Tick(object sender, EventArgs e)
        {
            _filterDebounceTimer.Stop();

            string text = txtGridFilter.Text;
            bool hasFilter = !string.IsNullOrWhiteSpace(text);

            if (_highlightMatches)
            {
                lblMatchCount.Visible = hasFilter;
                btnPrevMatch.Visible = hasFilter;
                btnNextMatch.Visible = hasFilter;
                if (!hasFilter)
                {
                    lblMatchCount.Text = "";
                }
            }
            else
            {
                lblMatchCount.Visible = false;
                btnPrevMatch.Visible = false;
                btnNextMatch.Visible = false;
            }

            if (dGrd.VirtualMode)
            {
                // Server-side filtering
                tslblRecordCnt.Text = "Filtering...";
                var columns = new List<string>();
                foreach (DataGridViewColumn col in dGrd.Columns)
                {
                    if (col.Visible) columns.Add(col.Name);
                }

                await _gridContext.ApplyFilterAsync(text, columns);

                if (_highlightMatches && hasFilter)
                {
                    lblMatchCount.Text = "Calculating...";
                    _lastTotalMatchCount = await _gridContext.GetTotalMatchCountAsync();
                    lblMatchCount.Text = $"Found: {_lastTotalMatchCount} matches";
                }
            }
            else if (bindingSource.DataSource != null)
            {
                // Legacy Client-side filtering
                string filter = "";
                string safeText = text.Replace("'", "''");
                if (!string.IsNullOrWhiteSpace(safeText))
                {
                    List<string> criteria = new List<string>();
                    foreach (DataGridViewColumn col in dGrd.Columns)
                    {
                        if (col.Visible)
                        {
                            criteria.Add(string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", col.Name, safeText));
                        }
                    }
                    if (criteria.Count > 0)
                    {
                        filter = string.Join(" OR ", criteria);
                    }
                }
                bindingSource.Filter = filter;

                if (bindingSource.DataSource is DataTable dt)
                    tslblRecordCnt.Text = !string.IsNullOrEmpty(filter) ? $"{bindingSource.Count} / {dt.Rows.Count}" : bindingSource.Count.ToString();
            }
        }

        private void dGrd_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dGrd.CurrentCell != null && e.RowIndex == dGrd.CurrentCell.RowIndex)
            {
                dGrd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            }
            else
            {
                // Reset to default
                dGrd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void dGrd_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (_showRowNumbers)
            {
                var grid = sender as DataGridView;
                var rowIdx = (e.RowIndex + 1).ToString();
                using (var centerFormat = new StringFormat())
                {
                    centerFormat.Alignment = StringAlignment.Far;
                    centerFormat.LineAlignment = StringAlignment.Center;

                    // Calculate position to center vertically, right align in header
                    // Padding 4 pixels from right edge. Use 0 for X to ensure it stays fixed in the header area.
                    var headerBounds = new Rectangle(0, e.RowBounds.Top, grid.RowHeadersWidth - 4, e.RowBounds.Height);
                    e.Graphics.DrawString(rowIdx, grid.DefaultCellStyle.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
                }
            }
        }

        private void UpdateRowHeaderWidth()
        {
            if (!_showRowNumbers)
            {
                dGrd.RowHeadersVisible = false;
                return;
            }

            dGrd.RowHeadersVisible = true;
            int rowCount = dGrd.RowCount;
            if (rowCount == 0) rowCount = 1; // Minimum width

            // Measure string of max row count
            string maxString = rowCount.ToString();
            Size size = TextRenderer.MeasureText(maxString, dGrd.Font);

            // Add some padding
            int width = size.Width + 20;
            if (width < 25) width = 25; // Min width

            dGrd.RowHeadersWidth = width;
        }

        private void dGrd_CurrentCellChanged(object sender, EventArgs e)
        {
            // Force redraw of all rows to update row highlight
            // Or optimize by invalidating only old and new row
            dGrd.Invalidate();

            // Reset "X of Y" text when user manually changes cell, unless we are in the middle of navigation?
            // User requirement: "If the user click off of a result then only show the total count again."
            if (_highlightMatches && lblMatchCount.Visible && lblMatchCount.Text.Contains("of"))
            {
                 lblMatchCount.Text = $"Found: {_lastTotalMatchCount} matches";
            }
        }

        private void dGrd_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (_highlightMatches && e.RowIndex >= 0 && e.ColumnIndex >= 0 && !string.IsNullOrEmpty(txtGridFilter.Text))
            {
                if (e.Value != null)
                {
                    string val = e.Value.ToString();
                    if (val.IndexOf(txtGridFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        e.CellStyle.BackColor = _matchHighlightColor;
                        e.PaintBackground(e.CellBounds, true);
                        e.PaintContent(e.CellBounds);
                        e.Handled = true;
                    }
                }
            }
        }

        private async void btnPrevMatch_Click(object sender, EventArgs e)
        {
            await NavigateMatch(false);
        }

        private async void btnNextMatch_Click(object sender, EventArgs e)
        {
            await NavigateMatch(true);
        }

        /// <summary>
        /// Navigates to the next or previous match in the grid.
        /// Performs a local search first, then falls back to a database search.
        /// </summary>
        /// <param name="forward">If true, searches forward (next); otherwise searches backward (previous).</param>
        private async Task NavigateMatch(bool forward)
        {
            if (dGrd.RowCount == 0) return;
            string filterText = txtGridFilter.Text;
            if (string.IsNullOrEmpty(filterText)) return;

            int startRow = dGrd.CurrentCell != null ? dGrd.CurrentCell.RowIndex : 0;
            int startCol = dGrd.CurrentCell != null ? dGrd.CurrentCell.ColumnIndex : 0;

            int currentRow = startRow;
            bool found = false;

            // Ensure current row is loaded for local check
            await _gridContext.WaitForRowAsync(currentRow);

            // Step 1: Check current row
            if (FindMatchInRow(currentRow, filterText, forward, forward ? startCol + 1 : startCol - 1, out int foundCol))
            {
                dGrd.CurrentCell = dGrd[foundCol, currentRow];
                found = true;
            }
            else
            {
                 // Step 2: DB Search
                 var visibleCols = dGrd.Columns.Cast<DataGridViewColumn>()
                                      .Where(c => c.Visible)
                                      .Select(c => c.Name)
                                      .ToList();

                 int nextRow = await _gridContext.FindMatchRowAsync(filterText, visibleCols, currentRow, forward);

                 if (nextRow == -1)
                 {
                     // Wrap around
                     int wrapStart = forward ? -1 : _gridContext.TotalCount;
                     nextRow = await _gridContext.FindMatchRowAsync(filterText, visibleCols, wrapStart, forward);
                 }

                 if (nextRow != -1)
                 {
                     currentRow = nextRow;
                     await _gridContext.WaitForRowAsync(currentRow);

                     // Find column in new row (Scan all columns)
                     int startOffset = forward ? -1 : dGrd.ColumnCount;

                     if (FindMatchInRow(currentRow, filterText, forward, startOffset, out foundCol))
                     {
                         dGrd.CurrentCell = dGrd[foundCol, currentRow];
                         found = true;
                     }
                 }
            }

            if (found)
            {
                // Force UI update to show the move immediately
                dGrd.Update();

                // Calculate X of Y
                lblMatchCount.Text = "Calculating...";

                // Use cached total count to avoid DB trip
                long total = _lastTotalMatchCount;
                if (total == 0)
                {
                    total = await _gridContext.GetTotalMatchCountAsync();
                    _lastTotalMatchCount = total;
                }

                // Only await the preceding count
                long preceding = await _gridContext.GetPrecedingMatchCountAsync(dGrd.CurrentCell.RowIndex);

                // Add matches in current row up to current col
                long currentMatches = 0;
                for (int c = 0; c <= dGrd.CurrentCell.ColumnIndex; c++)
                {
                    if (dGrd.Columns[c].Visible)
                    {
                         var v = dGrd[c, dGrd.CurrentCell.RowIndex].Value;
                         if (v != null && v.ToString().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                         {
                             currentMatches++;
                         }
                    }
                }

                long matchIndex = preceding + currentMatches;
                lblMatchCount.Text = $"Match {matchIndex} of {total}";
            }
        }

        private bool FindMatchInRow(int rowIndex, string filterText, bool forward, int startColIndex, out int foundColIndex)
        {
            foundColIndex = -1;
            int colCount = dGrd.ColumnCount;

            if (forward)
            {
                for (int c = Math.Max(0, startColIndex); c < colCount; c++)
                {
                     if (CheckCellMatch(c, rowIndex, filterText))
                     {
                         foundColIndex = c;
                         return true;
                     }
                }
            }
            else
            {
                for (int c = Math.Min(colCount - 1, startColIndex); c >= 0; c--)
                {
                     if (CheckCellMatch(c, rowIndex, filterText))
                     {
                         foundColIndex = c;
                         return true;
                     }
                }
            }
            return false;
        }

        private bool CheckCellMatch(int colIndex, int rowIndex, string filterText)
        {
             if (!dGrd.Columns[colIndex].Visible) return false;
             var val = dGrd[colIndex, rowIndex].Value;
             return val != null && val.ToString().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Exports all results from the current query to a file.
        /// </summary>
        /// <param name="format">The export format (CSV, JSON, Excel XML).</param>
        private async void ExportAllResultsAsync(ExportFormat format)
        {
            if (dGrd.RowCount == 0) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                string ext = format == ExportFormat.Json ? "json" : (format == ExportFormat.ExcelXml ? "xml" : "csv");
                string filter = format == ExportFormat.Json ? "JSON files (*.json)|*.json" : (format == ExportFormat.ExcelXml ? "Excel XML (*.xml)|*.xml" : "CSV files (*.csv)|*.csv");

                sfd.Filter = $"{filter}|All files (*.*)|*.*";
                sfd.FileName = $"SMS_Search_Export_All_{DateTime.Now:yyyyMMdd_HHmmss}.{ext}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool includeHeaders = true;
                    if (format == ExportFormat.Csv || format == ExportFormat.ExcelXml)
                    {
                         includeHeaders = MessageBox.Show("Include headers in export?", "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    }

                    SetBusy(true);

                    // Initialize Cancellation Token
                    _cts?.Dispose();
                    _cts = new CancellationTokenSource();
                    CancellationToken token = _cts.Token;

                    try
                    {
                        var headerMap = new Dictionary<string, string>();
                        foreach(DataGridViewColumn col in dGrd.Columns)
                        {
                            headerMap[col.Name] = col.HeaderText;
                        }

                        Stopwatch sw = Stopwatch.StartNew();

                        if (format == ExportFormat.Json)
                            await _gridContext.ExportToJsonAsync(sfd.FileName, headerMap, token);
                        else if (format == ExportFormat.ExcelXml)
                            await _gridContext.ExportToExcelXmlAsync(sfd.FileName, headerMap, token);
                        else
                            await _gridContext.ExportToCsvAsync(sfd.FileName, headerMap, includeHeaders, token);

                        sw.Stop();

                        tslblInfo.Text = $"Exported in {sw.ElapsedMilliseconds} ms";
                        GeneralUtils.showToast(0, "Export successful", "Export", Screen.FromControl(this));
                    }
                    catch (OperationCanceledException)
                    {
                        tslblInfo.Text = "Export cancelled.";
                        // Clean up partial file?
                        try { if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName); } catch { }
                    }
                    catch (Exception ex)
                    {
                        GeneralUtils.showToast(2, "Error exporting: " + ex.Message, "Export Error", Screen.FromControl(this));
                    }
                    finally
                    {
                        SetBusy(false);
                    }
                }
            }
        }

        private void ExportSelectedRowsToCsv()
        {
            if (dGrd.SelectedCells.Count == 0) return;

            // Get unique rows
            var rowIndices = dGrd.SelectedCells.Cast<DataGridViewCell>()
                                .Select(c => c.RowIndex)
                                .Distinct()
                                .OrderBy(r => r)
                                .ToList();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = "SMS_Search_Export_SelectedRows_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool includeHeaders = MessageBox.Show("Include headers in export?", "Export CSV", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                    SetBusy(true);
                    try
                    {
                        using (var writer = new StreamWriter(sfd.FileName))
                        {
                            // Headers (All visible columns)
                            var cols = dGrd.Columns.Cast<DataGridViewColumn>()
                                           .Where(c => c.Visible)
                                           .OrderBy(c => c.DisplayIndex) // Visual order
                                           .ToList();

                            if (includeHeaders)
                            {
                                writer.WriteLine(string.Join(",", cols.Select(c => "\"" + c.HeaderText.Replace("\"", "\"\"") + "\"")));
                            }

                            foreach (int r in rowIndices)
                            {
                                var values = new List<string>();
                                foreach (var col in cols)
                                {
                                    var val = dGrd[col.Index, r].Value; // Use indexer to trigger CellValueNeeded
                                    string sVal = val == null ? "" : val.ToString();
                                    values.Add("\"" + sVal.Replace("\"", "\"\"") + "\"");
                                }
                                writer.WriteLine(string.Join(",", values));
                            }
                        }
                        GeneralUtils.showToast(0, "Export successful", "Export", Screen.FromControl(this));
                    }
                    catch (Exception ex)
                    {
                        GeneralUtils.showToast(2, "Error exporting: " + ex.Message, "Export Error", Screen.FromControl(this));
                    }
                    finally
                    {
                        SetBusy(false);
                    }
                }
            }
        }

        private void ExportSelectedCellsToCsv()
        {
             if (dGrd.SelectedCells.Count == 0) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = "SMS_Search_Export_SelectedCells_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool includeHeaders = MessageBox.Show("Include headers in export?", "Export CSV", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                    SetBusy(true);
                    try
                    {
                        using (var writer = new StreamWriter(sfd.FileName))
                        {
                            // Determine bounds
                            var selectedCells = dGrd.SelectedCells.Cast<DataGridViewCell>().ToList();
                            // Sort columns by visual order
                            var uniqueColIndices = selectedCells.Select(c => c.ColumnIndex)
                                                                .Distinct()
                                                                .OrderBy(i => dGrd.Columns[i].DisplayIndex)
                                                                .ToList();
                            var uniqueRowIndices = selectedCells.Select(c => c.RowIndex)
                                                                .Distinct()
                                                                .OrderBy(r => r)
                                                                .ToList();

                            // Map column index to header text
                            if (includeHeaders)
                            {
                                var headers = uniqueColIndices.Select(i => "\"" + dGrd.Columns[i].HeaderText.Replace("\"", "\"\"") + "\"");
                                writer.WriteLine(string.Join(",", headers));
                            }

                            foreach (int r in uniqueRowIndices)
                            {
                                var rowValues = new List<string>();
                                foreach (int c in uniqueColIndices)
                                {
                                    // Check if this specific cell is selected
                                    if (dGrd[c, r].Selected)
                                    {
                                        var val = dGrd[c, r].Value;
                                        string sVal = val == null ? "" : val.ToString();
                                        rowValues.Add("\"" + sVal.Replace("\"", "\"\"") + "\"");
                                    }
                                    else
                                    {
                                        rowValues.Add("\"\""); // Empty for unselected cell in the bounding box
                                    }
                                }
                                writer.WriteLine(string.Join(",", rowValues));
                            }
                        }
                        GeneralUtils.showToast(0, "Export successful", "Export", Screen.FromControl(this));
                    }
                    catch (Exception ex)
                    {
                        GeneralUtils.showToast(2, "Error exporting: " + ex.Message, "Export Error", Screen.FromControl(this));
                    }
                    finally
                    {
                        SetBusy(false);
                    }
                }
            }
        }

        // Put DB online
        private void DbOnline(string database)
        {
            string queryString = "ALTER DATABASE " + database + " SET OFFLINE WITH ROLLBACK IMMEDIATE; ALTER DATABASE " + database + " SET ONLINE WITH ROLLBACK IMMEDIATE";

            using (SqlConnection connection = new SqlConnection(
                GetConnString(tscmbDbServer.Text, "master")))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }

        private void ReconnectDB_Click(object sender, EventArgs e)
        {
            DbOnline(tscmbDbDatabase.Text);
        }

	}
}
