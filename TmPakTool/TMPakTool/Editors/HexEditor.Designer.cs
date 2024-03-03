namespace paktool.Editors
{
    partial class HexEditor
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
            this._hexBox = new Be.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // _hexBox
            // 
            this._hexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._hexBox.LineInfoForeColor = System.Drawing.Color.Empty;
            this._hexBox.LineInfoVisible = true;
            this._hexBox.Location = new System.Drawing.Point(0, 0);
            this._hexBox.Name = "_hexBox";
            this._hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this._hexBox.Size = new System.Drawing.Size(358, 216);
            this._hexBox.StringViewVisible = true;
            this._hexBox.TabIndex = 0;
            this._hexBox.UseFixedBytesPerLine = true;
            this._hexBox.VScrollBarVisible = true;
            // 
            // HexEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._hexBox);
            this.Name = "HexEditor";
            this.Size = new System.Drawing.Size(358, 216);
            this.ResumeLayout(false);

        }

        #endregion

        private Be.Windows.Forms.HexBox _hexBox;
    }
}
