namespace SMS_Search.Settings
{
    partial class SearchBehaviorSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkSearchAny = new System.Windows.Forms.CheckBox();
            this.lblTableLookup = new System.Windows.Forms.Label();
            this.cmbTableLookup = new System.Windows.Forms.ComboBox();
            this.chkHighlightMatches = new System.Windows.Forms.CheckBox();
            this.picHighlightWarning = new System.Windows.Forms.PictureBox();
            this.lblHighlightColor = new System.Windows.Forms.Label();
            this.btnHighlightColor = new System.Windows.Forms.Button();
            this.grpQueryFields = new System.Windows.Forms.GroupBox();
            this.txtQryFct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQryTlz = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).BeginInit();
            this.grpQueryFields.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(15, 15);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(351, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Configure search behavior, matching options, and query fields.";
            // 
            // chkSearchAny
            // 
            this.chkSearchAny.AutoSize = true;
            this.chkSearchAny.Location = new System.Drawing.Point(18, 50);
            this.chkSearchAny.Name = "chkSearchAny";
            this.chkSearchAny.Size = new System.Drawing.Size(190, 19);
            this.chkSearchAny.TabIndex = 1;
            this.chkSearchAny.Text = "Search anywhere in description";
            this.toolTip1.SetToolTip(this.chkSearchAny, "Allow partial matches anywhere within the description field.");
            this.chkSearchAny.UseVisualStyleBackColor = true;
            // 
            // lblTableLookup
            // 
            this.lblTableLookup.AutoSize = true;
            this.lblTableLookup.Location = new System.Drawing.Point(15, 80);
            this.lblTableLookup.Name = "lblTableLookup";
            this.lblTableLookup.Size = new System.Drawing.Size(78, 15);
            this.lblTableLookup.TabIndex = 2;
            this.lblTableLookup.Text = "Table lookup:";
            // 
            // cmbTableLookup
            // 
            this.cmbTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableLookup.FormattingEnabled = true;
            this.cmbTableLookup.Items.AddRange(new object[] {
            "Show Fields",
            "Show Records"});
            this.cmbTableLookup.Location = new System.Drawing.Point(96, 77);
            this.cmbTableLookup.Name = "cmbTableLookup";
            this.cmbTableLookup.Size = new System.Drawing.Size(120, 23);
            this.cmbTableLookup.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cmbTableLookup, "Determine what data to display when performing a table lookup.");
            // 
            // chkHighlightMatches
            // 
            this.chkHighlightMatches.AutoSize = true;
            this.chkHighlightMatches.Location = new System.Drawing.Point(18, 110);
            this.chkHighlightMatches.Name = "chkHighlightMatches";
            this.chkHighlightMatches.Size = new System.Drawing.Size(116, 19);
            this.chkHighlightMatches.TabIndex = 4;
            this.chkHighlightMatches.Text = "Show filter count";
            this.toolTip1.SetToolTip(this.chkHighlightMatches, "Calculate and display the number of matches for the current filter.");
            this.chkHighlightMatches.UseVisualStyleBackColor = true;
            // 
            // picHighlightWarning
            // 
            this.picHighlightWarning.Location = new System.Drawing.Point(131, 111);
            this.picHighlightWarning.Name = "picHighlightWarning";
            this.picHighlightWarning.Size = new System.Drawing.Size(16, 16);
            this.picHighlightWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHighlightWarning.TabIndex = 5;
            this.picHighlightWarning.TabStop = false;
            // 
            // lblHighlightColor
            // 
            this.lblHighlightColor.AutoSize = true;
            this.lblHighlightColor.Location = new System.Drawing.Point(15, 140);
            this.lblHighlightColor.Name = "lblHighlightColor";
            this.lblHighlightColor.Size = new System.Drawing.Size(154, 15);
            this.lblHighlightColor.TabIndex = 6;
            this.lblHighlightColor.Text = "Filter match highlight color:";
            // 
            // btnHighlightColor
            // 
            this.btnHighlightColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighlightColor.Location = new System.Drawing.Point(171, 135);
            this.btnHighlightColor.Name = "btnHighlightColor";
            this.btnHighlightColor.Size = new System.Drawing.Size(40, 23);
            this.btnHighlightColor.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnHighlightColor, "Click to select the color used for highlighting matches.");
            this.btnHighlightColor.UseVisualStyleBackColor = true;
            // 
            // grpQueryFields
            // 
            this.grpQueryFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpQueryFields.Controls.Add(this.txtQryFct);
            this.grpQueryFields.Controls.Add(this.label2);
            this.grpQueryFields.Controls.Add(this.txtQryTlz);
            this.grpQueryFields.Controls.Add(this.label5);
            this.grpQueryFields.Location = new System.Drawing.Point(18, 180);
            this.grpQueryFields.Name = "grpQueryFields";
            this.grpQueryFields.Size = new System.Drawing.Size(410, 80);
            this.grpQueryFields.TabIndex = 8;
            this.grpQueryFields.TabStop = false;
            this.grpQueryFields.Text = "Define columns to include in query";
            // 
            // txtQryFct
            // 
            this.txtQryFct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQryFct.Location = new System.Drawing.Point(63, 19);
            this.txtQryFct.Name = "txtQryFct";
            this.txtQryFct.Size = new System.Drawing.Size(337, 23);
            this.txtQryFct.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtQryFct, "Comma-separated list of Function fields to query.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Function:";
            // 
            // txtQryTlz
            // 
            this.txtQryTlz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQryTlz.Location = new System.Drawing.Point(63, 52);
            this.txtQryTlz.Name = "txtQryTlz";
            this.txtQryTlz.Size = new System.Drawing.Size(337, 23);
            this.txtQryTlz.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtQryTlz, "Comma-separated list of Totalizer fields to query.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Totalizer:";
            // 
            // SearchBehaviorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.grpQueryFields);
            this.Controls.Add(this.btnHighlightColor);
            this.Controls.Add(this.lblHighlightColor);
            this.Controls.Add(this.picHighlightWarning);
            this.Controls.Add(this.chkHighlightMatches);
            this.Controls.Add(this.cmbTableLookup);
            this.Controls.Add(this.lblTableLookup);
            this.Controls.Add(this.chkSearchAny);
            this.Controls.Add(this.lblDescription);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SearchBehaviorSettings";
            this.Size = new System.Drawing.Size(450, 400);
            ((System.ComponentModel.ISupportInitialize)(this.picHighlightWarning)).EndInit();
            this.grpQueryFields.ResumeLayout(false);
            this.grpQueryFields.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkSearchAny;
        private System.Windows.Forms.Label lblTableLookup;
        private System.Windows.Forms.ComboBox cmbTableLookup;
        private System.Windows.Forms.CheckBox chkHighlightMatches;
        private System.Windows.Forms.PictureBox picHighlightWarning;
        private System.Windows.Forms.Label lblHighlightColor;
        private System.Windows.Forms.Button btnHighlightColor;
        private System.Windows.Forms.GroupBox grpQueryFields;
        private System.Windows.Forms.TextBox txtQryFct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQryTlz;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
