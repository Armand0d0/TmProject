namespace paktool.Editors
{
    partial class ScriptEditor
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
            this._txtScript = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _txtScript
            // 
            this._txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtScript.Location = new System.Drawing.Point(0, 0);
            this._txtScript.Multiline = true;
            this._txtScript.Name = "_txtScript";
            this._txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtScript.Size = new System.Drawing.Size(150, 150);
            this._txtScript.TabIndex = 0;
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._txtScript);
            this.Name = "ScriptEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtScript;
    }
}
