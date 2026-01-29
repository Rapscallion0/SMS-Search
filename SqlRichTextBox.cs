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

            try
            {
                // Reset text color to Normal
                this.SelectAll();
                this.SelectionColor = ColorNormal;

                string text = this.Text;

                // Combined Regex for efficiency and correct precedence
                // Group 1: Comment (--... or /*...*/)
                // Group 2: String ('...')
                // Group 3: Keyword
                string pattern = @"(?<Comment>--.*$|/\*[\s\S]*?\*/)|(?<String>'([^']|'')*')|(?<Keyword>\b(SELECT|FROM|WHERE|AND|OR|ORDER BY|GROUP BY|INSERT|UPDATE|DELETE|JOIN|LEFT|RIGHT|INNER|OUTER|TOP|DISTINCT|AS|CASE|WHEN|THEN|ELSE|END|IS|NULL|NOT|IN|EXISTS|LIKE|HAVING|UNION|ALL|CROSS|FULL|DEFAULT|VALUES|SET|CREATE|ALTER|DROP|TABLE|VIEW|INDEX|PROCEDURE|TRIGGER|FUNCTION|DECLARE|EXEC|EXECUTE)\b)";

                Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

                foreach (Match m in regex.Matches(text))
                {
                    this.Select(m.Index, m.Length);

                    if (m.Groups["Comment"].Success)
                    {
                        this.SelectionColor = ColorComment;
                    }
                    else if (m.Groups["String"].Success)
                    {
                        this.SelectionColor = ColorString;
                    }
                    else if (m.Groups["Keyword"].Success)
                    {
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
                this.SelectionColor = ColorNormal; // Ensure subsequent typing is normal color

                // Restore Redraw
                SendMessage(this.Handle, EM_SETEVENTMASK, IntPtr.Zero, eventMask);
                SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                this.Invalidate();

                _isHighlighting = false;
            }
        }
    }
}
