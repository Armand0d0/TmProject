namespace paktool
{
    partial class FolderProperties
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
            System.Windows.Forms.Label _lblName;
            this._bsFolder = new System.Windows.Forms.BindingSource(this.components);
            this._txtName = new System.Windows.Forms.TextBox();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            _lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._bsFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblName
            // 
            _lblName.AutoSize = true;
            _lblName.Location = new System.Drawing.Point(12, 9);
            _lblName.Name = "_lblName";
            _lblName.Size = new System.Drawing.Size(38, 13);
            _lblName.TabIndex = 1;
            _lblName.Text = "Name:";
            // 
            // _bsFolder
            // 
            this._bsFolder.DataSource = typeof(Arc.TrackMania.NadeoPak.NadeoPakFolder);
            // 
            // _txtName
            // 
            this._txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._bsFolder, "Name", true));
            this._txtName.Location = new System.Drawing.Point(56, 6);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(363, 20);
            this._txtName.TabIndex = 2;
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.Location = new System.Drawing.Point(243, 32);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(85, 28);
            this._btnOK.TabIndex = 3;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(334, 32);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(85, 28);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // FolderProperties
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(431, 69);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOK);
            this.Controls.Add(_lblName);
            this.Controls.Add(this._txtName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 107);
            this.Name = "FolderProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Folder properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FolderProperties_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._bsFolder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource _bsFolder;
        private System.Windows.Forms.TextBox _txtName;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
    }
}