using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class BenchmarkSqlGen
{
    private static readonly HashSet<string> SafeStringTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "char", "nchar", "varchar", "nvarchar", "text", "ntext"
    };

    public static void Run(string[] args)
    {
        Console.WriteLine("Starting BenchmarkSqlGen...");

        var columns = new Dictionary<string, string>
        {
            { "Name", "nvarchar" },
            { "Description", "text" },
            { "ID", "int" },
            { "CreatedDate", "datetime" },
            { "Settings", "xml" },
            { "Code", "varchar" }
        };

        string filterText = "foo";

        // 1. Current Logic (Baseline)
        Console.WriteLine("\n--- Current Logic (All CAST) ---");
        string currentSql = GenerateCurrentSql(columns.Keys, filterText);
        Console.WriteLine(currentSql);

        // 2. Optimized Logic
        Console.WriteLine("\n--- Optimized Logic (Smart CAST) ---");
        string optimizedSql = GenerateOptimizedSql(columns, filterText);
        Console.WriteLine(optimizedSql);

        // Verify correctness
        if (!optimizedSql.Contains("CAST([ID] AS NVARCHAR(MAX))")) Console.WriteLine("ERROR: ID should be CAST");
        if (!optimizedSql.Contains("CAST([Settings] AS NVARCHAR(MAX))")) Console.WriteLine("ERROR: Settings should be CAST");
        if (optimizedSql.Contains("CAST([Name] AS NVARCHAR(MAX))")) Console.WriteLine("ERROR: Name should NOT be CAST");
        if (optimizedSql.Contains("CAST([Code] AS NVARCHAR(MAX))")) Console.WriteLine("ERROR: Code should NOT be CAST");

        Console.WriteLine("\nVerification Complete.");
    }

    private static string GenerateCurrentSql(IEnumerable<string> columns, string filterText)
    {
        var clauses = new List<string>();
        string safeFilter = filterText.Replace("'", "''");
        foreach (var col in columns)
        {
            clauses.Add($"CAST([{col}] AS NVARCHAR(MAX)) LIKE '%{safeFilter}%'");
        }
        return string.Join(" OR ", clauses);
    }

    private static string GenerateOptimizedSql(Dictionary<string, string> columns, string filterText)
    {
        var clauses = new List<string>();
        string safeFilter = filterText.Replace("'", "''");
        foreach (var kvp in columns)
        {
            string col = kvp.Key;
            string type = kvp.Value;

            if (SafeStringTypes.Contains(type))
            {
                clauses.Add($"[{col}] LIKE '%{safeFilter}%'");
            }
            else
            {
                clauses.Add($"CAST([{col}] AS NVARCHAR(MAX)) LIKE '%{safeFilter}%'");
            }
        }
        return string.Join(" OR ", clauses);
    }
}
