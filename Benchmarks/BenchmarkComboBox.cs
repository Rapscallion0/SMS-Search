using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace SMS_Search.Benchmarks
{
    public class BenchmarkComboBox
    {
        // This benchmark is intended to be run in a Windows environment with UI capabilities.
        public static void Run()
        {
            Console.WriteLine("Benchmarking ComboBox population...");

            // Setup data
            int count = 5000;
            string[] items = Enumerable.Range(0, count).Select(i => "Database_" + i).ToArray();

            // Benchmark Add one by one
            // Simulating the original code structure
            var cbOriginal = new ComboBox();
            // Force handle creation to simulate real UI impact (if possible in headless, otherwise it's just collection overhead)
            // In a real app, the control is visible and painting.

            var swOriginal = Stopwatch.StartNew();
            foreach (var item in items)
            {
                cbOriginal.Items.Add(item);
            }
            swOriginal.Stop();
            Console.WriteLine($"Original Loop Add: {swOriginal.ElapsedMilliseconds} ms");

            // Benchmark AddRange (Optimized)
            var cbOptimized = new ComboBox();
            var swOptimized = Stopwatch.StartNew();
            cbOptimized.Items.AddRange(items);
            swOptimized.Stop();
            Console.WriteLine($"Optimized AddRange: {swOptimized.ElapsedMilliseconds} ms");

            Console.WriteLine("Note: The performance difference is significantly larger when the control is visible on a Form due to UI repaints.");
        }
    }
}
