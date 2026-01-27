using DbConn;
// using Ini;
using Log;
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

namespace SMS_Search
{
	public partial class frmMain : Form
	{
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

		private int FormHeightMin = 275 + SystemInformation.FrameBorderSize.Height * 2;
		private int FormHeightExpanded = 600;
		private int FormWidthMin = 600 + SystemInformation.FrameBorderSize.Width * 2;
		private int GridMinRowHeight = 92;
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
        private System.Windows.Forms.Timer _filterDebounceTimer;
        private ContextMenuStrip _headerContextMenu;

        private bool _highlightMatches = false;
        private Color _matchHighlightColor = Color.Yellow;
        private long _lastTotalMatchCount = 0;
        private bool _showRowNumbers = false;

		public frmMain(string[] Params)
		{
			InitializeComponent();
            _gridContext = new VirtualGridContext(_repo);
            _gridContext.DataReady += _gridContext_DataReady;
            _gridContext.LoadError += _gridContext_LoadError;

            dGrd.CellValueNeeded += dGrd_CellValueNeeded;
            dGrd.ColumnHeaderMouseClick += dGrd_ColumnHeaderMouseClick;
            dGrd.RowPrePaint += dGrd_RowPrePaint;
            dGrd.RowPostPaint += dGrd_RowPostPaint;
            dGrd.CellPainting += dGrd_CellPainting;
            dGrd.CurrentCellChanged += dGrd_CurrentCellChanged;

            _filterDebounceTimer = new System.Windows.Forms.Timer();
            _filterDebounceTimer.Interval = 500;
            _filterDebounceTimer.Tick += _filterDebounceTimer_Tick;

            SetupGridContextMenu();

			log.Logger(LogLevel.Info, "SMS Search V" + Application.ProductVersion + " initialized");
			MinimumSize = new Size(FormWidthMin, FormHeightMin);
			Height = FormHeightMin;
			Width = FormWidthMin;
			splitContainer.Panel2MinSize = splitContainer.Height - splitContainer.Panel1.Height - splitContainer.SplitterWidth - 3;
			StartPosition = FormStartPosition.Manual;
            toolStrip.Renderer = new ToolStripSystemRenderer();

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

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			FormHeightMin = splitContainer.SplitterDistance + 95;
		}

        private void frmMain_Load(object sender, EventArgs e)
		{
			if (config.GetValue("GENERAL", "EULA") != "1")
			{
				frmEula frmEula = new frmEula();
				frmEula.ShowDialog();
			}

            try
            {
                string retentionStr = config.GetValue("GENERAL", "LOG_RETENTION");
                int retention = 14;
                if (!string.IsNullOrEmpty(retentionStr) && int.TryParse(retentionStr, out int r)) retention = r;
                log.CleanupLogs(retention);
            }
            catch { }

            if (config.GetValue("GENERAL", "CHECKUPDATE") == "1")
            {
                CheckUpdateAsync();
            }

            bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
            string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
            string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

            if (!dbConn.TestDbConn(config.GetValue("CONNECTION", "SERVER"), config.GetValue("CONNECTION", "DATABASE"), false, user, pass) || !File.Exists(frmMain.ConfigFilePath))
            {
                using (frmConfig frmConfig = new frmConfig())
                {
                    frmConfig.ForceDatabaseSetup = true;
                    if (frmConfig.ShowDialog(this) != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }
                config.Load();
                log.ReloadConfig();
            }

			ValidateConfigFile();

			splitContainer.Paint += new PaintEventHandler(SplitContainer_Paint);
			tslblInfo.Text = "";
			setJulianDate();
			Text = Text + " - v" + Application.ProductVersion;
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
			if (config.GetValue("UNARCHIVE", "SHOWTARGET") == "1")
			{
				frmUnarchive frmUnarchive = new frmUnarchive();
				frmUnarchive.Show();
				btnShowTarget.Checked = true;
			}

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dGrd, new object[] { true });

			//Show();
			//Focus();
			//BringToFront();
		}

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

        protected override void OnShown(EventArgs e)
		{
            base.OnShown(e);
            log.Logger(LogLevel.Info, "frmMain_OnShown: Executing");

            string lastRunVersion = config.GetValue("GENERAL", "LAST_RUN_VERSION");
            string currentVersion = Application.ProductVersion;

            if (lastRunVersion != currentVersion)
            {
                if (!string.IsNullOrEmpty(lastRunVersion))
                {
                    Utils.showToast(0, "Updated from v" + lastRunVersion + " to v" + currentVersion, "Update", Screen.FromControl(this));
                }
                config.SetValue("GENERAL", "LAST_RUN_VERSION", currentVersion);
                config.Save();
            }

			setTabTextFocus();
		}

        private void SetBusy(bool busy)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetBusy(busy)));
                return;
            }

            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            tsProgressBar.Visible = busy;
            btnPopGrid.Enabled = !busy;
            picRefresh.Enabled = !busy;
            tscmbDbDatabase.Enabled = !busy;
        }

        public string GetConnString(string DbServer, string DbDatabase)
		{
            if (config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH")))
            {
			    return "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" + DbDatabase + ";Data Source=" + DbServer;
            }
            else
            {
                string user = config.GetValue("CONNECTION", "SQLUSER");
                string pass = Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));
                return "Data Source=" + DbServer + ";Initial Catalog=" + DbDatabase + ";User ID=" + user + ";Password=" + pass + ";Persist Security Info=False;";
            }
		}

        /// <summary>
        /// 
        /// </summary>
        private async void PopulateTableList()
		{
			string text = cmbTableFld.Text.ToString();
            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;

			SetBusy(true);

            try
            {
                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

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
                // MessageBox.Show(ex.Message, "SQL error encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                // Suppress error message box during auto-populate? The original code showed it.
            }
            finally
            {
			    setTabTextFocus();
                SetBusy(false);
            }
		}

        /// <summary>
        /// Validate Configuration file for SQL connection and set runtime configurations
        /// </summary>
        private void ValidateConfigFile()
		{
			SetBusy(true);

			// Read connection values once
			string server = config.GetValue("CONNECTION", "SERVER");
			string database = config.GetValue("CONNECTION", "DATABASE");

            bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
            string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
            string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

			// If connection test fails or config file missing, show config dialog
			if (!dbConn.TestDbConn(server, database, true, user, pass) || !File.Exists(frmMain.ConfigFilePath))
			{
				using (var cfg = new frmConfig())
				{
					cfg.ShowDialog(this);
				}
                config.Load(); // Re-load config in case it was changed
                log.ReloadConfig();

				// Re-read values in case the user updated configuration
				server = config.GetValue("CONNECTION", "SERVER");
				database = config.GetValue("CONNECTION", "DATABASE");
			}

			// Defensive UI updates: only touch controls if form and controls are valid
			if (!this.IsDisposed && !this.Disposing)
			{
				if (tscmbDbServer != null) tscmbDbServer.Text = server ?? string.Empty;
				if (tscmbDbDatabase != null) tscmbDbDatabase.Text = database ?? string.Empty;
			}

			PopulateTableList();

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
				chkToggleDesc.Checked = true;
			}
			else
			{
				chkToggleDesc.Checked = false;
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

			SetBusy(false);
		}

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

        private void CreateConfigFile()
		{
			if (File.Exists(frmMain.ConfigFilePath))
			{
				File.Delete(frmMain.ConfigFilePath);
			}
			WriteConfigConn(tscmbDbServer.Text, tscmbDbDatabase.Text);
		}

        private async void btnPopGrid_Click(object sender, EventArgs e)
		{
			SetBusy(true);
			tslblInfo.Text = "";

            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;

            bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
            string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
            string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

            _gridContext.SetConnection(server, database, user, pass);

			bool SQLConnected = await _repo.TestConnectionAsync(server, database, user, pass);
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
                var queryResult = _queryBuilder.Build(criteria);

                Stopwatch sw = Stopwatch.StartNew();

				try
				{
                    // 1. Get Schema to setup columns
                    var schemaDt = await _gridContext.GetSchemaAsync(queryResult.Sql, queryResult.Parameters);
                    foreach (DataColumn col in schemaDt.Columns)
                    {
                        dGrd.Columns.Add(new DataGridViewTextBoxColumn { Name = col.ColumnName, HeaderText = col.ColumnName });
                    }

                    // 2. Load Data (Count)
                    dGrd.VirtualMode = true;
                    tslblRecordCnt.Text = "Loading...";
                    string defaultSort = dGrd.Columns.Count > 0 ? dGrd.Columns[0].Name : null;
                    await _gridContext.LoadAsync(queryResult.Sql, queryResult.Parameters, defaultSort);

                    if (dGrd.Columns.Count > 0)
                    {
                         dGrd.Columns[0].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                    }

                    sw.Stop();
                    log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, _gridContext.TotalCount, true);
				}
				catch (Exception ex)
				{
                    // Fallback to legacy load
                    log.Logger(LogLevel.Warning, "Virtual Mode Load failed, attempting legacy load: " + ex.Message);
                    try
                    {
                        dGrd.VirtualMode = false;
                        dGrd.Rows.Clear();
                        dGrd.Columns.Clear();

                        var dataTable = await _repo.ExecuteQueryAsync(server, database, user, pass, queryResult.Sql, queryResult.Parameters);

                        bindingSource.DataSource = dataTable;
                        dGrd.DataSource = bindingSource;
                        UpdateRowHeaderWidth();

                        sw.Stop();
                        tslblRecordCnt.Text = dataTable.Rows.Count.ToString();
                        log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, dataTable.Rows.Count, true);
                    }
                    catch (Exception ex2)
                    {
                        sw.Stop();
                        MessageBox.Show("Virtual Load Error: " + ex.Message + "\n\nLegacy Load Error: " + ex2.Message, "SQL error encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        log.LogSql(queryResult.Sql, sw.ElapsedMilliseconds, 0, false, ex2.Message);
                    }
				}
				finally
				{
                    // Resize columns based on headers first
					dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
				}
				
                if (Height < FormHeightMin + GridMinRowHeight && dGrd.RowCount > 0)
				{
					Height = FormHeightExpanded;
				}
				else
				{
					if (dGrd.RowCount < 1)
					{
						Height = FormHeightMin;
					}
				}
				if (dGrd.RowCount < 1)
				{
					tslblInfo.Text = "Query returned no records!";
					tslblInfo.ForeColor = Color.Red;
				}
			}
			setTabTextFocus();
			await setColumnArrayAsync();
			await setHeadersAsync();
			SetBusy(false);
		}

        private void dGrd_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < _gridContext.TotalCount)
            {
                e.Value = _gridContext.GetValue(e.RowIndex, e.ColumnIndex);
            }
        }

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

        private void _gridContext_LoadError(object sender, string e)
        {
             if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => _gridContext_LoadError(sender, e)));
                return;
            }
            MessageBox.Show(e, "Error loading data", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void dGrd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _headerContextMenu.Show(Cursor.Position);
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
				MessageBox.Show("You must specify a valid Julian date (YYYYDDD).", "Julian date error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            tslblRecordCnt.Text = "0";
            Height = FormHeightMin;
        }

        private void SetupGridContextMenu()
        {
            var menu = new ContextMenuStrip();

            // 1. Select all
            var itemSelectAll = menu.Items.Add("Select all");
            itemSelectAll.Click += (s, e) => dGrd.SelectAll();

            // 2. Copy
            var itemCopy = menu.Items.Add("Copy");
            itemCopy.Click += (s, e) => CopyToClipboard(false);

            // 3. Copy with headers
            var itemCopyWithHeaders = menu.Items.Add("Copy with headers");
            itemCopyWithHeaders.Click += (s, e) => CopyToClipboard(true);

            // 4. Separator
            menu.Items.Add(new ToolStripSeparator());

            // 5. Resize content to fit
            var itemResize = menu.Items.Add("Resize to fit content");
            itemResize.Click += (s, e) => dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            // 6. Clear result
            var itemClear = menu.Items.Add("Clear result");
            itemClear.Click += (s, e) => ClearResults();

            dGrd.ContextMenuStrip = menu;
            _headerContextMenu = menu; // Reuse for header
        }

        private void CopyToClipboard(bool includeHeaders)
        {
            if (dGrd.GetCellCount(DataGridViewElementStates.Selected) > 0)
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
        }

        private void btnSetup_Click(object sender, EventArgs e)
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

                bool useWinAuth = config.GetValue("CONNECTION", "WINDOWSAUTH") == "1" || string.IsNullOrEmpty(config.GetValue("CONNECTION", "WINDOWSAUTH"));
                string user = useWinAuth ? null : config.GetValue("CONNECTION", "SQLUSER");
                string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

				if (!dbConn.TestDbConn(config.GetValue("CONNECTION", "SERVER"), config.GetValue("CONNECTION", "DATABASE"), true, user, pass) || !File.Exists(frmMain.ConfigFilePath))
				{
					//frmConfig frmConfig = new frmConfig();
					frmConfig.ShowDialog();
				}
                config.Load(); // Re-load config in case it was changed
                log.ReloadConfig();

				ValidateConfigFile();
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
            string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

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
            bool showDesc = chkToggleDesc.Checked;

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

                int firstDisplayedScrollingColumnIndex = dGrd.FirstDisplayedScrollingColumnIndex;

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

			    if (firstDisplayedScrollingColumnIndex >= 0)
			    {
				    dGrd.FirstDisplayedScrollingColumnIndex = firstDisplayedScrollingColumnIndex;
			    }
			}
		}

        private async Task ResizeColumnsBasedOnFirstRowsAsync(int rowLimit)
        {
            if (dGrd.RowCount == 0) return;

            // In Virtual Mode, we must ensure data is loaded before measuring
            if (dGrd.VirtualMode)
            {
                await _gridContext.WaitForRowAsync(0);
            }

            int rowsToCheck = Math.Min(dGrd.RowCount, rowLimit);

            foreach (DataGridViewColumn col in dGrd.Columns)
            {
                if (!col.Visible) continue;

                // Measure Header
                int maxWidth = col.HeaderCell.PreferredSize.Width;

                // Measure Cells
                for (int i = 0; i < rowsToCheck; i++)
                {
                    var cell = dGrd.Rows[i].Cells[col.Index];
                    int cellWidth = cell.PreferredSize.Width;
                    if (cellWidth > maxWidth) maxWidth = cellWidth;
                }

                col.Width = maxWidth;
            }
        }

		private async void chkToggleDesc_CheckedChanged(object sender, EventArgs e)
		{
			await setHeadersAsync();
			setTabTextFocus();
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

		private void picRefresh_Click(object sender, EventArgs e)
		{
			PopulateTableList();
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

		private void tscmbDbDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateTableList();
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
                string pass = useWinAuth ? null : Utils.Decrypt(config.GetValue("CONNECTION", "SQLPASSWORD"));

                var dbs = await _repo.GetDatabasesAsync(server, user, pass);
                foreach(var db in dbs)
                {
                    tscmbDbDatabase.Items.Add(db);
                }
                log.Logger(LogLevel.Info, $"getDbNames: Loaded {dbs.Count()} databases");
            }
            catch (Exception ex)
            {
                 log.Logger(LogLevel.Error, "getDbNames error: " + ex.Message);
                 MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                SetBusy(false);
            }
		}

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

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtGridFilter.Text = "";
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

        private async Task NavigateMatch(bool forward)
        {
            if (dGrd.RowCount == 0) return;
            string filterText = txtGridFilter.Text;
            if (string.IsNullOrEmpty(filterText)) return;

            int startRow = dGrd.CurrentCell != null ? dGrd.CurrentCell.RowIndex : 0;
            int startCol = dGrd.CurrentCell != null ? dGrd.CurrentCell.ColumnIndex : 0;

            int currentRow = startRow;
            int currentCol = startCol;

            bool found = false;

            // Simple client-side search because grid is filtered to only show matching rows.
            // We just need to find the next cell that matches.
            // Warning: If rows are many, we might need to fetch data.
            // But since grid is virtual, accessing dGrd[col, row].Value triggers LoadPage.
            // This might be slow if we traverse many rows.
            // However, every row has a match (since filtered). So we will find one very quickly (in current row or next row).

            // Search limit to avoid infinite loops or massive fetches (though every row should have a match)
            int rowsChecked = 0;
            int maxRowsToCheck = dGrd.RowCount;

            while (rowsChecked < maxRowsToCheck)
            {
                // Advance col
                if (forward)
                {
                    currentCol++;
                    if (currentCol >= dGrd.ColumnCount)
                    {
                        currentCol = 0;
                        currentRow++;
                        if (currentRow >= dGrd.RowCount) currentRow = 0; // Wrap around? Or stop? User didn't specify. Wrap is good.
                    }
                }
                else
                {
                    currentCol--;
                    if (currentCol < 0)
                    {
                        currentCol = dGrd.ColumnCount - 1;
                        currentRow--;
                        if (currentRow < 0) currentRow = dGrd.RowCount - 1;
                    }
                }

                // Check match
                // Only check visible columns
                if (dGrd.Columns[currentCol].Visible)
                {
                    var val = dGrd[currentCol, currentRow].Value;
                    // Accessing .Value triggers VirtualGridContext.GetValue -> RequestPage -> Async?
                    // VirtualGridContext.GetValue returns null if not in cache and triggers RequestPage.
                    // If it returns null, we can't check it immediately.
                    // We need to wait for data?
                    // This implementation of VirtualGridContext is async-void for RequestPage.
                    // We can't await it easily.
                    // BUT: RequestPage fetches a page (100 rows).
                    // If we hit a null value, we should probably wait.
                    // Since we can't await GetValue, this loop will fail if data isn't loaded.

                    if (val == null)
                    {
                        await _gridContext.WaitForRowAsync(currentRow);
                        val = dGrd[currentCol, currentRow].Value;
                    }

                    if (val != null && val.ToString().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        dGrd.CurrentCell = dGrd[currentCol, currentRow];
                        found = true;
                        break;
                    }
                }

                // If we wrapped around to start, stop
                if (currentRow == startRow && currentCol == startCol) break;

                // Optimization: If we moved to a new row, and we know every row has a match, we should find it in this row.
                // So rowsChecked increment is effectively checking how many cells? No.
                // Just guard against infinite loop.
                if (forward && currentCol == 0) rowsChecked++;
                if (!forward && currentCol == dGrd.ColumnCount - 1) rowsChecked++;
            }

            if (found)
            {
                // Calculate X of Y
                lblMatchCount.Text = "Calculating...";
                long total = await _gridContext.GetTotalMatchCountAsync();
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

        private async void btnExport_Click(object sender, EventArgs e)
        {
            if (dGrd.RowCount == 0) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = "SMS_Search_Export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool includeHeaders = MessageBox.Show("Include headers in export?", "Export CSV", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                    SetBusy(true);
                    try
                    {
                        var headerMap = new Dictionary<string, string>();
                        foreach(DataGridViewColumn col in dGrd.Columns)
                        {
                            headerMap[col.Name] = col.HeaderText;
                        }

                        await _gridContext.ExportToCsvAsync(sfd.FileName, headerMap, includeHeaders);
                        Utils.showToast(0, "Export successful", "Export", Screen.FromControl(this));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error exporting: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
