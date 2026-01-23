using DbConn;
// using Ini;
using Log;
// using Microsoft.SqlServer.TransactSql.ScriptDom;
using SMS_Search.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
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

		private BackgroundWorker backgroundWorker;
		private int FormHeightMin = 275 + SystemInformation.FrameBorderSize.Height * 2;
		private int FormHeightExpanded = 600;
		private int FormWidthMin = 600 + SystemInformation.FrameBorderSize.Width * 2;
		private int GridMinRowHeight = 92;
		private bool minimize = true;
		private bool keyPressHandled;
		private Logfile log = new Logfile();
		private SqlDataAdapter dataAdapter = new SqlDataAdapter();
		private static string ConfigFilePath = ".\\SMS Search.json";
		private ConfigManager config = new ConfigManager(frmMain.ConfigFilePath);
		private static UpdateChecker Versions = new UpdateChecker();
		private dbConnector dbConn = new dbConnector();
		private string FctFields = "";
		private string TlzFields = "";
		private ArrayList arrayGrdFld = new ArrayList();
		private ArrayList arrayGrdDesc = new ArrayList();

		public frmMain(string[] Params)
		{
			InitializeComponent();
			log.Logger(0, "SMS Search V" + Application.ProductVersion + " initialized");
			MinimumSize = new Size(FormWidthMin, FormHeightMin);
			Height = FormHeightMin;
			Width = FormWidthMin;
			splitContainer.Panel2MinSize = splitContainer.Height - splitContainer.Panel1.Height - splitContainer.SplitterWidth - 3;
			StartPosition = FormStartPosition.Manual;
			Top = (Screen.PrimaryScreen.WorkingArea.Height - FormHeightExpanded) / 2;
			Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
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

            if (config.GetValue("GENERAL", "CHECKUPDATE") == "1")
            {
                CheckUpdateAsync();
            }

            if (!dbConn.TestDbConn(config.GetValue("CONNECTION", "SERVER"), config.GetValue("CONNECTION", "DATABASE"), false) || !File.Exists(frmMain.ConfigFilePath))
            {
                frmConfig frmConfig = new frmConfig();
                frmConfig.ShowDialog();
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
			Show();
			Focus();
			BringToFront();
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

        private void frmMain_Shown(object sender, EventArgs e)
		{
			setTabTextFocus();
		}

        public string GetConnString(string DbServer, string DbDatabase)
		{
			return "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" + DbDatabase + ";Data Source=" + DbServer;
		}

        /// <summary>
        /// 
        /// </summary>
        private void PopulateTableList()
		{
			if (backgroundWorker.IsBusy) return;
			string text = cmbTableFld.Text.ToString();
			Cursor = Cursors.WaitCursor;
            backgroundWorker.RunWorkerCompleted += PopulateTableList_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(new Tuple<string, string, string, string>("PopulateTableList", tscmbDbServer.Text, tscmbDbDatabase.Text, text));
			setTabTextFocus();
		}

        /// <summary>
        /// Validate Configuration file for SQL connection and set runtime configurations
        /// </summary>
        private void ValidateConfigFile()
		{
			Cursor = Cursors.WaitCursor;

			// Read connection values once
			string server = config.GetValue("CONNECTION", "SERVER");
			string database = config.GetValue("CONNECTION", "DATABASE");

			// If connection test fails or config file missing, show config dialog
			if (!dbConn.TestDbConn(server, database, true) || !File.Exists(frmMain.ConfigFilePath))
			{
				using (var cfg = new frmConfig())
				{
					cfg.ShowDialog();
				}
                config.Load(); // Re-load config in case it was changed

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

			if (config.GetValue("QUERY", "FUNCTION") == "")
			{
				FctFields = "F1063, F1064, F1051, F1050, F1081";
			}
			else
			{
				FctFields = config.GetValue("QUERY", "FUNCTION");
			}

			if (config.GetValue("QUERY", "TOTALIZER") == "")
			{
				TlzFields = "F1034, F1039, F1128, F1129, F1179, F1253, F1710, F1131, F1048, F1709";
			}
			else
			{
				TlzFields = config.GetValue("QUERY", "TOTALIZER");
			}

            LoadCleanSqlRules();

			Cursor = Cursors.Default;
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
                    log.Logger(1, "Error compiling regex for Clean SQL rule: " + rule.Pattern + " - " + ex.Message);
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
			Cursor = Cursors.WaitCursor;
			tslblInfo.Text = "";

            string server = tscmbDbServer.Text;
            string database = tscmbDbDatabase.Text;

			bool SQLConnected = await dbConn.TestDbConnAsync(server, database);
			arrayGrdDesc.Clear();
			arrayGrdDesc.TrimToSize();
			arrayGrdFld.Clear();
			arrayGrdFld.TrimToSize();
			dGrd.DataSource = null;
			tslblRecordCnt.Text = "0";

			if (SQLConnected)
			{
				string text = CompileSqlSelect();
				bool SQLError = false;
				dGrd.DataSource = bindingSource;
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.InvariantCulture;

				try
				{
                    string connString = GetConnString(server, database);

                    await Task.Run(() =>
                    {
					    dataAdapter = new SqlDataAdapter(text, connString);
					    new SqlCommandBuilder(dataAdapter);
					    dataAdapter.Fill(dataTable);
                    });

					bindingSource.DataSource = dataTable;
					log.Logger(0, "SQL query executed");
					log.Logger(0, text);
				}
				catch (Exception ex)
				{
                    // Unwrap AggregateException if present, though Task.Run with await usually throws the inner exception.
                    Exception inner = ex;
                    if (ex is AggregateException ae) inner = ae.InnerException;

                    if (inner is SqlException sqlEx)
                    {
					    SQLError = true;
					    MessageBox.Show(sqlEx.Message, "SQL error encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					    log.Logger(0, "SQL query executed");
					    log.Logger(0, text);
					    log.Logger(1, sqlEx.Message);
                    }
                    else
                    {
                        throw;
                    }
				}
				finally
				{
					dataAdapter.Dispose();
					dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
					if (dGrd.RowCount > 0 && !SQLError)
					{
						dGrd.CurrentCell = dGrd[0, 0];
						log.Logger(0, "SQL records returned: " + dGrd.RowCount.ToString());
					}
				}

				tslblRecordCnt.Text = dGrd.RowCount.ToString();
				
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
			setHeaders();
			Cursor = Cursors.Default;
		}

        #region Compile SQL Select
        /// <summary>
        /// Compile SQL select from parameters provided
        /// </summary>
        /// <returns>SQL Select</returns>
		private string CompileSqlSelect()
		{
			string sqlConditionOperator = " = ";
			string tabName;
			string sqlResult;
			if ((tabName = tabCtl.SelectedTab.Name) != null)
			{
				if (!(tabName == "tabFct"))
				{
					if (!(tabName == "tabTlz"))
					{
						if (tabName == "tabFields")
						{
                            string fldSelect = "SELECT \r\n\tCOL.name AS 'Field', \r\n\tRBF.F1454 AS 'Description', \r\n\tTAB.name AS 'Table', \r\n\tREPLACE (RBF.F1458,'dt','') AS 'Type', \r\n\t(CASE WHEN PKEY.COLUMN_NAME IS NOT NULL THEN '1' ELSE '0' END) AS 'Key', \r\n\tCOL.max_length AS 'Size', \r\n\tCOL.scale AS 'Dec', \r\n\t(CASE WHEN COL.is_nullable = 0 THEN 1 ELSE 0 END) AS 'Required', \r\n\tTAB.create_date AS 'Created' \r\nFROM sys.columns COL \r\nJOIN sys.tables TAB ON \r\n\tCOL.object_id = TAB.object_id \r\nJOIN RB_FIELDS RBF ON \r\n\tCOL.name = RBF.F1453 AND\r\n\tRBF.F1452 = TAB.name_ADDED_JOIN_\r\nLEFT OUTER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE PKEY ON \r\n\tPKEY.COLUMN_NAME = COL.name AND \r\n\tTAB.name=PKEY.TABLE_NAME";
							string sqlOrderBy = " \r\nGROUP BY \r\n\tCOL.name, \r\n\tRBF.F1454, \r\n\tTAB.name, \r\n\tRBF.F1458, \r\n\tpkey.COLUMN_NAME, \r\n\tCOL.max_length, \r\n\tCOL.scale, \r\n\tCOL.is_nullable, \r\n\tTAB.create_date";
							sqlResult = fldSelect; 
							if (rdbNumFld.Checked && txtNumFld.Text != "")
							{
								string FldNumber;
								if (txtNumFld.Text.Contains("*") || txtNumFld.Text.Contains("?"))
								{
									FldNumber = txtNumFld.Text.Replace("*", "%");
									FldNumber = FldNumber.Replace("?", "_");
									sqlConditionOperator = " LIKE ";
								}
								else
								{
									FldNumber = txtNumFld.Text;
								}
								FldNumber = "F" + FldNumber;
								sqlResult = string.Concat(new string[]
								{
									sqlResult,
									" \r\nWHERE col.name",
									sqlConditionOperator,
									"'",
									FldNumber,
									"'",
									sqlOrderBy,
									" \r\nORDER BY \r\n\tTAB.name"
								});
								sqlResult = sqlResult.Replace("_ADDED_JOIN_", "");
								sqlResult = sqlResult.Replace("_CONDITION_", sqlConditionOperator);
								sqlResult = sqlResult.Replace("_SEARCH_TABLE_", FldNumber);
								return sqlResult;
							}
							if (rdbDescFld.Checked && txtDescFld.Text != "")
							{
								string FldDescriptor;
								if (txtDescFld.Text.Contains("*") || txtDescFld.Text.Contains("?"))
								{
									FldDescriptor = txtDescFld.Text.Replace("*", "%");
									FldDescriptor = FldDescriptor.Replace("?", "_");
									sqlConditionOperator = " LIKE ";
								}
								else
								{
									FldDescriptor = txtDescFld.Text;
								}
								if (chkSearchAnyFld.Checked)
								{
									FldDescriptor = "%" + FldDescriptor + "%";
									sqlConditionOperator = " LIKE ";
								}
								sqlResult = string.Concat(new string[]
								{
									sqlResult,
									" WHERE RBF.F1454",
									sqlConditionOperator,
									"'",
									FldDescriptor,
									"'",
									sqlOrderBy,
									" \r\nORDER BY \r\n\tCAST(SUBSTRING(COL.name,2,255) AS INT), \r\n\tTAB.name"
								});
								sqlResult = sqlResult.Replace("_ADDED_JOIN_", "");
								sqlResult = sqlResult.Replace("_CONDITION_", sqlConditionOperator);
								sqlResult = sqlResult.Replace("_SEARCH_TABLE_", FldDescriptor);
								return sqlResult;
							}
							if (rdbTableFld.Checked && cmbTableFld.Text != "")
							{
								string FldTable;
								if (cmbTableFld.Text.Contains("*") || cmbTableFld.Text.Contains("?"))
								{
									FldTable = cmbTableFld.Text.Replace("*", "%");
									FldTable = FldTable.Replace("?", "_");
									sqlConditionOperator = " LIKE ";
								}
								else
								{
									FldTable = cmbTableFld.Text;
								}
								if (rdbShowFields.Checked)
								{
									sqlResult = string.Concat(new string[]
									{
										sqlResult,
										" WHERE TAB.name",
										sqlConditionOperator,
										"'",
										FldTable,
										"' ",
										sqlOrderBy,
										" \r\nORDER BY \r\n\tCAST(SUBSTRING(COL.name,2,255) AS INT)"
									});
                                    sqlResult = sqlResult.Replace("AND\r\n\tRBF.F1452 = TAB.name_ADDED_JOIN_", "AND \r\n\tRBF.F1452 _CONDITION_ '_SEARCH_TABLE_'");
									sqlResult = sqlResult.Replace("_CONDITION_", sqlConditionOperator);
									sqlResult = sqlResult.Replace("_SEARCH_TABLE_", FldTable);
									return sqlResult;
								}
								sqlResult = "SELECT * FROM " + cmbTableFld.Text;
								if (chkLastTransaction.Checked && chkLastTransaction.Visible)
								{
									sqlResult = sqlResult + " WHERE F1032 = (SELECT DISTINCT TOP 1 F1032 FROM " + cmbTableFld.Text + " ORDER BY F1032 DESC)";
									return sqlResult;
								}
								return sqlResult;
							}
							else
							{
								if (!rdbCustSqlFld.Checked || !(txtCustSqlFld.Text != ""))
								{
									return sqlResult;
								}
								if (txtCustSqlFld.SelectedText != "")
								{
									sqlResult = txtCustSqlFld.SelectedText;
									return sqlResult;
								}
								sqlResult = txtCustSqlFld.Text;
								return sqlResult;
							}
						}
					}
					else
					{
						sqlResult = "Select " + TlzFields + " FROM TLZ_TAB";
						if (rdbNumTlz.Checked && txtNumTlz.Text != "")
						{
							string TlzNumber;
							if (txtNumTlz.Text.Contains("*") || txtNumTlz.Text.Contains("?"))
							{
								TlzNumber = txtNumTlz.Text.Replace("*", "%");
								TlzNumber = TlzNumber.Replace("?", "_");
								sqlConditionOperator = " LIKE ";
							}
							else
							{
								TlzNumber = txtNumTlz.Text;
							}
							sqlResult = string.Concat(new string[]
							{
								sqlResult,
								" WHERE F1034",
								sqlConditionOperator,
								"'",
								TlzNumber,
								"'"
							});
							return sqlResult;
						}
						if (rdbDescTlz.Checked && txtDescTlz.Text != "")
						{
							string TLZDescriptor;
							if (txtDescTlz.Text.Contains("*") || txtDescTlz.Text.Contains("?"))
							{
								TLZDescriptor = txtDescTlz.Text.Replace("*", "%");
								TLZDescriptor = TLZDescriptor.Replace("?", "_");
								sqlConditionOperator = " LIKE ";
							}
							else
							{
								TLZDescriptor = txtDescTlz.Text;
							}
							if (chkSearchAnyTlz.Checked)
							{
								TLZDescriptor = "%" + TLZDescriptor + "%";
							 sqlConditionOperator = " LIKE ";
							}
							sqlResult = string.Concat(new string[]
							{
								sqlResult,
								" WHERE F1039",
								sqlConditionOperator,
								"'",
								TLZDescriptor,
								"'"
							});
							return sqlResult;
						}
						if (!rdbCustSqlTlz.Checked || !(txtCustSqlTlz.Text != ""))
						{
							return sqlResult;
						}
						if (txtCustSqlTlz.SelectedText != "")
						{
							sqlResult = txtCustSqlTlz.SelectedText;
							return sqlResult;
						}
						sqlResult = txtCustSqlTlz.Text;
						return sqlResult;
					}
				}
				else
				{
					sqlResult = "Select " + FctFields + " FROM FCT_TAB";
					if (rdbNumFct.Checked && txtNumFct.Text != "")
					{
						string FctNumber;
						if (txtNumFct.Text.Contains("*") || txtNumFct.Text.Contains("?"))
						{
							FctNumber = txtNumFct.Text.Replace("*", "%");
							FctNumber = FctNumber.Replace("?", "_");
						 sqlConditionOperator = " LIKE ";
						}
						else
						{
							FctNumber = txtNumFct.Text;
						}
						sqlResult = string.Concat(new string[]
						{
							sqlResult,
							" WHERE F1063",
							sqlConditionOperator,
							"'",
							FctNumber,
							"'"
						});
						return sqlResult;
					}
					if (rdbDescFct.Checked && txtDescFct.Text != "")
					{
						string FctDescriptor;
						if (txtDescFct.Text.Contains("*") || txtDescFct.Text.Contains("?"))
						{
							FctDescriptor = txtDescFct.Text.Replace("*", "%");
							FctDescriptor = FctDescriptor.Replace("?", "_");
							sqlConditionOperator = " LIKE ";
						}
						else
						{
							FctDescriptor = txtDescFct.Text;
						}
						if (chkSearchAnyFct.Checked)
						{
							FctDescriptor = "%" + FctDescriptor + "%";
							sqlConditionOperator = " LIKE ";
						}
						sqlResult = string.Concat(new string[]
						{
							sqlResult,
							" WHERE F1064",
							sqlConditionOperator,
							"'",
							FctDescriptor,
							"'"
						});
						return sqlResult;
					}
					if (!rdbCustSqlFct.Checked || !(txtCustSqlFct.Text != ""))
					{
						return sqlResult;
					}
					if (txtCustSqlFct.SelectedText != "")
					{
						sqlResult = txtCustSqlFct.SelectedText;
						return sqlResult;
					}
					sqlResult = txtCustSqlFct.Text;
					return sqlResult;
				}
			}
			sqlResult = "SELECT * FROM SYS_TAB";
			return sqlResult;
		}
        #endregion

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
			string text = CompileSqlSelect();
			txtCustSqlFct.Text = text;
			setTabTextFocus();
		}

		private void btnBuildQryTlz_Click(object sender, EventArgs e)
		{
			string text = CompileSqlSelect();
			txtCustSqlTlz.Text = text;
			setTabTextFocus();
		}

		private void btnBuildQryFld_Click(object sender, EventArgs e)
		{
			string text = CompileSqlSelect();
			txtCustSqlFld.Text = text;
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
			}
		}
        #endregion

        #region Date conversion
        private void txtJulian_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Return) && txtJulian.Focused)
			{
				setGregorianDate();
				log.Logger(0, "Date converted: Julian \"" + txtJulian.Text + "\" to Gregorian \"" + dateGregorian.Text);
			}
		}

		private void dateGregorian_ValueChanged(object sender, EventArgs e)
		{
			if (dateGregorian.Focused)
			{
				setJulianDate();
				log.Logger(0, "Date converted: Gregorian \"" + dateGregorian.Text + "\" to Julian \"" + txtJulian.Text);
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

        private void btnClearResults_Click(object sender, EventArgs e)
        {
            dGrd.DataSource = null;
            tslblRecordCnt.Text = "0";
            Height = FormHeightMin;
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
				Cursor = Cursors.WaitCursor;
				frmConfig frmConfig = new frmConfig();
				frmConfig.StartPosition = FormStartPosition.CenterParent;
				frmConfig.ShowDialog();

				if (!dbConn.TestDbConn(config.GetValue("CONNECTION", "SERVER"), config.GetValue("CONNECTION", "DATABASE"), true) || !File.Exists(frmMain.ConfigFilePath))
				{
					//frmConfig frmConfig = new frmConfig();
					frmConfig.ShowDialog();
				}
                config.Load(); // Re-load config in case it was changed

				ValidateConfigFile();
			}
		}

		private void setColumnArray()
		{
			List<string> headers = new List<string>();
			foreach (DataGridViewColumn col in dGrd.Columns)
			{
				if (col.HeaderText != null)
				{
					headers.Add(col.HeaderText);
				}
			}

			Dictionary<string, string> descriptions = new Dictionary<string, string>();

			if (headers.Count > 0)
			{
				HashSet<string> uniqueHeaders = new HashSet<string>(headers);
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT F1453, F1454 FROM RB_FIELDS WHERE F1453 IN (");
				bool first = true;
				foreach (string h in uniqueHeaders)
				{
					if (!first) sb.Append(",");
					sb.Append("'");
					sb.Append(h.Replace("'", "''"));
					sb.Append("'");
					first = false;
				}
				sb.Append(")");

				try
				{
					using (SqlConnection sqlConnection = new SqlConnection(GetConnString(tscmbDbServer.Text, tscmbDbDatabase.Text)))
					{
						using (SqlCommand sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
						{
							sqlConnection.Open();
							using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
							{
								while (sqlDataReader.Read())
								{
									string key = sqlDataReader["F1453"].ToString();
									string val = sqlDataReader["F1454"].ToString();
									if (!descriptions.ContainsKey(key))
									{
										descriptions[key] = val;
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					log.Logger(1, "Error in setColumnArray: " + ex.Message);
				}
			}

			foreach (DataGridViewColumn dataGridViewColumn in dGrd.Columns)
			{
				string value = "";
				if (dataGridViewColumn.HeaderText != null && descriptions.ContainsKey(dataGridViewColumn.HeaderText))
				{
					value = descriptions[dataGridViewColumn.HeaderText];
				}
				arrayGrdFld.Add(dataGridViewColumn.Name);
				arrayGrdDesc.Add(value);
			}
		}

        private async Task setColumnArrayAsync()
		{
            var columns = new List<string>();
            foreach (DataGridViewColumn col in dGrd.Columns)
            {
                columns.Add(col.HeaderText);
            }

            var connString = GetConnString(tscmbDbServer.Text, tscmbDbDatabase.Text);

            // Fetch all descriptions in background
            var results = await Task.Run(() =>
            {
                var descList = new List<string>();
                using (var sqlConnection = new SqlConnection(connString))
                {
                    sqlConnection.Open();
                    foreach (var headerText in columns)
                    {
                        string cmdText = "Select top(1) F1454 from RB_FIELDS where F1453 = @headerText";
                        string value = "";

                        using(var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@headerText", headerText);
                            using(var reader = sqlCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    value = reader[0].ToString();
                                }
                            }
                        }
                        descList.Add(value);
                    }
                }
                return descList;
            });

			foreach (DataGridViewColumn dataGridViewColumn in dGrd.Columns)
			{
				arrayGrdFld.Add(dataGridViewColumn.Name);
				arrayGrdDesc.Add(results[dataGridViewColumn.Index]);
			}
		}

		private void setHeaders()
		{
			if (!chkToggleDesc.Checked)
			{
				IEnumerator enumerator = dGrd.Columns.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)enumerator.Current;
						dGrd.Columns[dataGridViewColumn.Index].HeaderText = arrayGrdFld[dataGridViewColumn.Index].ToString();
					}
					return;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			foreach (DataGridViewColumn dataGridViewColumn2 in dGrd.Columns)
			{
				dGrd.Columns[dataGridViewColumn2.Index].HeaderText = arrayGrdDesc[dataGridViewColumn2.Index].ToString();
			}
			if (config.GetValue("GENERAL", "RESIZECOLUMNS") == "1")
			{
				int firstDisplayedScrollingColumnIndex = dGrd.FirstDisplayedScrollingColumnIndex;
				dGrd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
				if (firstDisplayedScrollingColumnIndex >= 0)
				{
					dGrd.FirstDisplayedScrollingColumnIndex = firstDisplayedScrollingColumnIndex;
				}
			}
		}

		private void chkToggleDesc_CheckedChanged(object sender, EventArgs e)
		{
			setHeaders();
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
			foreach (Form form in Application.OpenForms)
			{
				if (form is frmUnarchive)
				{
					form.Close();
					break;
				}
			}
			log.Logger(0, "SMS Search V" + Application.ProductVersion + " terminated");
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

		private void getDbNames()
		{
			if (backgroundWorker.IsBusy) return;
			tscmbDbDatabase.Items.Clear();
			Cursor = Cursors.WaitCursor;
            backgroundWorker.RunWorkerCompleted += getDbNames_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(new Tuple<string, string>("getDbNames", tscmbDbServer.Text));
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

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
            log.Logger(0, "backgroundWorker_DoWork: start");
            try
            {
                if (e.Argument is Tuple<string, string, string, string> tuple4)
                {
                    log.Logger(0, $"backgroundWorker_DoWork: action='{tuple4.Item1}' server='{tuple4.Item2}' database='{tuple4.Item3}' tableLike='{tuple4.Item4}'");
                    switch (tuple4.Item1)
                    {
                        case "PopulateTableList":
                            try
                            {
                                string selectCommandText = "SELECT NAME FROM sys.tables ORDER BY NAME";
                                dataAdapter = new SqlDataAdapter(selectCommandText, GetConnString(tuple4.Item2, tuple4.Item3));
                                DataTable dataTable = new DataTable();
                                dataTable.Locale = CultureInfo.InvariantCulture;
                                dataTable.Clear();
                                dataAdapter.Fill(dataTable);
                                e.Result = new Tuple<DataTable, string>(dataTable, tuple4.Item4);
                                log.Logger(0, $"backgroundWorker_DoWork: PopulateTableList fetched {dataTable.Rows.Count} tables");
                            }
                            catch (Exception ex)
                            {
                                e.Result = ex;
                                log.Logger(1, "backgroundWorker_DoWork: PopulateTableList error - " + ex.Message);
                            }
                            finally
                            {
                                dataAdapter.Dispose();
                            }
                            break;
                    }
                }
                else if (e.Argument is Tuple<string, string> tuple2)
                {
                    log.Logger(0, $"backgroundWorker_DoWork: action='{tuple2.Item1}' server='{tuple2.Item2}'");
                    switch (tuple2.Item1)
                    {
                        case "getDbNames":
                            try
                            {
                                string connectionString = "Data Source=" + tuple2.Item2 + "; Integrated Security=True;";
                                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                                {
                                    sqlConnection.Open();
                                    DataTable schema = sqlConnection.GetSchema("Databases");
                                    sqlConnection.Close();
                                    e.Result = schema;
                                    log.Logger(0, $"backgroundWorker_DoWork: getDbNames fetched {schema.Rows.Count} databases");
                                }
                            }
                            catch (Exception ex)
                            {
                                e.Result = ex;
                                log.Logger(1, "backgroundWorker_DoWork: getDbNames error - " + ex.Message);
                            }
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                    log.Logger(0, "backgroundWorker_DoWork: no recognized argument");
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
                log.Logger(1, "backgroundWorker_DoWork: unexpected error - " + ex.Message);
            }
            finally
            {
                log.Logger(0, "backgroundWorker_DoWork: end");
            }
		}

        private void getDbNames_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.Logger(0, "getDbNames_RunWorkerCompleted: start");
            try
            {
                if (e.Result is Exception ex)
                {
                    log.Logger(1, "getDbNames_RunWorkerCompleted: error - " + ex.Message);
                    MessageBox.Show("Failed to connect to data source. \n\nSQL error:\n" + ex.Message, "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (e.Result is DataTable dt)
                {
                    log.Logger(0, $"getDbNames_RunWorkerCompleted: received {dt.Rows.Count} databases");
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        tscmbDbDatabase.Items.Add(dataRow["database_name"]);
                    }
                }
            }
            catch (Exception ex2)
            {
                log.Logger(1, "getDbNames_RunWorkerCompleted: exception - " + ex2.Message);
            }
            finally
            {
                backgroundWorker.RunWorkerCompleted -= getDbNames_RunWorkerCompleted;
                Cursor = Cursors.Default;
                log.Logger(0, "getDbNames_RunWorkerCompleted: end");
            }
        }

        private void PopulateTableList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.Logger(0, "PopulateTableList_RunWorkerCompleted: start");
            try
            {
                if (e.Result is Exception ex)
                {
                    log.Logger(1, "PopulateTableList_RunWorkerCompleted: error - " + ex.Message);
                    MessageBox.Show(ex.Message, "SQL error encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (e.Result is Tuple<DataTable, string> tuple)
                {
                    log.Logger(0, $"PopulateTableList_RunWorkerCompleted: received {tuple.Item1.Rows.Count} tables, requested selection '{tuple.Item2}'");
                    bindingSourceTbl.DataSource = tuple.Item1;
                    cmbTableFld.DataSource = bindingSourceTbl;
                    cmbTableFld.DisplayMember = "NAME";
                    cmbTableFld.ValueMember = "NAME";
                    int num = cmbTableFld.FindString(tuple.Item2);
                    if (num != -1)
                    {
                        cmbTableFld.Text = tuple.Item2;
                    }
                    else
                    {
                        cmbTableFld.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex2)
            {
                log.Logger(1, "PopulateTableList_RunWorkerCompleted: exception - " + ex2.Message);
            }
            finally
            {
                backgroundWorker.RunWorkerCompleted -= PopulateTableList_RunWorkerCompleted;
                Cursor = Cursors.Default;
                log.Logger(0, "PopulateTableList_RunWorkerCompleted: end");
            }
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
