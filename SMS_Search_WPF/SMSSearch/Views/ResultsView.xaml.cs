using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SMS_Search.ViewModels;

namespace SMS_Search.Views
{
    public partial class ResultsView : UserControl
    {
        private DispatcherTimer _debounceTimer;

        public ResultsView()
        {
            InitializeComponent();
            _debounceTimer = new DispatcherTimer();
            _debounceTimer.Interval = TimeSpan.FromMilliseconds(500);
            _debounceTimer.Tick += DebounceTimer_Tick;

            this.DataContextChanged += ResultsView_DataContextChanged;
        }

        private void ResultsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ResultsViewModel newVm)
            {
                newVm.ScrollToRowRequested += Vm_ScrollToRowRequested;
            }
            if (e.OldValue is ResultsViewModel oldVm)
            {
                oldVm.ScrollToRowRequested -= Vm_ScrollToRowRequested;
            }
        }

        private void Vm_ScrollToRowRequested(object sender, int rowIndex)
        {
            if (resultsGrid.Items.Count > rowIndex && rowIndex >= 0)
            {
                var item = resultsGrid.Items[rowIndex];
                resultsGrid.ScrollIntoView(item);
                resultsGrid.SelectedItems.Clear();
                resultsGrid.SelectedItems.Add(item);

                // Try to focus the grid so keyboard navigation works
                resultsGrid.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            if (DataContext is ResultsViewModel vm)
            {
                if (vm.ApplyFilterCommand.CanExecute(txtFilter.Text))
                {
                    vm.ApplyFilterCommand.Execute(txtFilter.Text);
                }
            }
        }

        private void resultsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ResultsViewModel vm && resultsGrid.SelectedItem != null)
            {
                if (resultsGrid.SelectedItem is SMS_Search.Data.VirtualRow vRow)
                {
                    vm.SetCurrentRowIndex(vRow.RowIndex);
                }
            }
        }

        private void FilterBySelection_Click(object sender, RoutedEventArgs e)
        {
            if (resultsGrid.SelectedCells.Count > 0)
            {
                var cellInfo = resultsGrid.SelectedCells[0];
                var val = GetCellValue(cellInfo.Item, cellInfo.Column);
                if (val != null)
                {
                    txtFilter.Text = val.ToString();
                }
            }
        }

        private void CopyWithHeaders_Click(object sender, RoutedEventArgs e)
        {
            CopySelectedCells(true);
        }

        private void CopyAsSqlInsert_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ResultsViewModel;
            if (vm == null || resultsGrid.SelectedCells.Count == 0) return;

            string tableName = vm.TableName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = "TableName";
            if (!tableName.StartsWith("[")) tableName = "[" + tableName + "]";

            // Get unique rows
            var selectedRows = resultsGrid.SelectedCells.Select(c => c.Item).Distinct().ToList();

            // Get visible columns (all visible columns for INSERT)
            var visibleCols = resultsGrid.Columns.Where(c => c.Visibility == Visibility.Visible).OrderBy(c => c.DisplayIndex).ToList();
            if (visibleCols.Count == 0) return;

            var sb = new StringBuilder();
            sb.Append($"INSERT INTO {tableName} (");
            sb.Append(string.Join(", ", visibleCols.Select(c => $"[{c.Header}]")));
            sb.AppendLine(") VALUES");

            // Sort rows by index
            selectedRows = selectedRows.OrderBy(r => (r as SMS_Search.Data.VirtualRow)?.RowIndex ?? 0).ToList();

            int batchSize = 1000;
            for (int i = 0; i < selectedRows.Count; i++)
            {
                if (i > 0 && i % batchSize == 0)
                {
                    sb.AppendLine(";");
                    sb.AppendLine($"INSERT INTO {tableName} (");
                    sb.Append(string.Join(", ", visibleCols.Select(c => $"[{c.Header}]")));
                    sb.AppendLine(") VALUES");
                }
                else if (i > 0)
                {
                    sb.AppendLine(",");
                }

                var row = selectedRows[i];
                var values = new List<string>();
                foreach (var col in visibleCols)
                {
                    var val = GetCellValue(row, col);
                    values.Add(FormatSqlValue(val));
                }
                sb.Append("(" + string.Join(", ", values) + ")");
            }
            sb.AppendLine(";");

            try
            {
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("SQL INSERT statements copied to clipboard.", "Copy", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to copy to clipboard: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopySelectedCells(bool includeHeaders)
        {
            if (resultsGrid.SelectedCells.Count == 0) return;

            // Get bounds
            var cells = resultsGrid.SelectedCells;
            var rows = cells.Select(c => c.Item).Distinct().ToList(); // Note: Item order might not match visual row order if sorting?
            // Usually SelectedCells returns in selection order.
            // But for copy we want visual order.
            // Items.IndexOf is expensive in virtual mode?
            // Actually, we can just sort by index if we can get it.
            // Since we can't easily get index without scanning, let's assume selection order is close enough or acceptable for now,
            // OR use a slower sort. Given virtual mode, scanning is bad.
            // Wait, we can use the order they appear in `resultsGrid.Items` if we iterate items and check selection? No, too slow.
            // Let's rely on `SelectedCells` order or try to sort by RowIndex if available.
            // VirtualRow has internal index? Not public.
            // But we can check if item implements an index property? No.

            // Group by row and sort by RowIndex
            var grouped = cells.GroupBy(c => c.Item)
                               .OrderBy(g => (g.Key as SMS_Search.Data.VirtualRow)?.RowIndex ?? 0)
                               .ToList();

            // Get unique columns in visual order
            var cols = cells.Select(c => c.Column).Distinct().OrderBy(c => c.DisplayIndex).ToList();

            var sb = new StringBuilder();

            if (includeHeaders)
            {
                sb.AppendLine(string.Join("\t", cols.Select(c => c.Header)));
            }

            foreach (var group in grouped)
            {
                var row = group.Key;
                var values = new List<string>();
                foreach (var col in cols)
                {
                    // Check if this cell is actually selected?
                    // Standard copy usually copies the rectangular region or just selected cells?
                    // "Copy with Headers" usually implies copying the selected data as a table.
                    // If selection is disjoint, we fill gaps with empty strings?
                    // Let's assume we output values for all columns in the bounding box of selection for that row.

                    // Actually, let's just iterate the columns we identified. If the cell (row, col) is in SelectedCells, output value. Else empty.
                    bool isSelected = group.Any(c => c.Column == col);
                    if (isSelected)
                    {
                        var val = GetCellValue(row, col);
                        values.Add(val?.ToString() ?? "");
                    }
                    else
                    {
                         values.Add("");
                    }
                }
                sb.AppendLine(string.Join("\t", values));
            }

            try
            {
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to copy to clipboard: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private object GetCellValue(object row, DataGridColumn col)
        {
            if (row == null) return null;
            string propName = col.SortMemberPath;
            if (string.IsNullOrEmpty(propName)) return null;

            var props = TypeDescriptor.GetProperties(row);
            var prop = props[propName];
            return prop?.GetValue(row);
        }

        private string FormatSqlValue(object value)
        {
            if (value == null || value == DBNull.Value) return "NULL";
            if (value is bool b) return b ? "1" : "0";
            if (IsNumeric(value)) return value.ToString();
            if (value is DateTime dt) return $"'{dt:yyyy-MM-dd HH:mm:ss.fff}'";
            return $"'{value.ToString().Replace("'", "''")}'";
        }

        private bool IsNumeric(object value)
        {
            return value is sbyte || value is byte || value is short || value is ushort ||
                   value is int || value is uint || value is long || value is ulong ||
                   value is float || value is double || value is decimal;
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                txtFilter.Focus();
                e.Handled = true;
            }
        }
    }
}
