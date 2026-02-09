using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMS_Search.Forms
{
    /// <summary>
    /// Dialog form for configuring clipboard copy options, specifically for handling disjoint selections.
    /// </summary>
    public partial class frmClipboardOptions : Form
    {
        /// <summary>
        /// Defines the action to take for the copy operation.
        /// </summary>
        public enum CopyAction
        {
            Cancel,
            CopyContent,
            PreserveLayout
        }

        /// <summary>
        /// Defines how long to remember the user's choice.
        /// </summary>
        public enum MemoryScope
        {
            Session,
            Forever
        }

        /// <summary>
        /// Gets the action selected by the user.
        /// </summary>
        public CopyAction SelectedAction { get; private set; } = CopyAction.Cancel;

        /// <summary>
        /// Gets a value indicating whether the user wants to remember this choice.
        /// </summary>
        public bool RememberChoice { get; private set; } = false;

        /// <summary>
        /// Gets the scope for remembering the choice (Session or Permanent).
        /// </summary>
        public MemoryScope RememberScope { get; private set; } = MemoryScope.Session;

        public frmClipboardOptions()
        {
            InitializeComponent();
            cmbScope.SelectedIndex = 0; // Default to Session
            cmbScope.Enabled = false;
        }

        private void btnCopyContent_Click(object sender, EventArgs e)
        {
            SelectedAction = CopyAction.CopyContent;
            Close();
        }

        private void btnPreserveLayout_Click(object sender, EventArgs e)
        {
            SelectedAction = CopyAction.PreserveLayout;
            Close();
        }

        private void chkDontAsk_CheckedChanged(object sender, EventArgs e)
        {
            cmbScope.Enabled = chkDontAsk.Checked;
            RememberChoice = chkDontAsk.Checked;
        }

        private void cmbScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbScope.SelectedIndex == 0)
                RememberScope = MemoryScope.Session;
            else
                RememberScope = MemoryScope.Forever;
        }
    }
}
