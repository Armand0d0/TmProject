namespace paktool
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._spltMain = new System.Windows.Forms.SplitContainer();
            this._tvMain = new System.Windows.Forms.TreeView();
            this._mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mnuAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this._mnuExtract = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this._imgList = new System.Windows.Forms.ImageList(this.components);
            this._dlgOpenPak = new System.Windows.Forms.OpenFileDialog();
            this._tsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnOpen = new System.Windows.Forms.ToolStripButton();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this._btnExtract = new System.Windows.Forms.ToolStripButton();
            this._btnRemove = new System.Windows.Forms.ToolStripButton();
            this._tsSep2 = new System.Windows.Forms.ToolStripSeparator();
            this._btnAddFolder = new System.Windows.Forms.ToolStripButton();
            this._btnAddFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnProperties = new System.Windows.Forms.ToolStripButton();
            this._btnAbout = new System.Windows.Forms.ToolStripButton();
            this._btnPackList = new System.Windows.Forms.ToolStripButton();
            this._dlgExtractFile = new System.Windows.Forms.SaveFileDialog();
            this._dlgChooseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this._dlgAddFile = new System.Windows.Forms.OpenFileDialog();
            this._dlgOpenPackList = new System.Windows.Forms.OpenFileDialog();
            this._spltMain.Panel1.SuspendLayout();
            this._spltMain.SuspendLayout();
            this._mnuContext.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _spltMain
            // 
            this._spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spltMain.Location = new System.Drawing.Point(0, 25);
            this._spltMain.Name = "_spltMain";
            // 
            // _spltMain.Panel1
            // 
            this._spltMain.Panel1.Controls.Add(this._tvMain);
            this._spltMain.Size = new System.Drawing.Size(783, 447);
            this._spltMain.SplitterDistance = 301;
            this._spltMain.TabIndex = 0;
            // 
            // _tvMain
            // 
            this._tvMain.AllowDrop = true;
            this._tvMain.ContextMenuStrip = this._mnuContext;
            this._tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tvMain.HideSelection = false;
            this._tvMain.ImageIndex = 0;
            this._tvMain.ImageList = this._imgList;
            this._tvMain.Location = new System.Drawing.Point(0, 0);
            this._tvMain.Name = "_tvMain";
            this._tvMain.SelectedImageIndex = 0;
            this._tvMain.Size = new System.Drawing.Size(301, 447);
            this._tvMain.TabIndex = 0;
            this._tvMain.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._tvMain_ItemDrag);
            this._tvMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tvMain_AfterSelect);
            this._tvMain.DragDrop += new System.Windows.Forms.DragEventHandler(this._tvMain_DragDrop);
            this._tvMain.DragEnter += new System.Windows.Forms.DragEventHandler(this._tvMain_DragEnter);
            this._tvMain.DoubleClick += new System.EventHandler(this._tvMain_DoubleClick);
            this._tvMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this._tvMain_KeyUp);
            // 
            // _mnuContext
            // 
            this._mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuAddFolder,
            this._mnuAddFile,
            this._mnuRemove,
            this._mnuSep1,
            this._mnuExtract,
            this._mnuProperties});
            this._mnuContext.Name = "_mnuContext";
            this._mnuContext.Size = new System.Drawing.Size(140, 120);
            this._mnuContext.Opening += new System.ComponentModel.CancelEventHandler(this._mnuContext_Opening);
            // 
            // _mnuAddFolder
            // 
            this._mnuAddFolder.Name = "_mnuAddFolder";
            this._mnuAddFolder.Size = new System.Drawing.Size(139, 22);
            this._mnuAddFolder.Text = "Add folder...";
            this._mnuAddFolder.Click += new System.EventHandler(this._btnAddFolder_Click);
            // 
            // _mnuAddFile
            // 
            this._mnuAddFile.Name = "_mnuAddFile";
            this._mnuAddFile.Size = new System.Drawing.Size(139, 22);
            this._mnuAddFile.Text = "Add file...";
            this._mnuAddFile.Click += new System.EventHandler(this._btnAddFile_Click);
            // 
            // _mnuRemove
            // 
            this._mnuRemove.Name = "_mnuRemove";
            this._mnuRemove.Size = new System.Drawing.Size(139, 22);
            this._mnuRemove.Text = "Remove";
            this._mnuRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _mnuSep1
            // 
            this._mnuSep1.Name = "_mnuSep1";
            this._mnuSep1.Size = new System.Drawing.Size(136, 6);
            // 
            // _mnuExtract
            // 
            this._mnuExtract.Name = "_mnuExtract";
            this._mnuExtract.Size = new System.Drawing.Size(139, 22);
            this._mnuExtract.Text = "Extract to...";
            this._mnuExtract.Click += new System.EventHandler(this._btnExtract_Click);
            // 
            // _mnuProperties
            // 
            this._mnuProperties.Name = "_mnuProperties";
            this._mnuProperties.Size = new System.Drawing.Size(139, 22);
            this._mnuProperties.Text = "Properties";
            this._mnuProperties.Click += new System.EventHandler(this._btnProperties_Click);
            // 
            // _imgList
            // 
            this._imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imgList.ImageStream")));
            this._imgList.TransparentColor = System.Drawing.Color.Transparent;
            this._imgList.Images.SetKeyName(0, "Pack");
            this._imgList.Images.SetKeyName(1, "Folder");
            this._imgList.Images.SetKeyName(2, "File");
            // 
            // _dlgOpenPak
            // 
            this._dlgOpenPak.Filter = "NadeoPak files|*.pak";
            this._dlgOpenPak.Title = "Open NadeoPak";
            // 
            // _tsSep1
            // 
            this._tsSep1.Name = "_tsSep1";
            this._tsSep1.Size = new System.Drawing.Size(6, 25);
            // 
            // _toolStrip
            // 
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnOpen,
            this._btnSave,
            this._tsSep1,
            this._btnExtract,
            this._btnRemove,
            this._tsSep2,
            this._btnAddFolder,
            this._btnAddFile,
            this.toolStripSeparator1,
            this._btnProperties,
            this._btnAbout,
            this._btnPackList});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(783, 25);
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnOpen
            // 
            this._btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("_btnOpen.Image")));
            this._btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnOpen.Name = "_btnOpen";
            this._btnOpen.Size = new System.Drawing.Size(23, 22);
            this._btnOpen.Text = "Open (Ctrl+O)";
            this._btnOpen.Click += new System.EventHandler(this._btnOpen_Click);
            // 
            // _btnSave
            // 
            this._btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSave.Enabled = false;
            this._btnSave.Image = ((System.Drawing.Image)(resources.GetObject("_btnSave.Image")));
            this._btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(23, 22);
            this._btnSave.Text = "Save (Ctrl+S)";
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _btnExtract
            // 
            this._btnExtract.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnExtract.Enabled = false;
            this._btnExtract.Image = ((System.Drawing.Image)(resources.GetObject("_btnExtract.Image")));
            this._btnExtract.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnExtract.Name = "_btnExtract";
            this._btnExtract.Size = new System.Drawing.Size(23, 22);
            this._btnExtract.Text = "Extract";
            this._btnExtract.Click += new System.EventHandler(this._btnExtract_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnRemove.Enabled = false;
            this._btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("_btnRemove.Image")));
            this._btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(23, 22);
            this._btnRemove.Text = "Remove";
            this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _tsSep2
            // 
            this._tsSep2.Name = "_tsSep2";
            this._tsSep2.Size = new System.Drawing.Size(6, 25);
            // 
            // _btnAddFolder
            // 
            this._btnAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnAddFolder.Enabled = false;
            this._btnAddFolder.Image = ((System.Drawing.Image)(resources.GetObject("_btnAddFolder.Image")));
            this._btnAddFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnAddFolder.Name = "_btnAddFolder";
            this._btnAddFolder.Size = new System.Drawing.Size(23, 22);
            this._btnAddFolder.Text = "Add folder";
            this._btnAddFolder.Click += new System.EventHandler(this._btnAddFolder_Click);
            // 
            // _btnAddFile
            // 
            this._btnAddFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnAddFile.Enabled = false;
            this._btnAddFile.Image = ((System.Drawing.Image)(resources.GetObject("_btnAddFile.Image")));
            this._btnAddFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnAddFile.Name = "_btnAddFile";
            this._btnAddFile.Size = new System.Drawing.Size(23, 22);
            this._btnAddFile.Text = "Add file";
            this._btnAddFile.Click += new System.EventHandler(this._btnAddFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _btnProperties
            // 
            this._btnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnProperties.Enabled = false;
            this._btnProperties.Image = ((System.Drawing.Image)(resources.GetObject("_btnProperties.Image")));
            this._btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnProperties.Name = "_btnProperties";
            this._btnProperties.Size = new System.Drawing.Size(23, 22);
            this._btnProperties.Text = "Properties (Ctrl+P)";
            this._btnProperties.Click += new System.EventHandler(this._btnProperties_Click);
            // 
            // _btnAbout
            // 
            this._btnAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("_btnAbout.Image")));
            this._btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnAbout.Name = "_btnAbout";
            this._btnAbout.Size = new System.Drawing.Size(23, 22);
            this._btnAbout.Text = "toolStripButton1";
            this._btnAbout.Click += new System.EventHandler(this._btnAbout_Click);
            // 
            // _btnPackList
            // 
            this._btnPackList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._btnPackList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPackList.Image = ((System.Drawing.Image)(resources.GetObject("_btnPackList.Image")));
            this._btnPackList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnPackList.Name = "_btnPackList";
            this._btnPackList.Size = new System.Drawing.Size(23, 22);
            this._btnPackList.Text = "View packlist.dat";
            this._btnPackList.Click += new System.EventHandler(this._btnPackList_Click);
            // 
            // _dlgExtractFile
            // 
            this._dlgExtractFile.Filter = "All files|*";
            this._dlgExtractFile.Title = "Extract file";
            // 
            // _dlgAddFile
            // 
            this._dlgAddFile.Filter = "All files|*";
            this._dlgAddFile.Multiselect = true;
            // 
            // _dlgOpenPackList
            // 
            this._dlgOpenPackList.Filter = "Pack lists|packlist.dat";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 472);
            this.Controls.Add(this._spltMain);
            this.Controls.Add(this._toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(430, 275);
            this.Name = "MainForm";
            this.Text = "TMPakTool";
            this._spltMain.Panel1.ResumeLayout(false);
            this._spltMain.ResumeLayout(false);
            this._mnuContext.ResumeLayout(false);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer _spltMain;
        private System.Windows.Forms.OpenFileDialog _dlgOpenPak;
        private System.Windows.Forms.ToolStripButton _btnOpen;
        private System.Windows.Forms.ToolStripButton _btnSave;
        private System.Windows.Forms.ToolStripSeparator _tsSep1;
        private System.Windows.Forms.ToolStripButton _btnExtract;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnRemove;
        private System.Windows.Forms.ToolStripSeparator _tsSep2;
        private System.Windows.Forms.ToolStripButton _btnAddFolder;
        private System.Windows.Forms.ToolStripButton _btnAddFile;
        private System.Windows.Forms.SaveFileDialog _dlgExtractFile;
        private System.Windows.Forms.FolderBrowserDialog _dlgChooseFolder;
        private System.Windows.Forms.TreeView _tvMain;
        private System.Windows.Forms.OpenFileDialog _dlgAddFile;
        private System.Windows.Forms.ImageList _imgList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _btnProperties;
        private System.Windows.Forms.ContextMenuStrip _mnuContext;
        private System.Windows.Forms.ToolStripMenuItem _mnuAddFolder;
        private System.Windows.Forms.ToolStripMenuItem _mnuAddFile;
        private System.Windows.Forms.ToolStripMenuItem _mnuRemove;
        private System.Windows.Forms.ToolStripSeparator _mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem _mnuProperties;
        private System.Windows.Forms.ToolStripMenuItem _mnuExtract;
        private System.Windows.Forms.ToolStripButton _btnPackList;
        private System.Windows.Forms.OpenFileDialog _dlgOpenPackList;
        private System.Windows.Forms.ToolStripButton _btnAbout;
    }
}

