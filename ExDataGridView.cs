using System;
using System.Windows.Forms;

namespace SMS_Search
{
    public class ExDataGridView : DataGridView
    {
        private const int WM_MOUSEHWHEEL = 0x020E;
        private int _anchorColumnIndex = -1;
        private int _anchorOffset = 0;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEHWHEEL)
            {
                // Parse the delta from WParam (high-order word)
                int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);

                if (delta != 0 && this.HorizontalScrollBar.Visible)
                {
                    try
                    {
                        // Determine scroll amount.
                        // Standard mouse wheel delta is 120.
                        // We scroll a fixed amount of pixels per notch, e.g., 40px.
                        // You can adjust this sensitivity as needed.
                        int pixelsPerTick = 40;
                        int move = (delta / 120) * pixelsPerTick;

                        // Positive delta (Tilt Right) -> Increase Offset (Scroll Right)
                        // Negative delta (Tilt Left)  -> Decrease Offset (Scroll Left)
                        int newOffset = this.HorizontalScrollingOffset + move;

                        // Clamp minimum to 0
                        if (newOffset < 0)
                        {
                            newOffset = 0;
                        }

                        // We rely on the DataGridView to clamp the maximum value internally.
                        // However, strictly speaking, we should check against the maximum scroll range
                        // to be perfectly safe, but the property setter typically handles the upper bound.
                        this.HorizontalScrollingOffset = newOffset;

                        // Mark message as handled (return 0 per Win32 API)
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    catch (Exception)
                    {
                        // Suppress errors during scrolling to prevent crashes
                        // (e.g., if transient state makes scrolling invalid)
                    }
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            UpdateAnchor();
        }

        private void UpdateAnchor()
        {
            if (this.Columns.Count == 0)
            {
                _anchorColumnIndex = -1;
                _anchorOffset = 0;
                return;
            }

            // FirstDisplayedScrollingColumnIndex corresponds to the first visible column in the SCROLLABLE area
            int firstIdx = this.FirstDisplayedScrollingColumnIndex;
            if (firstIdx >= 0 && firstIdx < this.Columns.Count)
            {
                _anchorColumnIndex = firstIdx;
                int anchorDisplayIndex = this.Columns[firstIdx].DisplayIndex;

                // Calculate offset into the column
                // HorizontalScrollingOffset is total pixels scrolled in the scrollable area.
                // We sum widths of visible, unfrozen columns that visually precede the anchor.

                int sumWidths = 0;
                foreach (DataGridViewColumn col in this.Columns)
                {
                    if (col.Visible && !col.Frozen && col.DisplayIndex < anchorDisplayIndex)
                    {
                        sumWidths += col.Width;
                    }
                }

                _anchorOffset = this.HorizontalScrollingOffset - sumWidths;
            }
        }

        protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            base.OnColumnWidthChanged(e);

            if (this.Columns.Count == 0 || _anchorColumnIndex < 0) return;

            // Check if anchor is still valid (bounds check)
            if (_anchorColumnIndex >= this.Columns.Count)
            {
                _anchorColumnIndex = -1;
                return;
            }

            // Only attempt to restore position if the anchor column is visible and not frozen
            if (this.Columns[_anchorColumnIndex].Visible && !this.Columns[_anchorColumnIndex].Frozen)
            {
                int anchorDisplayIndex = this.Columns[_anchorColumnIndex].DisplayIndex;

                int newSumWidths = 0;
                foreach (DataGridViewColumn col in this.Columns)
                {
                    if (col.Visible && !col.Frozen && col.DisplayIndex < anchorDisplayIndex)
                    {
                        newSumWidths += col.Width;
                    }
                }

                int newScrollOffset = newSumWidths + _anchorOffset;
                if (newScrollOffset < 0) newScrollOffset = 0;

                if (this.HorizontalScrollingOffset != newScrollOffset)
                {
                    try
                    {
                        this.HorizontalScrollingOffset = newScrollOffset;
                    }
                    catch
                    {
                        // Ignore range errors or transient states
                    }
                }
            }
        }
    }
}
