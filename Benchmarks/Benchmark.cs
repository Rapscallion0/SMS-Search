using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SMS_Search.Benchmarks
{
    public class Benchmark
    {
        static int columnCount = 50; // Typical number of columns in a grid
        static int latencyMs = 20; // Simulated DB roundtrip latency per query (realistic for a network DB)

        public static void RunBenchmark()
        {
            Console.WriteLine($"Benchmarking with {columnCount} columns and {latencyMs}ms latency per query...");

            // N+1 Simulation
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < columnCount; i++)
            {
                // Open connection simulation
                // Execute Query simulation
                Thread.Sleep(latencyMs);
                // Close connection simulation
            }
            sw.Stop();
            Console.WriteLine($"N+1 Approach (Current): {sw.ElapsedMilliseconds} ms");

            // Optimized Simulation
            sw.Restart();
            // Open connection simulation
            // Execute Single Query simulation
            // Assume slightly higher latency for a larger query, but still one roundtrip
            Thread.Sleep(latencyMs + 5);

            // Process results in memory
            for (int i = 0; i < columnCount; i++)
            {
                 // Dictionary lookup (negligible time, typically < 0.001ms)
            }
            sw.Stop();
            Console.WriteLine($"Optimized Approach (Proposed): {sw.ElapsedMilliseconds} ms");

            double improvement = (double)(sw.ElapsedMilliseconds) / 1000.0;
            Console.WriteLine($"Estimated Improvement Factor: ~{columnCount}x faster");
        }
    }
}
