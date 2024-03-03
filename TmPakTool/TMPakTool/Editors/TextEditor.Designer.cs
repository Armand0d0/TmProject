namespace paktool.Editors
{
    partial class TextEditor
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
            this._txtText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _txtText
            // 
            this._txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtText.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtText.Location = new System.Drawing.Point(0, 0);
            this._txtText.Multiline = true;
            this._txtText.Name = "_txtText";
            this._txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtText.Size = new System.Drawing.Size(285, 233);
            this._txtText.TabIndex = 0;
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._txtText);
            this.Name = "TextEditor";
            this.Size = new System.Drawing.Size(285, 233);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtText;
    }
}
