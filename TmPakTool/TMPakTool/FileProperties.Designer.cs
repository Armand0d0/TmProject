namespace paktool
{
    partial class FileProperties
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label _lblClass;
            System.Windows.Forms.Label _lblFlags;
            System.Windows.Forms.Label _lblName;
            this._txtFlags = new System.Windows.Forms.TextBox();
            this._bsFile = new System.Windows.Forms.BindingSource(this.components);
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._txtName = new System.Windows.Forms.TextBox();
            this._spltMain = new System.Windows.Forms.SplitContainer();
            this._tvClass = new paktool.EngineClassTree();
            this._hexMetaData = new Be.Windows.Forms.HexBox();
            this._lblMetaData = new System.Windows.Forms.Label();
            this._pnlBottom = new System.Windows.Forms.Panel();
            _lblClass = new System.Windows.Forms.Label();
            _lblFlags = new System.Windows.Forms.Label();
            _lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._bsFile)).BeginInit();
            this._spltMain.Panel1.SuspendLayout();
            this._spltMain.Panel2.SuspendLayout();
            this._spltMain.SuspendLayout();
            this._pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblClass
            // 
            _lblClass.AutoSize = true;
            _lblClass.Location = new System.Drawing.Point(9, 32);
            _lblClass.Name = "_lblClass";
            _lblClass.Size = new System.Drawing.Size(35, 13);
            _lblClass.TabIndex = 1;
            _lblClass.Text = "Class:";
            // 
            // _lblFlags
            // 
            _lblFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            _lblFlags.AutoSize = true;
            _lblFlags.Location = new System.Drawing.Point(9, 335);
            _lblFlags.Name = "_lblFlags";
            _lblFlags.Size = new System.Drawing.Size(35, 13);
            _lblFlags.TabIndex = 2;
            _lblFlags.Text = "Flags:";
            // 
            // _lblName
            // 
            _lblName.AutoSize = true;
            _lblName.Location = new System.Drawing.Point(9, 9);
            _lblName.Name = "_lblName";
            _lblName.Size = new System.Drawing.Size(38, 13);
            _lblName.TabIndex = 6;
            _lblName.Text = "Name:";
            // 
            // _txtFlags
            // 
            this._txtFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtFlags.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._bsFile, "Flags", true));
            this._txtFlags.Location = new System.Drawing.Point(50, 332);
            this._txtFlags.Name = "_txtFlags";
            this._txtFlags.Size = new System.Drawing.Size(254, 20);
            this._txtFlags.TabIndex = 2;
            // 
            // _bsFile
            // 
            this._bsFile.DataSource = typeof(Arc.TrackMania.NadeoPak.NadeoPakFile);
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.Location = new System.Drawing.Point(451, 10);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(92, 33);
            this._btnOK.TabIndex = 3;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(549, 10);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(99, 33);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _txtName
            // 
            this._txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._bsFile, "Name", true));
            this._txtName.Location = new System.Drawing.Point(50, 6);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(254, 20);
            this._txtName.TabIndex = 0;
            // 
            // _spltMain
            // 
            this._spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spltMain.Location = new System.Drawing.Point(0, 0);
            this._spltMain.Name = "_spltMain";
            // 
            // _spltMain.Panel1
            // 
            this._spltMain.Panel1.Controls.Add(this._txtFlags);
            this._spltMain.Panel1.Controls.Add(_lblFlags);
            this._spltMain.Panel1.Controls.Add(this._tvClass);
            this._spltMain.Panel1.Controls.Add(this._txtName);
            this._spltMain.Panel1.Controls.Add(_lblName);
            this._spltMain.Panel1.Controls.Add(_lblClass);
            // 
            // _spltMain.Panel2
            // 
            this._spltMain.Panel2.Controls.Add(this._hexMetaData);
            this._spltMain.Panel2.Controls.Add(this._lblMetaData);
            this._spltMain.Size = new System.Drawing.Size(659, 352);
            this._spltMain.SplitterDistance = 307;
            this._spltMain.TabIndex = 7;
            // 
            // _tvClass
            // 
            this._tvClass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tvClass.ClassID = ((uint)(0u));
            this._tvClass.DataBindings.Add(new System.Windows.Forms.Binding("ClassID", this._bsFile, "ClassID", true));
            this._tvClass.Location = new System.Drawing.Point(50, 32);
            this._tvClass.Name = "_tvClass";
            this._tvClass.Size = new System.Drawing.Size(254, 294);
            this._tvClass.TabIndex = 1;
            // 
            // _hexMetaData
            // 
            this._hexMetaData.BytesPerLine = 8;
            this._hexMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hexMetaData.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._hexMetaData.LineInfoForeColor = System.Drawing.Color.Empty;
            this._hexMetaData.LineInfoVisible = true;
            this._hexMetaData.Location = new System.Drawing.Point(0, 19);
            this._hexMetaData.Name = "_hexMetaData";
            this._hexMetaData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this._hexMetaData.Size = new System.Drawing.Size(348, 333);
            this._hexMetaData.StringViewVisible = true;
            this._hexMetaData.TabIndex = 0;
            this._hexMetaData.UseFixedBytesPerLine = true;
            this._hexMetaData.VScrollBarVisible = true;
            // 
            // _lblMetaData
            // 
            this._lblMetaData.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblMetaData.Location = new System.Drawing.Point(0, 0);
            this._lblMetaData.Name = "_lblMetaData";
            this._lblMetaData.Size = new System.Drawing.Size(348, 19);
            this._lblMetaData.TabIndex = 1;
            this._lblMetaData.Text = "Metadata:";
            this._lblMetaData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pnlBottom
            // 
            this._pnlBottom.Controls.Add(this._btnOK);
            this._pnlBottom.Controls.Add(this._btnCancel);
            this._pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlBottom.Location = new System.Drawing.Point(0, 352);
            this._pnlBottom.Name = "_pnlBottom";
            this._pnlBottom.Size = new System.Drawing.Size(659, 52);
            this._pnlBottom.TabIndex = 8;
            // 
            // FileProperties
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(659, 404);
            this.Controls.Add(this._spltMain);
            this.Controls.Add(this._pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 280);
            this.Name = "FileProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "File properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileProperties_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._bsFile)).EndInit();
            this._spltMain.Panel1.ResumeLayout(false);
            this._spltMain.Panel1.PerformLayout();
            this._spltMain.Panel2.ResumeLayout(false);
            this._spltMain.ResumeLayout(false);
            this._pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource _bsFile;
        private System.Windows.Forms.TextBox _txtFlags;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
        private EngineClassTree _tvClass;
        private System.Windows.Forms.TextBox _txtName;
        private System.Windows.Forms.SplitContainer _spltMain;
        private Be.Windows.Forms.HexBox _hexMetaData;
        private System.Windows.Forms.Panel _pnlBottom;
        private System.Windows.Forms.Label _lblMetaData;

    }
}