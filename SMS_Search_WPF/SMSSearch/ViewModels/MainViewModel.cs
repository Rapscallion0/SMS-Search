using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SMS_Search.Data;
using SMS_Search.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SMS_Search.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ILoggerService _logger;
        private readonly IDialogService _dialogService;
        private readonly IConfigService _configService;
        private readonly IQueryHistoryService _historyService;
        private readonly IHotkeyService _hotkeyService;
        private readonly IServiceProvider _serviceProvider;

        public event Action RequestOpenSettings;

        public MainViewModel(
            SearchViewModel searchViewModel,
            ResultsViewModel resultsViewModel,
            ILoggerService logger,
            IDialogService dialogService,
            IConfigService configService,
            IQueryHistoryService historyService,
            IHotkeyService hotkeyService,
            IServiceProvider serviceProvider)
        {
            SearchViewModel = searchViewModel;
            ResultsViewModel = resultsViewModel;
            _logger = logger;
            _dialogService = dialogService;
            _configService = configService;
            _historyService = historyService;
            _hotkeyService = hotkeyService;
            _serviceProvider = serviceProvider;

            ExecuteSearchCommand = new AsyncRelayCommand(ExecuteSearch);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            OpenUnarchiveCommand = new RelayCommand(OpenUnarchive);

            // Julian Date Converter
            UpdateJulianDate();
        }

        public SearchViewModel SearchViewModel { get; }
        public ResultsViewModel ResultsViewModel { get; }

        public IAsyncRelayCommand ExecuteSearchCommand { get; }
        public IRelayCommand OpenSettingsCommand { get; }
        public IRelayCommand OpenUnarchiveCommand { get; }

        [ObservableProperty]
        private string _julianDateText;

        [ObservableProperty]
        private DateTime _gregorianDate = DateTime.Today;

        [ObservableProperty]
        private bool _isUnarchiveTargetVisible;

        private bool _isUpdatingDate;

        partial void OnJulianDateTextChanged(string value)
        {
            if (_isUpdatingDate) return;
            if (value != null && value.Length == 7 && int.TryParse(value, out int _))
            {
                try
                {
                    int year = int.Parse(value.Substring(0, 4));
                    int day = int.Parse(value.Substring(4, 3));
                    _isUpdatingDate = true;
                    GregorianDate = new DateTime(year, 1, 1).AddDays(day - 1);
                    _isUpdatingDate = false;
                }
                catch { _isUpdatingDate = false; }
            }
        }

        partial void OnGregorianDateChanged(DateTime value)
        {
            if (_isUpdatingDate) return;
            UpdateJulianDate();
        }

        private void UpdateJulianDate()
        {
             _isUpdatingDate = true;
             DateTime dt = GregorianDate;
             int days = dt.DayOfYear;
             JulianDateText = $"{dt.Year}{days:D3}";
             _isUpdatingDate = false;
        }

        private void OpenUnarchive()
        {
            var window = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SMS_Search.Views.UnarchiveWindow>(_serviceProvider);
            window.Show();
            IsUnarchiveTargetVisible = true;
            window.Closed += (s, e) => IsUnarchiveTargetVisible = false;
        }

        private async Task ExecuteSearch()
        {
             var criteria = SearchViewModel.GetSearchCriteria();

             await ResultsViewModel.ExecuteSearchAsync(criteria);

             if (criteria.Type == SearchType.CustomSql)
             {
                 _historyService.AddQuery(criteria.Mode.ToString(), criteria.Value);
             }
        }

        private void OpenSettings()
        {
            RequestOpenSettings?.Invoke();
        }
    }
}
