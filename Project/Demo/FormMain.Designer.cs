namespace StreamDeckDemo
{
    partial class FormMain
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.iTextBoxKeyEditor = new System.Windows.Forms.TextBox();
            this.iButtonSave = new System.Windows.Forms.Button();
            this.iButtonFont = new System.Windows.Forms.Button();
            this.iFontDialog = new System.Windows.Forms.FontDialog();
            this.SuspendLayout();
            // 
            // iTextBoxKeyEditor
            // 
            this.iTextBoxKeyEditor.Location = new System.Drawing.Point(631, 257);
            this.iTextBoxKeyEditor.Multiline = true;
            this.iTextBoxKeyEditor.Name = "iTextBoxKeyEditor";
            this.iTextBoxKeyEditor.Size = new System.Drawing.Size(72, 72);
            this.iTextBoxKeyEditor.TabIndex = 0;
            this.iTextBoxKeyEditor.TextChanged += new System.EventHandler(this.iTextBoxKeyEditor_TextChanged);
            // 
            // iButtonSave
            // 
            this.iButtonSave.Location = new System.Drawing.Point(631, 392);
            this.iButtonSave.Name = "iButtonSave";
            this.iButtonSave.Size = new System.Drawing.Size(75, 23);
            this.iButtonSave.TabIndex = 1;
            this.iButtonSave.Text = "Save";
            this.iButtonSave.UseVisualStyleBackColor = true;
            this.iButtonSave.Click += new System.EventHandler(this.iButtonSave_Click);
            // 
            // iButtonFont
            // 
            this.iButtonFont.Location = new System.Drawing.Point(631, 335);
            this.iButtonFont.Name = "iButtonFont";
            this.iButtonFont.Size = new System.Drawing.Size(75, 23);
            this.iButtonFont.TabIndex = 2;
            this.iButtonFont.Text = "Font";
            this.iButtonFont.UseVisualStyleBackColor = true;
            this.iButtonFont.Click += new System.EventHandler(this.iButtonFont_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 427);
            this.Controls.Add(this.iButtonFont);
            this.Controls.Add(this.iButtonSave);
            this.Controls.Add(this.iTextBoxKeyEditor);
            this.Name = "FormMain";
            this.Text = "Stream Deck Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox iTextBoxKeyEditor;
        private System.Windows.Forms.Button iButtonSave;
        private System.Windows.Forms.Button iButtonFont;
        private System.Windows.Forms.FontDialog iFontDialog;
    }
}

