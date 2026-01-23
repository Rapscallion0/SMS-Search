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
                var list = new List<SqlCleaningRule>();
                string[,] cleanArray = new string[,]
                {
                    {"&amp;", "&"},
                    {"<(/|)(((logsql|sql|prm|msg|errsql|logurl|).*?)|(pre|p|(br(( |)/|))))>", ""},
                    {"&lt;", "<"},
                    {"&gt;", ">"},
                    {@"\[", "("},
                    {@"\]", ")"},
                    {"&quot;", "'"},
                    {@"( |)\b(JOIN|WHEN)\b", "\r\n\t$2"},
                    {@"\{09\}", ""},
                    {@"( |)\b(FROM|WHERE|GROUP BY|ORDER BY|HAVING|DECLARE)\b", "\r\n$2"},
                    {@"\b(UNION)\b ", "\r\n$1\r\n"},
                    {@"( |)\b(ON)\b", "\r\n\t\t$2"},
                    {"( AND | OR )","$1\r\n\t"}
                };

                for (int i = 0; i < cleanArray.GetLength(0); i++)
                {
                    list.Add(new SqlCleaningRule
                    {
                        Pattern = cleanArray[i, 0],
                        Replacement = cleanArray[i, 1]
                    });
                }
                return list;
            }
        }
    }
}
