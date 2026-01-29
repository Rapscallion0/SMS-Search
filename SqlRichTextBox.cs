using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SMS_Search
{
    public class SqlRichTextBox : RichTextBox
    {
        private const int WM_SETREDRAW = 0x000B;
        private const int EM_GETEVENTMASK = 0x043b;
        private const int EM_SETEVENTMASK = 0x0445;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private bool _isHighlighting = false;

        // SSMS-like Colors
        private readonly Color ColorKeyword = Color.Blue;
        private readonly Color ColorString = Color.Red;
        private readonly Color ColorComment = Color.Green;
        private readonly Color ColorNormal = Color.Black;

        public SqlRichTextBox()
        {
            // Set Font to Consolas or Courier New
            Font font = new Font("Consolas", 9.75f, FontStyle.Regular);
            if (font.Name != "Consolas")
            {
                font = new Font("Courier New", 9.75f, FontStyle.Regular);
            }
            this.Font = font;

            // Allow tab indentation
            this.AcceptsTab = true;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (_isHighlighting)
                return;

            base.OnTextChanged(e);
            HighlightSyntax();
        }

        public void HighlightSyntax()
        {
            if (_isHighlighting) return;
            _isHighlighting = true;

            // Stop Redraw and Events to prevent flickering and scrolling
            IntPtr eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
            SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);

            int selectionStart = this.SelectionStart;
            int selectionLength = this.SelectionLength;

            Font regularFont = null;
            Font boldFont = null;

            try
            {
                // Create font objects once to optimize resource usage
                regularFont = new Font(this.Font, FontStyle.Regular);
                boldFont = new Font(this.Font, FontStyle.Bold);

                // Reset text color to Normal and Font to Regular
                this.SelectAll();
                this.SelectionColor = ColorNormal;
                this.SelectionFont = regularFont;

                string text = this.Text;

                // Keywords list (Added INTO, ON)
                string keywordsList = "SELECT|FROM|WHERE|AND|OR|ORDER BY|GROUP BY|INSERT|UPDATE|DELETE|JOIN|LEFT|RIGHT|INNER|OUTER|TOP|DISTINCT|AS|CASE|WHEN|THEN|ELSE|END|IS|NULL|NOT|IN|EXISTS|LIKE|HAVING|UNION|ALL|CROSS|FULL|DEFAULT|VALUES|SET|CREATE|ALTER|DROP|TABLE|VIEW|INDEX|PROCEDURE|TRIGGER|FUNCTION|DECLARE|EXEC|EXECUTE|INTO|ON";

                // Triggers for Table/Alias bolding
                string tableTriggers = "FROM|JOIN|UPDATE|INTO";

                // Exclusion list for Aliases (Keywords + ORDER/GROUP split from ORDER BY/GROUP BY)
                // We split "ORDER BY" -> "ORDER" because "ORDER" is the start of the keyword, so it shouldn't be an alias.
                string aliasExclusions = keywordsList.Replace("ORDER BY", "ORDER").Replace("GROUP BY", "GROUP");

                // Robust Identifier Patterns
                // Part: [Bracketed] OR "Quoted" OR SimpleWord
                string idPart = @"(?:\[[^\]]+\]|""[^""]+""|[\w]+)";
                // Table: Part (. Part)*
                string tablePattern = idPart + @"(?:\." + idPart + @")*";
                // Alias: Part
                string aliasPattern = idPart;

                // Combined Regex for efficiency and correct precedence
                // Group 1: Comment (--... or /*...*/)
                // Group 2: String ('...')
                // Group 3: TableDefinition (Trigger + Table + Optional Alias)
                // Group 4: Keyword

                string pattern =
                    @"(?<Comment>--.*$|/\*[\s\S]*?\*/)|" +
                    @"(?<String>'([^']|'')*')|" +
                    @"(?<TableDef>\b(?<Trigger>" + tableTriggers + @")\s+(?<Table>" + tablePattern + @")(?:\s+(?:(?<As>AS)\s+)?(?<Alias>(?!\b(" + aliasExclusions + @")\b)" + aliasPattern + @"))?)|" +
                    @"(?<Keyword>\b(" + keywordsList + @")\b)";

                Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

                foreach (Match m in regex.Matches(text))
                {
                    if (m.Groups["Comment"].Success)
                    {
                        this.Select(m.Index, m.Length);
                        this.SelectionColor = ColorComment;
                    }
                    else if (m.Groups["String"].Success)
                    {
                        this.Select(m.Index, m.Length);
                        this.SelectionColor = ColorString;
                    }
                    else if (m.Groups["TableDef"].Success)
                    {
                        // 1. Trigger (FROM, JOIN, etc) - Blue
                        Group gTrigger = m.Groups["Trigger"];
                        if(gTrigger.Success) {
                            this.Select(gTrigger.Index, gTrigger.Length);
                            this.SelectionColor = ColorKeyword;
                        }

                        // 2. Table Name - Bold
                        Group gTable = m.Groups["Table"];
                        if(gTable.Success) {
                            this.Select(gTable.Index, gTable.Length);
                            this.SelectionFont = boldFont;
                        }

                        // 3. AS - Blue
                        Group gAs = m.Groups["As"];
                        if(gAs.Success) {
                            this.Select(gAs.Index, gAs.Length);
                            this.SelectionColor = ColorKeyword;
                        }

                        // 4. Alias - Bold
                        Group gAlias = m.Groups["Alias"];
                        if(gAlias.Success) {
                            this.Select(gAlias.Index, gAlias.Length);
                            this.SelectionFont = boldFont;
                        }
                    }
                    else if (m.Groups["Keyword"].Success)
                    {
                        this.Select(m.Index, m.Length);
                        this.SelectionColor = ColorKeyword;
                    }
                }
            }
            catch
            {
                // Ignore errors during highlighting
            }
            finally
            {
                // Restore selection
                this.Select(selectionStart, selectionLength);
                this.SelectionColor = ColorNormal;
                // Ensure subsequent typing is normal
                if (regularFont != null)
                    this.SelectionFont = regularFont;

                // Dispose fonts
                if (regularFont != null) regularFont.Dispose();
                if (boldFont != null) boldFont.Dispose();

                // Restore Redraw
                SendMessage(this.Handle, EM_SETEVENTMASK, IntPtr.Zero, eventMask);
                SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                this.Invalidate();

                _isHighlighting = false;
            }
        }
    }
}
