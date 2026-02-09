using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMS_Search.Forms
{
    public partial class frmClipboardOptions : Form
    {
        public enum CopyAction
        {
            Cancel,
            CopyContent,
            PreserveLayout
        }

        public enum MemoryScope
        {
            Session,
            Forever
        }

        public CopyAction SelectedAction { get; private set; } = CopyAction.Cancel;
        public bool RememberChoice { get; private set; } = false;
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
