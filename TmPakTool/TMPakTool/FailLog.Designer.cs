namespace paktool
{
    partial class FailLog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._spltMain = new System.Windows.Forms.SplitContainer();
            this._lstFails = new System.Windows.Forms.ListBox();
            this._txtStacktrace = new System.Windows.Forms.TextBox();
            this._pnlBottom = new System.Windows.Forms.Panel();
            this._btnClose = new System.Windows.Forms.Button();
            this._spltMain.Panel1.SuspendLayout();
            this._spltMain.Panel2.SuspendLayout();
            this._spltMain.SuspendLayout();
            this._pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _spltMain
            // 
            this._spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spltMain.Location = new System.Drawing.Point(0, 0);
            this._spltMain.Name = "_spltMain";
            this._spltMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _spltMain.Panel1
            // 
            this._spltMain.Panel1.Controls.Add(this._lstFails);
            // 
            // _spltMain.Panel2
            // 
            this._spltMain.Panel2.Controls.Add(this._txtStacktrace);
            this._spltMain.Size = new System.Drawing.Size(591, 300);
            this._spltMain.SplitterDistance = 184;
            this._spltMain.TabIndex = 0;
            // 
            // _lstFails
            // 
            this._lstFails.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstFails.FormattingEnabled = true;
            this._lstFails.IntegralHeight = false;
            this._lstFails.Location = new System.Drawing.Point(0, 0);
            this._lstFails.Name = "_lstFails";
            this._lstFails.Size = new System.Drawing.Size(591, 184);
            this._lstFails.TabIndex = 0;
            // 
            // _txtStacktrace
            // 
            this._txtStacktrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtStacktrace.Location = new System.Drawing.Point(0, 0);
            this._txtStacktrace.Multiline = true;
            this._txtStacktrace.Name = "_txtStacktrace";
            this._txtStacktrace.ReadOnly = true;
            this._txtStacktrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtStacktrace.Size = new System.Drawing.Size(591, 112);
            this._txtStacktrace.TabIndex = 0;
            // 
            // _pnlBottom
            // 
            this._pnlBottom.Controls.Add(this._btnClose);
            this._pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlBottom.Location = new System.Drawing.Point(0, 300);
            this._pnlBottom.Name = "_pnlBottom";
            this._pnlBottom.Size = new System.Drawing.Size(591, 37);
            this._pnlBottom.TabIndex = 1;
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnClose.Location = new System.Drawing.Point(505, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(83, 29);
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "OK";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // FailLog
            // 
            this.AcceptButton = this._btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnClose;
            this.ClientSize = new System.Drawing.Size(591, 337);
            this.Controls.Add(this._spltMain);
            this.Controls.Add(this._pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(344, 267);
            this.Name = "FailLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Errors";
            this._spltMain.Panel1.ResumeLayout(false);
            this._spltMain.Panel2.ResumeLayout(false);
            this._spltMain.Panel2.PerformLayout();
            this._spltMain.ResumeLayout(false);
            this._pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _spltMain;
        private System.Windows.Forms.TextBox _txtStacktrace;
        private System.Windows.Forms.ListBox _lstFails;
        private System.Windows.Forms.Panel _pnlBottom;
        private System.Windows.Forms.Button _btnClose;
    }
}