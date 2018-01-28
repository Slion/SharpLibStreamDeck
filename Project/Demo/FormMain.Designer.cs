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
            this.iTableLayoutPanelStreamDeck = new System.Windows.Forms.TableLayoutPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // iTableLayoutPanelStreamDeck
            // 
            this.iTableLayoutPanelStreamDeck.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.iTableLayoutPanelStreamDeck.ColumnCount = 5;
            this.iTableLayoutPanelStreamDeck.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.Location = new System.Drawing.Point(89, 58);
            this.iTableLayoutPanelStreamDeck.Margin = new System.Windows.Forms.Padding(0);
            this.iTableLayoutPanelStreamDeck.Name = "iTableLayoutPanelStreamDeck";
            this.iTableLayoutPanelStreamDeck.RowCount = 3;
            this.iTableLayoutPanelStreamDeck.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.iTableLayoutPanelStreamDeck.Size = new System.Drawing.Size(380, 228);
            this.iTableLayoutPanelStreamDeck.TabIndex = 1;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 427);
            this.Controls.Add(this.iTableLayoutPanelStreamDeck);
            this.Name = "FormMain";
            this.Text = "Stream Deck Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel iTableLayoutPanelStreamDeck;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

