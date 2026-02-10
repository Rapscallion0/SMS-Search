using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SMS_Search.Data;
using SMS_Search.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace SMS_Search.ViewModels
{
    public partial class ResultsViewModel : ObservableObject
    {
        private readonly VirtualGridContext _gridContext;
        private readonly ILoggerService _logger;
        private readonly IDialogService _dialogService;
        private readonly QueryBuilder _queryBuilder;
        private readonly IConfigService _configService;

        public ResultsViewModel(VirtualGridContext gridContext, ILoggerService logger, IDialogService dialogService, IConfigService configService)
        {
            _gridContext = gridContext;
            _logger = logger;
            _dialogService = dialogService;
            _configService = configService;

            string fctFields = _configService.GetValue("QUERY", "FUNCTION");
            string tlzFields = _configService.GetValue("QUERY", "TOTALIZER");
            _queryBuilder = new QueryBuilder(fctFields, tlzFields);

            _gridContext.DataReady += OnDataReady;
            _gridContext.LoadError += OnLoadError;

            ExportCsvCommand = new AsyncRelayCommand(ExportCsvAsync);
            ExportJsonCommand = new AsyncRelayCommand(ExportJsonAsync);
            ExportExcelCommand = new AsyncRelayCommand(ExportExcelAsync);

            ApplyFilterCommand = new AsyncRelayCommand<string>(ApplyFilterAsync);
            FindNextCommand = new AsyncRelayCommand(() => FindMatchAsync(true));
            FindPreviousCommand = new AsyncRelayCommand(() => FindMatchAsync(false));
        }

        [ObservableProperty]
        private IList _searchResults;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _statusText;

        [ObservableProperty]
        private int _totalRecords;

        [ObservableProperty]
        private string _filterText;

        [ObservableProperty]
        private string _matchStatusText;

        [ObservableProperty]
        private string _tableName;

        public event EventHandler<int> ScrollToRowRequested;

        public IAsyncRelayCommand<string> ApplyFilterCommand { get; }
        public IAsyncRelayCommand FindNextCommand { get; }
        public IAsyncRelayCommand FindPreviousCommand { get; }

        public IAsyncRelayCommand ExportCsvCommand { get; }
        public IAsyncRelayCommand ExportJsonCommand { get; }
        public IAsyncRelayCommand ExportExcelCommand { get; }

        private void OnDataReady(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (SearchResults is VirtualizingCollection vc)
                {
                    vc.Refresh();
                    TotalRecords = _gridContext.TotalCount;
                    if (!string.IsNullOrEmpty(_gridContext.FilterText))
                    {
                        StatusText = $"Found {TotalRecords} records (Filtered from {_gridContext.UnfilteredCount})";
                    }
                    else
                    {
                        StatusText = $"Found {TotalRecords} records";
                    }
                }
            });
        }

        private void OnLoadError(object sender, string msg)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _dialogService.ShowError(msg, "Data Load Error");
                StatusText = "Error loading data";
            });
        }

        public async Task ExecuteSearchAsync(SearchCriteria criteria)
        {
            IsBusy = true;
            StatusText = "Searching...";
            FilterText = string.Empty;
            MatchStatusText = string.Empty;
            _lastFoundRowIndex = -1;

            // Set Table Name if applicable
            TableName = criteria.Type == SearchType.Table ? criteria.Value : "ResultTable";

            try
            {
                var queryResult = _queryBuilder.Build(criteria);

                var server = _configService.GetValue("CONNECTION", "SERVER");
                var database = _configService.GetValue("CONNECTION", "DATABASE");
                var user = _configService.GetValue("CONNECTION", "SQLUSER");
                var pass = _configService.GetValue("CONNECTION", "SQLPASSWORD");
                string decryptedPass = !string.IsNullOrEmpty(pass) ? SMS_Search.Utils.GeneralUtils.Decrypt(pass) : null;

                _gridContext.SetConnection(server, database, user, decryptedPass);

                // Load initial count and cache schema
                var schema = await _gridContext.GetSchemaAsync(queryResult.Sql, queryResult.Parameters);
                await _gridContext.LoadAsync(queryResult.Sql, queryResult.Parameters);

                // Create new collection
                SearchResults = new VirtualizingCollection(_gridContext, schema);

                TotalRecords = _gridContext.TotalCount;
                StatusText = $"Found {TotalRecords} records";
            }
            catch (Exception ex)
            {
                StatusText = "Error";
                _dialogService.ShowError(ex.Message, "Search Failed");
                _logger.LogError("Search failed", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ApplyFilterAsync(string filterText)
        {
             var columns = new List<string>();
             if (SearchResults is VirtualizingCollection vc)
             {
                 var props = vc.GetItemProperties(null);
                 foreach(PropertyDescriptor p in props) columns.Add(p.Name);
             }

             await _gridContext.ApplyFilterAsync(filterText, columns);

             if (!string.IsNullOrEmpty(filterText))
             {
                 MatchStatusText = "Calculating...";
                 long count = await _gridContext.GetTotalMatchCountAsync();
                 MatchStatusText = $"Found: {count} matches";
             }
             else
             {
                 MatchStatusText = string.Empty;
             }
        }

        private int _lastFoundRowIndex = -1;

        public void SetCurrentRowIndex(int index)
        {
            _lastFoundRowIndex = index;
        }

        private async Task FindMatchAsync(bool forward)
        {
            if (string.IsNullOrEmpty(FilterText)) return;

            MatchStatusText = "Searching...";

            var columns = new List<string>();
             if (SearchResults is VirtualizingCollection vc)
             {
                 var props = vc.GetItemProperties(null);
                 foreach(PropertyDescriptor p in props) columns.Add(p.Name);
             }

            int startRow = _lastFoundRowIndex;
            // If starting fresh, pick based on direction
            if (startRow < 0) startRow = forward ? -1 : _gridContext.TotalCount;

            int nextRow = await _gridContext.FindMatchRowAsync(FilterText, columns, startRow, forward);

            if (nextRow == -1)
            {
                 // Wrap around
                 int wrapStart = forward ? -1 : _gridContext.TotalCount;
                 nextRow = await _gridContext.FindMatchRowAsync(FilterText, columns, wrapStart, forward);
            }

            if (nextRow != -1)
            {
                _lastFoundRowIndex = nextRow;
                ScrollToRowRequested?.Invoke(this, nextRow);

                // Update match status X of Y
                // Note: Getting match count can be expensive, maybe we skip or show "Found" only
                try
                {
                    long total = await _gridContext.GetTotalMatchCountAsync();
                    long preceding = await _gridContext.GetPrecedingMatchCountAsync(nextRow);
                    MatchStatusText = $"Match {preceding + 1} of {total}";
                }
                catch
                {
                    MatchStatusText = "Match found";
                }
            }
            else
            {
                MatchStatusText = "No more matches";
            }
        }

        private async Task ExportCsvAsync()
        {
            string filename = _dialogService.SaveFileDialog("CSV files (*.csv)|*.csv", $"SMS_Search_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            if (string.IsNullOrEmpty(filename)) return;
            await PerformExportAsync(() => _gridContext.ExportToCsvAsync(filename));
        }

        private async Task ExportJsonAsync()
        {
            string filename = _dialogService.SaveFileDialog("JSON files (*.json)|*.json", $"SMS_Search_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            if (string.IsNullOrEmpty(filename)) return;
            await PerformExportAsync(() => _gridContext.ExportToJsonAsync(filename));
        }

        private async Task ExportExcelAsync()
        {
            string filename = _dialogService.SaveFileDialog("Excel XML (*.xml)|*.xml", $"SMS_Search_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
            if (string.IsNullOrEmpty(filename)) return;
            await PerformExportAsync(() => _gridContext.ExportToExcelXmlAsync(filename));
        }

        private async Task PerformExportAsync(Func<Task> exportAction)
        {
            IsBusy = true;
            StatusText = "Exporting...";
            try
            {
                await exportAction();
                _dialogService.ShowMessage("Export successful", "Export");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message, "Export Error");
                _logger.LogError("Export failed", ex);
            }
            finally
            {
                IsBusy = false;
                StatusText = $"Found {TotalRecords} records";
            }
        }
    }
}
