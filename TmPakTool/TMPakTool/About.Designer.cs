namespace paktool
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this._picIcon = new System.Windows.Forms.PictureBox();
            this._lblTitle = new System.Windows.Forms.Label();
            this._lblAuthor = new System.Windows.Forms.Label();
            this._btnOK = new System.Windows.Forms.Button();
            this._lblDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // _picIcon
            // 
            this._picIcon.BackColor = System.Drawing.SystemColors.ControlLight;
            this._picIcon.Image = ((System.Drawing.Image)(resources.GetObject("_picIcon.Image")));
            this._picIcon.Location = new System.Drawing.Point(12, 12);
            this._picIcon.Name = "_picIcon";
            this._picIcon.Size = new System.Drawing.Size(60, 60);
            this._picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._picIcon.TabIndex = 0;
            this._picIcon.TabStop = false;
            // 
            // _lblTitle
            // 
            this._lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.Location = new System.Drawing.Point(78, 9);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(177, 27);
            this._lblTitle.TabIndex = 1;
            this._lblTitle.Text = "<Title>";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lblAuthor
            // 
            this._lblAuthor.Location = new System.Drawing.Point(81, 59);
            this._lblAuthor.Name = "_lblAuthor";
            this._lblAuthor.Size = new System.Drawing.Size(174, 13);
            this._lblAuthor.TabIndex = 2;
            this._lblAuthor.Text = "By arc_";
            this._lblAuthor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnOK.Location = new System.Drawing.Point(79, 90);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(98, 33);
            this._btnOK.TabIndex = 3;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _lblDate
            // 
            this._lblDate.Location = new System.Drawing.Point(81, 36);
            this._lblDate.Name = "_lblDate";
            this._lblDate.Size = new System.Drawing.Size(174, 15);
            this._lblDate.TabIndex = 2;
            this._lblDate.Text = "2010/10/13";
            this._lblDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // About
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnOK;
            this.ClientSize = new System.Drawing.Size(256, 135);
            this.Controls.Add(this._btnOK);
            this.Controls.Add(this._lblDate);
            this.Controls.Add(this._lblAuthor);
            this.Controls.Add(this._lblTitle);
            this.Controls.Add(this._picIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this._picIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _picIcon;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblAuthor;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Label _lblDate;
    }
}