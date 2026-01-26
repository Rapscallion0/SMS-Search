using System;
using System.Windows.Forms;

namespace SMS_Search
{
    public class ExDataGridView : DataGridView
    {
        private const int WM_MOUSEHWHEEL = 0x020E;

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
    }
}
