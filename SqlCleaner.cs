using System.Collections.Generic;

namespace SMS_Search
{
    public struct SqlCleaningRule
    {
        public string Pattern;
        public string Replacement;
    }

    public static class SqlCleaner
    {
        public static List<SqlCleaningRule> DefaultRules
        {
            get
            {
                return new List<SqlCleaningRule>
                {
                    new SqlCleaningRule { Pattern = "&amp;", Replacement = "&" },
                    new SqlCleaningRule { Pattern = "<(/|)(((logsql|sql|prm|msg|errsql|logurl|).*?)|(pre|p|(br(( |)/|))))>", Replacement = "" },
                    new SqlCleaningRule { Pattern = "&lt;", Replacement = "<" },
                    new SqlCleaningRule { Pattern = "&gt;", Replacement = ">" },
                    new SqlCleaningRule { Pattern = @"\[", Replacement = "(" },
                    new SqlCleaningRule { Pattern = @"\]", Replacement = ")" },
                    new SqlCleaningRule { Pattern = "&quot;", Replacement = "'" },
                    new SqlCleaningRule { Pattern = @"\{09\}", Replacement = "" },

                    // Main Keywords
                    new SqlCleaningRule { Pattern = @"\bselect\b", Replacement = "SELECT" },
                    new SqlCleaningRule { Pattern = @"\s*\binsert\s+into\b", Replacement = "\r\nINSERT INTO" },
                    new SqlCleaningRule { Pattern = @"\s*\bupdate\b", Replacement = "\r\nUPDATE" },
                    new SqlCleaningRule { Pattern = @"\s*\bdelete\s+from\b", Replacement = "\r\nDELETE FROM" },

                    // Generic FROM (excluding DELETE FROM)
                    new SqlCleaningRule { Pattern = @"(?<!\bdelete\s*)\s*\bfrom\b", Replacement = "\r\nFROM" },

                    new SqlCleaningRule { Pattern = @"\s*\bwhere\b", Replacement = "\r\nWHERE" },
                    new SqlCleaningRule { Pattern = @"\s*\bgroup\s+by\b", Replacement = "\r\nGROUP BY" },
                    new SqlCleaningRule { Pattern = @"\s*\border\s+by\b", Replacement = "\r\nORDER BY" },
                    new SqlCleaningRule { Pattern = @"\s*\bhaving\b", Replacement = "\r\nHAVING" },
                    new SqlCleaningRule { Pattern = @"\s*\bdeclare\b", Replacement = "\r\nDECLARE" },

                    // Specific Joins (Longest first)
                    new SqlCleaningRule { Pattern = @"\s*\bleft\s+outer\s+join\b", Replacement = "\r\n\tLEFT OUTER JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\bright\s+outer\s+join\b", Replacement = "\r\n\tRIGHT OUTER JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\bfull\s+outer\s+join\b", Replacement = "\r\n\tFULL OUTER JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\bleft\s+join\b", Replacement = "\r\n\tLEFT JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\bright\s+join\b", Replacement = "\r\n\tRIGHT JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\binner\s+join\b", Replacement = "\r\n\tINNER JOIN" },

                    // Outer Join needs protection from LEFT/RIGHT/FULL
                    new SqlCleaningRule { Pattern = @"(?<!\b(left|right|full)\s*)\s*\bouter\s+join\b", Replacement = "\r\n\tOUTER JOIN" },

                    new SqlCleaningRule { Pattern = @"\s*\bcross\s+join\b", Replacement = "\r\n\tCROSS JOIN" },
                    new SqlCleaningRule { Pattern = @"\s*\bfull\s+join\b", Replacement = "\r\n\tFULL JOIN" },

                    // Generic JOIN (excluding specific joins)
                    new SqlCleaningRule { Pattern = @"(?<!\b(left|right|inner|outer|cross|full)\s*)\s*\bjoin\b", Replacement = "\r\n\tJOIN" },

                    new SqlCleaningRule { Pattern = @"\s*\bwhen\b", Replacement = "\r\n\tWHEN" },

                    // ON
                    new SqlCleaningRule { Pattern = @"\s*\bon\b", Replacement = "\r\n\t\tON" },

                    // UNION
                    new SqlCleaningRule { Pattern = @"\s*\bunion\b\s*", Replacement = "\r\nUNION\r\n" },

                    // AND / OR
                    new SqlCleaningRule { Pattern = @"\s*\band\b", Replacement = "\r\n\tAND" },
                    new SqlCleaningRule { Pattern = @"\s*\bor\b", Replacement = "\r\n\tOR" }
                };
            }
        }
    }
}
