namespace paktool
{
    partial class PackListViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackListViewer));
            this._lstPacks = new System.Windows.Forms.ListView();
            this._colPack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colKeyString = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._pnlBottom = new System.Windows.Forms.Panel();
            this._btnClipboard = new System.Windows.Forms.Button();
            this._btnOK = new System.Windows.Forms.Button();
            this._pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lstPacks
            // 
            this._lstPacks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colPack,
            this._colKeyString,
            this._colKey});
            this._lstPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstPacks.FullRowSelect = true;
            this._lstPacks.HideSelection = false;
            this._lstPacks.Location = new System.Drawing.Point(0, 0);
            this._lstPacks.MultiSelect = false;
            this._lstPacks.Name = "_lstPacks";
            this._lstPacks.Size = new System.Drawing.Size(689, 266);
            this._lstPacks.TabIndex = 0;
            this._lstPacks.UseCompatibleStateImageBehavior = false;
            this._lstPacks.View = System.Windows.Forms.View.Details;
            // 
            // _colPack
            // 
            this._colPack.Text = "Pack";
            this._colPack.Width = 110;
            // 
            // _colKeyString
            // 
            this._colKeyString.Text = "Key string";
            this._colKeyString.Width = 297;
            // 
            // _colKey
            // 
            this._colKey.Text = "Resulting key";
            this._colKey.Width = 253;
            // 
            // _pnlBottom
            // 
            this._pnlBottom.Controls.Add(this._btnClipboard);
            this._pnlBottom.Controls.Add(this._btnOK);
            this._pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlBottom.Location = new System.Drawing.Point(0, 266);
            this._pnlBottom.Name = "_pnlBottom";
            this._pnlBottom.Size = new System.Drawing.Size(689, 39);
            this._pnlBottom.TabIndex = 1;
            // 
            // _btnClipboard
            // 
            this._btnClipboard.Location = new System.Drawing.Point(3, 6);
            this._btnClipboard.Name = "_btnClipboard";
            this._btnClipboard.Size = new System.Drawing.Size(93, 27);
            this._btnClipboard.TabIndex = 1;
            this._btnClipboard.Text = "To clipboard";
            this._btnClipboard.UseVisualStyleBackColor = true;
            this._btnClipboard.Click += new System.EventHandler(this._btnClipboard_Click);
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnOK.Location = new System.Drawing.Point(598, 6);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(88, 27);
            this._btnOK.TabIndex = 0;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // PackListViewer
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnOK;
            this.ClientSize = new System.Drawing.Size(689, 305);
            this.Controls.Add(this._lstPacks);
            this.Controls.Add(this._pnlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 271);
            this.Name = "PackListViewer";
            this.ShowInTaskbar = false;
            this.Text = "Packlist.dat";
            this._pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _lstPacks;
        private System.Windows.Forms.Panel _pnlBottom;
        private System.Windows.Forms.ColumnHeader _colPack;
        private System.Windows.Forms.ColumnHeader _colKeyString;
        private System.Windows.Forms.ColumnHeader _colKey;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnClipboard;
    }
}